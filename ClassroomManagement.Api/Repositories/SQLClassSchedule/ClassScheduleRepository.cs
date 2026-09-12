using System.Text.RegularExpressions;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs.ClassSchedule;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace ClassScheduleManagement.Api.Repositories
{
    public class SQLClassScheduleRepository : IClassScheduleRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public SQLClassScheduleRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ---------------------------------------------------------
        // Existing CRUD
        // ---------------------------------------------------------
        public async Task<ClassSchedule> CreateClassScheduleAsync(
            ClassSchedule classSchedule, List<int>? teacherIds)
        {
            var teachers = await _dbContext.Teachers
                .Where(x => teacherIds.Contains(x.Id))
                .ToListAsync();

            var classScheduleDomain = new ClassSchedule
            {
                Day = classSchedule.Day,
                StartTime = classSchedule.StartTime,
                EndTime = classSchedule.EndTime,
                Level = classSchedule.Level,
                Term = classSchedule.Term,
                Section = classSchedule.Section,
                WeekType = classSchedule.WeekType,
                ClassroomId = classSchedule.ClassroomId,
                LabroomId = classSchedule.LabroomId,
                CourseId = classSchedule.CourseId,
                SessionalId = classSchedule.SessionalId,
                Teachers = teachers
            };

            await _dbContext.ClassSchedules.AddAsync(classScheduleDomain);
            await _dbContext.SaveChangesAsync();
            return classScheduleDomain;
        }

        public async Task<List<ClassSchedule>?> DeleteClassScheduleByIdAsync(int id)
        {
            var classSchedule = await _dbContext.ClassSchedules
                .FirstOrDefaultAsync(x => x.Id == id);
            if (classSchedule is null) return null;

            _dbContext.ClassSchedules.Remove(classSchedule);
            await _dbContext.SaveChangesAsync();

            var classScheduleDomains = await _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .ToListAsync();

            return classScheduleDomains;
        }

        public async Task<List<ClassSchedule>> GetAllClassSchedulesAsync(
            int? level, int? term, string? section)
        {
            var classSchedules = _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .AsQueryable();

            if (level is not null && term is not null && section is not null)
            {
                classSchedules = classSchedules
                    .Where(x => x.Level == level && x.Term == term && x.Section == section);
            }

            return await classSchedules.ToListAsync();
        }

        public async Task<List<ClassSchedule>> GetAllClassSchedulesByDayAsync(
            int level, int term, string section, DayOfWeek day)
        {
            var classSchedules = _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .AsQueryable();

            classSchedules = classSchedules.Where(s =>
                s.Level == level &&
                s.Term == term &&
                s.Section == section &&
                s.Day == day);

            return await classSchedules.ToListAsync();
        }

        public async Task<ClassSchedule?> GetClassScheduleByIdAsync(int id)
        {
            return await _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ClassSchedule?> UpdateClassScheduleByIdAsync(
            int id, ClassSchedule updatedClassSchedule, List<int>? teacherIds)
        {
            var existing = await _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Course)
                .Include(x => x.Teachers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existing is null) return null;

            var teachers = await _dbContext.Teachers
                .Where(x => teacherIds.Contains(x.Id))
                .ToListAsync();

            existing.Day = updatedClassSchedule.Day;
            existing.StartTime = updatedClassSchedule.StartTime;
            existing.EndTime = updatedClassSchedule.EndTime;
            existing.Level = updatedClassSchedule.Level;
            existing.Term = updatedClassSchedule.Term;
            existing.Section = updatedClassSchedule.Section;
            existing.WeekType = updatedClassSchedule.WeekType;
            existing.ClassroomId = updatedClassSchedule.ClassroomId;
            existing.LabroomId = updatedClassSchedule.LabroomId;
            existing.CourseId = updatedClassSchedule.CourseId;
            existing.SessionalId = updatedClassSchedule.SessionalId;
            existing.Teachers = teachers;

            await _dbContext.SaveChangesAsync();
            return existing;
        }

        // ---------------------------------------------------------
        // DOCX import — main entry point
        // ---------------------------------------------------------
        public async Task<List<string>> InputClassSchedulesAsync(Stream stream)
        {
            var messages = new List<string>();

            // 1. Parse — every skip already adds a message
            var parsed = await ParseMasterRoutineAsync(stream, messages);

            if (parsed.Count == 0)
            {
                messages.Add("No valid schedules could be parsed. Database unchanged.");
                return messages;
            }

            // 2. Detect slot collisions and DROP conflicting schedules.
            //    A (Day, StartTime, Section) slot may hold at most:
            //      - one schedule, OR
            //      - one EVEN + one ODD pair.
            var grouped = parsed
                .GroupBy(s => new { s.Day, s.StartTime, s.Level, s.Term, s.Section })
                .ToList();

            var toDrop = new HashSet<ClassSchedule>();
            foreach (var group in grouped)
            {
                if (group.Count() <= 1) continue;

                var list = group.ToList();
                bool validEvenOddPair =
                    list.Count == 2 &&
                    list.Any(s => s.WeekType == "EVEN") &&
                    list.Any(s => s.WeekType == "ODD");

                if (!validEvenOddPair)
                {
                    messages.Add(
                        $"{group.Key.Day} {group.Key.StartTime:HH\\:mm} " +
                        $"{group.Key.Level}/{group.Key.Term}-{group.Key.Section}: " +
                        $"{list.Count} schedules overlap — all skipped.");
                    foreach (var s in list) toDrop.Add(s);
                }
            }

            if (toDrop.Count > 0)
                parsed = parsed.Where(s => !toDrop.Contains(s)).ToList();

            if (parsed.Count == 0)
            {
                messages.Add("All schedules were skipped due to errors. Database unchanged.");
                return messages;
            }

            // 3. Wipe + insert in a transaction
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.ClassSchedules.ExecuteDeleteAsync();
                await _dbContext.ClassSchedules.AddRangeAsync(parsed);
                await _dbContext.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (DbUpdateException dbEx)
            {
                await tx.RollbackAsync();
                messages.Add("Import rejected by database. Database unchanged.");
                messages.Add($"Details: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                messages.Add($"Import failed: {ex.Message}. Database unchanged.");
            }

            return messages;
        }

        // ---------------------------------------------------------
        // Parsing
        // ---------------------------------------------------------
        private static readonly (TimeOnly Start, TimeOnly End)[] Slots =
        {
            (new TimeOnly(8, 0),  new TimeOnly(8, 50)),
            (new TimeOnly(9, 0),  new TimeOnly(9, 50)),
            (new TimeOnly(10, 0), new TimeOnly(10, 50)),
            // BREAK column sits here — not a real slot
            (new TimeOnly(11, 30), new TimeOnly(12, 20)),
            (new TimeOnly(12, 30), new TimeOnly(13, 20)),
            (new TimeOnly(13, 30), new TimeOnly(14, 20)),
            (new TimeOnly(14, 30), new TimeOnly(15, 20)),
            (new TimeOnly(15, 30), new TimeOnly(16, 20)),
            (new TimeOnly(16, 30), new TimeOnly(17, 20)),
        };

        private static int MapColumnToSlot(int columnIdx)
        {
            if (columnIdx >= 0 && columnIdx <= 2) return columnIdx;
            if (columnIdx == 3) return -1;
            if (columnIdx >= 4 && columnIdx <= 9) return columnIdx - 1;
            return -1;
        }

        private async Task<List<ClassSchedule>> ParseMasterRoutineAsync(
            Stream stream, List<string> messages)
        {
            var results = new List<ClassSchedule>();

            var courses = await _dbContext.Courses
                .ToDictionaryAsync(c => c.CourseCode, c => c.Id);
            var sessionals = await _dbContext.Sessionals
                .ToDictionaryAsync(s => s.SessionalCode, s => s.Id);
            var classrooms = await _dbContext.Classrooms
                .ToDictionaryAsync(c => c.RoomNumber, c => c.Id);
            var labrooms = await _dbContext.Labrooms
                .ToDictionaryAsync(l => l.RoomNumber, l => l.Id);
            var teachers = await _dbContext.Teachers
                .ToDictionaryAsync(t => t.Code, t => t);

            using var doc = WordprocessingDocument.Open(stream, false);
            var body = doc.MainDocumentPart?.Document?.Body;
            if (body == null)
            {
                messages.Add("DOCX has no document body.");
                return results;
            }

            var allTables = body.Elements<Table>().ToList();

            // Day tables are those whose rows contain a section label like "1/I - A"
            var dayTables = allTables
                .Where(t => t.Elements<TableRow>().Any(row =>
                    row.Elements<TableCell>().Any(c =>
                        Regex.IsMatch(GetCellText(c).Trim(),
                            @"^\d+/(I|II|\d+)\s*-\s*[A-Z]$"))))
                .ToList();

            if (dayTables.Count < 5)
            {
                messages.Add($"Found {dayTables.Count} day tables, expected 5.");
                return results;
            }

            for (int dayIdx = 0; dayIdx < 5; dayIdx++)
            {
                var table = dayTables[dayIdx];
                var rows = table.Elements<TableRow>().ToList();
                if (rows.Count < 2) continue;

                var day = (DayOfWeek)dayIdx;

                for (int r = 1; r < rows.Count; r++)
                {
                    var cells = rows[r].Elements<TableCell>().ToList();

                    // Locate section cell by pattern "L/T - X"
                    int sectionCellIdx = -1;
                    string sectionLabel = "";
                    for (int c = 0; c < cells.Count; c++)
                    {
                        var text = GetCellText(cells[c]).Trim();
                        if (Regex.IsMatch(text, @"^\d+/(I|II|\d+)\s*-\s*[A-Z]$"))
                        {
                            sectionCellIdx = c;
                            sectionLabel = text;
                            break;
                        }
                    }
                    if (sectionCellIdx < 0) continue;

                    var sectionMatch = Regex.Match(
                        sectionLabel, @"^(\d+)/(I|II|\d+)\s*-\s*([A-Z])$");
                    if (!sectionMatch.Success) continue;

                    int level = int.Parse(sectionMatch.Groups[1].Value);
                    string termStr = sectionMatch.Groups[2].Value;
                    int term = termStr == "I" ? 1
                             : termStr == "II" ? 2
                             : int.Parse(termStr);
                    string section = sectionMatch.Groups[3].Value;

                    int columnIdx = 0;
                    for (int c = sectionCellIdx + 1; c < cells.Count; c++)
                    {
                        var cell = cells[c];
                        var cellText = GetCellText(cell).Trim();
                        var gridSpan = GetGridSpan(cell);
                        var vMerge = GetVMerge(cell);

                        // BREAK column
                        if (columnIdx == 3)
                        {
                            columnIdx += gridSpan;
                            continue;
                        }

                        // vMerge continuation cell — nothing to parse
                        if (vMerge == "continue")
                        {
                            columnIdx += gridSpan;
                            continue;
                        }

                        // Empty cell
                        if (string.IsNullOrWhiteSpace(cellText))
                        {
                            columnIdx += gridSpan;
                            continue;
                        }

                        int slotStart = MapColumnToSlot(columnIdx);
                        if (slotStart < 0)
                        {
                            columnIdx += gridSpan;
                            continue;
                        }

                        int slotEnd = slotStart;
                        if (gridSpan > 1)
                        {
                            int lastCol = columnIdx + gridSpan - 1;
                            int slotCandidate = MapColumnToSlot(lastCol);
                            if (slotCandidate >= 0) slotEnd = slotCandidate;
                        }

                        var startTime = Slots[slotStart].Start;
                        var endTime = Slots[slotEnd].End;

                        var parts = cellText
                            .Split('/', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim())
                            .Where(p => !string.IsNullOrEmpty(p));

                        foreach (var part in parts)
                        {
                            var schedule = ParseCellPart(
                                part, day, startTime, endTime,
                                level, term, section,
                                courses, sessionals, classrooms, labrooms, teachers,
                                messages);
                            if (schedule != null) results.Add(schedule);
                        }

                        columnIdx += gridSpan;
                    }
                }
            }

            return results;
        }

        private ClassSchedule? ParseCellPart(
            string text,
            DayOfWeek day, TimeOnly startTime, TimeOnly endTime,
            int level, int term, string section,
            Dictionary<string, int> courses,
            Dictionary<string, int> sessionals,
            Dictionary<int, int> classrooms,
            Dictionary<int, int> labrooms,
            Dictionary<string, Teacher> teachers,
            List<string> messages)
        {
            // 1. Course code
            var codeMatch = Regex.Match(text, @"^([A-Z]+[\s\-]?\d+[A-Z]?)");
            if (!codeMatch.Success)
            {
                messages.Add($"{day} {startTime:HH\\:mm}: unparsable cell '{text}' — skipped.");
                return null;
            }
            string code = codeMatch.Groups[1].Value.Trim();

            // 2. Resolve course OR sessional
            int? courseId = null;
            int? sessionalId = null;
            if (courses.TryGetValue(code, out var cid)) courseId = cid;
            else if (sessionals.TryGetValue(code, out var sid)) sessionalId = sid;
            else
            {
                messages.Add($"{day} {startTime:HH\\:mm}: unknown course/sessional '{code}' — skipped.");
                return null;
            }

            // 3. Teachers — every code must resolve
            var teacherMatch = Regex.Match(text, @"\(([^)]+)\)");
            var teacherCodes = teacherMatch.Success
                ? teacherMatch.Groups[1].Value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => s.Length > 0)
                    .ToList()
                : new List<string>();

            var resolvedTeachers = new List<Teacher>();
            foreach (var tc in teacherCodes)
            {
                if (teachers.TryGetValue(tc, out var t)) resolvedTeachers.Add(t);
                else
                {
                    messages.Add($"{day} {startTime:HH\\:mm} '{code}': unknown teacher '{tc}' — skipped.");
                    return null;
                }
            }

            // 4. Room — if present, must resolve
            var roomMatch = Regex.Match(text, @"\[(\d+)\]");
            int? classroomId = null;
            int? labroomId = null;
            if (roomMatch.Success)
            {
                int roomNumber = int.Parse(roomMatch.Groups[1].Value);
                if (classrooms.TryGetValue(roomNumber, out var crid)) classroomId = crid;
                else if (labrooms.TryGetValue(roomNumber, out var lrid)) labroomId = lrid;
                else
                {
                    messages.Add($"{day} {startTime:HH\\:mm} '{code}': unknown room '{roomNumber}' — skipped.");
                    return null;
                }
            }

            // 5. Week type (optional — never causes a skip)
            var weekMatch = Regex.Match(text, @"#([A-Z]+)#");
            string? weekType = weekMatch.Success ? weekMatch.Groups[1].Value : null;

            return new ClassSchedule
            {
                Day = day,
                StartTime = startTime,
                EndTime = endTime,
                Level = level,
                Term = term,
                Section = section,
                WeekType = weekType,
                CourseId = courseId,
                SessionalId = sessionalId,
                ClassroomId = classroomId,
                LabroomId = labroomId,
                Teachers = resolvedTeachers,
            };
        }

        // ---------------------------------------------------------
        // OpenXml helpers
        // ---------------------------------------------------------
        private static string GetCellText(TableCell cell)
        {
            return string.Join(" ",
                cell.Elements<Paragraph>()
                    .Select(p => string.Concat(
                        p.Descendants<Text>().Select(t => t.Text))));
        }

        private static int GetGridSpan(TableCell cell)
        {
            return cell.TableCellProperties?.GridSpan?.Val?.Value ?? 1;
        }

        private static string? GetVMerge(TableCell cell)
        {
            var vMerge = cell.TableCellProperties?.VerticalMerge;
            if (vMerge == null) return null;
            return vMerge.Val?.Value.ToString() ?? "continue";
        }
    }
}
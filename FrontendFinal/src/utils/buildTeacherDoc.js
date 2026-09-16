// src/utils/buildTeacherDoc.js
// Builds a teacher's weekly routine DOCX.
// Layout: ONE table. Rows = days (SUN–THU). Columns = slots + BREAK.
// Matches the on-screen TeacherRoutine + RoutineTable layout.

import {
  Document,
  Packer,
  Table,
  TableRow,
  Paragraph,
  WidthType,
  AlignmentType,
  PageOrientation,
  VerticalAlign,
  TableLayoutType,
  BorderStyle,
} from 'docx';

import {
  TIME_SLOTS_NORMAL,
  TIME_SLOTS_RAMADAN,
  DAYS_OF_WEEK,
  getSlotIndex,
  getSlotSpan,
  getSectionLabel,
} from './routineUtils';

import {
  run,
  para,
  cell,
  loadLogo,
  buildUniversityHeader,
  BODY_SIZE,
  SMALL_SIZE,
  HEADER_SIZE,
  DAY_LABEL_SIZE,
  CELL_PAD,
  COLOR_MUTED,
  BOTTOM_BORDER_DASHED,
  ALL_BORDERS,
  BORDER,
} from './docxCommon';

// Column widths: Day | 3 morning slots | BREAK | 6 afternoon slots
const COL = {
  day: 800,
  slot: 1080,
  brk: 420,
};

// Section label for a class in the teacher's routine: "(2/I-A)"
const classSectionLabel = (cls) =>
  `(${cls.level}/${cls.term === 1 ? 'I' : cls.term === 2 ? 'II' : cls.term}-${cls.section})`;

// One class => its display strings
const classDisplay = (cls) => {
  const courseCode = cls.course?.courseCode || cls.sessional?.sessionalCode || '';
  const room = cls.classroom?.roomNumber || cls.labroom?.roomNumber || '';
  const weekType = cls.weekType ? `#${cls.weekType}#` : '';
  return { courseCode, room, weekType, sectionLabel: classSectionLabel(cls) };
};

// Build grid: dayIndex -> array of 9 cells (null | 'covered' | cellObject)
function buildTeacherGrid(classes) {
  const grid = Array(5).fill(null).map(() => Array(9).fill(null));
  const grouped = {};

  classes.forEach((cls) => {
    let dayIndex = -1;
    if (typeof cls.day === 'number') dayIndex = cls.day;
    else if (typeof cls.day === 'string') {
      const d = cls.day.toLowerCase();
      if (d.includes('sun')) dayIndex = 0;
      else if (d.includes('mon')) dayIndex = 1;
      else if (d.includes('tue')) dayIndex = 2;
      else if (d.includes('wed')) dayIndex = 3;
      else if (d.includes('thu')) dayIndex = 4;
    }
    if (dayIndex < 0 || dayIndex > 4) return;

    const startSlot = getSlotIndex(cls.startTime);
    if (startSlot < 0 || startSlot > 8) return;

    const key = `${dayIndex}-${startSlot}`;
    if (!grouped[key]) grouped[key] = [];
    grouped[key].push(cls);
  });

  Object.keys(grouped).forEach((key) => {
    const [dayIdxStr, slotStr] = key.split('-');
    const dayIdx = Number(dayIdxStr);
    const slotIdx = Number(slotStr);
    const list = grouped[key];
    const span = getSlotSpan(list[0].startTime, list[0].endTime);

    grid[dayIdx][slotIdx] = {
      isMerged: list.length > 1,
      schedulesList: list,
      span,
    };

    for (let i = 1; i < span; i++) {
      if (slotIdx + i < 9) grid[dayIdx][slotIdx + i] = 'covered';
    }
  });

  return grid;
}

// Content paragraphs for a data cell
function buildClassParagraphs(cellData) {
  const paragraphs = [];

  cellData.schedulesList.forEach((cls, idx) => {
    const { courseCode, room, weekType, sectionLabel } = classDisplay(cls);
    const line1 =
      `${idx > 0 ? '/ ' : ''}${courseCode} ${sectionLabel}` +
      (weekType ? ` ${weekType}` : '');
    paragraphs.push(para([run(line1, { bold: true })], { before: 40, after: 20 }));
    if (room) {
      paragraphs.push(
        para([run(`[${room}]`, { size: SMALL_SIZE })], { before: 0, after: 40 })
      );
    }
  });

  return paragraphs;
}

// Empty cell with fixed width
const emptyCell = (width) =>
  cell([para([run('')])], { width });

// One data cell (handles columnSpan, lab shading)
function dataCell(cellData, width) {
  if (!cellData) return emptyCell(width);

  const isLab = cellData.schedulesList.some((s) => !!s.sessional);

  return cell(buildClassParagraphs(cellData), {
    width: cellData.span > 1 ? width * cellData.span : width,
    columnSpan: cellData.span > 1 ? cellData.span : undefined,
    shading: isLab ? 'F9F9F9' : undefined,
  });
}

// ---------- Main table builder ----------
function buildTeacherTable(grid, slots, isNormalMode) {
  const firstHalf = slots.slice(0, 3);
  const secondHalf = slots.slice(3);

  // Column widths array (12 in normal mode, 11 in ramadan)
  const columnWidths = [COL.day];
  firstHalf.forEach(() => columnWidths.push(COL.slot));
  if (isNormalMode) columnWidths.push(COL.brk);
  secondHalf.forEach(() => columnWidths.push(COL.slot));
  const tableWidth = columnWidths.reduce((a, b) => a + b, 0);

  // ----- Header row -----
  const headerCells = [
    cell([para([run('Day', { bold: true, size: HEADER_SIZE })])], {
      width: COL.day,
    }),
  ];
  firstHalf.forEach((slot) => {
    headerCells.push(
      cell([para([run(slot.label, { bold: true, size: HEADER_SIZE })])], {
        width: COL.slot,
      })
    );
  });
  if (isNormalMode) {
    headerCells.push(
      cell([para([run('BREAK', { bold: true, size: HEADER_SIZE })])], {
        width: COL.brk,
      })
    );
  }
  secondHalf.forEach((slot) => {
    headerCells.push(
      cell([para([run(slot.label, { bold: true, size: HEADER_SIZE })])], {
        width: COL.slot,
      })
    );
  });

  const rows = [
    new TableRow({ tableHeader: true, cantSplit: true, children: headerCells }),
  ];

  // ----- Day rows -----
  DAYS_OF_WEEK.forEach((day, dayIdx) => {
    const rowSlots = grid[dayIdx];
    const rowCells = [];

    // Day cell
    rowCells.push(
      cell(
        [para([run(day.label, { bold: true, size: DAY_LABEL_SIZE })])],
        { width: COL.day }
      )
    );

    // Morning slots (0–2), clamp colSpan so labs never cross BREAK
    for (let si = 0; si < 3; si++) {
      const c = rowSlots[si];
      if (c === 'covered') continue;
      if (!c) {
        rowCells.push(emptyCell(COL.slot));
      } else {
        const clampedSpan = Math.min(c.span || 1, 3 - si);
        rowCells.push(
          dataCell(
            { ...c, span: clampedSpan },
            COL.slot
          )
        );
      }
    }

    // BREAK column, merged across all 5 day rows
    if (isNormalMode) {
      if (dayIdx === 0) {
        // Rotated vertical label, matching the on-screen column
        rowCells.push(
          cell(
            [
              para([
                run('BREAK (10.50- 11.30)', {
                  bold: true,
                  size: SMALL_SIZE,
                  color: COLOR_MUTED,
                }),
              ]),
            ],
            {
              width: COL.brk,
              rowSpan: DAYS_OF_WEEK.length,
              shading: 'F2F2F2',
              verticalAlign: VerticalAlign.CENTER,
              textDirection: 'topToBottomRightToLeft',
            }
          )
        );
      }
      // subsequent rows skip this cell (rowSpan handles it)
    }

    // Afternoon slots (3–8)
    for (let si = 3; si < 9; si++) {
      const c = rowSlots[si];
      if (c === 'covered') continue;
      if (!c) {
        rowCells.push(emptyCell(COL.slot));
      } else {
        rowCells.push(dataCell(c, COL.slot));
      }
    }

    rows.push(new TableRow({ cantSplit: true, children: rowCells }));
  });

  return new Table({
    width: { size: tableWidth, type: WidthType.DXA },
    columnWidths,
    layout: TableLayoutType.FIXED,
    rows,
  });
}

// ---------- Info strip (Designation | Credits) ----------
function buildInfoStrip(teacherDetail, season, year, routineMode) {
  const leftPara = para(
    [
      run('Designation: ', { bold: true }),
      run(teacherDetail.designation || ''),
      run('\n'),
      run('Teacher Initials: ', { bold: true }),
      run(teacherDetail.code || ''),
    ],
    { alignment: AlignmentType.LEFT, after: 100 }
  );

  const rightPara = para(
    [
      run('Total Assigned Credits: ', { bold: true }),
      run(`${(teacherDetail.assignedCredit ?? 0).toFixed(1)} Hours`),
    ],
    { alignment: AlignmentType.RIGHT, after: 100 }
  );

  const strip = new Table({
    width: { size: 12200, type: WidthType.DXA },
    columnWidths: [6100, 6100],
    layout: TableLayoutType.FIXED,
    borders: {
      top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
      bottom: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
      left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
      right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
      insideHorizontal: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
      insideVertical: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
    },
    rows: [
      new TableRow({
        children: [
          cell([leftPara], { width: 6100, borders: {
            top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
            bottom: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
            left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
            right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
          } }),
          cell([rightPara], { width: 6100, borders: {
            top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
            bottom: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
            left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
            right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
          } }),
        ],
      }),
    ],
  });

  const dashed = para([run('')], {
    borders: BOTTOM_BORDER_DASHED,
    before: 20,
    after: 200,
  });

  return [strip, dashed];
}

// ---------- Public API ----------
export async function buildTeacherDoc({
  teacherDetail,
  routineMode,
  season,
  year,
  logoUrl = '/baust_logo.png',
}) {
  const isNormalMode = routineMode === 'normal';
  const slots = isNormalMode ? TIME_SLOTS_NORMAL : TIME_SLOTS_RAMADAN;
  const logoBuffer = await loadLogo(logoUrl);

  const line1 =
    'Bangladesh Army University of Science and Technology (BAUST), Saidpur';
  const line2 = 'Department of Computer Science and Engineering (CSE)';
  const line3 =
    `Weekly Class Routine — Teacher: ${teacherDetail.name} — ${season} ${year}` +
    (routineMode === 'ramadan' ? ' (Ramadan Days)' : '');

  const children = [
    ...buildUniversityHeader(logoBuffer, line1, line2, line3),
    ...buildInfoStrip(teacherDetail, season, year, routineMode),
  ];

  const grid = buildTeacherGrid(teacherDetail.classes || []);
  children.push(buildTeacherTable(grid, slots, isNormalMode));

  // Footnote
  children.push(
    para(
      [
        run(
          'Note: Routine cells display CourseCode followed by (Level-TermSection) and [Room Number].',
          { size: SMALL_SIZE, italics: true, color: COLOR_MUTED }
        ),
      ],
      { before: 300 }
    )
  );

  return new Document({
    sections: [
      {
        properties: {
          page: {
            size: { orientation: PageOrientation.LANDSCAPE },
            margin: { top: 360, right: 360, bottom: 360, left: 360 },
          },
        },
        children,
      },
    ],
  });
}

export async function downloadTeacherDocx(opts) {
  const doc = await buildTeacherDoc(opts);
  return Packer.toBlob(doc);
}
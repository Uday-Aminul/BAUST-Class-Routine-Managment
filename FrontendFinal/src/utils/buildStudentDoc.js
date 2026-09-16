// src/utils/buildStudentDoc.js
// Batchwise/student routine DOCX:
//   1. University header
//   2. Meta row (Level-Term / Section | Advisor / DPC / Phone)
//   3. Main routine table (5 days × 12 cols)
//   4. Two sub-tables side by side: COURSES + TEACHERS
//   5. Footnote

import {
  Document,
  Packer,
  Table,
  TableRow,
  TableCell,
  Paragraph,
  TextRun,
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
  termRoman,
} from './routineUtils';

import {
  run,
  para,
  cell,
  loadLogo,
  buildUniversityHeader,
  SMALL_SIZE,
  HEADER_SIZE,
  DAY_LABEL_SIZE,
  BODY_SIZE,
  COLOR_MUTED,
  COLOR_HEADING,
  BOTTOM_BORDER_DASHED,
  BORDER,
  ALL_BORDERS,
  NO_BORDERS,
} from './docxCommon';

// ---------------------------------------------------------
// Main routine table
// ---------------------------------------------------------
const COL = {
  day: 800,
  slot: 1080,
  brk: 420,
};

function buildScheduleGrid(schedules) {
  const grid = Array(5).fill(null).map(() => Array(9).fill(null));
  const grouped = {};

  schedules.forEach((s) => {
    let dayIndex = -1;
    if (typeof s.day === 'number') dayIndex = s.day;
    else if (typeof s.day === 'string') {
      const d = s.day.toLowerCase();
      if (d.includes('sun')) dayIndex = 0;
      else if (d.includes('mon')) dayIndex = 1;
      else if (d.includes('tue')) dayIndex = 2;
      else if (d.includes('wed')) dayIndex = 3;
      else if (d.includes('thu')) dayIndex = 4;
    }
    if (dayIndex < 0 || dayIndex > 4) return;

    const startSlot = getSlotIndex(s.startTime);
    if (startSlot < 0 || startSlot > 8) return;

    const key = `${dayIndex}-${startSlot}`;
    if (!grouped[key]) grouped[key] = [];
    grouped[key].push(s);
  });

  Object.keys(grouped).forEach((key) => {
    const [dayStr, slotStr] = key.split('-');
    const dayIdx = Number(dayStr);
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

const scheduleDisplay = (s) => {
  const courseCode = s.course?.courseCode || s.sessional?.sessionalCode || '';
  const teachers = s.teachers?.map((t) => t.code).join(', ') || '';
  const room = s.classroom?.roomNumber || s.labroom?.roomNumber || '';
  const weekType = s.weekType ? `#${s.weekType}#` : '';
  return { courseCode, teachers, room, weekType };
};

function buildScheduleCellParagraphs(cellData) {
  const paragraphs = [];

  cellData.schedulesList.forEach((s, idx) => {
    const { courseCode, teachers, room, weekType } = scheduleDisplay(s);
    const line1 =
      `${idx > 0 ? '/ ' : ''}${courseCode}` +
      (teachers ? ` (${teachers})` : '') +
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

const emptyCell = (width) => cell([para([run('')])], { width });

function scheduleCell(cellData, width) {
  if (!cellData) return emptyCell(width);
  const isLab = cellData.schedulesList.some((s) => !!s.sessional);
  return cell(buildScheduleCellParagraphs(cellData), {
    width: cellData.span > 1 ? width * cellData.span : width,
    columnSpan: cellData.span > 1 ? cellData.span : undefined,
    shading: isLab ? 'F9F9F9' : undefined,
  });
}

function buildScheduleTable(grid, slots, isNormalMode) {
  const firstHalf = slots.slice(0, 3);
  const secondHalf = slots.slice(3);

  const columnWidths = [COL.day];
  firstHalf.forEach(() => columnWidths.push(COL.slot));
  if (isNormalMode) columnWidths.push(COL.brk);
  secondHalf.forEach(() => columnWidths.push(COL.slot));
  const tableWidth = columnWidths.reduce((a, b) => a + b, 0);

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

  DAYS_OF_WEEK.forEach((day, dayIdx) => {
    const rowSlots = grid[dayIdx];
    const rowCells = [];

    rowCells.push(
      cell([para([run(day.label, { bold: true, size: DAY_LABEL_SIZE })])], {
        width: COL.day,
      })
    );

    for (let si = 0; si < 3; si++) {
      const c = rowSlots[si];
      if (c === 'covered') continue;
      if (!c) rowCells.push(emptyCell(COL.slot));
      else {
        const clampedSpan = Math.min(c.span || 1, 3 - si);
        rowCells.push(scheduleCell({ ...c, span: clampedSpan }, COL.slot));
      }
    }

    if (isNormalMode && dayIdx === 0) {
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

    for (let si = 3; si < 9; si++) {
      const c = rowSlots[si];
      if (c === 'covered') continue;
      if (!c) rowCells.push(emptyCell(COL.slot));
      else rowCells.push(scheduleCell(c, COL.slot));
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

// ---------------------------------------------------------
// Meta row (Level-Term / Section  |  Advisor / DPC / Phone)
// ---------------------------------------------------------
function buildMetaTable({ level, term, section, advisor, dpcG2, dpcPhone }) {
  const leftPara = para(
    [
      run('Level-Term: ', { bold: true }),
      run(`${level}-${termRoman(term)}`, { bold: true }),
      run('   '),
      run('Section: ', { bold: true }),
      run(String(section), { bold: true }),
    ],
    { alignment: AlignmentType.LEFT, after: 40 }
  );

  const rightLine1 = para(
    [
      run('Batch Advisor: ', { bold: true }),
      run(advisor || '__________________'),
    ],
    { alignment: AlignmentType.RIGHT, after: 40 }
  );

  const rightLine2 = para(
    [
      run('DPC/G2: ', { bold: true }),
      run(dpcG2 || ''),
      run('   '),
      run(dpcPhone || ''),
    ],
    { alignment: AlignmentType.RIGHT, after: 40 }
  );

  const leftCell = cell([leftPara], { width: 6100, borders: NO_BORDERS });
  const rightCell = cell([rightLine1, rightLine2], {
    width: 6100,
    borders: NO_BORDERS,
  });

  const metaTable = new Table({
    width: { size: 12200, type: WidthType.DXA },
    columnWidths: [6100, 6100],
    layout: TableLayoutType.FIXED,
    borders: NO_BORDERS,
    rows: [new TableRow({ children: [leftCell, rightCell] })],
  });

  const dashed = para([run('')], {
    borders: BOTTOM_BORDER_DASHED,
    before: 20,
    after: 200,
  });

  return [metaTable, dashed];
}

// ---------------------------------------------------------
// COURSES sub-table
// ---------------------------------------------------------
function buildCoursesTable(courses, sessionals) {
  const THIN_BORDER = { style: BorderStyle.SINGLE, size: 4, color: '1A1A1A' };
  const CELL_BORDERS = {
    top: THIN_BORDER,
    bottom: THIN_BORDER,
    left: THIN_BORDER,
    right: THIN_BORDER,
  };

  // Build rows data
  const rowsData = [];
  courses.forEach((c) => {
    rowsData.push({
      code: c.courseCode,
      title: c.name,
      theory: c.credit,
      sessional: 0,
      credit: c.credit,
    });
  });
  sessionals.forEach((s) => {
    rowsData.push({
      code: s.sessionalCode,
      title: s.name,
      theory: 0,
      sessional: s.credit * 0.5,
      credit: s.credit,
    });
  });

  const totalTheory = rowsData.reduce((sum, r) => sum + r.theory, 0);
  const totalSessional = rowsData.reduce((sum, r) => sum + r.sessional, 0);
  const totalCredit = rowsData.reduce((sum, r) => sum + r.credit, 0);

  const fmt = (n, digits) => (n > 0 ? n.toFixed(digits) : '');

  const headerRow1 = new TableRow({
    tableHeader: true,
    children: [
      cell([para([run('COURSES', { bold: true })])], {
        width: 5400,
        columnSpan: 4,
        shading: 'B4C6E7',
      }),
    ],
  });

  const headerRow2 = new TableRow({
    tableHeader: true,
    children: [
      cell([para([run('Course No.', { bold: true, size: SMALL_SIZE })])], {
        width: 900,
        shading: 'B4C6E7',
      }),
      cell([para([run('Course Title', { bold: true, size: SMALL_SIZE })])], {
        width: 2600,
        shading: 'B4C6E7',
      }),
      cell([para([run('Hours/Week', { bold: true, size: SMALL_SIZE })])], {
        width: 800,
        columnSpan: 2,
        shading: 'B4C6E7',
      }),
      cell([para([run('Credit', { bold: true, size: SMALL_SIZE })])], {
        width: 700,
        shading: 'B4C6E7',
      }),
    ],
  });

  const headerRow3 = new TableRow({
    tableHeader: true,
    children: [
      cell([para([run('', { size: SMALL_SIZE })])], {
        width: 900,
        shading: 'B4C6E7',
      }),
      cell([para([run('', { size: SMALL_SIZE })])], {
        width: 2600,
        shading: 'B4C6E7',
      }),
      cell([para([run('Theory', { bold: true, size: SMALL_SIZE })])], {
        width: 400,
        shading: 'B4C6E7',
      }),
      cell([para([run('Sessional', { bold: true, size: SMALL_SIZE })])], {
        width: 400,
        shading: 'B4C6E7',
      }),
      cell([para([run('Hours', { bold: true, size: SMALL_SIZE })])], {
        width: 700,
        shading: 'B4C6E7',
      }),
    ],
  });

  const bodyRows = rowsData.map(
    (r) =>
      new TableRow({
        children: [
          cell([para([run(r.code, { bold: true, size: SMALL_SIZE })])], {
            width: 900,
          }),
          cell([para([run(r.title, { size: SMALL_SIZE })])], { width: 2600 }),
          cell([para([run(fmt(r.theory, 1), { size: SMALL_SIZE })])], {
            width: 400,
          }),
          cell([para([run(fmt(r.sessional, 2), { size: SMALL_SIZE })])], {
            width: 400,
          }),
          cell([para([run(r.credit.toFixed(1), { size: SMALL_SIZE })])], {
            width: 700,
          }),
        ],
      })
  );

  const totalRow = new TableRow({
    children: [
      cell([para([run('Total:', { bold: true, size: SMALL_SIZE })])], {
        width: 3500,
        columnSpan: 2,
      }),
      cell([para([run(totalTheory.toFixed(1), { bold: true, size: SMALL_SIZE })])], {
        width: 400,
      }),
      cell([para([run(totalSessional.toFixed(1), { bold: true, size: SMALL_SIZE })])], {
        width: 400,
      }),
      cell([para([run(totalCredit.toFixed(1), { bold: true, size: SMALL_SIZE })])], {
        width: 700,
      }),
    ],
  });

  return new Table({
    width: { size: 5400, type: WidthType.DXA },
    columnWidths: [900, 2600, 400, 400, 700],
    layout: TableLayoutType.FIXED,
    rows: [headerRow1, headerRow2, headerRow3, ...bodyRows, totalRow],
  });
}

// ---------------------------------------------------------
// TEACHERS sub-table
// ---------------------------------------------------------
function buildTeachersTable(teachers) {
  // Deduplicate by code
  const seen = new Set();
  const unique = [];
  teachers.forEach((t) => {
    if (!t?.code) return;
    if (seen.has(t.code)) return;
    seen.add(t.code);
    unique.push(t);
  });

  // Pad to 5 minimum
  const rows = [...unique];
  while (rows.length < 5) rows.push({ code: '', name: '', designation: '' });

  const headerRow = new TableRow({
    tableHeader: true,
    children: [
      cell([para([run('Short Form', { bold: true, size: SMALL_SIZE })])], {
        width: 800,
        shading: 'B4C6E7',
      }),
      cell([para([run('Teachers Name', { bold: true, size: SMALL_SIZE })])], {
        width: 1200,
        shading: 'B4C6E7',
      }),
      cell([para([run('Designation', { bold: true, size: SMALL_SIZE })])], {
        width: 1500,
        shading: 'B4C6E7',
      }),
    ],
  });

  const bodyRows = rows.map(
    (t) =>
      new TableRow({
        children: [
          cell(
            [para([run(t.code, { bold: true, size: SMALL_SIZE })])],
            { width: 800 }
          ),
          cell([para([run(t.name, { size: SMALL_SIZE })])], { width: 1200 }),
          cell([para([run(t.designation, { size: SMALL_SIZE })])], {
            width: 1500,
          }),
        ],
      })
  );

  const noteRow = new TableRow({
    children: [
      cell(
        [
          para([
            run('**Names are arranged randomly', {
              bold: true,
              size: SMALL_SIZE,
            }),
          ]),
        ],
        { width: 3500, columnSpan: 3, shading: 'F2F2F2' }
      ),
    ],
  });

  return new Table({
    width: { size: 3500, type: WidthType.DXA },
    columnWidths: [800, 1200, 1500],
    layout: TableLayoutType.FIXED,
    rows: [headerRow, ...bodyRows, noteRow],
  });
}

// ---------------------------------------------------------
// Public API
// ---------------------------------------------------------
export async function buildStudentDoc({
  level,
  term,
  section,
  schedules,
  allCourses,
  allSessionals,
  assignedTeachers,
  advisor,
  dpcG2,
  dpcPhone,
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
    `Batchwise Class Routine, ${season} ${year}` +
    (routineMode === 'ramadan' ? ' (Ramadan Days)' : '');

  const children = [
    ...buildUniversityHeader(logoBuffer, line1, line2, line3),
    ...buildMetaTable({ level, term, section, advisor, dpcG2, dpcPhone }),
  ];

  // Main routine table
  const grid = buildScheduleGrid(schedules);
  children.push(buildScheduleTable(grid, slots, isNormalMode));
  children.push(para([run('')], { before: 200, after: 100 }));

  // Courses + Teachers side by side (one 2-cell table)
  const coursesTable = buildCoursesTable(allCourses, allSessionals);
  const teachersTable = buildTeachersTable(assignedTeachers);

  const sideBySide = new Table({
    width: { size: 12200, type: WidthType.DXA },
    columnWidths: [6200, 6000],
    layout: TableLayoutType.FIXED,
    borders: NO_BORDERS,
    rows: [
      new TableRow({
        children: [
          cell([coursesTable], {
            width: 6200,
            borders: NO_BORDERS,
            verticalAlign: VerticalAlign.TOP,
          }),
          cell([teachersTable], {
            width: 6000,
            borders: NO_BORDERS,
            verticalAlign: VerticalAlign.TOP,
          }),
        ],
      }),
    ],
  });
  children.push(sideBySide);

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

export async function downloadStudentDocx(opts) {
  const doc = await buildStudentDoc(opts);
  return Packer.toBlob(doc);
}
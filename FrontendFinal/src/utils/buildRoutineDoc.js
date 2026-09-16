// src/utils/buildRoutineDoc.js
import {
  Document,
  Packer,
  Paragraph,
  TextRun,
  Table,
  TableRow,
  TableCell,
  WidthType,
  AlignmentType,
  ImageRun,
  BorderStyle,
  PageOrientation,
  VerticalAlign,
  TableLayoutType,
  TextDirection,
} from 'docx';

import {
  DAYS_OF_WEEK,
  TIME_SLOTS_NORMAL,
  TIME_SLOTS_RAMADAN,
  getDayGrid,
  groupSchedules,
  sortSections,
  getScheduleDisplay,
  getSectionLabel,
} from './routineUtils';

// ---------- Layout constants ----------
const FONT = 'Arial';
const BODY_SIZE = 16;    // 8pt
const SMALL_SIZE = 14;   // 7pt (room line)
const HEADER_SIZE = 16;  // 8pt
const TITLE_SIZE = 20;   // 10pt
const DAY_LABEL_SIZE = 17; // ~8.5pt, matches on-screen 0.85rem Day label
const BREAK_LABEL_SIZE = 15; // ~7.5pt, matches on-screen 0.75rem BREAK label
const CELL_PAD = 40;     // twips

// Table columns: Day | Section | 3 slots | BREAK | 6 slots  → 12 columns (normal mode)
const COL = {
  day: 700,
  section: 900,
  slot: 980,
  brk: 350,
};

const BORDER = { style: BorderStyle.SINGLE, size: 4, color: '000000' };
const ALL_BORDERS = { top: BORDER, bottom: BORDER, left: BORDER, right: BORDER };
const NO_BORDERS = {
  top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  bottom: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
};
// Matches the on-screen BREAK column's 1px solid black side borders
const BREAK_SIDE_BORDER = { style: BorderStyle.SINGLE, size: 6, color: '1A1A1A' };
const BREAK_BORDERS = {
  top: BORDER,
  bottom: BORDER,
  left: BREAK_SIDE_BORDER,
  right: BREAK_SIDE_BORDER,
};

// ---------- Small helpers ----------
const run = (text, { size = BODY_SIZE, bold = false, color } = {}) =>
  new TextRun({
    text: text || '',
    font: FONT,
    size,
    bold,
    color,
  });

const para = (children, { alignment = AlignmentType.CENTER } = {}) =>
  new Paragraph({
    alignment,
    spacing: { before: 0, after: 0, line: 200 },
    children: Array.isArray(children) ? children : [children],
  });

const cell = (children, opts = {}) =>
  new TableCell({
    width: opts.width
      ? { size: opts.width, type: WidthType.DXA }
      : undefined,
    columnSpan: opts.columnSpan,
    rowSpan: opts.rowSpan,
    shading: opts.shading ? { fill: opts.shading } : undefined,
    verticalAlign: opts.verticalAlign || VerticalAlign.CENTER,
    textDirection: opts.textDirection,
    margins: {
      top: CELL_PAD,
      bottom: CELL_PAD,
      left: CELL_PAD,
      right: CELL_PAD,
    },
    borders: opts.borders || ALL_BORDERS,
    children: Array.isArray(children) ? children : [children],
  });

// ---------- Header ----------
async function loadLogo(logoUrl) {
  try {
    const res = await fetch(logoUrl);
    if (!res.ok) return null;
    return await res.arrayBuffer();
  } catch {
    return null;
  }
}

function buildHeader(logoBuffer, subTitle) {
  const logoCell = cell(
    [
      para([
        logoBuffer
          ? new ImageRun({
              type: 'png',
              data: logoBuffer,
              transformation: { width: 75, height: 75 },
            })
          : run(''),
      ]),
    ],
    { width: 1200, borders: NO_BORDERS }
  );

  const textCell = cell(
    [
      para([
        run(
          'Bangladesh Army University of Science and Technology (BAUST), Saidpur',
          { size: TITLE_SIZE, bold: true }
        ),
      ]),
      para([
        run('Department of Computer Science and Engineering (CSE)', {
          size: HEADER_SIZE,
        }),
      ]),
      para([run(subTitle, { size: HEADER_SIZE })]),
    ],
    { width: 11000, borders: NO_BORDERS }
  );

  return new Table({
    width: { size: 12200, type: WidthType.DXA },
    columnWidths: [1200, 11000],
    layout: TableLayoutType.FIXED,
    borders: NO_BORDERS,
    rows: [new TableRow({ children: [logoCell, textCell] })],
  });
}

// ---------- One data cell ----------
function buildDataCell(cellData, colWidth) {
  if (!cellData) {
    return cell([para(run(''))], { width: colWidth });
  }

  const isLab = cellData.isMerged
    ? cellData.schedulesList.some((s) => s.sessional)
    : !!cellData.schedulesList[0].sessional;

  const paragraphs = [];

  cellData.schedulesList.forEach((sched, idx) => {
    const { courseCode, teachers, room, weekType } = getScheduleDisplay(sched);
    const line1 =
      `${idx > 0 ? '/ ' : ''}${courseCode}` +
      (teachers ? ` (${teachers})` : '') +
      (weekType ? ` ${weekType}` : '');
    paragraphs.push(para(run(line1)));
    if (room) {
      paragraphs.push(para(run(`[${room}]`, { size: SMALL_SIZE })));
    }
  });

  return cell(paragraphs, {
    width: colWidth,
    columnSpan: cellData.span > 1 ? cellData.span : undefined,
    shading: isLab ? 'F9F9F9' : undefined,
  });
}

// ---------- One day table ----------
function buildDayTable(day, dayGrid, sortedSections, slots, isNormalMode) {
  const rows = [];
  const firstHalf = slots.slice(0, 3);
  const secondHalf = slots.slice(3);

  // ----- Header row -----
  const headerCells = [
    cell([para(run('Day', { bold: true, size: HEADER_SIZE }))], {
      width: COL.day,
    }),
    cell([para(run('Section', { bold: true, size: HEADER_SIZE }))], {
      width: COL.section,
    }),
  ];
  firstHalf.forEach((slot) => {
    headerCells.push(
      cell([para(run(slot.label, { bold: true, size: HEADER_SIZE }))], {
        width: COL.slot,
      })
    );
  });
  if (isNormalMode) {
    headerCells.push(
      cell([para(run('BREAK', { bold: true, size: HEADER_SIZE }))], {
        width: COL.brk,
      })
    );
  }
  secondHalf.forEach((slot) => {
    headerCells.push(
      cell([para(run(slot.label, { bold: true, size: HEADER_SIZE }))], {
        width: COL.slot,
      })
    );
  });
  // cantSplit: true keeps the header row intact if it ever falls at a page edge
  rows.push(
    new TableRow({ tableHeader: true, cantSplit: true, children: headerCells })
  );

  // ----- Section rows -----
  sortedSections.forEach((sec, secIdx) => {
    const secRow = dayGrid[sec.id] || Array(9).fill(null);
    const rowCells = [];

    // --- Day column: single cell with rowSpan across all sections,
    // rotated bottom-to-top to match the on-screen vertical "Day" label
    if (secIdx === 0) {
      rowCells.push(
        cell([para(run(day.label.toUpperCase(), { bold: true, size: DAY_LABEL_SIZE }))], {
          width: COL.day,
          rowSpan: sortedSections.length,
          shading: 'F2F2F2',
          verticalAlign: VerticalAlign.CENTER,
          textDirection: TextDirection.BOTTOM_TO_TOP_LEFT_TO_RIGHT,
        })
      );
    }
    // (skip day cell for subsequent rows — rowSpan handles it)

    // --- Section label
    rowCells.push(
      cell([para(run(getSectionLabel(sec), { bold: true }))], {
        width: COL.section,
      })
    );

    // --- First half of slots (before the break)
    for (let slotIdx = 0; slotIdx < 3; slotIdx++) {
      const cellData = secRow[slotIdx];
      if (cellData === 'covered') continue;
      rowCells.push(buildDataCell(cellData, COL.slot));
    }

    // --- BREAK column: single cell with rowSpan across all sections,
    // rotated top-to-bottom to match the on-screen vertical "BREAK" label
    if (isNormalMode && secIdx === 0) {
      rowCells.push(
        cell(
          [para(run('BREAK (10.50- 11.30)', { bold: true, size: BREAK_LABEL_SIZE, color: '4A4A4A' }))],
          {
            width: COL.brk,
            rowSpan: sortedSections.length,
            shading: 'F2F2F2',
            verticalAlign: VerticalAlign.CENTER,
            textDirection: TextDirection.TOP_TO_BOTTOM_RIGHT_TO_LEFT,
            borders: BREAK_BORDERS,
          }
        )
      );
    }

    // --- Second half of slots (after the break)
    for (let slotIdx = 3; slotIdx < 9; slotIdx++) {
      const cellData = secRow[slotIdx];
      if (cellData === 'covered') continue;
      rowCells.push(buildDataCell(cellData, COL.slot));
    }

    // cantSplit: true stops Word/LibreOffice from slicing a row's content
    // (e.g. a merged EVEN/ODD cell) across a page boundary.
    rows.push(new TableRow({ cantSplit: true, children: rowCells }));
  });

  const columnWidths = [COL.day, COL.section];
  firstHalf.forEach(() => columnWidths.push(COL.slot));
  if (isNormalMode) columnWidths.push(COL.brk);
  secondHalf.forEach(() => columnWidths.push(COL.slot));
  const tableWidth = columnWidths.reduce((a, b) => a + b, 0);

  return new Table({
    width: { size: tableWidth, type: WidthType.DXA },
    columnWidths,
    layout: TableLayoutType.FIXED,
    rows,
  });
}

// ---------- Public API ----------
export async function buildRoutineDoc({
  sections,
  schedules,
  routineMode,
  season,
  year,
  logoUrl = '/baust_logo.png',
}) {
  const isNormalMode = routineMode === 'normal';
  const slots = isNormalMode ? TIME_SLOTS_NORMAL : TIME_SLOTS_RAMADAN;
  const sortedSections = sortSections(sections);
  const grouped = groupSchedules(schedules, sections);

  const logoBuffer = await loadLogo(logoUrl);

  const subTitle =
    `Master Weekly Class Routine — ${season} ${year}` +
    (routineMode === 'ramadan' ? ' (Ramadan Days)' : '');

  const children = [];
  children.push(buildHeader(logoBuffer, subTitle));
  children.push(para(run('')));

  DAYS_OF_WEEK.forEach((day, idx) => {
    children.push(
      para(
        [
          run(`${day.label.toUpperCase()} SCHEDULE`, {
            bold: true,
            size: TITLE_SIZE,
            color: '0B3D1B',
          }),
        ],
        { alignment: AlignmentType.LEFT }
      )
    );

    const dayGrid = getDayGrid(day.value, sortedSections, grouped);
    children.push(buildDayTable(day, dayGrid, sortedSections, slots, isNormalMode));

    if (idx < DAYS_OF_WEEK.length - 1) {
      children.push(new Paragraph({ pageBreakBefore: true, children: [] }));
    }
  });

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

export async function downloadRoutineDocx(opts) {
  const doc = await buildRoutineDoc(opts);
  return Packer.toBlob(doc);
}
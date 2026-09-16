// src/utils/docxCommon.js
// Shared DOCX primitives. Used by buildTeacherDoc, buildClassroomDoc, etc.
// buildRoutineDoc.js (master) is intentionally NOT refactored to use this yet.

import {
  Paragraph,
  TextRun,
  Table,
  TableRow,
  TableCell,
  WidthType,
  AlignmentType,
  ImageRun,
  BorderStyle,
  VerticalAlign,
  TableLayoutType,
} from 'docx';

// ---------- Shared constants ----------
export const FONT = 'Arial';
export const BODY_SIZE = 16;      // 8pt
export const SMALL_SIZE = 14;     // 7pt
export const HEADER_SIZE = 16;    // 8pt
export const TITLE_SIZE = 24;     // 12pt
export const SUBTITLE_SIZE = 20;  // 10pt
export const DAY_LABEL_SIZE = 18; // 9pt
export const CELL_PAD = 40;       // twips

export const COLOR_TITLE = '1B5E20';   // dark green
export const COLOR_HEADING = '0B3D1B';
export const COLOR_MUTED = '4A4A4A';

export const BORDER = { style: BorderStyle.SINGLE, size: 4, color: '000000' };
export const ALL_BORDERS = {
  top: BORDER,
  bottom: BORDER,
  left: BORDER,
  right: BORDER,
};
export const NO_BORDERS = {
  top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  bottom: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
};
export const BOTTOM_BORDER_SOLID = {
  top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  bottom: { style: BorderStyle.SINGLE, size: 8, color: '1A1A1A' },
  left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
};
export const BOTTOM_BORDER_DASHED = {
  top: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  bottom: { style: BorderStyle.DASHED, size: 6, color: '1A1A1A' },
  left: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
  right: { style: BorderStyle.NONE, size: 0, color: 'FFFFFF' },
};

// ---------- Shared primitives ----------
export const run = (text, opts = {}) =>
  new TextRun({
    text: text || '',
    font: FONT,
    size: opts.size || BODY_SIZE,
    bold: !!opts.bold,
    italics: !!opts.italics,
    color: opts.color,
  });

export const para = (children, opts = {}) =>
  new Paragraph({
    alignment: opts.alignment || AlignmentType.CENTER,
    spacing: {
      before: opts.before ?? 0,
      after: opts.after ?? 0,
      line: opts.line ?? 200,
    },
    borders: opts.borders,
    children: Array.isArray(children) ? children : [children],
  });

export const cell = (children, opts = {}) =>
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
      top: opts.padTop ?? CELL_PAD,
      bottom: opts.padBottom ?? CELL_PAD,
      left: opts.padLeft ?? CELL_PAD,
      right: opts.padRight ?? CELL_PAD,
    },
    borders: opts.borders || ALL_BORDERS,
    children: Array.isArray(children) ? children : [children],
  });

// ---------- Logo loader ----------
export async function loadLogo(logoUrl) {
  try {
    const res = await fetch(logoUrl);
    if (!res.ok) return null;
    return await res.arrayBuffer();
  } catch {
    return null;
  }
}

// ---------- University header (logo + 3 centered lines) ----------
export function buildUniversityHeader(logoBuffer, line1, line2, line3) {
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
    { width: 1200, borders: NO_BORDERS, verticalAlign: VerticalAlign.TOP }
  );

  const textCell = cell(
    [
      para([run(line1, { size: TITLE_SIZE, bold: true, color: COLOR_TITLE })], {
        before: 100,
      }),
      para([run(line2, { size: SUBTITLE_SIZE })], { before: 60 }),
      para([run(line3, { size: SUBTITLE_SIZE })], { before: 60, after: 100 }),
    ],
    { width: 11000, borders: NO_BORDERS, verticalAlign: VerticalAlign.TOP }
  );

  const headerTable = new Table({
    width: { size: 12200, type: WidthType.DXA },
    columnWidths: [1200, 11000],
    layout: TableLayoutType.FIXED,
    borders: NO_BORDERS,
    rows: [new TableRow({ children: [logoCell, textCell] })],
  });

  // Solid rule below the header table
  const rule = para([run('')], { borders: BOTTOM_BORDER_SOLID, after: 200 });

  return [headerTable, rule];
}
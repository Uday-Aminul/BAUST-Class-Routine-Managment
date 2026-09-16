import React from 'react';

const TIME_SLOTS_NORMAL = [
  { id: 1, label: "08.00-08.50" },
  { id: 2, label: "09.00-09.50" },
  { id: 3, label: "10.00-10.50" },
  // BREAK column will be placed here
  { id: 4, label: "11.30-12.20" },
  { id: 5, label: "12.30-01.20" },
  { id: 6, label: "01.30-02.20" },
  { id: 7, label: "2.30-3.20" },
  { id: 8, label: "3.30-4.20" },
  { id: 9, label: "4.30-5.20" }
];

const TIME_SLOTS_RAMADAN = [
  { id: 1, label: "09.00-09.45" },
  { id: 2, label: "09.50-10.35" },
  { id: 3, label: "10.40-11.25" },
  { id: 4, label: "11.30-12.15" },
  { id: 5, label: "12.20-01.05" },
  { id: 6, label: "01.40-02.25" },
  { id: 7, label: "2.30-3.15" },
  { id: 8, label: "3.20-4.05" },
  { id: 9, label: "4.10-4.55" }
];

const DAYS_OF_WEEK = [
  { value: 0, label: "SUN" },
  { value: 1, label: "MON" },
  { value: 2, label: "TUE" },
  { value: 3, label: "WED" },
  { value: 4, label: "THU" }
];

// Helper to determine slot index (0 to 8) from backend start time
const getSlotIndex = (startTime) => {
  if (!startTime) return -1;
  const parts = startTime.split(':');
  const hour = parseInt(parts[0], 10);
  const minute = parseInt(parts[1], 10);

  if (hour === 8) return 0;
  if (hour === 9) return 1;
  if (hour === 10) return 2;
  if (hour === 11) return 3;
  if (hour === 12) return 4;
  if (hour === 13) return 5;
  if (hour === 14) return 6;
  if (hour === 15) return 7;
  if (hour === 16) return 8;

  return -1;
};

// Helper to determine slot span from start/end times
// Labs run ~3 hours (e.g. 08:00–10:50), theory runs ~50 min (e.g. 11:30–12:20)
const getSlotSpan = (startTime, endTime) => {
  if (!startTime || !endTime) return 1;
  const startParts = startTime.split(':');
  const endParts = endTime.split(':');
  const startHour = parseInt(startParts[0], 10); // e.g. 8
  const endHour   = parseInt(endParts[0],   10); // e.g. 10  ← FIXED (was endParts[1] which is minutes!)

  const diffHours = endHour - startHour;
  if (diffHours >= 2) return 3; // Labs span 3 slots
  return 1;
};

const RoutineTable = ({ schedules = [], mode = 'normal' }) => {
  const isNormalMode = mode === 'normal';
  const slots = isNormalMode ? TIME_SLOTS_NORMAL : TIME_SLOTS_RAMADAN;

  // Build a grid: dayIndex -> array of 9 slots
  const grid = Array(5).fill(null).map(() => Array(9).fill(null));

  // First pass: Group schedules by day and start slot to handle overlaps (ODD/EVEN weeks)
  const groupedSchedules = {};

  schedules.forEach(schedule => {
    let dayIndex = -1;
    if (typeof schedule.day === 'number') {
      dayIndex = schedule.day;
    } else if (typeof schedule.day === 'string') {
      const dayStr = schedule.day.toLowerCase();
      if (dayStr.includes('sun')) dayIndex = 0;
      else if (dayStr.includes('mon')) dayIndex = 1;
      else if (dayStr.includes('tue')) dayIndex = 2;
      else if (dayStr.includes('wed')) dayIndex = 3;
      else if (dayStr.includes('thu')) dayIndex = 4;
    }

    if (dayIndex < 0 || dayIndex > 4) return;

    const startSlot = getSlotIndex(schedule.startTime);
    if (startSlot < 0 || startSlot > 8) return;

    const key = `${dayIndex}-${startSlot}`;
    if (!groupedSchedules[key]) {
      groupedSchedules[key] = [];
    }
    groupedSchedules[key].push(schedule);
  });

  // Second pass: Populate grid with merged/spanned cells
  Object.keys(groupedSchedules).forEach(key => {
    const [dayIndex, startSlot] = key.split('-').map(Number);
    const list = groupedSchedules[key];
    const span = getSlotSpan(list[0].startTime, list[0].endTime);

    grid[dayIndex][startSlot] = {
      isMerged: list.length > 1,
      schedulesList: list,
      span: span,
      ...list[0]
    };

    for (let i = 1; i < span; i++) {
      if (startSlot + i < 9) {
        grid[dayIndex][startSlot + i] = 'covered';
      }
    }
  });

  const renderCellContent = (cell) => {
    if (cell.isMerged) {
      return cell.schedulesList.map((sched, idx) => {
        const courseCode = sched.course?.courseCode || sched.sessional?.sessionalCode || '';
        const teachers = sched.teachers?.map(t => t.code).join(', ') || '';
        const room = sched.classroom?.roomNumber || sched.labroom?.roomNumber || '';
        const weekType = sched.weekType ? `#${sched.weekType}#` : '';

        return (
          <span key={idx}>
            {idx > 0 && '/'}
            <span className="class-info">
              {courseCode} {teachers && `(${teachers})`} {weekType}
            </span>
            {room && <span className="class-room" style={{ display: 'inline' }}> [{room}]</span>}
          </span>
        );
      });
    } else {
      const courseCode = cell.course?.courseCode || cell.sessional?.sessionalCode || '';
      const teachers = cell.teachers?.map(t => t.code).join(', ') || '';
      const room = cell.classroom?.roomNumber || cell.labroom?.roomNumber || '';
      const weekType = cell.weekType ? `#${cell.weekType}#` : '';

      return (
        <>
          <div className="class-info">
            {courseCode} {teachers && `(${teachers})`} {weekType}
          </div>
          {room && <span className="class-room">[{room}]</span>}
        </>
      );
    }
  };

  return (
    <div style={{ overflowX: 'auto', marginBottom: '2rem' }}>
      <table className="routine-grid-table">
        <thead>
          <tr>
            <th style={{ width: '60px' }}>Day</th>

            {/* Morning slots 1-3 */}
            {slots.slice(0, 3).map(slot => (
              <th key={slot.id}>{slot.label}</th>
            ))}

            {/* Break column header (Normal Mode only) */}
            {isNormalMode && <th style={{ width: '30px', padding: '0.2rem', fontSize: '0.75rem' }}></th>}

            {/* Afternoon slots 4-9 */}
            {slots.slice(3).map(slot => (
              <th key={slot.id}>{slot.label}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {DAYS_OF_WEEK.map((day, dayIdx) => {
            const rowSlots = grid[dayIdx];

            // Build morning cells (slots 0-2), clamp colSpan to never cross the break column
            const morningCells = [];
            for (let si = 0; si < 3; si++) {
              const cell = rowSlots[si];
              if (cell === 'covered') continue;
              if (!cell) {
                morningCells.push(<td key={si} className="empty-cell"></td>);
              } else {
                const isLab = cell.isMerged ? cell.schedulesList.some(s => s.sessional) : !!cell.sessional;
                // Clamp so a lab colSpan never spills past morning boundary into break column
                const maxMorningSpan = 3 - si;
                const span = Math.min(cell.span || 1, maxMorningSpan);
                morningCells.push(
                  <td key={si} colSpan={span}
                    style={{ backgroundColor: isLab ? '#f9f9f9' : '#ffffff', fontWeight: '500' }}
                  >
                    {renderCellContent(cell)}
                  </td>
                );
              }
            }

            // Build afternoon cells (slots 3-8)
            const afternoonCells = [];
            for (let si = 3; si < 9; si++) {
              const cell = rowSlots[si];
              if (cell === 'covered') continue;
              if (!cell) {
                afternoonCells.push(<td key={si} className="empty-cell"></td>);
              } else {
                const isLab = cell.isMerged ? cell.schedulesList.some(s => s.sessional) : !!cell.sessional;
                afternoonCells.push(
                  <td key={si} colSpan={cell.span || 1}
                    style={{ backgroundColor: isLab ? '#f9f9f9' : '#ffffff', fontWeight: '500' }}
                  >
                    {renderCellContent(cell)}
                  </td>
                );
              }
            }

            return (
              <tr key={day.value}>
                <td className="day-cell">{day.label}</td>

                {morningCells}

                {/* BREAK column: rendered once on first row with rowSpan=5 to keep rows slim */}
                {isNormalMode && dayIdx === 0 && (
                  <td
                    rowSpan={5}
                    style={{
                      writingMode: 'vertical-rl',
                      textTransform: 'uppercase',
                      letterSpacing: '1px',
                      backgroundColor: '#f2f2f2',
                      fontWeight: 'bold',
                      fontSize: '0.75rem',
                      textAlign: 'center',
                      verticalAlign: 'middle',
                      width: '30px',
                      color: '#4a4a4a',
                      borderLeft: '1px solid #1a1a1a',
                      borderRight: '1px solid #1a1a1a'
                    }}
                  >
                    BREAK (10.50- 11.30)
                  </td>
                )}

                {afternoonCells}
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default RoutineTable;
export { TIME_SLOTS_NORMAL, TIME_SLOTS_RAMADAN };

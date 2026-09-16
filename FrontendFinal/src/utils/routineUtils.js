// src/utils/routineUtils.js
//
// Shared logic between the on-screen view and the DOCX export.
// Nothing here is allowed to be duplicated in the component.

export const TIME_SLOTS_NORMAL = [
  { id: 1, label: '08.00-08.50' },
  { id: 2, label: '09.00-09.50' },
  { id: 3, label: '10.00-10.50' },
  { id: 4, label: '11.30-12.20' },
  { id: 5, label: '12.30-01.20' },
  { id: 6, label: '01.30-02.20' },
  { id: 7, label: '2.30-3.20' },
  { id: 8, label: '3.30-4.20' },
  { id: 9, label: '4.30-5.20' },
];

export const TIME_SLOTS_RAMADAN = [
  { id: 1, label: '09.00-09.45' },
  { id: 2, label: '09.50-10.35' },
  { id: 3, label: '10.40-11.25' },
  { id: 4, label: '11.30-12.15' },
  { id: 5, label: '12.20-01.05' },
  { id: 6, label: '01.40-02.25' },
  { id: 7, label: '2.30-3.15' },
  { id: 8, label: '3.20-4.05' },
  { id: 9, label: '4.10-4.55' },
];

export const DAYS_OF_WEEK = [
  { value: 0, label: 'Sunday' },
  { value: 1, label: 'Monday' },
  { value: 2, label: 'Tuesday' },
  { value: 3, label: 'Wednesday' },
  { value: 4, label: 'Thursday' },
];

const hourToSlot = (hour) => {
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

export const getSlotIndex = (startTime) => {
  if (!startTime) return -1;
  const hour = parseInt(startTime.split(':')[0], 10);
  return hourToSlot(hour);
};

export const getSlotSpan = (startTime, endTime) => {
  if (!startTime || !endTime) return 1;
  const startHour = parseInt(startTime.split(':')[0], 10);
  const endHour = parseInt(endTime.split(':')[0], 10);
  const diff = endHour - startHour;
  if (diff >= 2) return 3;
  if (diff === 1) return 1;
  return 1;
};

export const termRoman = (termVal) => {
  if (termVal === 1) return 'I';
  if (termVal === 2) return 'II';
  return String(termVal);
};

export const sortSections = (sections) => {
  return [...sections].sort((a, b) => {
    if (a.level !== b.level) return a.level - b.level;
    if (a.term !== b.term) return a.term - b.term;
    return a.section.localeCompare(b.section);
  });
};

/**
 * Returns a map keyed by "dayIndex_sectionId_startSlot" with arrays of schedules.
 */
export const groupSchedules = (schedules, sections) => {
  const grouped = {};
  schedules.forEach((schedule) => {
    let dayIndex = -1;
    if (typeof schedule.day === 'number') {
      dayIndex = schedule.day;
    } else if (typeof schedule.day === 'string') {
      const d = schedule.day.toLowerCase();
      if (d.includes('sun')) dayIndex = 0;
      else if (d.includes('mon')) dayIndex = 1;
      else if (d.includes('tue')) dayIndex = 2;
      else if (d.includes('wed')) dayIndex = 3;
      else if (d.includes('thu')) dayIndex = 4;
    }
    if (dayIndex < 0 || dayIndex > 4) return;

    const startSlot = getSlotIndex(schedule.startTime);
    if (startSlot < 0 || startSlot > 8) return;

    const matchingSection = sections.find(
      (s) =>
        s.level === schedule.level &&
        s.term === schedule.term &&
        s.section === schedule.section
    );
    if (!matchingSection) return;

    const key = `${dayIndex}_${matchingSection.id}_${startSlot}`;
    if (!grouped[key]) grouped[key] = [];
    grouped[key].push(schedule);
  });
  return grouped;
};

/**
 * dayGrid: sectionId -> array of 9 cells. Each cell is:
 *   - null          (empty)
 *   - 'covered'     (skipped because a previous cell spans here)
 *   - { isMerged, schedulesList, span }
 */
export const getDayGrid = (dayIndex, sortedSections, groupedSchedules) => {
  const dayGrid = {};
  const covered = {};

  sortedSections.forEach((section) => {
    dayGrid[section.id] = Array(9).fill(null);

    for (let slotIdx = 0; slotIdx < 9; slotIdx++) {
      if (covered[`${section.id}_${slotIdx}`]) {
        dayGrid[section.id][slotIdx] = 'covered';
        continue;
      }

      const key = `${dayIndex}_${section.id}_${slotIdx}`;
      const list = groupedSchedules[key];
      if (list && list.length > 0) {
        const span = getSlotSpan(list[0].startTime, list[0].endTime);
        dayGrid[section.id][slotIdx] = {
          isMerged: list.length > 1,
          schedulesList: list,
          span,
        };
        for (let s = 1; s < span; s++) {
          if (slotIdx + s < 9) covered[`${section.id}_${slotIdx + s}`] = true;
        }
      }
    }
  });

  return dayGrid;
};

export const getScheduleDisplay = (sched) => {
  const courseCode =
    sched.course?.courseCode || sched.sessional?.sessionalCode || '';
  const teachers = sched.teachers?.map((t) => t.code).join(', ') || '';
  const room = sched.classroom?.roomNumber || sched.labroom?.roomNumber || '';
  const weekType = sched.weekType ? `#${sched.weekType}#` : '';
  return { courseCode, teachers, room, weekType };
};

export const getSectionLabel = (sec) =>
  `${sec.level}/${termRoman(sec.term)} - ${sec.section}`;
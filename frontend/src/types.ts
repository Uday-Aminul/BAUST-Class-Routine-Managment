// ─── Shared Sub-DTOs ───────────────────────────────────────────────

export interface ClassroomForScheduleDto {
  id: number;
  roomNumber: number;
}

export interface LabroomForScheduleDto {
  id: number;
  roomNumber: number;
  name: string;
}

export interface CourseForScheduleDto {
  id: number;
  name: string;
  courseCode?: string;
  level: number;
  term: number;
  credit: number;
}

export interface SessionalForScheduleDto {
  id: number;
  name: string;
  courseCode?: string;
  level: number;
  term: number;
  credit: number;
}

export interface TeacherForScheduleDto {
  id: number;
  name: string;
  code?: string;
  designation: string;
  assignedCredit: number;
}

// ─── ClassSchedule DTO (from GET /ClassSchedule) ──────────────────

export interface ClassScheduleDto {
  id: number;
  day: number; // DayOfWeek enum: 0=Sun,1=Mon,...,6=Sat
  startTime: string; // "HH:mm:ss"
  endTime: string;
  weekType?: string; // null | "ODD" | "EVEN"
  classroom?: ClassroomForScheduleDto;
  labroom?: LabroomForScheduleDto;
  course?: CourseForScheduleDto;
  sessional?: SessionalForScheduleDto;
  teachers: TeacherForScheduleDto[];
}

// ─── Teacher DTO (from GET /api/Teachers/{id}) ────────────────────

export interface ClassScheduleDtoForTeacher {
  id: number;
  day: number;
  startTime: string;
  endTime: string;
  weekType?: string;
  classroom?: ClassroomForScheduleDto;
  labroom?: LabroomForScheduleDto;
  course?: CourseForScheduleDto;
  sessional?: SessionalForScheduleDto;
}

export interface TeacherDto {
  id: number;
  name: string;
  code?: string;
  designation: string;
  classes?: ClassScheduleDtoForTeacher[];
}

// ─── Admin/CRUD DTOs ──────────────────────────────────────────────

export interface CourseAdminDto {
  id: number;
  name: string;
  courseCode?: string;
  level: number;
  term: number;
  credit: number;
  currentTeacherId?: number;
  currentTeacherCode?: string;
}

export interface SessionalAdminDto {
  id: number;
  name: string;
  sessionalCode?: string;
  level: number;
  term: number;
  credit: number;
  currentTeacherCodes?: string[];
}

export interface AddTeacherDto {
  name: string;
  code?: string;
  designation: string;
  assignedCourses?: number[];
  assignedSessionals?: number[];
}

export interface UpdateTeacherDto {
  name: string;
  code?: string;
  designation: string;
  courses?: number[];
  assignedSessionals?: number[];
}

// ─── UI Helpers ───────────────────────────────────────────────────

export type SearchMode = 'section' | 'teacher' | 'room' | 'admin';

export const DAYS = [
  { label: 'Sunday',    value: 0 },
  { label: 'Monday',    value: 1 },
  { label: 'Tuesday',   value: 2 },
  { label: 'Wednesday', value: 3 },
  { label: 'Thursday',  value: 4 },
];

// Standard BAUST time slots (9 slots, 50 mins each, Break at 10:50 - 11:30)
export const TIME_SLOTS = [
  { label: '08:00 – 08:50', start: '08:00:00', end: '08:50:00' },
  { label: '09:00 – 09:50', start: '09:00:00', end: '09:50:00' },
  { label: '10:00 – 10:50', start: '10:00:00', end: '10:50:00' },
  { label: '11:30 – 12:20', start: '11:30:00', end: '12:20:00' },
  { label: '12:30 – 01:20', start: '12:30:00', end: '13:20:00' },
  { label: '01:30 – 02:20', start: '13:30:00', end: '14:20:00' },
  { label: '02:30 – 03:20', start: '14:30:00', end: '15:20:00' },
  { label: '03:30 – 04:20', start: '15:30:00', end: '16:20:00' },
  { label: '04:30 – 05:20', start: '16:30:00', end: '17:20:00' },
];

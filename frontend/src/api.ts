import axios from 'axios';
import type { ClassScheduleDto, TeacherDto, CourseAdminDto, SessionalAdminDto, AddTeacherDto, UpdateTeacherDto } from './types';

const BASE_URL = 'http://localhost:5299';

const api = axios.create({ baseURL: BASE_URL });

export async function fetchSectionSchedule(
  level: number, term: number, section: string
): Promise<ClassScheduleDto[]> {
  const { data } = await api.get<ClassScheduleDto[]>('/ClassSchedule', {
    params: { level, term, section },
  });
  return data;
}

export async function fetchAllSchedules(): Promise<ClassScheduleDto[]> {
  const { data } = await api.get<ClassScheduleDto[]>('/ClassSchedule');
  return data;
}

export async function fetchTeacherById(id: number): Promise<TeacherDto> {
  const { data } = await api.get<TeacherDto>(`/api/Teachers/${id}`);
  return data;
}

export async function fetchAllTeachers(): Promise<TeacherDto[]> {
  const { data } = await api.get<TeacherDto[]>('/api/Teachers');
  return data;
}

// ─── Admin/CRUD ───────────────────────────────────────────────────

export async function createTeacher(dto: AddTeacherDto): Promise<TeacherDto> {
  const { data } = await api.post<TeacherDto>('/api/Teachers', dto);
  return data;
}

export async function updateTeacher(id: number, dto: UpdateTeacherDto): Promise<TeacherDto> {
  const { data } = await api.put<TeacherDto>(`/api/Teachers/${id}`, dto);
  return data;
}

export async function deleteTeacher(id: number): Promise<TeacherDto[]> {
  const { data } = await api.delete<TeacherDto[]>(`/api/Teachers/${id}`);
  return data;
}

export async function fetchAllCourses(): Promise<CourseAdminDto[]> {
  const { data } = await api.get<CourseAdminDto[]>('/api/Teachers/Courses');
  return data;
}

export async function fetchAllSessionals(): Promise<SessionalAdminDto[]> {
  const { data } = await api.get<SessionalAdminDto[]>('/api/Teachers/Sessionals');
  return data;
}

/** Trigger full schedule regeneration — returns array of result messages */
export async function generateAllSchedules(): Promise<string[]> {
  const { data } = await api.post<string[]>(
    '/ClassSchedule/GenerateClassSchedulesForAll',
    null,
    { params: { act: true } }
  );
  return data;
}

/** Download all routines as a single .xlsx file */
export async function downloadAllRoutinesXlsx(): Promise<void> {
  const response = await api.get('/ClassSchedule/Download/xlsx', {
    responseType: 'blob',
  });
  triggerDownload(response.data, 'AllRoutines.xlsx',
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
}

/** Download master routine as a single .docx file */
export async function downloadMasterRoutineDocx(): Promise<void> {
  const response = await api.get('/ClassSchedule/Download/master-docx', {
    responseType: 'blob',
  });
  triggerDownload(response.data, 'MasterRoutine.docx',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document');
}

/** Download a single routine as .docx */
export async function downloadRoutineDocx(
  level: number, term: number, section: string
): Promise<void> {
  const response = await api.get('/ClassSchedule/Download/docx', {
    params: { level, term, section },
    responseType: 'blob',
  });
  triggerDownload(response.data,
    `Routine_L${level}T${term}${section}.docx`,
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document');
}

function triggerDownload(blob: Blob, filename: string, mimeType: string) {
  const url = URL.createObjectURL(new Blob([blob], { type: mimeType }));
  const a = document.createElement('a');
  a.href = url;
  a.download = filename;
  a.click();
  URL.revokeObjectURL(url);
}

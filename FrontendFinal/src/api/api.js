import axios from 'axios';

const BASE_URL = 'http://localhost:5299';

const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const routineApi = {
  getAll: (level, term, section) => {
    const params = {};
    if (level) params.level = level;
    if (term) params.term = term;
    if (section) params.section = section;
    return api.get('/ClassSchedule', { params });
  },
  getByDay: (level, term, section, day) => {
    return api.get('/ClassSchedule/ByDay', { params: { level, term, section, day } });
  },
  getById: (id) => api.get(`/ClassSchedule/${id}`),
  create: (data) => api.post('/ClassSchedule', data),
  update: (id, data) => api.put(`/ClassSchedule/${id}`, data),
  delete: (id) => api.delete(`/ClassSchedule/${id}`),
  generateAll: () => api.post('/ClassSchedule/GenerateClassSchedulesForAll?act=true'),
};

export const teacherApi = {
  getAll: () => api.get('/api/Teachers'),
  getById: (id) => api.get(`/api/Teachers/${id}`),
  create: (data) => api.post('/api/Teachers', data),
  update: (id, data) => api.put(`/api/Teachers/${id}`, data),
  delete: (id) => api.delete(`/api/Teachers/${id}`),
};

export const courseApi = {
  getAll: () => api.get('/api/Courses'),
  getById: (id) => api.get(`/api/Courses/${id}`),
  create: (data) => api.post('/api/Courses', data),
  update: (id, data) => api.put(`/api/Courses/${id}`, data),
  delete: (id) => api.delete(`/api/Courses/${id}`),
};

export const sessionalApi = {
  getAll: () => api.get('/api/Sessionals'),
  getById: (id) => api.get(`/api/Sessionals/${id}`),
  create: (data) => api.post('/api/Sessionals', data),
  update: (id, data) => api.put(`/api/Sessionals/${id}`, data),
  delete: (id) => api.delete(`/api/Sessionals/${id}`),
};

export const classroomApi = {
  getAll: () => api.get('/api/Classroom'),
  getByRoomNumber: (roomNumber) => api.get(`/api/Classroom/${roomNumber}`),
  create: (data) => api.post('/api/Classroom', data),
  update: (id, data) => api.put(`/api/Classroom/${id}`, data),
  delete: (id) => api.delete(`/api/Classroom/${id}`),
};

export const labroomApi = {
  getAll: () => api.get('/api/Labrooms'),
  getByRoomNumber: (roomNumber) => api.get(`/api/Labrooms/${roomNumber}`),
  create: (data) => api.post('/api/Labrooms', data),
  update: (id, data) => api.put(`/api/Labrooms/${id}`, data),
  delete: (id) => api.delete(`/api/Labrooms/${id}`),
  assignSessionals: () => api.post('/api/Labrooms/AssignSessionals'),
};

export const sectionApi = {
  getAll: () => api.get('/LevelTermSections'),
  getById: (level, term, section) => api.get('/LevelTermSections/GetByRoomNumber', { params: { level, term, section } }), // wait, the controller has Route("{id:int}") but parameters are FromQuery
  create: (data) => api.post('/LevelTermSections', data),
  update: (id, data) => api.put(`/LevelTermSections/${id}`, data),
  delete: (id) => api.delete(`/LevelTermSections/${id}`),
  assignClassrooms: () => api.post('/LevelTermSections/AssignClassrooms'),
  assignTeachers: () => api.post('/LevelTermSections/AssignTeachers'),
};

export const teacherAssignmentApi = {
  getAll: () => api.get('/api/TeacherAssignments'),
  getById: (id) => api.get(`/api/TeacherAssignments/${id}`),
  create: (data) => api.post('/api/TeacherAssignments', data),
  update: (id, data) => api.put(`/api/TeacherAssignments/${id}`, data),
  delete: (id) => api.delete(`/api/TeacherAssignments/${id}`),
};

export default api;

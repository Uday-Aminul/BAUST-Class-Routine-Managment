import React, { useState, useEffect } from 'react';
import { fetchAllTeachers, createTeacher, updateTeacher, deleteTeacher, fetchAllCourses, fetchAllSessionals } from '../api';
import { SearchableSelect } from './SearchableSelect';

export default function TeacherAdmin() {
  const [teachers, setTeachers] = useState<TeacherDto[]>([]);
  const [courses, setCourses] = useState<CourseAdminDto[]>([]);
  const [sessionals, setSessionals] = useState<SessionalAdminDto[]>([]);
  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Form State
  const [editingTeacher, setEditingTeacher] = useState<TeacherDto | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formName, setFormName] = useState('');
  const [formCode, setFormCode] = useState('');
  const [formDesignation, setFormDesignation] = useState('Lecturer');
  const [selectedCourses, setSelectedCourses] = useState<number[]>([]);
  const [selectedSessionals, setSelectedSessionals] = useState<number[]>([]);
  const [theoryCount, setTheoryCount] = useState(0);
  const [sessionalCount, setSessionalCount] = useState(0);

  function handleTheoryCountChange(e: React.ChangeEvent<HTMLSelectElement>) {
    const val = parseInt(e.target.value);
    setTheoryCount(val);
    setSelectedCourses(prev => {
      const newArr = [...prev];
      if (newArr.length > val) return newArr.slice(0, val);
      while (newArr.length < val) newArr.push(0);
      return newArr;
    });
  }

  function handleSessionalCountChange(e: React.ChangeEvent<HTMLSelectElement>) {
    const val = parseInt(e.target.value);
    setSessionalCount(val);
    setSelectedSessionals(prev => {
      const newArr = [...prev];
      if (newArr.length > val) return newArr.slice(0, val);
      while (newArr.length < val) newArr.push(0);
      return newArr;
    });
  }

  function handleCourseSelect(index: number, val: number) {
    const newArr = [...selectedCourses];
    newArr[index] = val;
    setSelectedCourses(newArr);
  }

  function handleSessionalSelect(index: number, val: number) {
    const newArr = [...selectedSessionals];
    newArr[index] = val;
    setSelectedSessionals(newArr);
  }
  
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  async function loadData() {
    setLoading(true);
    setError(null);
    try {
      const [tData, cData, sData] = await Promise.all([
        fetchAllTeachers(),
        fetchAllCourses(),
        fetchAllSessionals()
      ]);
      setTeachers(tData);
      setCourses(cData);
      setSessionals(sData);
    } catch (err) {
      setError('Failed to load data. Make sure the API is running.');
    } finally {
      setLoading(false);
    }
  }

  function openCreateModal() {
    setEditingTeacher(null);
    setFormName('');
    setFormCode('');
    setFormDesignation('Lecturer');
    setTheoryCount(0);
    setSessionalCount(0);
    setSelectedCourses([]);
    setSelectedSessionals([]);
    setIsModalOpen(true);
  }

  function openEditModal(teacher: TeacherDto) {
    setEditingTeacher(teacher);
    setFormName(teacher.name);
    setFormCode(teacher.code || '');
    setFormDesignation(teacher.designation);
    
    // In our backend, teacher.classes has the schedules, but not the direct IDs in a simple list.
    // However, we can extract what they teach from the course/sessional list we fetched.
    const myCourseIds = courses.filter(c => c.currentTeacherId === teacher.id).map(c => c.id);
    const mySessionalIds = sessionals.filter(s => s.currentTeacherCodes?.includes(teacher.code!)).map(s => s.id);
    
    setTheoryCount(myCourseIds.length);
    setSessionalCount(mySessionalIds.length);
    setSelectedCourses(myCourseIds);
    setSelectedSessionals(mySessionalIds);
    setIsModalOpen(true);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!formName.trim()) return;

    setSaving(true);
    try {
      const validCourses = selectedCourses.filter(id => id > 0);
      const validSessionals = selectedSessionals.filter(id => id > 0);

      if (editingTeacher) {
        const dto: UpdateTeacherDto = {
          name: formName,
          code: formCode || undefined,
          designation: formDesignation,
          courses: validCourses,
          assignedSessionals: validSessionals
        };
        await updateTeacher(editingTeacher.id, dto);
      } else {
        const dto: AddTeacherDto = {
          name: formName,
          code: formCode || undefined,
          designation: formDesignation,
          assignedCourses: validCourses,
          assignedSessionals: validSessionals
        };
        await createTeacher(dto);
      }
      setIsModalOpen(false);
      await loadData(); // Reload everything to get updated relationships
    } catch (err) {
      alert('Error saving teacher.');
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(id: number) {
    if (!window.confirm('Are you sure you want to delete this teacher?')) return;
    try {
      await deleteTeacher(id);
      await loadData();
    } catch (err) {
      alert('Error deleting teacher.');
    }
  }

  // Assignment toggles not used anymore

  return (
    <div className="search-view">
      <div className="admin-header">
        <div>
          <h2>Teacher Management</h2>
          <p className="sg-subtitle">Add/Edit teachers and assign their courses.</p>
        </div>
        <button onClick={openCreateModal} className="btn-search" style={{ minWidth: 'auto' }}>
          + Add Teacher
        </button>
      </div>

      {error && (
        <div className="error-banner animate-fade-in">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>{error}
        </div>
      )}

      {loading ? (
        <div className="empty-state"><span className="spinner" /></div>
      ) : (
        <div className="admin-list-card glass-card animate-fade-in">
          <table className="sg-table admin-table">
            <thead>
              <tr>
                <th className="sg-th" style={{ textAlign: 'left', paddingLeft: '20px' }}>Name</th>
                <th className="sg-th">Code</th>
                <th className="sg-th">Designation</th>
                <th className="sg-th">Assigned</th>
                <th className="sg-th">Actions</th>
              </tr>
            </thead>
            <tbody>
              {teachers.length === 0 ? (
                <tr><td colSpan={5} className="sg-td-time" style={{ textAlign: 'center', padding: '20px' }}>No teachers found.</td></tr>
              ) : (
                teachers.map(t => {
                  const tCourses = courses.filter(c => c.currentTeacherId === t.id).length;
                  const tSessionals = sessionals.filter(s => s.currentTeacherCodes?.includes(t.code!)).length;
                  
                  return (
                    <tr key={t.id} className="admin-tr">
                      <td className="sg-td sg-td-time" style={{ paddingLeft: '20px', color: 'var(--text-primary)' }}>
                        <strong>{t.name}</strong>
                      </td>
                      <td className="sg-td" style={{ textAlign: 'center' }}>
                        {t.code && <span className="tag tag--green">{t.code}</span>}
                      </td>
                      <td className="sg-td" style={{ textAlign: 'center' }}>
                        <span className="tag tag--gray">{t.designation}</span>
                      </td>
                      <td className="sg-td" style={{ textAlign: 'center' }}>
                        <span className="tag tag--gold">{tCourses} theory, {tSessionals} lab</span>
                      </td>
                      <td className="sg-td" style={{ textAlign: 'center' }}>
                        <div className="admin-actions">
                          <button onClick={() => openEditModal(t)} className="btn-icon">✏️</button>
                          <button onClick={() => handleDelete(t.id)} className="btn-icon">🗑️</button>
                        </div>
                      </td>
                    </tr>
                  )
                })
              )}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal Overlay */}
      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content glass-card animate-fade-in">
            <h3 style={{ marginBottom: '16px' }}>{editingTeacher ? 'Edit Teacher' : 'Add Teacher'}</h3>
            <form onSubmit={handleSubmit} className="admin-form">
              
              <div className="form-row">
                <div className="form-group form-group--grow">
                  <label>Name</label>
                  <input required className="text-input" value={formName} onChange={e => setFormName(e.target.value)} />
                </div>
                <div className="form-group" style={{ width: '100px' }}>
                  <label>Code</label>
                  <input className="text-input" value={formCode} onChange={e => setFormCode(e.target.value)} placeholder="e.g. MAS" />
                </div>
              </div>

              <div className="form-group" style={{ marginBottom: '16px' }}>
                <label>Designation</label>
                <div className="select-wrapper">
                  <select value={formDesignation} onChange={e => setFormDesignation(e.target.value)}>
                    <option>Professor</option>
                    <option>Associate Professor</option>
                    <option>Assistant Professor</option>
                    <option>Lecturer</option>
                  </select>
                </div>
              </div>

              {/* Assignment Selectors */}
              <div className="assignment-grid" style={{ display: 'flex', gap: '24px', marginBottom: '24px' }}>
                <div className="assignment-col" style={{ flex: 1 }}>
                  <div className="form-group" style={{ marginBottom: '16px' }}>
                    <label>Number of Theory Courses</label>
                    <div className="select-wrapper">
                      <select value={theoryCount} onChange={handleTheoryCountChange}>
                        {[0,1,2,3,4,5,6,7,8,9,10].map(n => <option key={n} value={n}>{n}</option>)}
                      </select>
                    </div>
                  </div>
                  <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
                    {Array.from({ length: theoryCount }).map((_, i) => (
                      <SearchableSelect
                        key={`course-${i}`}
                        value={selectedCourses[i] || 0}
                        onChange={val => handleCourseSelect(i, val)}
                        placeholder="-- Select Course --"
                        options={courses.map(c => ({
                          id: c.id,
                          label: `${c.courseCode} - ${c.name} (${c.credit}cr) - L${c.level}T${c.term}`
                        }))}
                      />
                    ))}
                  </div>
                </div>

                <div className="assignment-col" style={{ flex: 1 }}>
                  <div className="form-group" style={{ marginBottom: '16px' }}>
                    <label>Number of Sessionals</label>
                    <div className="select-wrapper">
                      <select value={sessionalCount} onChange={handleSessionalCountChange}>
                        {[0,1,2,3,4,5,6,7,8,9,10].map(n => <option key={n} value={n}>{n}</option>)}
                      </select>
                    </div>
                  </div>
                  <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
                    {Array.from({ length: sessionalCount }).map((_, i) => (
                      <SearchableSelect
                        key={`sessional-${i}`}
                        value={selectedSessionals[i] || 0}
                        onChange={val => handleSessionalSelect(i, val)}
                        placeholder="-- Select Sessional --"
                        options={sessionals.map(s => ({
                          id: s.id,
                          label: `${s.sessionalCode} - ${s.name} (${s.credit}cr) - L${s.level}T${s.term}`
                        }))}
                      />
                    ))}
                  </div>
                </div>
              </div>

              <div className="modal-actions">
                <button type="button" className="btn-secondary" onClick={() => setIsModalOpen(false)}>Cancel</button>
                <button type="submit" className="btn-search" disabled={saving}>
                  {saving ? 'Saving...' : 'Save Teacher'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

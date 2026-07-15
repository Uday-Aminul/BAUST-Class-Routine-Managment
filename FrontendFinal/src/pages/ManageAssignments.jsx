import React, { useState, useEffect } from 'react';
import { UserCheck, Plus, Edit, Trash2, X, Check } from 'lucide-react';
import { teacherAssignmentApi, sectionApi, courseApi, sessionalApi, teacherApi } from '../api/api';

const ManageAssignments = () => {
  const [assignments, setAssignments] = useState([]);
  const [sections, setSections] = useState([]);
  const [courses, setCourses] = useState([]);
  const [sessionals, setSessionals] = useState([]);
  const [teachers, setTeachers] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Form states
  const [isEditing, setIsEditing] = useState(false);
  const [currentId, setCurrentId] = useState(null);

  // Fields
  const [levelTermSectionId, setLevelTermSectionId] = useState('');
  const [assignmentType, setAssignmentType] = useState('course'); // 'course' or 'sessional'
  const [courseId, setCourseId] = useState('');
  const [sessionalId, setSessionalId] = useState('');
  const [selectedTeacherIds, setSelectedTeacherIds] = useState([]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [assignmentRes, sectionRes, courseRes, sessionalRes, teacherRes] = await Promise.all([
        teacherAssignmentApi.getAll(),
        sectionApi.getAll(),
        courseApi.getAll(),
        sessionalApi.getAll(),
        teacherApi.getAll()
      ]);
      setAssignments(assignmentRes.data);
      setSections(sectionRes.data);
      setCourses(courseRes.data);
      setSessionals(sessionalRes.data);
      setTeachers(teacherRes.data);

      if (sectionRes.data.length > 0) {
        setLevelTermSectionId(sectionRes.data[0].id.toString());
      }
    } catch (err) {
      console.error(err);
      setError('Could not fetch teacher assignments list.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const resetForm = () => {
    setCourseId('');
    setSessionalId('');
    setSelectedTeacherIds([]);
    setIsEditing(false);
    setCurrentId(null);
  };

  const showSuccess = (msg) => {
    setSuccess(msg);
    setTimeout(() => setSuccess(''), 3000);
  };

  const handleTeacherCheckboxChange = (teacherId) => {
    setSelectedTeacherIds(prev =>
      prev.includes(teacherId) ? prev.filter(id => id !== teacherId) : [...prev, teacherId]
    );
  };

  // Get active section info to filter courses & sessionals matching the section level-term
  const activeSection = sections.find(s => s.id === parseInt(levelTermSectionId, 10));

  const filteredCourses = activeSection 
    ? courses.filter(c => c.level === activeSection.level && c.term === activeSection.term)
    : [];

  const filteredSessionals = activeSection
    ? sessionals.filter(s => s.level === activeSection.level && s.term === activeSection.term)
    : [];

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!levelTermSectionId) {
      setError('Level-Term Section selection is required.');
      return;
    }
    if (assignmentType === 'course' && !courseId) {
      setError('Course selection is required.');
      return;
    }
    if (assignmentType === 'sessional' && !sessionalId) {
      setError('Sessional selection is required.');
      return;
    }
    if (selectedTeacherIds.length === 0) {
      setError('At least one teacher must be selected.');
      return;
    }

    setLoading(true);
    setError('');

    const payload = {
      levelTermSectionId: parseInt(levelTermSectionId, 10),
      courseId: assignmentType === 'course' ? parseInt(courseId, 10) : null,
      sessionalId: assignmentType === 'sessional' ? parseInt(sessionalId, 10) : null,
      teacherIds: selectedTeacherIds
    };

    try {
      if (isEditing) {
        await teacherAssignmentApi.update(currentId, payload);
        showSuccess('Assignment updated successfully.');
      } else {
        await teacherAssignmentApi.create(payload);
        showSuccess('Assignment created successfully.');
      }

      resetForm();
      await fetchData();
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || 'Error occurred while saving assignment.');
      setLoading(false);
    }
  };

  const handleEdit = (item) => {
    setIsEditing(true);
    setCurrentId(item.id);
    setLevelTermSectionId(item.levelTermSection.id.toString());
    
    if (item.course) {
      setAssignmentType('course');
      setCourseId(item.course.id.toString());
      setSessionalId('');
    } else if (item.sessional) {
      setAssignmentType('sessional');
      setSessionalId(item.sessional.id.toString());
      setCourseId('');
    }

    setSelectedTeacherIds(item.teachers?.map(t => t.id) || []);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this teacher assignment?')) {
      return;
    }

    setLoading(true);
    setError('');
    try {
      await teacherAssignmentApi.delete(id);
      showSuccess('Teacher assignment removed successfully.');
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to delete teacher assignment.');
      setLoading(false);
    }
  };

  const termRoman = (termVal) => {
    if (termVal === 1) return 'I';
    if (termVal === 2) return 'II';
    return termVal;
  };

  return (
    <div>
      <div style={{ marginBottom: '2rem' }}>
        <h1 style={{ fontSize: '2rem', fontWeight: 800, color: '#f0f6fc', marginBottom: '0.5rem', display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
          <UserCheck color="#008f4c" /> Teacher Assignments
        </h1>
        <p style={{ color: '#8b949e' }}>
          Assign faculty members to teach specific courses or sessional labs inside cohort sections.
        </p>
      </div>

      {success && (
        <div style={{
          backgroundColor: 'rgba(16, 185, 129, 0.15)',
          border: '1px solid #10b981',
          color: '#34d399',
          padding: '1rem',
          borderRadius: '8px',
          marginBottom: '2rem',
          display: 'flex',
          alignItems: 'center',
          gap: '0.5rem'
        }}>
          <Check size={18} /> {success}
        </div>
      )}

      {error && (
        <div style={{
          backgroundColor: 'rgba(125, 26, 26, 0.2)',
          border: '1px solid #7d1a1a',
          color: '#ff8a8a',
          padding: '1rem',
          borderRadius: '8px',
          marginBottom: '2rem'
        }}>
          {error}
        </div>
      )}

      <div style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
        gap: '2rem',
        alignItems: 'start'
      }}>
        {/* Form Editor */}
        <div className="glass-card">
          <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc', marginBottom: '1.5rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            {isEditing ? <Edit size={18} color="#f59e0b" /> : <Plus size={18} color="#008f4c" />}
            {isEditing ? 'Edit Assignment' : 'Create Assignment'}
          </h3>

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label className="form-label">Level-Term Section</label>
              <select 
                className="form-select" 
                value={levelTermSectionId} 
                onChange={e => {
                  setLevelTermSectionId(e.target.value);
                  setCourseId('');
                  setSessionalId('');
                }}
              >
                <option value="">-- Select Section --</option>
                {sections.map(s => (
                  <option key={s.id} value={s.id}>Level {s.level} - Term {termRoman(s.term)} ({s.section})</option>
                ))}
              </select>
            </div>

            <div className="form-group">
              <label className="form-label">Assignment Type</label>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <button 
                  type="button" 
                  className={`btn ${assignmentType === 'course' ? 'btn-primary' : 'btn-secondary'}`}
                  onClick={() => { setAssignmentType('course'); resetForm(); }}
                  style={{ flexGrow: 1 }}
                >
                  Theory Course
                </button>
                <button 
                  type="button" 
                  className={`btn ${assignmentType === 'sessional' ? 'btn-primary' : 'btn-secondary'}`}
                  onClick={() => { setAssignmentType('sessional'); resetForm(); }}
                  style={{ flexGrow: 1 }}
                >
                  Sessional Lab
                </button>
              </div>
            </div>

            {assignmentType === 'course' ? (
              <div className="form-group">
                <label className="form-label">Theory Course</label>
                <select className="form-select" value={courseId} onChange={e => setCourseId(e.target.value)}>
                  <option value="">-- Select Course --</option>
                  {filteredCourses.map(c => (
                    <option key={c.id} value={c.id}>{c.courseCode} - {c.name}</option>
                  ))}
                </select>
                {filteredCourses.length === 0 && <span style={{ color: '#8b949e', fontSize: '0.8rem' }}>No matching courses. Ensure section is selected.</span>}
              </div>
            ) : (
              <div className="form-group">
                <label className="form-label">Sessional Lab</label>
                <select className="form-select" value={sessionalId} onChange={e => setSessionalId(e.target.value)}>
                  <option value="">-- Select Sessional --</option>
                  {filteredSessionals.map(s => (
                    <option key={s.id} value={s.id}>{s.sessionalCode} - {s.name}</option>
                  ))}
                </select>
                {filteredSessionals.length === 0 && <span style={{ color: '#8b949e', fontSize: '0.8rem' }}>No matching sessionals. Ensure section is selected.</span>}
              </div>
            )}

            {/* Teachers checkboxes list */}
            <div className="form-group" style={{ marginTop: '1rem' }}>
              <label className="form-label">Select Assigned Teachers</label>
              <div style={{
                maxHeight: '180px',
                overflowY: 'auto',
                border: '1px solid rgba(255,255,255,0.08)',
                padding: '0.5rem',
                borderRadius: '6px',
                display: 'flex',
                flexDirection: 'column',
                gap: '0.35rem',
                backgroundColor: 'var(--bg-secondary)'
              }}>
                {teachers.map(t => (
                  <label key={t.id} style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', cursor: 'pointer', fontSize: '0.9rem' }}>
                    <input 
                      type="checkbox" 
                      checked={selectedTeacherIds.includes(t.id)} 
                      onChange={() => handleTeacherCheckboxChange(t.id)}
                    />
                    <span>{t.name} ({t.code || 'N/A'})</span>
                  </label>
                ))}
              </div>
            </div>

            <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
              <button type="submit" className="btn btn-primary" style={{ flexGrow: 1 }} disabled={loading}>
                {isEditing ? 'Update Assignment' : 'Assign Faculty'}
              </button>
              {isEditing && (
                <button type="button" className="btn btn-secondary" onClick={resetForm}>
                  <X size={16} /> Cancel
                </button>
              )}
            </div>
          </form>
        </div>

        {/* Database List */}
        <div className="glass-card" style={{ gridColumn: 'span 2' }}>
          <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc', marginBottom: '1.5rem' }}>
            Faculty Assignment Registry ({assignments.length} assignments)
          </h3>

          {loading && assignments.length === 0 ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>Loading registry...</div>
          ) : assignments.length === 0 ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>No teacher assignments found. Make one above!</div>
          ) : (
            <div style={{ overflowX: 'auto' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.95rem' }}>
                <thead>
                  <tr style={{ borderBottom: '1px solid rgba(255,255,255,0.08)', color: '#8b949e', textAlign: 'left' }}>
                    <th style={{ padding: '0.75rem' }}>Section</th>
                    <th style={{ padding: '0.75rem' }}>Course/Lab Code</th>
                    <th style={{ padding: '0.75rem' }}>Course Name</th>
                    <th style={{ padding: '0.75rem' }}>Assigned Faculty</th>
                    <th style={{ padding: '0.75rem', textAlign: 'center' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {assignments.map(item => {
                    const sectionName = item.levelTermSection 
                      ? `L${item.levelTermSection.level}-T${termRoman(item.levelTermSection.term)} (${item.levelTermSection.section})`
                      : 'Unknown';
                    const courseCode = item.course ? item.course.courseCode : (item.sessional ? item.sessional.sessionalCode : '-');
                    const courseName = item.course ? item.course.name : (item.sessional ? item.sessional.name : '-');
                    const facultyNames = item.teachers?.map(t => `${t.name} (${t.code})`).join(', ') || '-';

                    return (
                      <tr key={item.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.04)', color: '#e2e8f0' }}>
                        <td style={{ padding: '0.75rem', fontWeight: 'bold' }}>{sectionName}</td>
                        <td style={{ padding: '0.75rem', color: '#00b35e', fontWeight: 'bold' }}>{courseCode}</td>
                        <td style={{ padding: '0.75rem' }}>{courseName}</td>
                        <td style={{ padding: '0.75rem', fontSize: '0.85rem' }}>{facultyNames}</td>
                        <td style={{ padding: '0.75rem', display: 'flex', gap: '0.5rem', justifyContent: 'center' }}>
                          <button className="btn btn-secondary" style={{ padding: '0.35rem' }} onClick={() => handleEdit(item)}>
                            <Edit size={14} />
                          </button>
                          <button className="btn btn-danger" style={{ padding: '0.35rem' }} onClick={() => handleDelete(item.id)}>
                            <Trash2 size={14} />
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ManageAssignments;

import React, { useState, useEffect } from 'react';
import { Layers, Plus, Edit, Trash2, X, Check } from 'lucide-react';
import { sectionApi, classroomApi } from '../api/api';

const ManageSections = () => {
  const [sections, setSections] = useState([]);
  const [classrooms, setClassrooms] = useState([]);
  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Form states
  const [isEditing, setIsEditing] = useState(false);
  const [currentId, setCurrentId] = useState(null);
  
  // Fields
  const [level, setLevel] = useState(3);
  const [term, setTerm] = useState(2);
  const [sectionLetter, setSectionLetter] = useState('A');
  const [selectedClassroomIds, setSelectedClassroomIds] = useState([]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [sectionRes, classroomRes] = await Promise.all([
        sectionApi.getAll(),
        classroomApi.getAll()
      ]);
      setSections(sectionRes.data);
      setClassrooms(classroomRes.data);
    } catch (err) {
      console.error(err);
      setError('Could not fetch level-term sections database.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const resetForm = () => {
    setLevel(3);
    setTerm(2);
    setSectionLetter('A');
    setSelectedClassroomIds([]);
    setIsEditing(false);
    setCurrentId(null);
  };

  const showSuccess = (msg) => {
    setSuccess(msg);
    setTimeout(() => setSuccess(''), 3000);
  };

  const handleClassroomCheckboxChange = (classroomId) => {
    setSelectedClassroomIds(prev => 
      prev.includes(classroomId) ? prev.filter(id => id !== classroomId) : [...prev, classroomId]
    );
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!sectionLetter) {
      setError('Section is required.');
      return;
    }

    setLoading(true);
    setError('');

    const payload = {
      level,
      term,
      section: sectionLetter.toUpperCase(),
      classroomIds: selectedClassroomIds
    };

    try {
      if (isEditing) {
        await sectionApi.update(currentId, payload);
        showSuccess('Section updated successfully.');
      } else {
        await sectionApi.create(payload);
        showSuccess('Section created successfully.');
      }

      resetForm();
      await fetchData();
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || 'Error occurred while saving section info.');
      setLoading(false);
    }
  };

  const handleEdit = (item) => {
    setIsEditing(true);
    setCurrentId(item.id);
    setLevel(item.level);
    setTerm(item.term);
    setSectionLetter(item.section);
    setSelectedClassroomIds(item.classrooms?.map(c => c.id) || []);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this level-term section?')) {
      return;
    }

    setLoading(true);
    setError('');
    try {
      await sectionApi.delete(id);
      showSuccess('Section deleted successfully.');
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to delete section. There may be schedules or assignments tied to it.');
      setLoading(false);
    }
  };

  const triggerSeedClassrooms = async () => {
    setLoading(true);
    setError('');
    try {
      await sectionApi.assignClassrooms();
      showSuccess('Classrooms seeded and assigned to level-term sections.');
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to auto-assign classrooms.');
      setLoading(false);
    }
  };

  const triggerSeedTeachers = async () => {
    setLoading(true);
    setError('');
    try {
      await sectionApi.assignTeachers();
      showSuccess('Teachers seeded and assigned to level-term sections.');
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to auto-assign teachers.');
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
          <Layers color="#008f4c" /> Level-Term Sections
        </h1>
        <p style={{ color: '#8b949e' }}>
          Configure cohort sections (e.g. 3-II Section B) and assign classrooms available to them for scheduling.
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
        {/* Editor Form */}
        <div className="glass-card">
          <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc', marginBottom: '1.5rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            {isEditing ? <Edit size={18} color="#f59e0b" /> : <Plus size={18} color="#008f4c" />}
            {isEditing ? 'Edit Section Configuration' : 'Add New Cohort Section'}
          </h3>

          <form onSubmit={handleSubmit}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
              <div className="form-group">
                <label className="form-label">Level</label>
                <select className="form-select" value={level} onChange={e => setLevel(parseInt(e.target.value, 10))}>
                  <option value={1}>Level 1</option>
                  <option value={2}>Level 2</option>
                  <option value={3}>Level 3</option>
                  <option value={4}>Level 4</option>
                </select>
              </div>

              <div className="form-group">
                <label className="form-label">Term</label>
                <select className="form-select" value={term} onChange={e => setTerm(parseInt(e.target.value, 10))}>
                  <option value={1}>Term I</option>
                  <option value={2}>Term II</option>
                </select>
              </div>
            </div>

            <div className="form-group">
              <label className="form-label">Section Identifier</label>
              <select className="form-select" value={sectionLetter} onChange={e => setSectionLetter(e.target.value)}>
                <option value="A">Section A</option>
                <option value="B">Section B</option>
                <option value="C">Section C</option>
              </select>
            </div>

            {/* Allowed Classrooms Checkboxes */}
            <div className="form-group" style={{ marginTop: '1rem' }}>
              <label className="form-label">Assign Available Classrooms (Theory)</label>
              <div style={{
                maxHeight: '150px',
                overflowY: 'auto',
                border: '1px solid rgba(255,255,255,0.08)',
                padding: '0.5rem',
                borderRadius: '6px',
                display: 'flex',
                flexDirection: 'column',
                gap: '0.35rem',
                backgroundColor: 'var(--bg-secondary)'
              }}>
                {classrooms.map(room => (
                  <label key={room.id} style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', cursor: 'pointer', fontSize: '0.9rem' }}>
                    <input 
                      type="checkbox" 
                      checked={selectedClassroomIds.includes(room.id)} 
                      onChange={() => handleClassroomCheckboxChange(room.id)}
                    />
                    <span>Room {room.roomNumber}</span>
                  </label>
                ))}
                {classrooms.length === 0 && <span style={{ color: '#8b949e', fontSize: '0.8rem' }}>No classrooms found in database.</span>}
              </div>
            </div>

            <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
              <button type="submit" className="btn btn-primary" style={{ flexGrow: 1 }} disabled={loading}>
                {isEditing ? 'Update Section' : 'Create Section'}
              </button>
              {isEditing && (
                <button type="button" className="btn btn-secondary" onClick={resetForm}>
                  <X size={16} /> Cancel
                </button>
              )}
            </div>
          </form>

          {!isEditing && (
            <div style={{
              marginTop: '2rem',
              paddingTop: '1.5rem',
              borderTop: '1px solid rgba(255,255,255,0.08)',
              display: 'flex',
              flexDirection: 'column',
              gap: '0.75rem'
            }}>
              <span style={{ display: 'block', fontSize: '0.85rem', color: '#8b949e' }}>
                Batch Database Seed Utilities:
              </span>
              <button type="button" className="btn btn-secondary" onClick={triggerSeedClassrooms} disabled={loading}>
                Auto-Assign Classrooms (Seeds)
              </button>
              <button type="button" className="btn btn-secondary" onClick={triggerSeedTeachers} disabled={loading}>
                Auto-Assign Teachers (Seeds)
              </button>
            </div>
          )}
        </div>

        {/* Database List */}
        <div className="glass-card" style={{ gridColumn: 'span 2' }}>
          <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc', marginBottom: '1.5rem' }}>
            Sections List ({sections.length} defined)
          </h3>

          {loading && sections.length === 0 ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>Loading sections...</div>
          ) : sections.length === 0 ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>No cohort sections configured. Create one above!</div>
          ) : (
            <div style={{ overflowX: 'auto' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.95rem' }}>
                <thead>
                  <tr style={{ borderBottom: '1px solid rgba(255,255,255,0.08)', color: '#8b949e', textAlign: 'left' }}>
                    <th style={{ padding: '0.75rem' }}>Section</th>
                    <th style={{ padding: '0.75rem' }}>Classrooms Assigned</th>
                    <th style={{ padding: '0.75rem' }}>Assigned Teachers Count</th>
                    <th style={{ padding: '0.75rem', textAlign: 'center' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {sections.map(item => (
                    <tr key={item.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.04)', color: '#e2e8f0' }}>
                      <td style={{ padding: '0.75rem', fontWeight: 'bold', color: '#00b35e' }}>
                        Level {item.level} - Term {termRoman(item.term)} ({item.section})
                      </td>
                      <td style={{ padding: '0.75rem', fontSize: '0.85rem' }}>
                        {item.classrooms?.map(c => `Room ${c.roomNumber}`).join(', ') || 'No Classrooms'}
                      </td>
                      <td style={{ padding: '0.75rem' }}>
                        {item.assignedTeachers?.length || 0} teachers assigned
                      </td>
                      <td style={{ padding: '0.75rem', display: 'flex', gap: '0.5rem', justifyContent: 'center' }}>
                        <button className="btn btn-secondary" style={{ padding: '0.35rem' }} onClick={() => handleEdit(item)}>
                          <Edit size={14} />
                        </button>
                        <button className="btn btn-danger" style={{ padding: '0.35rem' }} onClick={() => handleDelete(item.id)}>
                          <Trash2 size={14} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ManageSections;

import React, { useState, useEffect } from 'react';
import { BookOpen, Plus, Edit, Trash2, X, Check, Shield } from 'lucide-react';
import { courseApi, sessionalApi, labroomApi } from '../api/api';

const ManageCourses = () => {
  const [activeTab, setActiveTab] = useState('theory'); // 'theory' or 'sessional'
  const [courses, setCourses] = useState([]);
  const [sessionals, setSessionals] = useState([]);
  const [labrooms, setLabrooms] = useState([]);
  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Form states
  const [isEditing, setIsEditing] = useState(false);
  const [currentId, setCurrentId] = useState(null);
  
  // Common fields
  const [name, setName] = useState('');
  const [code, setCode] = useState('');
  const [level, setLevel] = useState(3);
  const [term, setTerm] = useState(2);
  const [credit, setCredit] = useState(3.0);
  
  // Sessional fields
  const [selectedLabroomIds, setSelectedLabroomIds] = useState([]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [coursesRes, sessionalsRes, labroomsRes] = await Promise.all([
        courseApi.getAll(),
        sessionalApi.getAll(),
        labroomApi.getAll()
      ]);
      setCourses(coursesRes.data);
      setSessionals(sessionalsRes.data);
      setLabrooms(labroomsRes.data);
    } catch (err) {
      console.error(err);
      setError('Could not fetch courses and sessional labs.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const resetForm = () => {
    setName('');
    setCode('');
    setLevel(3);
    setTerm(2);
    setCredit(activeTab === 'theory' ? 3.0 : 1.5);
    setSelectedLabroomIds([]);
    setIsEditing(false);
    setCurrentId(null);
  };

  useEffect(() => {
    resetForm();
  }, [activeTab]);

  const showSuccess = (msg) => {
    setSuccess(msg);
    setTimeout(() => setSuccess(''), 3000);
  };

  const handleLabroomCheckboxChange = (labId) => {
    setSelectedLabroomIds(prev => 
      prev.includes(labId) ? prev.filter(id => id !== labId) : [...prev, labId]
    );
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!name || !code) {
      setError('Course code and course name are required fields.');
      return;
    }

    setLoading(true);
    setError('');

    try {
      if (activeTab === 'theory') {
        const payload = {
          name,
          courseCode: code.toUpperCase(),
          level,
          term,
          credit: parseFloat(credit)
        };

        if (isEditing) {
          await courseApi.update(currentId, payload);
          showSuccess('Course details updated successfully.');
        } else {
          await courseApi.create(payload);
          showSuccess('Theory course created successfully.');
        }
      } else {
        const payload = {
          name,
          sessionalCode: code.toUpperCase(),
          level,
          term,
          credit: parseFloat(credit),
          labroomIds: selectedLabroomIds
        };

        if (isEditing) {
          await sessionalApi.update(currentId, payload);
          showSuccess('Sessional lab details updated successfully.');
        } else {
          await sessionalApi.create(payload);
          showSuccess('Sessional lab course created successfully.');
        }
      }

      resetForm();
      await fetchData();
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || 'Error occurred while saving course information.');
      setLoading(false);
    }
  };

  const handleEdit = (item) => {
    setIsEditing(true);
    setCurrentId(item.id);
    setName(item.name);
    
    if (activeTab === 'theory') {
      setCode(item.courseCode || '');
      setCredit(item.credit || 3.0);
    } else {
      setCode(item.sessionalCode || '');
      setCredit(item.credit || 1.5);
      setSelectedLabroomIds(item.labrooms?.map(l => l.id) || []);
    }
    
    setLevel(item.level || 3);
    setTerm(item.term || 2);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this entry? It will remove all scheduling records for it.')) {
      return;
    }

    setLoading(true);
    setError('');
    try {
      if (activeTab === 'theory') {
        await courseApi.delete(id);
        showSuccess('Theory course deleted successfully.');
      } else {
        await sessionalApi.delete(id);
        showSuccess('Sessional lab deleted successfully.');
      }
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to delete course. It is likely tied to teacher assignments or class schedules.');
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
          <BookOpen color="#008f4c" /> Manage Curriculum
        </h1>
        <p style={{ color: '#8b949e' }}>
          Manage theory courses, sessional practical labs, credit hours, and map allowed lab spaces.
        </p>
      </div>

      {/* Tabs */}
      <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '2rem', borderBottom: '1px solid rgba(255,255,255,0.08)', paddingBottom: '0.5rem' }}>
        <button 
          className={`btn ${activeTab === 'theory' ? 'btn-primary' : 'btn-secondary'}`}
          onClick={() => setActiveTab('theory')}
          style={{ borderRadius: '8px 8px 0 0', border: 'none' }}
        >
          Theory Courses ({courses.length})
        </button>
        <button 
          className={`btn ${activeTab === 'sessional' ? 'btn-primary' : 'btn-secondary'}`}
          onClick={() => setActiveTab('sessional')}
          style={{ borderRadius: '8px 8px 0 0', border: 'none' }}
        >
          Sessional Labs ({sessionals.length})
        </button>
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
            {isEditing 
              ? (activeTab === 'theory' ? 'Edit Theory Course' : 'Edit Sessional Lab') 
              : (activeTab === 'theory' ? 'Add Theory Course' : 'Add Sessional Lab')
            }
          </h3>

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label className="form-label">Course Code</label>
              <input 
                type="text" 
                className="form-control" 
                value={code} 
                onChange={e => setCode(e.target.value)} 
                placeholder={activeTab === 'theory' ? 'e.g. CSE 3203' : 'e.g. CSE 3204'}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Course Name</label>
              <input 
                type="text" 
                className="form-control" 
                value={name} 
                onChange={e => setName(e.target.value)} 
                placeholder={activeTab === 'theory' ? 'e.g. Operating System' : 'e.g. Operating System Sessional'}
              />
            </div>

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
              <label className="form-label">Credit Hours</label>
              <select className="form-select" value={credit} onChange={e => setCredit(parseFloat(e.target.value))}>
                {activeTab === 'theory' ? (
                  <>
                    <option value={3.0}>3.0 Credits</option>
                    <option value={2.0}>2.0 Credits</option>
                    <option value={4.0}>4.0 Credits</option>
                  </>
                ) : (
                  <>
                    <option value={1.5}>1.5 Credits (3h lab)</option>
                    <option value={0.75}>0.75 Credits (1.5h lab)</option>
                    <option value={3.0}>3.0 Credits (Project)</option>
                  </>
                )}
              </select>
            </div>

            {/* Allowed Labrooms Selector for Sessionals */}
            {activeTab === 'sessional' && (
              <div className="form-group" style={{ marginTop: '1rem' }}>
                <label className="form-label" style={{ display: 'flex', alignItems: 'center', gap: '0.4rem' }}>
                  <Shield size={14} /> Assign Allowed Lab Rooms
                </label>
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
                  {labrooms.map(lab => (
                    <label key={lab.id} style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', cursor: 'pointer', fontSize: '0.9rem' }}>
                      <input 
                        type="checkbox" 
                        checked={selectedLabroomIds.includes(lab.id)} 
                        onChange={() => handleLabroomCheckboxChange(lab.id)}
                      />
                      <span>Room {lab.roomNumber} ({lab.name || 'Lab'})</span>
                    </label>
                  ))}
                  {labrooms.length === 0 && <span style={{ color: '#8b949e', fontSize: '0.8rem' }}>No labrooms found in database.</span>}
                </div>
              </div>
            )}

            <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
              <button type="submit" className="btn btn-primary" style={{ flexGrow: 1 }} disabled={loading}>
                {isEditing ? 'Update Course' : 'Create Course'}
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
            {activeTab === 'theory' ? 'Theory Course Database' : 'Sessional Lab Database'}
          </h3>

          {loading && (activeTab === 'theory' ? courses.length === 0 : sessionals.length === 0) ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>Loading database...</div>
          ) : (activeTab === 'theory' ? courses.length === 0 : sessionals.length === 0) ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>No records found. Add one on the left!</div>
          ) : (
            <div style={{ overflowX: 'auto' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.95rem' }}>
                <thead>
                  <tr style={{ borderBottom: '1px solid rgba(255,255,255,0.08)', color: '#8b949e', textAlign: 'left' }}>
                    <th style={{ padding: '0.75rem' }}>Code</th>
                    <th style={{ padding: '0.75rem' }}>Name</th>
                    <th style={{ padding: '0.75rem' }}>Level-Term</th>
                    <th style={{ padding: '0.75rem' }}>Credits</th>
                    {activeTab === 'sessional' && <th style={{ padding: '0.75rem' }}>Allowed Labs</th>}
                    <th style={{ padding: '0.75rem', textAlign: 'center' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {(activeTab === 'theory' ? courses : sessionals).map(item => (
                    <tr key={item.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.04)', color: '#e2e8f0' }}>
                      <td style={{ padding: '0.75rem', fontWeight: 'bold', color: '#00b35e' }}>
                        {activeTab === 'theory' ? item.courseCode : item.sessionalCode}
                      </td>
                      <td style={{ padding: '0.75rem' }}>{item.name}</td>
                      <td style={{ padding: '0.75rem' }}>Level {item.level} - Term {termRoman(item.term)}</td>
                      <td style={{ padding: '0.75rem' }}>{item.credit?.toFixed(1)} Credits</td>
                      {activeTab === 'sessional' && (
                        <td style={{ padding: '0.75rem', fontSize: '0.8rem', color: '#8b949e', maxWidth: '200px', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                          {item.labrooms?.map(l => l.roomNumber).join(', ') || 'None Assigned'}
                        </td>
                      )}
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

export default ManageCourses;

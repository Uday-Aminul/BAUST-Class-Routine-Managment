import React, { useState, useEffect } from 'react';
import { Users, Plus, Edit, Trash2, X, Check } from 'lucide-react';
import { teacherApi } from '../api/api';

const ManageTeachers = () => {
  const [teachers, setTeachers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Form states
  const [isEditing, setIsEditing] = useState(false);
  const [currentId, setCurrentId] = useState(null);
  const [name, setName] = useState('');
  const [code, setCode] = useState('');
  const [designation, setDesignation] = useState('Lecturer');

  const fetchTeachers = async () => {
    setLoading(true);
    try {
      const res = await teacherApi.getAll();
      setTeachers(res.data);
    } catch (err) {
      console.error(err);
      setError('Could not fetch teachers list.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTeachers();
  }, []);

  const resetForm = () => {
    setName('');
    setCode('');
    setDesignation('Lecturer');
    setIsEditing(false);
    setCurrentId(null);
  };

  const showSuccess = (msg) => {
    setSuccess(msg);
    setTimeout(() => setSuccess(''), 3000);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!name || !code) {
      setError('Name and Short Form Code are required.');
      return;
    }

    setLoading(true);
    setError('');

    const payload = {
      name,
      code: code.toUpperCase(),
      designation
    };

    try {
      if (isEditing) {
        await teacherApi.update(currentId, payload);
        showSuccess('Teacher updated successfully.');
      } else {
        await teacherApi.create(payload);
        showSuccess('Teacher added successfully.');
      }
      resetForm();
      await fetchTeachers();
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || 'Error occurred while saving teacher data.');
      setLoading(false);
    }
  };

  const handleEdit = (teacher) => {
    setIsEditing(true);
    setCurrentId(teacher.id);
    setName(teacher.name);
    setCode(teacher.code || '');
    setDesignation(teacher.designation || 'Lecturer');
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this teacher? This might impact their schedules.')) {
      return;
    }

    setLoading(true);
    setError('');
    try {
      await teacherApi.delete(id);
      showSuccess('Teacher deleted successfully.');
      await fetchTeachers();
    } catch (err) {
      console.error(err);
      setError('Failed to delete teacher. They might be assigned to a section or schedule.');
      setLoading(false);
    }
  };

  return (
    <div>
      <div style={{ marginBottom: '2rem' }}>
        <h1 style={{ fontSize: '2rem', fontWeight: 800, color: '#f0f6fc', marginBottom: '0.5rem', display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
          <Users color="#008f4c" /> Manage Teachers
        </h1>
        <p style={{ color: '#8b949e' }}>
          Add, update, or remove faculty members and configure short forms for routine displays.
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
            {isEditing ? 'Edit Faculty Details' : 'Add New Faculty Member'}
          </h3>

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label className="form-label">Full Name</label>
              <input 
                type="text" 
                className="form-control" 
                value={name} 
                onChange={e => setName(e.target.value)} 
                placeholder="e.g. Md. Nahid Anam Opu"
              />
            </div>

            <div className="form-group">
              <label className="form-label">Short Form (Initials)</label>
              <input 
                type="text" 
                className="form-control" 
                value={code} 
                onChange={e => setCode(e.target.value)} 
                placeholder="e.g. NAO"
                maxLength={5}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Designation</label>
              <select className="form-select" value={designation} onChange={e => setDesignation(e.target.value)}>
                <option value="Professor">Professor</option>
                <option value="Associate Professor">Associate Professor</option>
                <option value="Assistant Professor">Assistant Professor</option>
                <option value="Lecturer">Lecturer</option>
              </select>
            </div>

            <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
              <button type="submit" className="btn btn-primary" style={{ flexGrow: 1 }} disabled={loading}>
                {isEditing ? 'Update Details' : 'Add Teacher'}
              </button>
              {isEditing && (
                <button type="button" className="btn btn-secondary" onClick={resetForm}>
                  <X size={16} /> Cancel
                </button>
              )}
            </div>
          </form>
        </div>

        {/* Teachers List */}
        <div className="glass-card" style={{ gridColumn: 'span 2' }}>
          <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc', marginBottom: '1.5rem' }}>
            Faculty Database ({teachers.length} members)
          </h3>

          {loading && teachers.length === 0 ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>Loading database entries...</div>
          ) : teachers.length === 0 ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>No teachers found. Add one above!</div>
          ) : (
            <div style={{ overflowX: 'auto' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.95rem' }}>
                <thead>
                  <tr style={{ borderBottom: '1px solid rgba(255,255,255,0.08)', color: '#8b949e', textAlign: 'left' }}>
                    <th style={{ padding: '0.75rem' }}>Short Form</th>
                    <th style={{ padding: '0.75rem' }}>Name</th>
                    <th style={{ padding: '0.75rem' }}>Designation</th>
                    <th style={{ padding: '0.75rem' }}>Credit Load</th>
                    <th style={{ padding: '0.75rem', textAlign: 'center' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {teachers.map(t => (
                    <tr key={t.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.04)', color: '#e2e8f0' }}>
                      <td style={{ padding: '0.75rem', fontWeight: 'bold', color: '#00b35e' }}>{t.code || '-'}</td>
                      <td style={{ padding: '0.75rem' }}>{t.name}</td>
                      <td style={{ padding: '0.75rem' }}>{t.designation}</td>
                      <td style={{ padding: '0.75rem' }}>{t.assignedCredit?.toFixed(1) || '0.0'} Hours</td>
                      <td style={{ padding: '0.75rem', display: 'flex', gap: '0.5rem', justifyContent: 'center' }}>
                        <button className="btn btn-secondary" style={{ padding: '0.35rem' }} onClick={() => handleEdit(t)}>
                          <Edit size={14} />
                        </button>
                        <button className="btn btn-danger" style={{ padding: '0.35rem' }} onClick={() => handleDelete(t.id)}>
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

export default ManageTeachers;

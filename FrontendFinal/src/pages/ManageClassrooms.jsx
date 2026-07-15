import React, { useState, useEffect } from 'react';
import { Home, Plus, Edit, Trash2, X, Check, Shield } from 'lucide-react';
import { classroomApi, labroomApi, sessionalApi } from '../api/api';

const ManageClassrooms = () => {
  const [activeTab, setActiveTab] = useState('classroom'); // 'classroom' or 'labroom'
  const [classrooms, setClassrooms] = useState([]);
  const [labrooms, setLabrooms] = useState([]);
  const [sessionals, setSessionals] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Form states
  const [isEditing, setIsEditing] = useState(false);
  const [currentId, setCurrentId] = useState(null);
  
  // Fields
  const [roomNumber, setRoomNumber] = useState('');
  const [name, setName] = useState(''); // Lab name (only for labroom)
  const [selectedSessionalIds, setSelectedSessionalIds] = useState([]); // Sessional IDs (only for labroom)

  const fetchData = async () => {
    setLoading(true);
    try {
      const [classroomRes, labroomRes, sessionalRes] = await Promise.all([
        classroomApi.getAll(),
        labroomApi.getAll(),
        sessionalApi.getAll()
      ]);
      setClassrooms(classroomRes.data);
      setLabrooms(labroomRes.data);
      setSessionals(sessionalRes.data);
    } catch (err) {
      console.error(err);
      setError('Could not fetch classrooms and labrooms list.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const resetForm = () => {
    setRoomNumber('');
    setName('');
    setSelectedSessionalIds([]);
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

  const handleSessionalCheckboxChange = (sessionalId) => {
    setSelectedSessionalIds(prev => 
      prev.includes(sessionalId) ? prev.filter(id => id !== sessionalId) : [...prev, sessionalId]
    );
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!roomNumber) {
      setError('Room Number is required.');
      return;
    }

    setLoading(true);
    setError('');

    try {
      if (activeTab === 'classroom') {
        const payload = {
          roomNumber: parseInt(roomNumber, 10)
        };
        if (isEditing) {
          await classroomApi.update(currentId, payload);
          showSuccess('Classroom updated successfully.');
        } else {
          await classroomApi.create(payload);
          showSuccess('Classroom created successfully.');
        }
      } else {
        const payload = {
          roomNumber: parseInt(roomNumber, 10),
          name: name || `Lab ${roomNumber}`,
          allowedSessionalIds: selectedSessionalIds
        };
        if (isEditing) {
          await labroomApi.update(currentId, payload);
          showSuccess('Labroom updated successfully.');
        } else {
          await labroomApi.create(payload);
          showSuccess('Labroom created successfully.');
        }
      }

      resetForm();
      await fetchData();
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message || 'Error occurred while saving room information.');
      setLoading(false);
    }
  };

  const handleEdit = (item) => {
    setIsEditing(true);
    setCurrentId(item.id);
    setRoomNumber(item.roomNumber.toString());
    if (activeTab === 'labroom') {
      setName(item.name || '');
      setSelectedSessionalIds(item.allowedSessionals?.map(s => s.id) || []);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this room? This will clear all schedule records associated with it.')) {
      return;
    }

    setLoading(true);
    setError('');
    try {
      if (activeTab === 'classroom') {
        await classroomApi.delete(id);
        showSuccess('Classroom deleted successfully.');
      } else {
        await labroomApi.delete(id);
        showSuccess('Labroom deleted successfully.');
      }
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to delete room. Ensure it is not currently occupied by class schedules.');
      setLoading(false);
    }
  };

  const triggerSeedSessionals = async () => {
    setLoading(true);
    setError('');
    try {
      await labroomApi.assignSessionals();
      showSuccess('Sessionals automatically seeded and assigned to labrooms.');
      await fetchData();
    } catch (err) {
      console.error(err);
      setError('Failed to seed sessionals to labrooms.');
      setLoading(false);
    }
  };

  return (
    <div>
      <div style={{ marginBottom: '2rem' }}>
        <h1 style={{ fontSize: '2rem', fontWeight: 800, color: '#f0f6fc', marginBottom: '0.5rem', display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
          <Home color="#008f4c" /> Manage Rooms
        </h1>
        <p style={{ color: '#8b949e' }}>
          Configure classrooms for theory sessions and specialized labs for practical sessionals.
        </p>
      </div>

      {/* Tabs */}
      <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '2rem', borderBottom: '1px solid rgba(255,255,255,0.08)', paddingBottom: '0.5rem' }}>
        <button 
          className={`btn ${activeTab === 'classroom' ? 'btn-primary' : 'btn-secondary'}`}
          onClick={() => setActiveTab('classroom')}
          style={{ borderRadius: '8px 8px 0 0', border: 'none' }}
        >
          Theory Classrooms ({classrooms.length})
        </button>
        <button 
          className={`btn ${activeTab === 'labroom' ? 'btn-primary' : 'btn-secondary'}`}
          onClick={() => setActiveTab('labroom')}
          style={{ borderRadius: '8px 8px 0 0', border: 'none' }}
        >
          Labrooms ({labrooms.length})
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
              ? (activeTab === 'classroom' ? 'Edit Classroom' : 'Edit Labroom') 
              : (activeTab === 'classroom' ? 'Add Classroom' : 'Add Labroom')
            }
          </h3>

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label className="form-label">Room Number</label>
              <input 
                type="number" 
                className="form-control" 
                value={roomNumber} 
                onChange={e => setRoomNumber(e.target.value)} 
                placeholder="e.g. 308"
              />
            </div>

            {activeTab === 'labroom' && (
              <>
                <div className="form-group">
                  <label className="form-label">Lab Name / Description</label>
                  <input 
                    type="text" 
                    className="form-control" 
                    value={name} 
                    onChange={e => setName(e.target.value)} 
                    placeholder="e.g. CSE Lab 302"
                  />
                </div>

                <div className="form-group" style={{ marginTop: '1rem' }}>
                  <label className="form-label" style={{ display: 'flex', alignItems: 'center', gap: '0.4rem' }}>
                    <Shield size={14} /> Map Allowed Sessionals
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
                    {sessionals.map(sess => (
                      <label key={sess.id} style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', cursor: 'pointer', fontSize: '0.9rem' }}>
                        <input 
                          type="checkbox" 
                          checked={selectedSessionalIds.includes(sess.id)} 
                          onChange={() => handleSessionalCheckboxChange(sess.id)}
                        />
                        <span>{sess.sessionalCode} - {sess.name}</span>
                      </label>
                    ))}
                    {sessionals.length === 0 && <span style={{ color: '#8b949e', fontSize: '0.8rem' }}>No sessionals found in database.</span>}
                  </div>
                </div>
              </>
            )}

            <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
              <button type="submit" className="btn btn-primary" style={{ flexGrow: 1 }} disabled={loading}>
                {isEditing ? 'Update Room' : 'Add Room'}
              </button>
              {isEditing && (
                <button type="button" className="btn btn-secondary" onClick={resetForm}>
                  <X size={16} /> Cancel
                </button>
              )}
            </div>
          </form>

          {activeTab === 'labroom' && !isEditing && (
            <div style={{ marginTop: '2rem', paddingTop: '1.5rem', borderTop: '1px solid rgba(255,255,255,0.08)' }}>
              <span style={{ display: 'block', fontSize: '0.85rem', color: '#8b949e', marginBottom: '0.75rem' }}>
                Quick Setup: Automatically match allowed sessionals for labs based on department configuration.
              </span>
              <button type="button" className="btn btn-secondary" style={{ width: '100%' }} onClick={triggerSeedSessionals} disabled={loading}>
                Seed Labroom Assignments
              </button>
            </div>
          )}
        </div>

        {/* Database List */}
        <div className="glass-card" style={{ gridColumn: 'span 2' }}>
          <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc', marginBottom: '1.5rem' }}>
            {activeTab === 'classroom' ? 'Classroom Directory' : 'Labroom Directory'}
          </h3>

          {loading && (activeTab === 'classroom' ? classrooms.length === 0 : labrooms.length === 0) ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>Loading directory...</div>
          ) : (activeTab === 'classroom' ? classrooms.length === 0 : labrooms.length === 0) ? (
            <div style={{ color: '#8b949e', textAlign: 'center', padding: '2rem' }}>No rooms configured. Add one on the left!</div>
          ) : (
            <div style={{ overflowX: 'auto' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.95rem' }}>
                <thead>
                  <tr style={{ borderBottom: '1px solid rgba(255,255,255,0.08)', color: '#8b949e', textAlign: 'left' }}>
                    <th style={{ padding: '0.75rem' }}>Room Number</th>
                    {activeTab === 'labroom' && <th style={{ padding: '0.75rem' }}>Lab Name</th>}
                    {activeTab === 'labroom' && <th style={{ padding: '0.75rem' }}>Allowed Sessionals</th>}
                    <th style={{ padding: '0.75rem', textAlign: 'center' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {(activeTab === 'classroom' ? classrooms : labrooms).map(item => (
                    <tr key={item.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.04)', color: '#e2e8f0' }}>
                      <td style={{ padding: '0.75rem', fontWeight: 'bold', color: '#00b35e' }}>Room {item.roomNumber}</td>
                      {activeTab === 'labroom' && <td style={{ padding: '0.75rem' }}>{item.name}</td>}
                      {activeTab === 'labroom' && (
                        <td style={{ padding: '0.75rem', fontSize: '0.8rem', color: '#8b949e', maxWidth: '250px', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                          {item.allowedSessionals?.map(s => s.sessionalCode).join(', ') || 'Any Sessional'}
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

export default ManageClassrooms;

import React, { useState, useEffect } from 'react';
import { Home, Printer } from 'lucide-react';
import { classroomApi, labroomApi } from '../api/api';
import UniversityHeader from '../components/UniversityHeader';
import RoutineTable from '../components/RoutineTable';

const ClassroomRoutine = () => {
  const [rooms, setRooms] = useState([]);
  const [labs, setLabs] = useState([]);
  
  const [selectedRoomType, setSelectedRoomType] = useState('classroom'); // 'classroom' or 'labroom'
  const [selectedRoomNumber, setSelectedRoomNumber] = useState('');
  const [routineMode, setRoutineMode] = useState('normal'); // 'normal' or 'ramadan'
  const [activeSeason, setActiveSeason] = useState(() => localStorage.getItem('activeSeason') || 'Winter');
  const [activeYear, setActiveYear] = useState(() => localStorage.getItem('activeYear') || '2026');

  useEffect(() => {
    const handleTermChange = () => {
      setActiveSeason(localStorage.getItem('activeSeason') || 'Winter');
      setActiveYear(localStorage.getItem('activeYear') || '2026');
    };
    window.addEventListener('academicTermChanged', handleTermChange);
    return () => window.removeEventListener('academicTermChanged', handleTermChange);
  }, []);
  
  const [roomDetail, setRoomDetail] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const [classroomRes, labroomRes] = await Promise.all([
          classroomApi.getAll(),
          labroomApi.getAll()
        ]);
        setRooms(classroomRes.data);
        setLabs(labroomRes.data);
        
        if (classroomRes.data.length > 0) {
          setSelectedRoomNumber(classroomRes.data[0].roomNumber.toString());
        }
      } catch (err) {
        console.error(err);
        setError('Failed to load classrooms and labrooms list.');
      }
    };
    fetchRooms();
  }, []);

  useEffect(() => {
    if (!selectedRoomNumber) return;

    const fetchRoomDetails = async () => {
      setLoading(true);
      setError('');
      try {
        let res;
        if (selectedRoomType === 'classroom') {
          res = await classroomApi.getByRoomNumber(parseInt(selectedRoomNumber, 10));
        } else {
          res = await labroomApi.getByRoomNumber(parseInt(selectedRoomNumber, 10));
        }
        setRoomDetail(res.data);
      } catch (err) {
        console.error(err);
        setError(`Failed to fetch schedules for Room ${selectedRoomNumber}`);
        setRoomDetail(null);
      } finally {
        setLoading(false);
      }
    };

    fetchRoomDetails();
  }, [selectedRoomNumber, selectedRoomType]);

  const handleRoomTypeChange = (type) => {
    setSelectedRoomType(type);
    const list = type === 'classroom' ? rooms : labs;
    if (list.length > 0) {
      setSelectedRoomNumber(list[0].roomNumber.toString());
    } else {
      setSelectedRoomNumber('');
    }
  };

  const getMappedSchedules = () => {
    if (!roomDetail?.classSchedules) return [];
    
    return roomDetail.classSchedules.map(cls => ({
      ...cls,
      // Map to correct properties so RoutineTable can render details
      course: cls.course ? {
        ...cls.course,
        courseCode: `${cls.course.courseCode || cls.course.name} (${cls.level}-${cls.term}${cls.section})`
      } : null,
      sessional: cls.sessional ? {
        ...cls.sessional,
        sessionalCode: `${cls.sessional.sessionalCode || cls.sessional.name} (${cls.level}-${cls.term}${cls.section})`
      } : null,
      // Classroom/Labroom is already known so we don't display room inside cell
      classroom: null,
      labroom: null,
      // Keep teachers
      teachers: cls.teacher ? cls.teacher : (cls.teachers ? cls.teachers : [])
    }));
  };

  const handlePrint = () => {
    window.print();
  };

  return (
    <div>
      <div className="glass-card no-print" style={{ marginBottom: '2rem' }}>
        <h3 style={{ fontSize: '1.25rem', color: '#f0f6fc', marginBottom: '1.25rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <Home size={20} color="#008f4c" /> Select Room Schedule
        </h3>

        <div style={{ display: 'flex', gap: '1.5rem', alignItems: 'end', flexWrap: 'wrap' }}>
          <div className="form-group" style={{ marginBottom: 0 }}>
            <label className="form-label">Room Type</label>
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <button 
                type="button"
                className={`btn ${selectedRoomType === 'classroom' ? 'btn-primary' : 'btn-secondary'}`}
                onClick={() => handleRoomTypeChange('classroom')}
              >
                Theory Classrooms
              </button>
              <button 
                type="button"
                className={`btn ${selectedRoomType === 'labroom' ? 'btn-primary' : 'btn-secondary'}`}
                onClick={() => handleRoomTypeChange('labroom')}
              >
                Practical Labs
              </button>
            </div>
          </div>

          <div className="form-group" style={{ flexGrow: 1, minWidth: '150px', marginBottom: 0 }}>
            <label className="form-label">Select Room Number</label>
            <select 
              className="form-select" 
              value={selectedRoomNumber} 
              onChange={e => setSelectedRoomNumber(e.target.value)}
              style={{ width: '100%' }}
            >
              <option value="">-- Select Room --</option>
              {selectedRoomType === 'classroom' ? (
                rooms.map(r => (
                  <option key={r.id} value={r.roomNumber}>Room {r.roomNumber}</option>
                ))
              ) : (
                labs.map(l => (
                  <option key={l.id} value={l.roomNumber}>Room {l.roomNumber} - {l.name || 'Lab'}</option>
                ))
              )}
            </select>
          </div>

          <div className="form-group" style={{ minWidth: '150px', marginBottom: 0 }}>
            <label className="form-label">Routine Mode</label>
            <select className="form-select" value={routineMode} onChange={e => setRoutineMode(e.target.value)}>
              <option value="normal">Normal Days</option>
              <option value="ramadan">Ramadan Days</option>
            </select>
          </div>

          <button className="btn btn-primary" onClick={handlePrint} disabled={!selectedRoomNumber}>
            <Printer size={16} /> Print Routine
          </button>
        </div>
      </div>

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

      {loading ? (
        <div style={{ display: 'flex', justifyContent: 'center', margin: '4rem 0', color: '#8b949e' }}>
          <div>Loading room timetable...</div>
        </div>
      ) : roomDetail ? (
        <div className="routine-export-container">
          <UniversityHeader subTitle={`Room Occupancy Class Routine — Room ${roomDetail.roomNumber} ${selectedRoomType === 'labroom' ? `(${roomDetail.name || 'Lab'})` : '(Theory)'} — ${activeSeason} ${activeYear} ${routineMode === 'ramadan' ? '(Ramadan Days)' : ''}`} />

          <div style={{
            display: 'flex',
            justifyContent: 'space-between',
            marginBottom: '1.25rem',
            borderBottom: '1px dashed #1a1a1a',
            paddingBottom: '0.5rem',
            fontSize: '0.95rem'
          }}>
            <div>
              <strong>Room Type:</strong> {selectedRoomType === 'classroom' ? 'Theory Classroom' : 'Sessional Lab'} <br />
              <strong>Room Number:</strong> {roomDetail.roomNumber}
            </div>
            {selectedRoomType === 'labroom' && roomDetail.allowedSessionals && (
              <div style={{ textAlign: 'right', maxWidth: '400px' }}>
                <strong>Allowed Labs:</strong> <span style={{ fontSize: '0.75rem' }}>{roomDetail.allowedSessionals.map(s => s.sessionalCode).join(', ') || 'Any'}</span>
              </div>
            )}
          </div>

          <RoutineTable schedules={getMappedSchedules()} mode={routineMode} />

          <div style={{ fontSize: '0.8rem', color: '#555', marginTop: '1rem', textAlign: 'center' }}>
            Note: Routine cells display CourseCode followed by (Level-TermSection) and (Teacher Initials).
          </div>
        </div>
      ) : (
        <div style={{ textAlign: 'center', padding: '3rem', color: '#8b949e' }}>
          Choose a room or lab above to view its weekly timetable.
        </div>
      )}
    </div>
  );
};

export default ClassroomRoutine;

import React, { useState, useEffect } from 'react';
import { Users, Printer } from 'lucide-react';
import { teacherApi } from '../api/api';
import UniversityHeader from '../components/UniversityHeader';
import RoutineTable from '../components/RoutineTable';

const TeacherRoutine = () => {
  const [teachers, setTeachers] = useState([]);
  const [selectedTeacherId, setSelectedTeacherId] = useState('');
  const [teacherDetail, setTeacherDetail] = useState(null);
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

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        const res = await teacherApi.getAll();
        setTeachers(res.data);
        if (res.data.length > 0) {
          setSelectedTeacherId(res.data[0].id.toString());
        }
      } catch (err) {
        console.error(err);
        setError('Failed to fetch teachers database.');
      }
    };
    fetchTeachers();
  }, []);

  useEffect(() => {
    if (!selectedTeacherId) return;

    const fetchTeacherSchedule = async () => {
      setLoading(true);
      setError('');
      try {
        const res = await teacherApi.getById(parseInt(selectedTeacherId, 10));
        setTeacherDetail(res.data);
      } catch (err) {
        console.error(err);
        setError('Failed to fetch schedules for selected teacher.');
      } finally {
        setLoading(false);
      }
    };

    fetchTeacherSchedule();
  }, [selectedTeacherId]);

  // Map teacher classes format to match RoutineTable expectations
  const getMappedSchedules = () => {
    if (!teacherDetail?.classes) return [];
    
    return teacherDetail.classes.map(cls => ({
      ...cls,
      // Map properties so RoutineTable can display section & level info
      course: cls.course ? {
        ...cls.course,
        courseCode: `${cls.course.courseCode} (${cls.level}-${cls.term}${cls.section})`
      } : null,
      sessional: cls.sessional ? {
        ...cls.sessional,
        sessionalCode: `${cls.sessional.sessionalCode} (${cls.level}-${cls.term}${cls.section})`
      } : null,
      teachers: [] // Don't show teacher codes since this is the teacher's own routine
    }));
  };

  const handlePrint = () => {
    window.print();
  };

  return (
    <div>
      <div className="glass-card no-print" style={{ marginBottom: '2rem' }}>
        <h3 style={{ fontSize: '1.25rem', color: '#f0f6fc', marginBottom: '1.25rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <Users size={20} color="#008f4c" /> Select Teacher Routine
        </h3>

        <div style={{ display: 'flex', gap: '1rem', alignItems: 'end', flexWrap: 'wrap' }}>
          <div className="form-group" style={{ flexGrow: 1, minWidth: '200px', marginBottom: 0 }}>
            <label className="form-label">Teacher</label>
            <select 
              className="form-select" 
              value={selectedTeacherId} 
              onChange={e => setSelectedTeacherId(e.target.value)}
              style={{ width: '100%' }}
            >
              <option value="">-- Choose Teacher --</option>
              {teachers.map(t => (
                <option key={t.id} value={t.id}>{t.name} ({t.code || 'N/A'}) - {t.designation}</option>
              ))}
            </select>
          </div>

          <div className="form-group" style={{ minWidth: '150px', marginBottom: 0 }}>
            <label className="form-label">Routine Mode</label>
            <select className="form-select" value={routineMode} onChange={e => setRoutineMode(e.target.value)}>
              <option value="normal">Normal Days</option>
              <option value="ramadan">Ramadan Days</option>
            </select>
          </div>

          <button className="btn btn-primary" onClick={handlePrint} disabled={!selectedTeacherId}>
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
          <div>Loading teacher weekly routine...</div>
        </div>
      ) : teacherDetail ? (
        <div className="routine-export-container">
          <UniversityHeader subTitle={`Weekly Class Routine — Teacher: ${teacherDetail.name} — ${activeSeason} ${activeYear} ${routineMode === 'ramadan' ? '(Ramadan Days)' : ''}`} />

          <div style={{
            display: 'flex',
            justifyContent: 'space-between',
            marginBottom: '1.25rem',
            borderBottom: '1px dashed #1a1a1a',
            paddingBottom: '0.5rem',
            fontSize: '0.95rem'
          }}>
            <div>
              <strong>Designation:</strong> {teacherDetail.designation} <br />
              <strong>Teacher Initials:</strong> {teacherDetail.code}
            </div>
            <div style={{ textAlign: 'right' }}>
              <strong>Total Assigned Credits:</strong> <span style={{ textDecoration: 'underline', fontWeight: 'bold' }}>{teacherDetail.assignedCredit?.toFixed(1) || '0.0'} Hours</span>
            </div>
          </div>

          <RoutineTable schedules={getMappedSchedules()} mode={routineMode} />

          <div style={{ fontSize: '0.8rem', color: '#555', marginTop: '1rem', textAlign: 'center' }}>
            Note: Routine cells display CourseCode followed by (Level-TermSection) and [Room Number].
          </div>
        </div>
      ) : (
        <div style={{ textAlign: 'center', padding: '3rem', color: '#8b949e' }}>
          Select a teacher above to render their weekly schedule.
        </div>
      )}
    </div>
  );
};

export default TeacherRoutine;

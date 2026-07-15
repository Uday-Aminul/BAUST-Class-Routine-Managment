import React, { useState, useEffect } from 'react';
import { Printer, Calendar } from 'lucide-react';
import { routineApi, courseApi, sessionalApi, teacherApi } from '../api/api';
import UniversityHeader from '../components/UniversityHeader';
import RoutineTable from '../components/RoutineTable';
import CourseTable from '../components/CourseTable';
import TeacherTable from '../components/TeacherTable';

const StudentRoutine = () => {
  const [level, setLevel] = useState(3);
  const [term, setTerm] = useState(2);
  const [section, setSection] = useState('B');
  
  const [advisor, setAdvisor] = useState('');
  const [dpcG2, setDpcG2] = useState('Md. Zahim Hassan');
  const [dpcPhone, setDpcPhone] = useState('01736393334');

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

  const [schedules, setSchedules] = useState([]);
  const [allCourses, setAllCourses] = useState([]);
  const [allSessionals, setAllSessionals] = useState([]);
  const [allTeachers, setAllTeachers] = useState([]);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const levels = [1, 2, 3, 4];
  const terms = [1, 2];
  const sections = ['A', 'B', 'C'];

  const termRoman = (termVal) => {
    if (termVal === 1) return 'I';
    if (termVal === 2) return 'II';
    return termVal;
  };

  const fetchRoutineData = async () => {
    setLoading(true);
    setError('');
    try {
      // Fetch schedules matching selected criteria
      const scheduleRes = await routineApi.getAll(level, term, section);
      setSchedules(scheduleRes.data);

      // Fetch all courses and sessionals
      const [coursesRes, sessionalsRes, teachersRes] = await Promise.all([
        courseApi.getAll(),
        sessionalApi.getAll(),
        teacherApi.getAll()
      ]);

      // Filter courses & sessionals matching selected level & term
      const filteredCourses = coursesRes.data.filter(c => c.level === level && c.term === term);
      const filteredSessionals = sessionalsRes.data.filter(s => s.level === level && s.term === term);
      
      setAllCourses(filteredCourses);
      setAllSessionals(filteredSessionals);
      setAllTeachers(teachersRes.data);
    } catch (err) {
      console.error(err);
      setError('Could not fetch routing details. Ensure backend API is active.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRoutineData();
    // Default mock data updates based on section
    if (level === 3 && term === 2 && section === 'B') {
      setDpcG2('Md. Zahim Hassan');
      setDpcPhone('01736393334');
    } else {
      setDpcG2('Department Office');
      setDpcPhone('CSE Office');
    }
  }, [level, term, section]);

  const handlePrint = () => {
    window.print();
  };

  // Get de-duplicated teachers assigned to class schedules of this routine section
  const assignedTeachers = [];
  schedules.forEach(s => {
    s.teachers?.forEach(t => {
      // Find teacher info from allTeachers list to get full designation and name
      const fullTeacher = allTeachers.find(at => at.id === t.id);
      if (fullTeacher) {
        assignedTeachers.push(fullTeacher);
      } else {
        assignedTeachers.push(t);
      }
    });
  });

  return (
    <div>
      {/* Control Widgets Panel */}
      <div className="glass-card no-print" style={{ marginBottom: '2rem' }}>
        <h3 style={{ fontSize: '1.25rem', color: '#f0f6fc', marginBottom: '1.25rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <Calendar size={20} color="#008f4c" /> Filter Batch Routine
        </h3>
        
        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(130px, 1fr))',
          gap: '1rem',
          alignItems: 'end'
        }}>
          <div className="form-group">
            <label className="form-label">Level</label>
            <select className="form-select" value={level} onChange={e => setLevel(parseInt(e.target.value, 10))}>
              {levels.map(l => <option key={l} value={l}>Level {l}</option>)}
            </select>
          </div>

          <div className="form-group">
            <label className="form-label">Term</label>
            <select className="form-select" value={term} onChange={e => setTerm(parseInt(e.target.value, 10))}>
              {terms.map(t => <option key={t} value={t}>Term {termRoman(t)}</option>)}
            </select>
          </div>

          <div className="form-group">
            <label className="form-label">Section</label>
            <select className="form-select" value={section} onChange={e => setSection(e.target.value)}>
              {sections.map(s => <option key={s} value={s}>Section {s}</option>)}
            </select>
          </div>

          <div className="form-group">
            <label className="form-label">Routine Mode</label>
            <select className="form-select" value={routineMode} onChange={e => setRoutineMode(e.target.value)}>
              <option value="normal">Normal Days</option>
              <option value="ramadan">Ramadan Days</option>
            </select>
          </div>

          <button className="btn btn-primary" onClick={handlePrint} style={{ height: '44px', marginBottom: '1.25rem' }}>
            <Printer size={16} /> Print/Export Routine
          </button>
        </div>

        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
          gap: '1rem',
          marginTop: '1rem',
          borderTop: '1px solid rgba(255,255,255,0.08)',
          paddingTop: '1rem'
        }}>
          <div className="form-group">
            <label className="form-label">Batch Advisor</label>
            <input type="text" className="form-control" value={advisor} onChange={e => setAdvisor(e.target.value)} placeholder="Type name..." />
          </div>

          <div className="form-group">
            <label className="form-label">DPC/G2 Advisor</label>
            <input type="text" className="form-control" value={dpcG2} onChange={e => setDpcG2(e.target.value)} />
          </div>

          <div className="form-group">
            <label className="form-label">DPC Contact Phone</label>
            <input type="text" className="form-control" value={dpcPhone} onChange={e => setDpcPhone(e.target.value)} />
          </div>
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
          <div>Loading routine details...</div>
        </div>
      ) : (
        /* Printable DOCX replica sheet */
        <div className="routine-export-container">
          <UniversityHeader subTitle={`Batchwise Class Routine, ${activeSeason} ${activeYear} ${routineMode === 'ramadan' ? '(Ramadan Days)' : ''}`} />
          
          <div className="routine-meta-row">
            <div className="left">
              <div>Level-Term: <span style={{ textDecoration: 'underline', fontWeight: 'bold' }}>{level}-{termRoman(term)}</span></div>
              <div>Section: <span style={{ textDecoration: 'underline', fontWeight: 'bold' }}>{section}</span></div>
            </div>
            <div className="right">
              <div>Batch Advisor: <span style={{ textDecoration: 'underline' }}>{advisor || '                                  '}</span></div>
              <div>DPC/G2: <span style={{ textDecoration: 'underline', fontWeight: 'bold' }}>{dpcG2}</span> <span style={{ marginLeft: '2rem', textDecoration: 'underline' }}>{dpcPhone}</span></div>
            </div>
          </div>

          <RoutineTable schedules={schedules} mode={routineMode} />

          <div className="routine-tables-flex">
            <CourseTable courses={allCourses} sessionals={allSessionals} />
            <TeacherTable teachers={assignedTeachers} />
          </div>
        </div>
      )}
    </div>
  );
};

export default StudentRoutine;

import React, { useState, useEffect } from 'react';
import { Calendar, Printer } from 'lucide-react';
import { routineApi, sectionApi } from '../api/api';
import UniversityHeader from '../components/UniversityHeader';

const TIME_SLOTS_NORMAL = [
  { id: 1, label: "08.00-08.50" },
  { id: 2, label: "09.00-09.50" },
  { id: 3, label: "10.00-10.50" },
  { id: 4, label: "11.30-12.20" },
  { id: 5, label: "12.30-01.20" },
  { id: 6, label: "01.30-02.20" },
  { id: 7, label: "2.30-3.20" },
  { id: 8, label: "3.30-4.20" },
  { id: 9, label: "4.30-5.20" }
];

const TIME_SLOTS_RAMADAN = [
  { id: 1, label: "09.00-09.45" },
  { id: 2, label: "09.50-10.35" },
  { id: 3, label: "10.40-11.25" },
  { id: 4, label: "11.30-12.15" },
  { id: 5, label: "12.20-01.05" },
  { id: 6, label: "01.40-02.25" },
  { id: 7, label: "2.30-3.15" },
  { id: 8, label: "3.20-4.05" },
  { id: 9, label: "4.10-4.55" }
];

const DAYS_OF_WEEK = [
  { value: 0, label: "Sunday" },
  { value: 1, label: "Monday" },
  { value: 2, label: "Tuesday" },
  { value: 3, label: "Wednesday" },
  { value: 4, label: "Thursday" }
];

const getSlotIndex = (startTime) => {
  if (!startTime) return -1;
  const parts = startTime.split(':');
  const hour = parseInt(parts[0], 10);
  const minute = parseInt(parts[1], 10);

  if (hour === 8) return 0;
  if (hour === 9) return 1;
  if (hour === 10) return 2;
  if (hour === 11) return 3;
  if (hour === 12) return 4;
  if (hour === 13) return 5;
  if (hour === 14) return 6;
  if (hour === 15) return 7;
  if (hour === 16) return 8;

  return -1;
};

const getSlotSpan = (startTime, endTime) => {
  if (!startTime || !endTime) return 1;
  const startParts = startTime.split(':');
  const endParts = endTime.split(':');
  const startHour = parseInt(startParts[0], 10);
  const endHour = parseInt(endParts[1], 10);

  const diffHours = endHour - startHour;
  if (diffHours >= 2) return 3;
  return 1;
};

const MasterRoutine = () => {
  const [sections, setSections] = useState([]);
  const [schedules, setSchedules] = useState([]);
  
  const [routineMode, setRoutineMode] = useState('normal'); // 'normal' or 'ramadan'
  const [activeSeason, setActiveSeason] = useState(() => localStorage.getItem('activeSeason') || 'Winter');
  const [activeYear, setActiveYear] = useState(() => localStorage.getItem('activeYear') || '2026');

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const handleTermChange = () => {
      setActiveSeason(localStorage.getItem('activeSeason') || 'Winter');
      setActiveYear(localStorage.getItem('activeYear') || '2026');
    };
    window.addEventListener('academicTermChanged', handleTermChange);
    return () => window.removeEventListener('academicTermChanged', handleTermChange);
  }, []);

  const fetchAllData = async () => {
    setLoading(true);
    try {
      const [sectionsRes, schedulesRes] = await Promise.all([
        sectionApi.getAll(),
        routineApi.getAll()
      ]);
      setSections(sectionsRes.data);
      setSchedules(schedulesRes.data);
    } catch (err) {
      console.error(err);
      setError('Failed to fetch master schedule records from API database.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAllData();
  }, []);

  const termRoman = (termVal) => {
    if (termVal === 1) return 'I';
    if (termVal === 2) return 'II';
    return termVal;
  };

  const handlePrint = () => {
    window.print();
  };

  // Sort sections: Level ascending, Term ascending, Section letter ascending
  const sortedSections = [...sections].sort((a, b) => {
    if (a.level !== b.level) return a.level - b.level;
    if (a.term !== b.term) return a.term - b.term;
    return a.section.localeCompare(b.section);
  });

  const slots = routineMode === 'normal' ? TIME_SLOTS_NORMAL : TIME_SLOTS_RAMADAN;
  const isNormalMode = routineMode === 'normal';

  // Group schedules by day Index, section ID, and start slot
  // Key format: dayIndex_sectionId_startSlot
  const groupedSchedules = {};

  schedules.forEach(schedule => {
    let dayIndex = -1;
    if (typeof schedule.day === 'number') {
      dayIndex = schedule.day;
    } else if (typeof schedule.day === 'string') {
      const dayStr = schedule.day.toLowerCase();
      if (dayStr.includes('sun')) dayIndex = 0;
      else if (dayStr.includes('mon')) dayIndex = 1;
      else if (dayStr.includes('tue')) dayIndex = 2;
      else if (dayStr.includes('wed')) dayIndex = 3;
      else if (dayStr.includes('thu')) dayIndex = 4;
    }

    if (dayIndex < 0 || dayIndex > 4) return;

    const startSlot = getSlotIndex(schedule.startTime);
    if (startSlot < 0 || startSlot > 8) return;

    // Find the section that matches schedule's Level-Term-Section
    const matchingSection = sections.find(s => 
      s.level === schedule.level && 
      s.term === schedule.term && 
      s.section === schedule.section
    );

    if (!matchingSection) return;

    const key = `${dayIndex}_${matchingSection.id}_${startSlot}`;
    if (!groupedSchedules[key]) {
      groupedSchedules[key] = [];
    }
    groupedSchedules[key].push(schedule);
  });

  // Function to build cell matrix for a given day
  const getDayGrid = (dayIndex) => {
    // sectionId -> array of 9 slots
    const dayGrid = {};
    const coveredCells = {}; // sectionId_slotIdx -> true

    sortedSections.forEach(section => {
      dayGrid[section.id] = Array(9).fill(null);
      
      for (let slotIdx = 0; slotIdx < 9; slotIdx++) {
        const key = `${dayIndex}_${section.id}_${slotIdx}`;
        const schedulesList = groupedSchedules[key];
        
        if (coveredCells[`${section.id}_${slotIdx}`]) {
          dayGrid[section.id][slotIdx] = 'covered';
          continue;
        }

        if (schedulesList && schedulesList.length > 0) {
          const span = getSlotSpan(schedulesList[0].startTime, schedulesList[0].endTime);
          dayGrid[section.id][slotIdx] = {
            isMerged: schedulesList.length > 1,
            schedulesList,
            span
          };

          // Mark covered slots
          for (let s = 1; s < span; s++) {
            if (slotIdx + s < 9) {
              coveredCells[`${section.id}_${slotIdx + s}`] = true;
            }
          }
        }
      }
    });

    return dayGrid;
  };

  const renderCellContent = (cell) => {
    if (cell.isMerged) {
      return cell.schedulesList.map((sched, idx) => {
        const courseCode = sched.course?.courseCode || sched.sessional?.sessionalCode || '';
        const teachers = sched.teachers?.map(t => t.code).join(', ') || '';
        const room = sched.classroom?.roomNumber || sched.labroom?.roomNumber || '';
        const weekType = sched.weekType ? `#${sched.weekType}#` : '';
        
        return (
          <span key={idx}>
            {idx > 0 && '/'}
            <span className="class-info" style={{ fontSize: '0.75rem' }}>
              {courseCode} {teachers && `(${teachers})`} {weekType}
            </span>
            {room && <span className="class-room" style={{ display: 'inline', fontSize: '0.75rem' }}> [{room}]</span>}
          </span>
        );
      });
    } else {
      const courseCode = cell.schedulesList[0].course?.courseCode || cell.schedulesList[0].sessional?.sessionalCode || '';
      const teachers = cell.schedulesList[0].teachers?.map(t => t.code).join(', ') || '';
      const room = cell.schedulesList[0].classroom?.roomNumber || cell.schedulesList[0].labroom?.roomNumber || '';
      const weekType = cell.schedulesList[0].weekType ? `#${cell.schedulesList[0].weekType}#` : '';

      return (
        <>
          <div className="class-info" style={{ fontSize: '0.75rem' }}>
            {courseCode} {teachers && `(${teachers})`} {weekType}
          </div>
          {room && <span className="class-room" style={{ fontSize: '0.75rem' }}>[{room}]</span>}
        </>
      );
    }
  };

  return (
    <div>
      <div className="glass-card no-print" style={{ marginBottom: '2rem' }}>
        <h3 style={{ fontSize: '1.25rem', color: '#f0f6fc', marginBottom: '1.25rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <Calendar size={20} color="#008f4c" /> Master Weekly Routine
        </h3>
        
        <div style={{ display: 'flex', gap: '1rem', alignItems: 'end', flexWrap: 'wrap' }}>
          <div className="form-group" style={{ minWidth: '180px', marginBottom: 0 }}>
            <label className="form-label">Routine Mode</label>
            <select className="form-select" value={routineMode} onChange={e => setRoutineMode(e.target.value)} style={{ width: '100%' }}>
              <option value="normal">Normal Days</option>
              <option value="ramadan">Ramadan Days</option>
            </select>
          </div>

          <button className="btn btn-primary" onClick={handlePrint} disabled={loading || sections.length === 0}>
            <Printer size={16} /> Print Master Routine (PDF)
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
          <div>Loading weekly master schedule...</div>
        </div>
      ) : sections.length === 0 ? (
        <div style={{ textAlign: 'center', padding: '3rem', color: '#8b949e' }}>
          No cohort sections available to construct routine grids.
        </div>
      ) : (
        /* Printable Master sheet */
        <div className="routine-export-container" style={{ display: 'flex', flexDirection: 'column', gap: '4rem' }}>
          
          <UniversityHeader subTitle={`Master Weekly Class Routine — ${activeSeason} ${activeYear} ${routineMode === 'ramadan' ? '(Ramadan Days)' : ''}`} />
          
          <div style={{ fontSize: '0.9rem', color: '#333', textAlign: 'center', marginTop: '-1rem', marginBottom: '2rem', borderBottom: '1px solid #1a1a1a', paddingBottom: '0.5rem' }}>
            Bangladesh Army University of Science and Technology (BAUST), Saidpur — Dept. of CSE
          </div>

          {DAYS_OF_WEEK.map((day, dayIdx) => {
            const dayGrid = getDayGrid(day.value);
            return (
              <div key={day.value} style={{ pageBreakInside: 'avoid', marginBottom: '2rem' }}>
                <h4 style={{ fontSize: '1.1rem', fontWeight: 'bold', color: '#0b3d1b', borderBottom: '2px solid #1a1a1a', paddingBottom: '0.35rem', marginBottom: '1rem', textTransform: 'uppercase' }}>
                  {day.label} Schedule
                </h4>
                
                <div style={{ overflowX: 'auto' }}>
                  <table className="routine-grid-table" style={{ fontSize: '0.75rem', tableLayout: 'fixed' }}>
                    <thead>
                      <tr>
                        <th style={{ width: '80px' }}>Day</th>
                        <th style={{ width: '80px' }}>Section</th>
                        {/* Slots 1-3 */}
                        {slots.slice(0, 3).map(slot => (
                          <th key={slot.id}>{slot.label}</th>
                        ))}
                        {/* Break */}
                        {isNormalMode && <th style={{ width: '30px' }}>BREAK</th>}
                        {/* Slots 4-9 */}
                        {slots.slice(3).map(slot => (
                          <th key={slot.id}>{slot.label}</th>
                        ))}
                      </tr>
                    </thead>
                    <tbody>
                      {sortedSections.map((sec, secIdx) => {
                        const secRow = dayGrid[sec.id] || Array(9).fill(null);
                        const sectionLabel = `${sec.level}/${termRoman(sec.term)} - ${sec.section}`;
                        
                        return (
                          <tr key={sec.id}>
                            {/* Day column: Rendered only once at the beginning of the day table */}
                            {secIdx === 0 && (
                              <td 
                                rowSpan={sortedSections.length}
                                style={{
                                  writingMode: 'vertical-rl',
                                  textTransform: 'uppercase',
                                  letterSpacing: '1px',
                                  backgroundColor: '#f2f2f2',
                                  fontWeight: 'bold',
                                  fontSize: '0.85rem',
                                  textAlign: 'center',
                                  verticalAlign: 'middle',
                                  width: '45px',
                                  transform: 'rotate(180deg)'
                                }}
                              >
                                {day.label}
                              </td>
                            )}

                            {/* Section Label */}
                            <td className="day-cell" style={{ fontWeight: 'bold' }}>{sectionLabel}</td>

                            {/* Slots 1-3 */}
                            {secRow.slice(0, 3).map((cell, slotIdx) => {
                              if (cell === 'covered') return null;
                              if (!cell) return <td key={slotIdx} className="empty-cell"></td>;
                              const isLab = cell.isMerged ? cell.schedulesList.some(s => s.sessional) : !!cell.schedulesList[0].sessional;
                              return (
                                <td 
                                  key={slotIdx} 
                                  colSpan={cell.span}
                                  style={{
                                    backgroundColor: isLab ? '#f9f9f9' : '#ffffff',
                                    fontWeight: '500',
                                    padding: '0.35rem 0.2rem'
                                  }}
                                >
                                  {renderCellContent(cell)}
                                </td>
                              );
                            })}

                            {/* Vertical Break column (Normal Mode only) */}
                            {isNormalMode && secIdx === 0 && (
                              <td 
                                rowSpan={sortedSections.length}
                                style={{
                                  writingMode: 'vertical-rl',
                                  textTransform: 'uppercase',
                                  letterSpacing: '1px',
                                  backgroundColor: '#f2f2f2',
                                  fontWeight: 'bold',
                                  fontSize: '0.75rem',
                                  textAlign: 'center',
                                  verticalAlign: 'middle',
                                  width: '30px',
                                  color: '#4a4a4a',
                                  borderLeft: '1px solid #1a1a1a',
                                  borderRight: '1px solid #1a1a1a'
                                }}
                              >
                                BREAK (10.50- 11.30)
                              </td>
                            )}

                            {/* Slots 4-9 */}
                            {secRow.slice(3).map((cell, slotIdx) => {
                              const actualSlotIdx = slotIdx + 3;
                              if (cell === 'covered') return null;
                              if (!cell) return <td key={actualSlotIdx} className="empty-cell"></td>;
                              const isLab = cell.isMerged ? cell.schedulesList.some(s => s.sessional) : !!cell.schedulesList[0].sessional;
                              return (
                                <td 
                                  key={actualSlotIdx} 
                                  colSpan={cell.span}
                                  style={{
                                    backgroundColor: isLab ? '#f9f9f9' : '#ffffff',
                                    fontWeight: '500',
                                    padding: '0.35rem 0.2rem'
                                  }}
                                >
                                  {renderCellContent(cell)}
                                </td>
                              );
                            })}
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default MasterRoutine;

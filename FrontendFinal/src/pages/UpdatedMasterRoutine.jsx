import React, { useState, useEffect, useRef } from 'react';
import { Calendar, Printer } from 'lucide-react';
import { routineApi, sectionApi } from '../api/api';
import UniversityHeader from '../components/UniversityHeader';

import { downloadRoutineDocx } from '../utils/buildRoutineDoc';
import {
  TIME_SLOTS_NORMAL,
  TIME_SLOTS_RAMADAN,
  DAYS_OF_WEEK,
  getSlotIndex,
  getSlotSpan,
  termRoman,
  sortSections,
  groupSchedules,
  getDayGrid,
  getScheduleDisplay,
  getSectionLabel,
} from '../utils/routineUtils';

const UpdatedMasterRoutine = () => {
  const contentRef = useRef(null);
  const [sections, setSections] = useState([]);
  const [schedules, setSchedules] = useState([]);

  const [routineMode, setRoutineMode] = useState('normal'); // 'normal' or 'ramadan'
  const [activeSeason, setActiveSeason] = useState(
    () => localStorage.getItem('activeSeason') || 'Winter'
  );
  const [activeYear, setActiveYear] = useState(
    () => localStorage.getItem('activeYear') || '2026'
  );

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // ---------- DOCX export ----------
  const downloadDocx = async () => {
    if (!sections.length || !schedules.length) {
      alert('No routine data available.');
      return;
    }
    try {
      const blob = await downloadRoutineDocx({
        sections,
        schedules,
        routineMode,
        season: activeSeason,
        year: activeYear,
        logoUrl: '/baust_logo.png',
      });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `master-routine-${activeSeason}-${activeYear}.docx`;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      URL.revokeObjectURL(url);
    } catch (err) {
      console.error('DOCX generation failed:', err);
      alert('Failed to generate DOCX: ' + err.message);
    }
  };

  // ---------- Effects ----------
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
        routineApi.getAll(),
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

  // ---------- Handlers ----------
  const handlePrint = () => {
    window.print();
  };

  // ---------- Derived data ----------
  const sortedSections = sortSections(sections);
  const slots = routineMode === 'normal' ? TIME_SLOTS_NORMAL : TIME_SLOTS_RAMADAN;
  const isNormalMode = routineMode === 'normal';
  const groupedSchedules = groupSchedules(schedules, sections);

  // ---------- Cell renderer (on-screen view) ----------
  const renderCellContent = (cell) => {
    if (cell.isMerged) {
      return cell.schedulesList.map((sched, idx) => {
        const { courseCode, teachers, room, weekType } = getScheduleDisplay(sched);
        return (
          <span key={idx}>
            {idx > 0 && '/'}
            <span className="class-info" style={{ fontSize: '0.75rem' }}>
              {courseCode} {teachers && `(${teachers})`} {weekType}
            </span>
            {room && (
              <span
                className="class-room"
                style={{ display: 'inline', fontSize: '0.75rem' }}
              >
                {' '}
                [{room}]
              </span>
            )}
          </span>
        );
      });
    } else {
      const { courseCode, teachers, room, weekType } = getScheduleDisplay(
        cell.schedulesList[0]
      );
      return (
        <>
          <div className="class-info" style={{ fontSize: '0.75rem' }}>
            {courseCode} {teachers && `(${teachers})`} {weekType}
          </div>
          {room && (
            <span className="class-room" style={{ fontSize: '0.75rem' }}>
              [{room}]
            </span>
          )}
        </>
      );
    }
  };

  // ---------- Render ----------
  return (
    <div>
      <div className="glass-card no-print" style={{ marginBottom: '2rem' }}>
        <h3
          style={{
            fontSize: '1.25rem',
            color: '#f0f6fc',
            marginBottom: '1.25rem',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem',
          }}
        >
          <Calendar size={20} color="#008f4c" /> Master Weekly Routine
        </h3>

        <div
          style={{
            display: 'flex',
            gap: '1rem',
            alignItems: 'end',
            flexWrap: 'wrap',
          }}
        >
          <div
            className="form-group"
            style={{ minWidth: '180px', marginBottom: 0 }}
          >
            <label className="form-label">Routine Mode</label>
            <select
              className="form-select"
              value={routineMode}
              onChange={(e) => setRoutineMode(e.target.value)}
              style={{ width: '100%' }}
            >
              <option value="normal">Normal Days</option>
              <option value="ramadan">Ramadan Days</option>
            </select>
          </div>

          <button
            className="btn btn-primary"
            onClick={handlePrint}
            disabled={loading || sections.length === 0}
          >
            <Printer size={16} /> Print Master Routine (PDF)
          </button>
          <button
            className="btn btn-primary"
            onClick={downloadDocx}
            disabled={loading || sections.length === 0}
          >
            Download Master Routine (DOCX)
          </button>
        </div>
      </div>

      {error && (
        <div
          style={{
            backgroundColor: 'rgba(125, 26, 26, 0.2)',
            border: '1px solid #7d1a1a',
            color: '#ff8a8a',
            padding: '1rem',
            borderRadius: '8px',
            marginBottom: '2rem',
          }}
        >
          {error}
        </div>
      )}

      {loading ? (
        <div
          style={{
            display: 'flex',
            justifyContent: 'center',
            margin: '4rem 0',
            color: '#8b949e',
          }}
        >
          <div>Loading weekly master schedule...</div>
        </div>
      ) : sections.length === 0 ? (
        <div style={{ textAlign: 'center', padding: '3rem', color: '#8b949e' }}>
          No cohort sections available to construct routine grids.
        </div>
      ) : (
        /* Printable Master sheet */
        <div
          className="routine-export-container"
          ref={contentRef}
          style={{ display: 'flex', flexDirection: 'column', gap: '4rem' }}
        >
          <UniversityHeader
            subTitle={`Master Weekly Class Routine — ${activeSeason} ${activeYear} ${
              routineMode === 'ramadan' ? '(Ramadan Days)' : ''
            }`}
          />

          {DAYS_OF_WEEK.map((day) => {
            const dayGrid = getDayGrid(day.value, sortedSections, groupedSchedules);
            return (
              <div
                key={day.value}
                style={{ pageBreakInside: 'avoid', marginBottom: '2rem' }}
              >
                <h4
                  style={{
                    fontSize: '1.1rem',
                    fontWeight: 'bold',
                    color: '#0b3d1b',
                    borderBottom: '2px solid #1a1a1a',
                    paddingBottom: '0.35rem',
                    marginBottom: '1rem',
                    textTransform: 'uppercase',
                  }}
                >
                  {day.label} Schedule
                </h4>

                <div style={{ overflowX: 'auto' }}>
                  <table
                    className="routine-grid-table"
                    style={{ fontSize: '0.75rem', tableLayout: 'fixed' }}
                  >
                    <thead>
                      <tr>
                        <th style={{ width: '80px' }}>Day</th>
                        <th style={{ width: '80px' }}>Section</th>
                        {slots.slice(0, 3).map((slot) => (
                          <th key={slot.id}>{slot.label}</th>
                        ))}
                        {/* For the BREAK Column */}
                        {isNormalMode && <th style={{ width: '30px' }}></th>}
                        {slots.slice(3).map((slot) => (
                          <th key={slot.id}>{slot.label}</th>
                        ))}
                      </tr>
                    </thead>
                    <tbody>
                      {sortedSections.map((sec, secIdx) => {
                        const secRow = dayGrid[sec.id] || Array(9).fill(null);
                        const sectionLabel = getSectionLabel(sec);

                        return (
                          <tr key={sec.id}>
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
                                  transform: 'rotate(180deg)',
                                }}
                              >
                                {day.label}
                              </td>
                            )}

                            <td className="day-cell" style={{ fontWeight: 'bold' }}>
                              {sectionLabel}
                            </td>

                            {secRow.slice(0, 3).map((cell, slotIdx) => {
                              if (cell === 'covered') return null;
                              if (!cell)
                                return <td key={slotIdx} className="empty-cell"></td>;
                              const isLab = cell.isMerged
                                ? cell.schedulesList.some((s) => s.sessional)
                                : !!cell.schedulesList[0].sessional;
                              return (
                                <td
                                  key={slotIdx}
                                  colSpan={cell.span}
                                  style={{
                                    backgroundColor: isLab ? '#f9f9f9' : '#ffffff',
                                    fontWeight: '500',
                                    padding: '0.35rem 0.2rem',
                                  }}
                                >
                                  {renderCellContent(cell)}
                                </td>
                              );
                            })}

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
                                  borderRight: '1px solid #1a1a1a',
                                }}
                              >
                                BREAK (10.50- 11.30)
                              </td>
                            )}

                            {secRow.slice(3).map((cell, slotIdx) => {
                              const actualSlotIdx = slotIdx + 3;
                              if (cell === 'covered') return null;
                              if (!cell)
                                return (
                                  <td
                                    key={actualSlotIdx}
                                    className="empty-cell"
                                  ></td>
                                );
                              const isLab = cell.isMerged
                                ? cell.schedulesList.some((s) => s.sessional)
                                : !!cell.schedulesList[0].sessional;
                              return (
                                <td
                                  key={actualSlotIdx}
                                  colSpan={cell.span}
                                  style={{
                                    backgroundColor: isLab ? '#f9f9f9' : '#ffffff',
                                    fontWeight: '500',
                                    padding: '0.35rem 0.2rem',
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

export default UpdatedMasterRoutine;
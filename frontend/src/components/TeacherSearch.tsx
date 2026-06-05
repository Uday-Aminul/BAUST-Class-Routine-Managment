import React, { useState, useEffect } from 'react';
import { fetchTeacherById, fetchAllTeachers } from '../api';
import type { TeacherDto } from '../types';
import ScheduleGrid from './ScheduleGrid';
import { SearchableSelect } from './SearchableSelect';

export default function TeacherSearch() {
  const [teacherId, setTeacherId] = useState<number>(0);
  const [teacher, setTeacher] = useState<TeacherDto | null>(null);
  const [allTeachers, setAllTeachers] = useState<{id: number, label: string}[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchAllTeachers().then(data => {
      setAllTeachers(data.map(t => ({ id: t.id, label: `${t.name} ${t.code ? `(${t.code})` : ''}` })));
    }).catch(err => console.error("Failed to fetch teachers for search", err));
  }, []);

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    if (teacherId <= 0) { setError('Please select a Teacher.'); return; }
    setLoading(true); setError(null); setTeacher(null);
    try {
      const data = await fetchTeacherById(teacherId);
      setTeacher(data);
    } catch (err: any) {
      if (err?.response?.status === 404) setError(`No teacher found with ID ${teacherId}.`);
      else setError('Failed to fetch schedule. Is the API running?');
    } finally { setLoading(false); }
  }

  return (
    <div className="search-view">
      <form className="search-form glass-panel" onSubmit={handleSearch}>
        <div className="form-row">
          <div className="form-group form-group--grow">
            <label>Select Teacher</label>
            <SearchableSelect 
              value={teacherId} 
              onChange={setTeacherId} 
              options={allTeachers} 
              placeholder="-- Search by Name or Code --" 
            />
          </div>
          <button type="submit" className="btn-search" disabled={loading || !teacherId}>
            {loading ? <span className="spinner" /> : (
              <><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <circle cx="11" cy="11" r="8" /><path d="m21 21-4.35-4.35" />
              </svg>View Schedule</>
            )}
          </button>
        </div>
      </form>

      {error && (
        <div className="error-banner animate-fade-in">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>{error}
        </div>
      )}

      {teacher && !loading && (
        <div className="info-card glass-card animate-fade-in">
          <div className="info-card-avatar">{teacher.name.charAt(0).toUpperCase()}</div>
          <div className="info-card-body">
            <h3 className="info-card-name">{teacher.name}</h3>
            <div className="info-card-tags">
              {teacher.code && <span className="tag tag--green">{teacher.code}</span>}
              <span className="tag tag--gray">{teacher.designation}</span>
              <span className="tag tag--gold">{teacher.classes?.length ?? 0} classes/week</span>
            </div>
          </div>
        </div>
      )}

      {teacher && !loading && (
        teacher.classes && teacher.classes.length > 0
          ? <ScheduleGrid schedules={teacher.classes}
              title={teacher.name}
              subtitle={`${teacher.designation} · ${teacher.code ?? `ID ${teacher.id}`}`} />
          : <div className="empty-state animate-fade-in"><div className="empty-icon">🗓️</div>
              <p>No scheduled classes found for this teacher.</p></div>
      )}
    </div>
  );
}

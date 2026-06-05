import React, { useState } from 'react';
import { fetchAllSchedules } from '../api';
import type { ClassScheduleDto } from '../types';
import ScheduleGrid from './ScheduleGrid';

export default function RoomSearch() {
  const [roomInput, setRoomInput] = useState<string>('');
  const [roomType, setRoomType] = useState<'classroom' | 'labroom'>('classroom');
  const [schedules, setSchedules] = useState<ClassScheduleDto[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [searchedRoom, setSearchedRoom] = useState<number | null>(null);

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    const roomNum = parseInt(roomInput, 10);
    if (isNaN(roomNum) || roomNum <= 0) { setError('Please enter a valid room number.'); return; }
    setLoading(true); setError(null); setSchedules(null); setSearchedRoom(roomNum);
    try {
      const all = await fetchAllSchedules();
      const filtered = all.filter((s) =>
        roomType === 'classroom'
          ? s.classroom?.roomNumber === roomNum
          : s.labroom?.roomNumber === roomNum
      );
      setSchedules(filtered);
    } catch {
      setError('Failed to fetch schedules. Is the API running?');
    } finally { setLoading(false); }
  }

  const roomLabel = roomType === 'classroom' ? 'Classroom' : 'Lab Room';

  return (
    <div className="search-view">
      <form className="search-form glass-panel" onSubmit={handleSearch}>
        <div className="form-row">
          <div className="form-group">
            <label htmlFor="r-type">Room Type</label>
            <div className="select-wrapper">
              <select id="r-type" value={roomType}
                onChange={(e) => setRoomType(e.target.value as 'classroom' | 'labroom')}>
                <option value="classroom">Classroom</option>
                <option value="labroom">Lab Room</option>
              </select>
            </div>
          </div>
          <div className="form-group form-group--grow">
            <label htmlFor="r-num">Room Number</label>
            <input id="r-num" type="number" min="1" placeholder="e.g. 301"
              value={roomInput} onChange={(e) => setRoomInput(e.target.value)} className="text-input" />
          </div>
          <button type="submit" className="btn-search" disabled={loading || !roomInput}>
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

      {schedules !== null && !loading && (
        schedules.length === 0
          ? <div className="empty-state animate-fade-in">
              <div className="empty-icon">🚪</div>
              <p>No schedule found for {roomLabel} {searchedRoom}</p>
            </div>
          : <>
              <div className="info-card glass-card animate-fade-in">
                <div className="info-card-avatar room-avatar">
                  {roomType === 'classroom' ? '🏫' : '🔬'}
                </div>
                <div className="info-card-body">
                  <h3 className="info-card-name">{roomLabel} {searchedRoom}</h3>
                  <div className="info-card-tags">
                    <span className="tag tag--green">{schedules.length} slots/week</span>
                    <span className="tag tag--gray">
                      {[...new Set(schedules.flatMap(s => s.teachers?.map(t => t.code ?? t.name) ?? []))].join(' · ')}
                    </span>
                  </div>
                </div>
              </div>
              <ScheduleGrid schedules={schedules}
                title={`${roomLabel} ${searchedRoom}`}
                subtitle="Weekly Room Occupancy · BAUST CSE" />
            </>
      )}
    </div>
  );
}

import React, { useState } from 'react';
import { fetchSectionSchedule, downloadRoutineDocx } from '../api';
import type { ClassScheduleDto } from '../types';
import ScheduleGrid from './ScheduleGrid';

const SECTIONS = ['A', 'B', 'C', 'D', 'E', 'F'];
const LEVELS = [1, 2, 3, 4];
const TERMS = [1, 2];

export default function SectionSearch() {
  const [level, setLevel] = useState<number>(3);
  const [term, setTerm] = useState<number>(2);
  const [section, setSection] = useState<string>('B');
  const [schedules, setSchedules] = useState<ClassScheduleDto[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [docxLoading, setDocxLoading] = useState(false);

  async function handleDocxDownload() {
    setDocxLoading(true);
    try {
      await downloadRoutineDocx(level, term, section);
    } finally {
      setDocxLoading(false);
    }
  }

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSchedules(null);
    try {
      const data = await fetchSectionSchedule(level, term, section);
      setSchedules(data);
    } catch {
      setError('Failed to fetch schedule. Please check that the API is running.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="search-view">
      {/* Search Form */}
      <form className="search-form glass-panel" onSubmit={handleSearch}>
        <div className="form-row">
          <div className="form-group">
            <label htmlFor="s-level">Level</label>
            <div className="select-wrapper">
              <select
                id="s-level"
                value={level}
                onChange={(e) => setLevel(Number(e.target.value))}
              >
                {LEVELS.map((l) => (
                  <option key={l} value={l}>Level {l}</option>
                ))}
              </select>
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="s-term">Term</label>
            <div className="select-wrapper">
              <select
                id="s-term"
                value={term}
                onChange={(e) => setTerm(Number(e.target.value))}
              >
                {TERMS.map((t) => (
                  <option key={t} value={t}>Term {t}</option>
                ))}
              </select>
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="s-section">Section</label>
            <div className="select-wrapper">
              <select
                id="s-section"
                value={section}
                onChange={(e) => setSection(e.target.value)}
              >
                {SECTIONS.map((s) => (
                  <option key={s} value={s}>{s}</option>
                ))}
              </select>
            </div>
          </div>

          <button type="submit" className="btn-search" disabled={loading}>
            {loading ? (
              <span className="spinner" />
            ) : (
              <>
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                  <circle cx="11" cy="11" r="8" /><path d="m21 21-4.35-4.35" />
                </svg>
                View Schedule
              </>
            )}
          </button>

          {schedules && schedules.length > 0 && (
            <button
              type="button"
              className="btn-action btn-docx"
              onClick={handleDocxDownload}
              disabled={docxLoading}
            >
              {docxLoading ? <span className="spinner" /> : '📄'}
              {docxLoading ? 'Downloading…' : 'Download DOCX'}
            </button>
          )}
        </div>
      </form>

      {/* Error */}
      {error && (
        <div className="error-banner animate-fade-in">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>
          {error}
        </div>
      )}

      {/* Results */}
      {schedules !== null && !loading && (
        schedules.length === 0 ? (
          <div className="empty-state animate-fade-in">
            <div className="empty-icon">📅</div>
            <p>No schedule found for Level {level} Term {term} Section {section}</p>
          </div>
        ) : (
          <ScheduleGrid
            schedules={schedules}
            title={`Level ${level} | Term ${term} | Section ${section}`}
            subtitle="Weekly Class Routine · BAUST CSE"
          />
        )
      )}
    </div>
  );
}

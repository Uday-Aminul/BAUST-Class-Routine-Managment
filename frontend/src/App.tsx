import { useState } from 'react';
import type { SearchMode } from './types';
import SectionSearch from './components/SectionSearch';
import TeacherSearch from './components/TeacherSearch';
import RoomSearch from './components/RoomSearch';
import TeacherAdmin from './components/TeacherAdmin';
import { generateAllSchedules, downloadAllRoutinesXlsx, downloadMasterRoutineDocx } from './api';
import './App.css';

const TABS: { id: SearchMode; label: string; icon: string; desc: string }[] = [
  { id: 'section',  label: 'By Section',  icon: '🎓', desc: 'Level · Term · Section' },
  { id: 'teacher',  label: 'By Teacher',  icon: '👨‍🏫', desc: 'Teacher ID' },
  { id: 'room',     label: 'By Room',     icon: '🏫', desc: 'Room Number' },
  { id: 'admin',    label: 'Teacher Admin', icon: '⚙️', desc: 'Manage Teachers' },
];

export default function App() {
  const [activeTab, setActiveTab] = useState<SearchMode>('section');
  const [generating, setGenerating] = useState(false);
  const [genResults, setGenResults] = useState<string[] | null>(null);
  const [genError, setGenError] = useState<string | null>(null);
  const [xlsxLoading, setXlsxLoading] = useState(false);
  const [docxLoading, setDocxLoading] = useState(false);

  async function handleGenerate() {
    setGenerating(true);
    setGenResults(null);
    setGenError(null);
    try {
      const results = await generateAllSchedules();
      setGenResults(results);
    } catch {
      setGenError('Generation failed. Check API connection.');
    } finally {
      setGenerating(false);
    }
  }

  async function handleDownloadXlsx() {
    setXlsxLoading(true);
    try {
      await downloadAllRoutinesXlsx();
    } catch {
      setGenError('Download failed.');
    } finally {
      setXlsxLoading(false);
    }
  }

  async function handleDownloadMasterDocx() {
    setDocxLoading(true);
    try {
      await downloadMasterRoutineDocx();
    } catch {
      setGenError('Download failed.');
    } finally {
      setDocxLoading(false);
    }
  }

  return (
    <div className="app-root">

      {/* ── Header ── */}
      <header className="app-header">
        <div className="header-inner">
          <div className="header-brand">
            <img
              src="/BAUST-1.png"
              alt="BAUST"
              className="brand-logo"
              onError={(e) => {
                e.currentTarget.style.display = 'none';
                (e.currentTarget.nextElementSibling as HTMLElement | null)?.style.setProperty('display', 'flex');
              }}
            />
            {/* fallback emblem shown if logo file missing */}
            <div className="brand-emblem-fallback" style={{ display: 'none' }}>
              <svg viewBox="0 0 44 44" fill="none" className="emblem-svg">
                <rect width="44" height="44" rx="22" fill="var(--baust-green)" opacity="0.18"/>
                <path d="M9 32 L22 10 L35 32 Z" stroke="var(--baust-green-light)" strokeWidth="2.5" fill="none"/>
                <rect x="17" y="25" width="10" height="7" fill="var(--baust-green-light)" rx="1"/>
                <circle cx="22" cy="19" r="2.5" fill="var(--baust-gold)"/>
              </svg>
            </div>

            <div className="brand-text">
              <span className="brand-university">
                Bangladesh Army University of Science and Technology (BAUST), Saidpur
              </span>
              <span className="brand-dept">
                Department of Computer Science and Engineering (CSE)
              </span>
              <span className="brand-system">Class Routine Management System</span>
            </div>
          </div>
        </div>
      </header>

      {/* ── Hero Banner ── */}
      <div className="hero-banner">
        <div className="hero-content">
          <h1 className="hero-title">
            <span className="text-gradient-green">Weekly Class Routine</span>
          </h1>
          <p className="hero-sub">
            View scheduled classes by section, teacher, or room
          </p>

          {/* Admin action buttons */}
          <div className="hero-actions">
            <button
              className="btn-action btn-generate"
              onClick={handleGenerate}
              disabled={generating}
            >
              {generating ? <span className="spinner" /> : '⚡'}
              {generating ? 'Generating…' : 'Generate Routine'}
            </button>

            <button
              className="btn-action btn-xlsx"
              onClick={handleDownloadXlsx}
              disabled={xlsxLoading || docxLoading}
            >
              {xlsxLoading ? <span className="spinner" /> : '📊'}
              {xlsxLoading ? 'Downloading…' : 'Download All (XLSX)'}
            </button>

            <button
              className="btn-action btn-xlsx"
              onClick={handleDownloadMasterDocx}
              disabled={xlsxLoading || docxLoading}
              style={{ background: 'linear-gradient(135deg, #2b5876 0%, #4e4376 100%)' }}
            >
              {docxLoading ? <span className="spinner" /> : '📄'}
              {docxLoading ? 'Downloading…' : 'Master Routine (DOCX)'}
            </button>
          </div>

          {/* Generation results */}
          {genError && (
            <div className="gen-error animate-fade-in">{genError}</div>
          )}
          {genResults && (
            <div className="gen-results animate-fade-in">
              {genResults.map((r, i) => (
                <div key={i} className={`gen-result-item ${r.toLowerCase().includes('error') || r.toLowerCase().includes('partial') ? 'warn' : 'ok'}`}>
                  {r.toLowerCase().includes('error') || r.toLowerCase().includes('partial') ? '⚠️' : '✅'} {r}
                </div>
              ))}
            </div>
          )}
        </div>
        <div className="hero-glow" />
      </div>

      {/* ── Tabs ── */}
      <div className="tabs-bar">
        <div className="tabs-inner">
          {TABS.map((tab) => (
            <button
              key={tab.id}
              id={`tab-${tab.id}`}
              className={`tab-btn${activeTab === tab.id ? ' tab-btn--active' : ''}`}
              onClick={() => setActiveTab(tab.id)}
            >
              <span className="tab-icon">{tab.icon}</span>
              <span className="tab-label">{tab.label}</span>
              <span className="tab-desc">{tab.desc}</span>
            </button>
          ))}
        </div>
      </div>

      {/* ── Content ── */}
      <main className="app-main">
        {activeTab === 'section' && <SectionSearch />}
        {activeTab === 'teacher' && <TeacherSearch />}
        {activeTab === 'room'    && <RoomSearch />}
        {activeTab === 'admin'   && <TeacherAdmin />}
      </main>

      {/* ── Footer ── */}
      <footer className="app-footer">
        <span>Bangladesh Army University of Science and Technology (BAUST)</span>
        <span className="dept-dot" />
        <span>Department of Computer Science and Engineering</span>
        <span className="dept-dot" />
        <span>Saidpur</span>
      </footer>
    </div>
  );
}

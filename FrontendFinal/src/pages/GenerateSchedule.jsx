import React, { useState, useRef } from 'react';
import {
  Cpu,
  AlertCircle,
  Play,
  CheckCircle2,
  Upload,
  FileText,
  Trash2,
} from 'lucide-react';
import { routineApi } from '../api/api';
import { Link } from 'react-router-dom';

const GenerateSchedule = () => {
  // ---- Solver state ----
  const [running, setRunning] = useState(false);
  const [logs, setLogs] = useState([]);
  const [error, setError] = useState('');
  const [finished, setFinished] = useState(false);

  // ---- Import DOCX state ----
  const fileInputRef = useRef(null);
  const [selectedFile, setSelectedFile] = useState(null);
  const [uploading, setUploading] = useState(false);
  const [uploadMessages, setUploadMessages] = useState(null);
  const [uploadError, setUploadError] = useState('');

  // ---- Delete-all state ----
  const [clearing, setClearing] = useState(false);
  const [clearMessage, setClearMessage] = useState(null);

  // ---- Solver handler ----
  const handleGenerate = async () => {
    setRunning(true);
    setFinished(false);
    setError('');
    setLogs([
      'Initiating solver algorithm...',
      'Clearing existing schedule registry...',
    ]);

    try {
      const res = await routineApi.generateAll();
      setLogs((prev) => [
        ...prev,
        'Running schedule logic constraints placement...',
        ...res.data,
        'Schedule solver finished execution successfully!',
      ]);
      setFinished(true);
    } catch (err) {
      console.error(err);
      setError(
        'Solver failed to complete. Verify server is active and configuration limits are satisfied.'
      );
      setLogs((prev) => [
        ...prev,
        'Solver crashed due to internal database constraints error.',
      ]);
    } finally {
      setRunning(false);
    }
  };

  // ---- DOCX handlers ----
  const handleFilePick = (e) => {
    const file = e.target.files?.[0] || null;
    setSelectedFile(file);
    setUploadMessages(null);
    setUploadError('');
  };

  const handleUploadDocx = async () => {
    if (!selectedFile) {
      setUploadError('Please choose a DOCX file first.');
      return;
    }
    setUploading(true);
    setUploadError('');
    setUploadMessages(null);
    try {
      const res = await routineApi.importMasterDocx(selectedFile);
      setUploadMessages(res.data || []);
    } catch (err) {
      console.error(err);
      setUploadError(
        err.response?.data?.message ||
          'Upload failed. Verify the file is a valid master routine DOCX.'
      );
    } finally {
      setUploading(false);
    }
  };

  // ---- Delete-all handler ----
  const handleDeleteAll = async () => {
    if (!window.confirm('Delete ALL class schedules? This cannot be undone.')) {
      return;
    }
    setClearing(true);
    setClearMessage(null);
    try {
      const res = await routineApi.deleteAll();
      setClearMessage(
        typeof res.data === 'string' ? res.data : 'Schedules cleared.'
      );
    } catch (err) {
      console.error(err);
      setClearMessage('Failed to delete schedules.');
    } finally {
      setClearing(false);
    }
  };

  return (
    <div style={{ maxWidth: '800px', margin: '0 auto' }}>
      <div style={{ marginBottom: '2.5rem', textAlign: 'center' }}>
        <div
          style={{
            display: 'inline-flex',
            backgroundColor: 'rgba(0, 143, 76, 0.12)',
            color: '#00b35e',
            padding: '1.25rem',
            borderRadius: '50%',
            marginBottom: '1rem',
            boxShadow: '0 0 20px rgba(0, 143, 76, 0.2)',
          }}
        >
          <Cpu size={48} />
        </div>
        <h1
          style={{
            fontSize: '2.2rem',
            fontWeight: 800,
            color: '#f0f6fc',
            marginBottom: '0.75rem',
          }}
        >
          Routine Placement Solver
        </h1>
        <p
          style={{
            color: '#8b949e',
            fontSize: '1.1rem',
            maxWidth: '600px',
            margin: '0 auto',
          }}
        >
          Trigger the backtracking scheduler solver. The system will automatically
          check teacher availabilities, assign rooms, and schedule theory and lab
          sessions for all sections.
        </p>
      </div>

      <div
        className="glass-card"
        style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}
      >
        <div
          style={{
            backgroundColor: 'rgba(245, 158, 11, 0.1)',
            border: '1px solid rgba(245, 158, 11, 0.25)',
            borderRadius: '8px',
            padding: '1rem',
            display: 'flex',
            gap: '0.75rem',
            alignItems: 'flex-start',
            color: '#f59e0b',
            fontSize: '0.95rem',
          }}
        >
          <AlertCircle size={20} style={{ flexShrink: 0, marginTop: '0.1rem' }} />
          <div>
            <strong>Warning:</strong> Running the solver will delete all currently
            saved schedules and regenerate routines from scratch based on current
            course and teacher assignments.
          </div>
        </div>

        <button
          className="btn btn-primary"
          onClick={handleGenerate}
          disabled={running}
          style={{
            padding: '1rem 2rem',
            fontSize: '1.1rem',
            fontWeight: '600',
            gap: '0.75rem',
            borderRadius: '10px',
            alignSelf: 'center',
            boxShadow: '0 4px 14px rgba(0, 143, 76, 0.4)',
          }}
        >
          <Play size={18} fill="currentColor" />{' '}
          {running ? 'Generating Schedules...' : 'Run Automated Solver'}
        </button>

        {/* Divider */}
        <div
          style={{
            display: 'flex',
            alignItems: 'center',
            gap: '1rem',
            margin: '0.5rem 0',
          }}
        >
          <div
            style={{
              flex: 1,
              height: '1px',
              backgroundColor: 'rgba(255,255,255,0.1)',
            }}
          />
          <span
            style={{ color: '#8b949e', fontSize: '0.85rem', fontWeight: 500 }}
          >
            OR
          </span>
          <div
            style={{
              flex: 1,
              height: '1px',
              backgroundColor: 'rgba(255,255,255,0.1)',
            }}
          />
        </div>

        {/* Import edited DOCX section */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div>
            <h4
              style={{
                color: '#f0f6fc',
                fontSize: '1.05rem',
                marginBottom: '0.25rem',
              }}
            >
              Import Edited Routine
            </h4>
            <p style={{ color: '#8b949e', fontSize: '0.9rem', margin: 0 }}>
              Download a routine DOCX, edit it in Word, then upload it here to
              replace the database.
            </p>
          </div>

          <div
            style={{
              display: 'flex',
              gap: '0.75rem',
              alignItems: 'center',
              flexWrap: 'wrap',
            }}
          >
            <input
              ref={fileInputRef}
              type="file"
              accept=".docx"
              onChange={handleFilePick}
              style={{ display: 'none' }}
            />
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => fileInputRef.current?.click()}
              disabled={uploading}
              style={{ gap: '0.5rem' }}
            >
              <FileText size={16} /> {selectedFile ? 'Change File' : 'Choose DOCX'}
            </button>

            {selectedFile && (
              <span style={{ color: '#c9d1d9', fontSize: '0.9rem' }}>
                {selectedFile.name} ({(selectedFile.size / 1024).toFixed(1)} KB)
              </span>
            )}

            <button
              type="button"
              className="btn btn-primary"
              onClick={handleUploadDocx}
              disabled={uploading || !selectedFile}
              style={{ gap: '0.5rem' }}
            >
              <Upload size={16} /> {uploading ? 'Uploading...' : 'Upload & Replace'}
            </button>
          </div>

          {uploadError && (
            <div
              style={{
                backgroundColor: 'rgba(125, 26, 26, 0.2)',
                border: '1px solid #7d1a1a',
                color: '#ff8a8a',
                padding: '0.75rem 1rem',
                borderRadius: '8px',
                fontSize: '0.9rem',
              }}
            >
              {uploadError}
            </div>
          )}

          {uploadMessages !== null &&
            (uploadMessages.length === 0 ? (
              <div
                style={{
                  backgroundColor: 'rgba(0, 143, 76, 0.12)',
                  border: '1px solid rgba(0, 143, 76, 0.35)',
                  color: '#56d364',
                  padding: '0.75rem 1rem',
                  borderRadius: '8px',
                  fontSize: '0.9rem',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '0.5rem',
                }}
              >
                <CheckCircle2 size={16} /> Import succeeded. No issues found.
              </div>
            ) : (
              <div
                style={{
                  backgroundColor: 'rgba(245, 158, 11, 0.1)',
                  border: '1px solid rgba(245, 158, 11, 0.3)',
                  borderRadius: '8px',
                  padding: '1rem',
                  color: '#f59e0b',
                  fontSize: '0.9rem',
                }}
              >
                <div style={{ fontWeight: 600, marginBottom: '0.5rem' }}>
                  Imported with {uploadMessages.length} issue
                  {uploadMessages.length === 1 ? '' : 's'}:
                </div>
                <ul
                  style={{
                    margin: 0,
                    paddingLeft: '1.25rem',
                    display: 'flex',
                    flexDirection: 'column',
                    gap: '0.35rem',
                  }}
                >
                  {uploadMessages.map((msg, i) => (
                    <li key={i}>{msg}</li>
                  ))}
                </ul>
              </div>
            ))}
        </div>

        {/* Divider */}
        <div
          style={{
            display: 'flex',
            alignItems: 'center',
            gap: '1rem',
            margin: '0.5rem 0',
          }}
        >
          <div
            style={{
              flex: 1,
              height: '1px',
              backgroundColor: 'rgba(255,255,255,0.1)',
            }}
          />
          <span
            style={{ color: '#8b949e', fontSize: '0.85rem', fontWeight: 500 }}
          >
            OR
          </span>
          <div
            style={{
              flex: 1,
              height: '1px',
              backgroundColor: 'rgba(255,255,255,0.1)',
            }}
          />
        </div>

        {/* Clear all section */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div>
            <h4
              style={{
                color: '#f0f6fc',
                fontSize: '1.05rem',
                marginBottom: '0.25rem',
              }}
            >
              Clear All Schedules
            </h4>
            <p style={{ color: '#8b949e', fontSize: '0.9rem', margin: 0 }}>
              Permanently remove every class schedule from the database. Use this
              before re-importing a fresh routine.
            </p>
          </div>

          <button
            type="button"
            className="btn btn-secondary"
            onClick={handleDeleteAll}
            disabled={clearing}
            style={{
              alignSelf: 'flex-start',
              gap: '0.5rem',
              borderColor: '#7d1a1a',
              color: '#ff8a8a',
            }}
          >
            <Trash2 size={16} /> {clearing ? 'Deleting...' : 'Delete All Schedules'}
          </button>

          {clearMessage && (
            <div
              style={{
                backgroundColor: 'rgba(245, 158, 11, 0.1)',
                border: '1px solid rgba(245, 158, 11, 0.3)',
                color: '#f59e0b',
                padding: '0.75rem 1rem',
                borderRadius: '8px',
                fontSize: '0.9rem',
              }}
            >
              {clearMessage}
            </div>
          )}
        </div>

        {error && (
          <div
            style={{
              backgroundColor: 'rgba(125, 26, 26, 0.2)',
              border: '1px solid #7d1a1a',
              color: '#ff8a8a',
              padding: '1rem',
              borderRadius: '8px',
              marginTop: '1rem',
            }}
          >
            {error}
          </div>
        )}

        {/* Execution Logs */}
        {logs.length > 0 && (
          <div style={{ marginTop: '1rem' }}>
            <span
              style={{
                display: 'block',
                fontSize: '0.9rem',
                color: '#8b949e',
                marginBottom: '0.5rem',
                fontWeight: '500',
              }}
            >
              Solver Execution Console Logs:
            </span>
            <div
              style={{
                backgroundColor: '#090d13',
                border: '1px solid rgba(255,255,255,0.08)',
                borderRadius: '8px',
                padding: '1.25rem',
                fontFamily: 'monospace',
                fontSize: '0.85rem',
                color: '#c9d1d9',
                display: 'flex',
                flexDirection: 'column',
                gap: '0.5rem',
                maxHeight: '280px',
                overflowY: 'auto',
              }}
            >
              {logs.map((log, index) => {
                const isSuccess =
                  log.includes('succesfull') || log.includes('finished');
                const isError =
                  log.includes('Error') || log.includes('crashed');

                let textColor = '#c9d1d9';
                if (isSuccess) textColor = '#56d364';
                else if (isError) textColor = '#f85149';
                else if (log.includes('Clearing') || log.includes('Initiating'))
                  textColor = '#79c0ff';

                return (
                  <div
                    key={index}
                    style={{
                      color: textColor,
                      display: 'flex',
                      alignItems: 'center',
                      gap: '0.5rem',
                    }}
                  >
                    {isSuccess && (
                      <CheckCircle2 size={14} style={{ color: '#56d364' }} />
                    )}
                    <span>{log}</span>
                  </div>
                );
              })}
            </div>
          </div>
        )}

        {finished && (
          <div
            style={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              gap: '1rem',
              marginTop: '1.5rem',
              paddingTop: '1.5rem',
              borderTop: '1px solid rgba(255,255,255,0.08)',
            }}
          >
            <h4 style={{ color: '#f0f6fc', fontSize: '1.1rem' }}>
              Timetables Generated Successfully!
            </h4>
            <div style={{ display: 'flex', gap: '1rem' }}>
              <Link to="/student-routine" className="btn btn-secondary">
                View Batch Routines
              </Link>
              <Link to="/teacher-routine" className="btn btn-secondary">
                View Teacher Schedules
              </Link>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default GenerateSchedule;
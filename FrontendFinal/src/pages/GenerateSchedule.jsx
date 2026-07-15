import React, { useState } from 'react';
import { Cpu, AlertCircle, Play, CheckCircle2 } from 'lucide-react';
import { routineApi } from '../api/api';
import { Link } from 'react-router-dom';

const GenerateSchedule = () => {
  const [running, setRunning] = useState(false);
  const [logs, setLogs] = useState([]);
  const [error, setError] = useState('');
  const [finished, setFinished] = useState(false);

  const handleGenerate = async () => {
    setRunning(true);
    setFinished(false);
    setError('');
    setLogs(['Initiating solver algorithm...', 'Clearing existing schedule registry...']);
    
    try {
      const res = await routineApi.generateAll();
      setLogs(prev => [
        ...prev,
        'Running schedule logic constraints placement...',
        ...res.data,
        'Schedule solver finished execution successfully!'
      ]);
      setFinished(true);
    } catch (err) {
      console.error(err);
      setError('Solver failed to complete. Verify server is active and configuration limits are satisfied.');
      setLogs(prev => [...prev, 'Solver crashed due to internal database constraints error.']);
    } finally {
      setRunning(false);
    }
  };

  return (
    <div style={{ maxWidth: '800px', margin: '0 auto' }}>
      <div style={{ marginBottom: '2.5rem', textAlign: 'center' }}>
        <div style={{
          display: 'inline-flex',
          backgroundColor: 'rgba(0, 143, 76, 0.12)',
          color: '#00b35e',
          padding: '1.25rem',
          borderRadius: '50%',
          marginBottom: '1rem',
          boxShadow: '0 0 20px rgba(0, 143, 76, 0.2)'
        }}>
          <Cpu size={48} />
        </div>
        <h1 style={{ fontSize: '2.2rem', fontWeight: 800, color: '#f0f6fc', marginBottom: '0.75rem' }}>
          Routine Placement Solver
        </h1>
        <p style={{ color: '#8b949e', fontSize: '1.1rem', maxWidth: '600px', margin: '0 auto' }}>
          Trigger the backtracking scheduler solver. The system will automatically check teacher availabilities, assign rooms, and schedule theory and lab sessions for all sections.
        </p>
      </div>

      <div className="glass-card" style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
        <div style={{
          backgroundColor: 'rgba(245, 158, 11, 0.1)',
          border: '1px solid rgba(245, 158, 11, 0.25)',
          borderRadius: '8px',
          padding: '1rem',
          display: 'flex',
          gap: '0.75rem',
          alignItems: 'flex-start',
          color: '#f59e0b',
          fontSize: '0.95rem'
        }}>
          <AlertCircle size={20} style={{ flexShrink: 0, marginTop: '0.1rem' }} />
          <div>
            <strong>Warning:</strong> Running the solver will delete all currently saved schedules and regenerate routines from scratch based on current course and teacher assignments.
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
            boxShadow: '0 4px 14px rgba(0, 143, 76, 0.4)'
          }}
        >
          <Play size={18} fill="currentColor" /> {running ? 'Generating Schedules...' : 'Run Automated Solver'}
        </button>

        {error && (
          <div style={{
            backgroundColor: 'rgba(125, 26, 26, 0.2)',
            border: '1px solid #7d1a1a',
            color: '#ff8a8a',
            padding: '1rem',
            borderRadius: '8px',
            marginTop: '1rem'
          }}>
            {error}
          </div>
        )}

        {/* Execution Logs */}
        {logs.length > 0 && (
          <div style={{ marginTop: '1rem' }}>
            <span style={{ display: 'block', fontSize: '0.9rem', color: '#8b949e', marginBottom: '0.5rem', fontWeight: '500' }}>
              Solver Execution Console Logs:
            </span>
            <div style={{
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
              overflowY: 'auto'
            }}>
              {logs.map((log, index) => {
                const isSuccess = log.includes('succesfull') || log.includes('finished');
                const isError = log.includes('Error') || log.includes('crashed');
                
                let textColor = '#c9d1d9';
                if (isSuccess) textColor = '#56d364';
                else if (isError) textColor = '#f85149';
                else if (log.includes('Clearing') || log.includes('Initiating')) textColor = '#79c0ff';

                return (
                  <div key={index} style={{ color: textColor, display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                    {isSuccess && <CheckCircle2 size={14} style={{ color: '#56d364' }} />}
                    <span>{log}</span>
                  </div>
                );
              })}
            </div>
          </div>
        )}

        {finished && (
          <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            gap: '1rem',
            marginTop: '1.5rem',
            paddingTop: '1.5rem',
            borderTop: '1px solid rgba(255,255,255,0.08)'
          }}>
            <h4 style={{ color: '#f0f6fc', fontSize: '1.1rem' }}>Timetables Generated Successfully!</h4>
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

import React from 'react';
import type { ClassScheduleDto, ClassScheduleDtoForTeacher } from '../types';
import { DAYS, TIME_SLOTS } from '../types';

type AnySchedule = ClassScheduleDto | ClassScheduleDtoForTeacher;

interface Props {
  schedules: AnySchedule[];
  title?: string;
  subtitle?: string;
}

// Normalize time string from API ("HH:mm:ss" or "HH:mm")
function normalizeTime(t: string): string {
  if (!t) return '';
  // take only HH:mm:ss
  return t.slice(0, 8).padEnd(8, ':00');
}

function timesOverlap(slotStart: string, slotEnd: string, schStart: string, schEnd: string): boolean {
  const toMins = (t: string) => {
    const [h, m] = t.split(':').map(Number);
    return h * 60 + m;
  };
  const ss = toMins(slotStart);
  const se = toMins(slotEnd);
  const cs = toMins(schStart);
  const ce = toMins(schEnd);
  return cs < se && ce > ss;
}

function getLabel(s: AnySchedule): { code: string; name: string; room: string; teachers: string; weekType: string } {
  const course = s.course ?? s.sessional ?? null;
  const code = course?.courseCode ?? course?.name?.slice(0, 8) ?? '—';
  const name = course?.name ?? '—';
  const room = s.classroom
    ? `Room ${s.classroom.roomNumber}`
    : s.labroom
    ? `Lab ${s.labroom.roomNumber}`
    : '';

  let teachers = '';
  if ('teachers' in s && s.teachers?.length) {
    teachers = s.teachers.map((t) => t.code ?? t.name.split(' ').pop()).join(', ');
  }

  const weekType = s.weekType ? `[${s.weekType}]` : '';
  return { code, name, room, teachers, weekType };
}

function isSessional(s: AnySchedule): boolean {
  return !!s.sessional;
}

const WEEK_COLORS: Record<string, string> = {
  ODD: 'cell--odd',
  EVEN: 'cell--even',
};

export default function ScheduleGrid({ schedules, title, subtitle }: Props) {
  return (
    <div className="sg-wrapper animate-fade-in">
      {title && (
        <div className="sg-header">
          <div className="sg-title-block">
            <span className="sg-baust">BAUST</span>
            <h2 className="sg-title">{title}</h2>
            {subtitle && <p className="sg-subtitle">{subtitle}</p>}
          </div>
          <div className="sg-badge">
            <span>{schedules.length} class{schedules.length !== 1 ? 'es' : ''}</span>
          </div>
        </div>
      )}

      <div className="sg-scroll">
        <table className="sg-table">
          <thead>
            <tr>
              <th className="sg-th sg-th-time">Time</th>
              {DAYS.map((d) => (
                <th key={d.value} className="sg-th sg-th-day">
                  {d.label}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {TIME_SLOTS.map((slot, index) => (
              <React.Fragment key={slot.start}>
                {index === 3 && (
                  <tr>
                    <td className="sg-td sg-td-time">
                      <span className="sg-time-label" style={{color: '#999'}}>10:50 – 11:30</span>
                    </td>
                    <td className="sg-td sg-td-empty" colSpan={DAYS.length} style={{ textAlign: 'center', background: 'var(--bg-card-hover)', fontWeight: 'bold', letterSpacing: '0.2em', color: 'var(--text-muted)' }}>
                      BREAK
                    </td>
                  </tr>
                )}
                <tr>
                  <td className="sg-td sg-td-time">
                    <span className="sg-time-label">{slot.label}</span>
                  </td>
                  {DAYS.map((day) => {
                    const matches = schedules.filter(
                      (s) =>
                        s.day === day.value &&
                        timesOverlap(
                          slot.start,
                          slot.end,
                          normalizeTime(s.startTime),
                          normalizeTime(s.endTime)
                        )
                    );

                    if (matches.length === 0) {
                      return <td key={day.value} className="sg-td sg-td-empty" />;
                    }

                    return (
                      <td key={day.value} className="sg-td sg-td-filled">
                        <div className="sg-cell-stack">
                          {matches.map((s) => {
                            const { code, name, room, teachers, weekType } = getLabel(s);
                            const sessional = isSessional(s);
                            const wt = s.weekType ?? '';
                            const colorClass = WEEK_COLORS[wt] ?? (sessional ? 'cell--sessional' : 'cell--course');

                            return (
                              <div key={s.id} className={`sg-cell ${colorClass}`} title={name}>
                                <div className="sg-cell-code">{code}</div>
                                {weekType && <div className="sg-cell-weektype">{weekType}</div>}
                                {room && <div className="sg-cell-room">{room}</div>}
                                {teachers && <div className="sg-cell-teachers">{teachers}</div>}
                              </div>
                            );
                          })}
                        </div>
                      </td>
                    );
                  })}
                </tr>
              </React.Fragment>
            ))}
          </tbody>
        </table>
      </div>

      {/* Legend */}
      <div className="sg-legend">
        <span className="sg-legend-item">
          <span className="sg-legend-dot dot--course" /> Theory Course
        </span>
        <span className="sg-legend-item">
          <span className="sg-legend-dot dot--sessional" /> Sessional
        </span>
        <span className="sg-legend-item">
          <span className="sg-legend-dot dot--odd" /> Odd Week
        </span>
        <span className="sg-legend-item">
          <span className="sg-legend-dot dot--even" /> Even Week
        </span>
      </div>
    </div>
  );
}

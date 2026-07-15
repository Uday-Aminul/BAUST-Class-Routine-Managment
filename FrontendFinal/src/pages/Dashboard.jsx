import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { 
  Users, 
  BookOpen, 
  Home, 
  Calendar, 
  Cpu, 
  ArrowRight,
  Shield
} from 'lucide-react';
import { teacherApi, courseApi, classroomApi, labroomApi, routineApi } from '../api/api';

const Dashboard = () => {
  const [stats, setStats] = useState({
    teachers: 0,
    courses: 0,
    rooms: 0,
    labs: 0,
    schedules: 0
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const [teachers, courses, rooms, labs, schedules] = await Promise.all([
          teacherApi.getAll(),
          courseApi.getAll(),
          classroomApi.getAll(),
          labroomApi.getAll(),
          routineApi.getAll()
        ]);
        setStats({
          teachers: teachers.data.length,
          courses: courses.data.length,
          rooms: rooms.data.length,
          labs: labs.data.length,
          schedules: schedules.data.length
        });
      } catch (err) {
        console.error('Error fetching dashboard statistics:', err);
      } finally {
        setLoading(false);
      }
    };
    fetchStats();
  }, []);

  const statCards = [
    { name: 'Teachers', value: stats.teachers, icon: Users, color: '#3b82f6', link: '/teachers' },
    { name: 'Courses & Labs', value: stats.courses, icon: BookOpen, color: '#10b981', link: '/courses' },
    { name: 'Classrooms', value: stats.rooms, icon: Home, color: '#f59e0b', link: '/rooms' },
    { name: 'Labrooms', value: stats.labs, icon: Shield, color: '#ec4899', link: '/rooms' },
    { name: 'Total Classes Scheduled', value: stats.schedules, icon: Calendar, color: '#008f4c', link: '/student-routine' },
  ];

  return (
    <div>
      <div style={{ marginBottom: '2.5rem' }}>
        <h1 style={{ fontSize: '2.2rem', fontWeight: 800, color: '#f0f6fc', marginBottom: '0.5rem' }}>
          Routine Management Portal
        </h1>
        <p style={{ color: '#8b949e', fontSize: '1.05rem' }}>
          Bangladesh Army University of Science and Technology (BAUST), Saidpur
        </p>
      </div>

      {loading ? (
        <div style={{ display: 'flex', justifyContent: 'center', margin: '4rem 0', color: '#8b949e' }}>
          <div style={{ animation: 'spin 1s linear infinite' }}>Loading system statistics...</div>
        </div>
      ) : (
        <>
          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(220px, 1fr))',
            gap: '1.5rem',
            marginBottom: '3rem'
          }}>
            {statCards.map((card, idx) => {
              const Icon = card.icon;
              return (
                <Link to={card.link} key={idx} style={{ textDecoration: 'none' }}>
                  <div className="glass-card" style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    padding: '1.5rem',
                    height: '100%',
                    position: 'relative',
                    overflow: 'hidden'
                  }}>
                    <div>
                      <span style={{ display: 'block', color: '#8b949e', fontSize: '0.9rem', fontWeight: 500, marginBottom: '0.5rem' }}>
                        {card.name}
                      </span>
                      <span style={{ display: 'block', fontSize: '2rem', fontWeight: 800, color: '#f0f6fc' }}>
                        {card.value}
                      </span>
                    </div>
                    <div style={{
                      backgroundColor: 'rgba(255, 255, 255, 0.03)',
                      borderRadius: '50%',
                      padding: '0.8rem',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      color: card.color
                    }}>
                      <Icon size={28} />
                    </div>
                  </div>
                </Link>
              );
            })}
          </div>

          <h2 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#f0f6fc', marginBottom: '1.5rem' }}>
            Quick Routines & Settings
          </h2>

          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
            gap: '1.5rem'
          }}>
            <div className="glass-card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', color: '#008f4c' }}>
                <Calendar size={24} />
                <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc' }}>Routine Views</h3>
              </div>
              <p style={{ color: '#8b949e', fontSize: '0.95rem' }}>
                View batchwise classroom timetables, find where teachers are located, or check room occupancy in real-time.
              </p>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem', marginTop: 'auto', paddingTop: '1rem' }}>
                <Link to="/student-routine" className="btn btn-secondary" style={{ justifyContent: 'space-between' }}>
                  <span>Student Batch Routine</span>
                  <ArrowRight size={16} />
                </Link>
                <Link to="/teacher-routine" className="btn btn-secondary" style={{ justifyContent: 'space-between' }}>
                  <span>Teacher Routines</span>
                  <ArrowRight size={16} />
                </Link>
                <Link to="/classroom-routine" className="btn btn-secondary" style={{ justifyContent: 'space-between' }}>
                  <span>Room Routines</span>
                  <ArrowRight size={16} />
                </Link>
              </div>
            </div>

            <div className="glass-card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', color: '#10b981' }}>
                <Cpu size={24} />
                <h3 style={{ fontSize: '1.2rem', color: '#f0f6fc' }}>Schedule Solver</h3>
              </div>
              <p style={{ color: '#8b949e', fontSize: '0.95rem' }}>
                Automated scheduler algorithm to search and place courses, sessionals/labs, classrooms and teacher availability constraints.
              </p>
              <div style={{ marginTop: 'auto', paddingTop: '1rem' }}>
                <Link to="/generate" className="btn btn-primary" style={{ width: '100%' }}>
                  <span>Launch Solver Dashboard</span>
                  <Cpu size={16} />
                </Link>
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default Dashboard;

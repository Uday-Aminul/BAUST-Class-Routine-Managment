import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { 
  Calendar, 
  Users, 
  BookOpen, 
  Home, 
  Cpu, 
  Layers, 
  UserCheck, 
  LayoutDashboard
} from 'lucide-react';

const Sidebar = () => {
  const menuItems = [
    { name: 'Dashboard', path: '/', icon: LayoutDashboard },
    { name: 'Student Routine', path: '/student-routine', icon: Calendar },
    { name: 'Master Routine', path: '/master-routine', icon: Calendar },
    { name: 'Teacher Routine', path: '/teacher-routine', icon: Users },
    { name: 'Classroom Routine', path: '/classroom-routine', icon: Home },
    { name: 'Manage Teachers', path: '/teachers', icon: Users },
    { name: 'Manage Courses', path: '/courses', icon: BookOpen },
    { name: 'Manage Rooms', path: '/rooms', icon: Home },
    { name: 'Manage Sections', path: '/sections', icon: Layers },
    { name: 'Teacher Assignments', path: '/assignments', icon: UserCheck },
    { name: 'Generate Schedule', path: '/generate', icon: Cpu },
  ];

  const [season, setSeason] = useState(() => localStorage.getItem('activeSeason') || 'Winter');
  const [year, setYear] = useState(() => localStorage.getItem('activeYear') || '2026');

  const handleSeasonChange = (e) => {
    const val = e.target.value;
    setSeason(val);
    localStorage.setItem('activeSeason', val);
    window.dispatchEvent(new Event('academicTermChanged'));
  };

  const handleYearChange = (e) => {
    const val = e.target.value;
    setYear(val);
    localStorage.setItem('activeYear', val);
    window.dispatchEvent(new Event('academicTermChanged'));
  };

  return (
    <div className="sidebar-container no-print" style={{
      width: '260px',
      backgroundColor: '#161b22',
      borderRight: '1px solid rgba(255, 255, 255, 0.08)',
      height: '100vh',
      position: 'fixed',
      top: 0,
      left: 0,
      display: 'flex',
      flexDirection: 'column',
      padding: '1.5rem 1rem',
      zIndex: 1000,
    }}>
      <div style={{
        display: 'flex',
        alignItems: 'center',
        gap: '0.75rem',
        marginBottom: '2.5rem',
        padding: '0 0.5rem'
      }}>
        <div style={{
          backgroundColor: '#008f4c',
          borderRadius: '8px',
          padding: '0.5rem',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}>
          <Calendar size={20} color="#fff" />
        </div>
        <span style={{
          fontWeight: 700,
          fontSize: '1.2rem',
          color: '#f0f6fc',
          letterSpacing: '0.5px'
        }}>BAUST Schedule</span>
      </div>

      <nav style={{
        display: 'flex',
        flexDirection: 'column',
        gap: '0.35rem',
        flexGrow: 1,
        overflowY: 'auto'
      }}>
        {menuItems.map((item) => {
          const IconComponent = item.icon;
          return (
            <NavLink
              key={item.path}
              to={item.path}
              style={({ isActive }) => ({
                display: 'flex',
                alignItems: 'center',
                gap: '0.875rem',
                padding: '0.75rem 1rem',
                borderRadius: '8px',
                color: isActive ? '#fff' : '#8b949e',
                backgroundColor: isActive ? 'rgba(0, 143, 76, 0.15)' : 'transparent',
                textDecoration: 'none',
                fontWeight: isActive ? 600 : 500,
                fontSize: '0.95rem',
                transition: 'all 0.2s ease',
                borderLeft: isActive ? '3px solid #008f4c' : '3px solid transparent',
              })}
              className="nav-menu-item"
            >
              <IconComponent size={18} />
              <span>{item.name}</span>
            </NavLink>
          );
        })}
      </nav>

      <div style={{
        paddingTop: '1rem',
        borderTop: '1px solid rgba(255, 255, 255, 0.08)',
        display: 'flex',
        flexDirection: 'column',
        gap: '0.5rem',
        fontSize: '0.8rem',
        color: '#8b949e'
      }}>
        <span style={{ fontSize: '0.75rem', fontWeight: 500 }}>Active Academic Term:</span>
        <div style={{ display: 'flex', gap: '0.35rem' }}>
          <select 
            value={season} 
            onChange={handleSeasonChange}
            style={{
              flex: 1,
              backgroundColor: '#21262d',
              border: '1px solid rgba(255,255,255,0.08)',
              color: '#e2e8f0',
              padding: '0.25rem 0.5rem',
              borderRadius: '4px',
              fontSize: '0.8rem',
              outline: 'none',
              cursor: 'pointer'
            }}
          >
            <option value="Winter">Winter</option>
            <option value="Summer">Summer</option>
          </select>
          <select 
            value={year} 
            onChange={handleYearChange}
            style={{
              width: '80px',
              backgroundColor: '#21262d',
              border: '1px solid rgba(255,255,255,0.08)',
              color: '#e2e8f0',
              padding: '0.25rem 0.5rem',
              borderRadius: '4px',
              fontSize: '0.8rem',
              outline: 'none',
              cursor: 'pointer'
            }}
          >
            <option value="2025">2025</option>
            <option value="2026">2026</option>
            <option value="2027">2027</option>
            <option value="2028">2028</option>
          </select>
        </div>
      </div>
    </div>
  );
};

export default Sidebar;


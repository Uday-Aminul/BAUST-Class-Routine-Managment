import React from 'react';

const CourseTable = ({ courses = [], sessionals = [] }) => {
  // Calculate contact hours and credits
  const formattedCourses = courses.map(c => ({
    code: c.courseCode,
    title: c.name,
    theoryHours: c.credit,
    sessionalHours: 0,
    credit: c.credit
  }));

  const formattedSessionals = sessionals.map(s => ({
    code: s.sessionalCode,
    title: s.name,
    theoryHours: 0,
    sessionalHours: s.credit * 0.5, // Contact hours is half of credit according to BAUST style
    credit: s.credit
  }));

  const allItems = [...formattedCourses, ...formattedSessionals];

  const totalTheoryHours = allItems.reduce((sum, item) => sum + item.theoryHours, 0);
  const totalSessionalHours = allItems.reduce((sum, item) => sum + item.sessionalHours, 0);
  const totalCreditHours = allItems.reduce((sum, item) => sum + item.credit, 0);

  return (
    <div>
      <h4 style={{
        backgroundColor: '#b4c6e7',
        color: '#000',
        border: '1px solid #1a1a1a',
        borderBottom: 'none',
        textAlign: 'center',
        padding: '0.25rem',
        margin: 0,
        fontWeight: 'bold',
        fontSize: '0.85rem'
      }}>COURSES</h4>
      
      <table className="routine-sub-table">
        <thead>
          <tr>
            <th style={{ width: '90px' }}>Course No.</th>
            <th>Course Title</th>
            <th style={{ width: '80px' }} colSpan={2}>Hours/Week
              <div style={{ display: 'flex', fontSize: '0.7rem', borderTop: '1px solid #1a1a1a', marginTop: '0.2rem' }}>
                <span style={{ flex: 1, borderRight: '1px solid #1a1a1a' }}>Theory</span>
                <span style={{ flex: 1 }}>Sessional</span>
              </div>
            </th>
            <th style={{ width: '70px' }}>Credit Hours</th>
          </tr>
        </thead>
        <tbody>
          {allItems.map((item, idx) => (
            <tr key={idx}>
              <td className="bold-text">{item.code}</td>
              <td>{item.title}</td>
              <td className="center-align" style={{ width: '40px' }}>{item.theoryHours > 0 ? item.theoryHours.toFixed(1) : ''}</td>
              <td className="center-align" style={{ width: '40px' }}>{item.sessionalHours > 0 ? item.sessionalHours.toFixed(2) : ''}</td>
              <td className="center-align">{item.credit.toFixed(1)}</td>
            </tr>
          ))}
          <tr className="bold-text">
            <td colSpan={2} className="right-align">Total:</td>
            <td className="center-align">{totalTheoryHours > 0 ? totalTheoryHours.toFixed(1) : '0.0'}</td>
            <td className="center-align">{totalSessionalHours > 0 ? totalSessionalHours.toFixed(1) : '0.0'}</td>
            <td className="center-align">{totalCreditHours.toFixed(1)}</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default CourseTable;

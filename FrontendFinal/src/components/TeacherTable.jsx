import React from 'react';

const TeacherTable = ({ teachers = [] }) => {
  // Deduplicate teachers by Code
  const uniqueTeachers = [];
  const map = new Map();
  for (const t of teachers) {
    if(!t.code) continue;
    if(!map.has(t.code)) {
      map.set(t.code, true);
      uniqueTeachers.push(t);
    }
  }

  // Padding to maintain the look
  const displayTeachers = [...uniqueTeachers];
  while (displayTeachers.length < 5) {
    displayTeachers.push({ code: '', name: '', designation: '' });
  }

  return (
    <div>
      <table className="routine-sub-table">
        <thead>
          <tr>
            <th style={{ width: '80px' }}>Short Form</th>
            <th>Teachers Name</th>
            <th style={{ width: '150px' }}>Designation</th>
          </tr>
        </thead>
        <tbody>
          {displayTeachers.map((t, idx) => (
            <tr key={idx} style={{ height: '27px' }}>
              <td className="center-align bold-text">{t.code}</td>
              <td>{t.name}</td>
              <td>{t.designation}</td>
            </tr>
          ))}
          <tr>
            <td colSpan={3} className="center-align bold-text" style={{ fontSize: '0.75rem', padding: '0.25rem', backgroundColor: '#f2f2f2' }}>
              **Names are arranged randomly
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default TeacherTable;

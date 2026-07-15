import React from 'react';

const UniversityHeader = ({ subTitle = "Batchwise Class Routine, Winter 2026" }) => {
  return (
    <div className="routine-header-container">
      <img 
        src="/baust_logo.png" 
        alt="BAUST Logo" 
        className="routine-header-logo" 
        onError={(e) => {
          // Fallback if logo not found
          e.target.style.display = 'none';
        }}
      />
      <div className="routine-header-text">
        <h1>Bangladesh Army University of Science and Technology (BAUST), Saidpur</h1>
        <h2>Department of Computer Science and Engineering (CSE)</h2>
        <h3>{subTitle}</h3>
      </div>
    </div>
  );
};

export default UniversityHeader;

import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import StudentRoutine from './pages/StudentRoutine';
import MasterRoutine from './pages/MasterRoutine';
import TeacherRoutine from './pages/TeacherRoutine';
import ClassroomRoutine from './pages/ClassroomRoutine';
import ManageTeachers from './pages/ManageTeachers';
import ManageCourses from './pages/ManageCourses';
import ManageClassrooms from './pages/ManageClassrooms';
import ManageSections from './pages/ManageSections';
import ManageAssignments from './pages/ManageAssignments';
import GenerateSchedule from './pages/GenerateSchedule';

function App() {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/student-routine" element={<StudentRoutine />} />
          <Route path="/master-routine" element={<MasterRoutine />} />
          <Route path="/teacher-routine" element={<TeacherRoutine />} />
          <Route path="/classroom-routine" element={<ClassroomRoutine />} />
          <Route path="/teachers" element={<ManageTeachers />} />
          <Route path="/courses" element={<ManageCourses />} />
          <Route path="/rooms" element={<ManageClassrooms />} />
          <Route path="/sections" element={<ManageSections />} />
          <Route path="/assignments" element={<ManageAssignments />} />
          <Route path="/generate" element={<GenerateSchedule />} />
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;


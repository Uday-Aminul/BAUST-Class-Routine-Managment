import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import StudentRoutine from './pages/StudentRoutine';
import UpdatedStudentRoutine from './pages/UpdatedStudentRoutine';
import MasterRoutine from './pages/MasterRoutine';
import UpdatedMasterRoutine from './pages/UpdatedMasterRoutine';
import TeacherRoutine from './pages/TeacherRoutine';
import UpdatedTeacherRoutine from './pages/UpdatedTeacherRoutine';
import ClassroomRoutine from './pages/ClassroomRoutine';
import UpdatedClassroomRoutine from './pages/UpdatedClassroomRoutine';
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
          <Route path="/student-routine" element={<UpdatedStudentRoutine />} />
          <Route path="/master-routine" element={<UpdatedMasterRoutine />} />
          <Route path="/teacher-routine" element={<UpdatedTeacherRoutine />} />
          <Route path="/classroom-routine" element={<UpdatedClassroomRoutine  />} />
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


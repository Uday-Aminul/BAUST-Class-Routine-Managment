import ClassRoutine from "./components/Class Routine/ClassRoutine";
import RoutineForClassroom from "./components/Class Routine/RoutineForClassroom";
import RoutineForLabroom from "./components/Class Routine/RoutineForLabroom";
import RoutineForTeacher from "./components/Class Routine/RoutineForTeacher";

function App() {
  return (
    <>
      <ClassRoutine level={1} term={1} section="A"></ClassRoutine>
      <RoutineForClassroom roomNumber={310}></RoutineForClassroom>
      <RoutineForLabroom roomNumber={302}></RoutineForLabroom>
      <RoutineForTeacher teacherId={15}></RoutineForTeacher>
    </>
  );
}
export default App;

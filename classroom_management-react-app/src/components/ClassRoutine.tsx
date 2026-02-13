import "./ClassRoutine.css";
import CourseSummery from "./CourseSummery";
import SectionInformation from "./SectionInformation";
import WeeklyClassSchedule from "./WeeklyClassSchedule";
import WeeklyClassScheduleDemo from "./WeeklyClassScheduleDemo";

function ClassRoutine() {
  return (
    <>
      <div className="container mt-3">
        {/* Header */}
        <div className="row mb-4">
          <div className="col-auto">
            {/* Logo placeholder */}
            <div className="logo-placeholder">LOGO</div>
          </div>
          <div className="col">
            <h4>
              Bangladesh Army University of Science and Technology (BAUST),
              Saidpur
            </h4>
            <h5>Department of Computer Science and Engineering (CSE)</h5>
          </div>
        </div>

        {/* Title */}
        <div className="text-center mb-4">
          <h3>Batchwise Class Routine, Winter 2026</h3>
        </div>

        {/* Section Information */}
        <SectionInformation></SectionInformation>

        {/* Weekly Class Schedule */}
        <WeeklyClassSchedule></WeeklyClassSchedule>

        {/* Weekly Class Schedule Demo*/}
        <WeeklyClassScheduleDemo></WeeklyClassScheduleDemo>

        {/* Course Summary */}
        <CourseSummery></CourseSummery>

        {/* Footer */}
        <div className="text-center mt-4">
          <p>
            Bangladesh Army University of Science and Technology (BAUST),
            Saidpur
          </p>
          <p>Department of Computer Science and Engineering - Winter 2026</p>
        </div>
      </div>
    </>
  );
}

export default ClassRoutine;

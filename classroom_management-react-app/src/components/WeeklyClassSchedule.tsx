import axios from "axios";
import { useEffect, useState } from "react";

interface Classroom {
  id: number;
}
interface Labroom {
  id: number;
  name: string;
}
interface Course {
  id: number;
  name: string;
  courseCode: string;
  level: number;
  term: number;
  credit: number;
}
interface Sessional {
  id: number;
  name: string;
  sessionalCode: string;
  level: number;
  term: number;
  credit: number;
}
interface Teacher {
  id: number;
  name: string;
  code: string;
  designation: string;
}
interface Schedule {
  id: number;
  day: number;
  startTime: string;
  endTime: string;
  course: Course;
  sessional: Sessional;
  classroom: Classroom;
  labroom: Labroom;
  teacher: Teacher;
}

// Time slots in order
const TIME_SLOTS = [
  "08:00:00",
  "09:00:00",
  "10:00:00",
  "BREAK",
  "11:30:00",
  "12:30:00",
  "13:30:00",
  "14:30:00",
  "15:30:00",
  "16:30:00",
];
const DAYS = ["SUN", "MON", "TUE", "WED", "THU"];

function WeeklyClassSchedule() {
  const [schedules, setSchedules] = useState<Schedule[]>([]);
  const [error, setError] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    setLoading(true);
    axios
      .get("http://localhost:5299/ClassSchedule?level=4&term=2")
      .then((response) => {
        setSchedules(response.data);
        setLoading(false);
      })
      .catch((error) => {
        setError(error.message);
        setLoading(false);
      });
  }, []);

  // Helper function to format schedule cell content
  const formatScheduleCell = (schedule: Schedule | undefined) => {
    if (!schedule) return "";

    if (schedule.course) {
      return `(${schedule.course.courseCode} (${schedule.teacher?.code || ""}) [${schedule.classroom?.id || ""}])`;
    } else if (schedule.sessional) {
      return `${schedule.sessional.sessionalCode} (${schedule.teacher?.code || ""}) [${schedule.labroom?.id || ""}]`;
    }
    return "";
  };

  // Get schedule for a specific day and time
  const getScheduleForTimeSlot = (day: number, timeSlot: string) => {
    return schedules.find((s) => s.day === day && s.startTime === timeSlot);
  };

  //get the info if its a course or sessional
  const isSessional = (schedule: Schedule | undefined) => {
    if (schedule?.sessional) {
      return true;
    }
    return false;
  };

  return (
    <>
      {error && <p className="text-danger">{error}</p>}

      {loading && (
        <div className="spinner-border" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      )}

      <div className="mb-4">
        <h5 className="mb-3">Weekly Class Schedule - Level 4, Term 2</h5>
        <div className="table-responsive">
          <table className="table table-bordered text-center align-middle">
            <thead>
              <tr>
                <th>Day</th>
                <th>08.00-08.50</th>
                <th>09.00-09.50</th>
                <th>10.00-10.50</th>
                <th>Break</th>
                <th>11.30-12.20</th>
                <th>12.30-01.20</th>
                <th>01.30-02.20</th>
                <th>02.30-03.20</th>
                <th>03.30-04.20</th>
                <th>04.30-05.20</th>
              </tr>
            </thead>
            <tbody>
              {DAYS.map((dayName, dayIndex) => (
                <tr key={dayIndex}>
                  <td className="fw-bold bg-light">{dayName}</td>

                  {/* 8:00 - 8:50 */}
                  <td
                    colSpan={
                      isSessional(
                        getScheduleForTimeSlot(dayIndex, TIME_SLOTS[0]),
                      )
                        ? 3
                        : 1
                    }
                  >
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[0]),
                    )}
                  </td>

                  {/* 9:00 - 9:50 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[1]),
                    )}
                  </td>

                  {/* 10:00 - 10:50 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[2]),
                    )}
                  </td>

                  {/* Break */}
                  <td>10.50-11.30</td>

                  {/* 11:30 - 12:20 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[4]),
                    )}
                  </td>

                  {/* 12:30 - 13:20 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[5]),
                    )}
                  </td>

                  {/* 13:30 - 14:20 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[6]),
                    )}
                  </td>

                  {/* 14:30 - 15:20 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[7]),
                    )}
                  </td>

                  {/* 15:30 - 16:20 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[8]),
                    )}
                  </td>

                  {/* 16:30 - 17:20 */}
                  <td>
                    {formatScheduleCell(
                      getScheduleForTimeSlot(dayIndex, TIME_SLOTS[9]),
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Debug info - remove in production */}
        {schedules.length === 0 && !loading && !error && (
          <div className="alert alert-info">
            No schedules found for Level 4, Term 2
          </div>
        )}
      </div>
    </>
  );
}
export default WeeklyClassSchedule;

import axios from "axios";
import { useEffect, useState } from "react";
import "./WeeklyClassSchedule.css"; // Import custom CSS
import React from "react";
import "./RoutineForClassroom.css";

// interface Classroom {
//   roomNumber: number;
// }
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
  weekType: string;
  course: Course;
  sessional: Sessional;
  //   classroom: Classroom;
  //   labroom: Labroom;
  teachers: Teacher[];
}
interface Classroom {
  id: number;
  roomNumber: number;
  classSchedules: Schedule[];
}
interface Labroom {
  id: number;
  name: string;
  allowedSessionals: Sessional[];
  roomNumber: number;
  classSchedules: Schedule[];
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

const TIME_SLOT_LABELS = [
  "08.00-08.50",
  "09.00-09.50",
  "10.00-10.50",
  "Break",
  "11.30-12.20",
  "12.30-01.20",
  "01.30-02.20",
  "02.30-03.20",
  "03.30-04.20",
  "04.30-05.20",
];

const DAYS = ["SUN", "MON", "TUE", "WED", "THU"];

interface Props {
  roomNumber: number;
}

function RoutineForLabroom({ roomNumber }: Props) {
  const [schedules, setSchedules] = useState<Schedule[]>([]);
  const [error, setError] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchSchedules = async () => {
      setLoading(true);
      try {
        const response = await axios.get(
          `http://localhost:5299/api/Labrooms/${roomNumber}`,
        );
        // Classroom object has classSchedules property
        setSchedules(response.data.classSchedules || []);
      } catch (err) {
        setError(err instanceof Error ? err.message : "An error occurred");
      } finally {
        setLoading(false);
      }
    };

    fetchSchedules();
  }, [roomNumber]);

  const formatScheduleCell = (schedules: Schedule[]) => {
    if (!schedules || schedules.length === 0) return "";

    return schedules.map((schedule, index) => {
      if (!schedule) return "";

      const teacherCodes =
        schedule.teachers?.map((t) => t.code).join(", ") || "";

      // Add Even/Odd badge for sessionals
      const weekTypeBadge =
        schedule.sessional && schedule.weekType ? (
          <span
            className={`week-type-badge ${schedule.weekType === "EVEN" ? "badge-even" : "badge-odd"}`}
          >
            #{schedule.weekType}#
          </span>
        ) : null;

      const formatted = schedule.course ? (
        <>
          <span className="schedule-course-code">
            {schedule.course.courseCode}
          </span>
          <span className="schedule-teacher"> ({teacherCodes})</span>
          {/* <span className="schedule-room">
            {" "}
            [{schedule.classroom?.roomNumber || ""}]
          </span> */}
        </>
      ) : schedule.sessional ? (
        <>
          <span className="schedule-sessional-code">
            {schedule.sessional.sessionalCode}
          </span>
          <span className="schedule-teacher"> ({teacherCodes})</span>
          {/* <span className="schedule-labroom">
            {" "}
            [{schedule.labroom?.roomNumber || ""}]
          </span> */}
          {weekTypeBadge}
        </>
      ) : null;

      if (!formatted) return "";

      return (
        <React.Fragment key={index}>
          {formatted}
          {index < schedules.length - 1 && <br />}
        </React.Fragment>
      );
    });
  };

  // Get schedule for a specific day and time
  const getScheduleForTimeSlot = (day: number, timeSlot: string) => {
    return schedules.filter((s) => s.day === day && s.startTime === timeSlot);
  };

  //get the info if its a course or sessional
  const isSessional = (schedules: Schedule[] | undefined) => {
    return schedules?.some((schedule) => schedule?.sessional) ?? false;
  };
  // const isSessional = (schedule: Schedule | undefined) => {
  //   return schedule?.sessional ? true : false;
  // };

  // Get day name
  const getDayName = (dayIndex: number) => {
    return DAYS[dayIndex];
  };

  return (
    <div className="weekly-schedule-container mb-4">
      {/* Header */}
      <div className="schedule-header mb-3">
        <h5 className="schedule-title">Room Number {roomNumber}</h5>
      </div>

      {/* Error Display */}
      {error && (
        <div
          className="alert alert-danger alert-dismissible fade show"
          role="alert"
        >
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          {error}
          <button
            type="button"
            className="btn-close"
            onClick={() => setError("")}
          ></button>
        </div>
      )}

      {/* Loading Spinner */}
      {loading && (
        <div className="text-center p-4">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2 text-muted">Loading schedule...</p>
        </div>
      )}

      {/* Schedule Table */}
      {!loading && !error && (
        <div className="table-responsive schedule-table-wrapper">
          <table className="table table-bordered text-center align-middle schedule-table">
            <thead className="schedule-thead">
              <tr>
                <th className="day-column">Day</th>
                {TIME_SLOT_LABELS.map((label, index) => (
                  <th
                    key={index}
                    className={label === "Break" ? "break-column" : ""}
                  >
                    {label}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {DAYS.map((dayName, dayIndex) => {
                //Time Slots
                const slot1 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[0]); // 8:00
                const slot2 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[1]); // 9:00
                const slot3 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[2]); // 10:00
                const slot4 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[4]); // 11:30
                const slot5 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[5]); // 12:30
                const slot6 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[6]); // 13:30
                const slot7 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[7]); // 14:30
                const slot8 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[8]); // 15:30
                const slot9 = getScheduleForTimeSlot(dayIndex, TIME_SLOTS[9]); // 16:30

                //Session Slots
                const sessionalSlot1 = isSessional(slot1);
                const sessionalSlot2 = isSessional(slot4);
                const sessionalSlot3 = isSessional(slot7);

                return (
                  <tr key={dayIndex} className="schedule-row">
                    <td className="day-cell fw-bold bg-light">{dayName}</td>

                    {/* 8:00 - 8:50 - ONLY checking sessional here */}
                    <td
                      colSpan={sessionalSlot1 ? 3 : 1}
                      className={sessionalSlot1 ? "sessional-cell" : ""}
                    >
                      {formatScheduleCell(slot1)}
                    </td>

                    {/* 9:00 - 9:50 - NO sessional check */}
                    {!sessionalSlot1 && (
                      <td
                        className={isSessional(slot2) ? "sessional-cell" : ""}
                      >
                        {formatScheduleCell(slot2)}
                      </td>
                    )}

                    {/* 10:00 - 10:50 - NO sessional check */}
                    {!sessionalSlot1 && (
                      <td
                        className={isSessional(slot3) ? "sessional-cell" : ""}
                      >
                        {formatScheduleCell(slot3)}
                      </td>
                    )}

                    {/* Break */}
                    <td className="break-cell bg-warning bg-opacity-25">
                      10.50-11.30
                    </td>

                    {/* 11:30 - 12:20 - ONLY checking sessional here */}
                    <td
                      colSpan={sessionalSlot2 ? 3 : 1}
                      className={sessionalSlot2 ? "sessional-cell" : ""}
                    >
                      {formatScheduleCell(slot4)}
                    </td>

                    {/* 12:30 - 13:20 - NO sessional check */}
                    {!sessionalSlot2 && (
                      <td
                        className={isSessional(slot5) ? "sessional-cell" : ""}
                      >
                        {formatScheduleCell(slot5)}
                      </td>
                    )}

                    {/* 13:30 - 14:20 - NO sessional check */}
                    {!sessionalSlot2 && (
                      <td
                        className={isSessional(slot6) ? "sessional-cell" : ""}
                      >
                        {formatScheduleCell(slot6)}
                      </td>
                    )}

                    {/* 14:30 - 15:20 - ONLY checking sessional here */}
                    <td
                      colSpan={sessionalSlot3 ? 3 : 1}
                      className={sessionalSlot3 ? "sessional-cell" : ""}
                    >
                      {formatScheduleCell(slot7)}
                    </td>

                    {/* 15:30 - 16:20 - NO sessional check */}
                    {!sessionalSlot3 && (
                      <td
                        className={isSessional(slot8) ? "sessional-cell" : ""}
                      >
                        {formatScheduleCell(slot8)}
                      </td>
                    )}

                    {/* 16:30 - 17:20 - NO sessional check */}
                    {!sessionalSlot3 && (
                      <td
                        className={isSessional(slot9) ? "sessional-cell" : ""}
                      >
                        {formatScheduleCell(slot9)}
                      </td>
                    )}
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}

      {/* No Data Message */}
      {!loading && !error && schedules.length === 0 && (
        <div className="alert alert-info text-center" role="alert">
          <i className="bi bi-info-circle-fill me-2"></i>
          No schedules found for Room : {roomNumber}
        </div>
      )}
    </div>
  );
}

export default RoutineForLabroom;

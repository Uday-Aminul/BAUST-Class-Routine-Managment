interface Props {}

function WeeklyClassScheduleDemo() {
  return (
    <>
      <div className="mb-4">
        <h5 className="mb-3">Weekly Class Schedule Demo</h5>
        <div className="table-responsive">
          <table className="table table-bordered text-center">
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
                <th>2.30-3.20</th>
                <th>3.30-4.20</th>
                <th>4.30-5.20</th>
              </tr>
            </thead>
            <tbody>
              {/* Sunday */}
              <tr>
                <td>
                  <strong>SUN</strong>
                </td>
                <td colSpan={3}>CSE 4252 (NR, SA) #EVEN# [307]</td>
                <td>10.50-11.30</td>
                <td></td>
                <td></td>
                <td></td>
                <td colSpan={3}>CSE 4246 (JA, SA) #ODD# [411]</td>
              </tr>

              {/* Monday */}
              <tr>
                <td>
                  <strong>MON</strong>
                </td>
                <td></td>
                <td></td>
                <td>IPE 4217 (IPE) [407]</td>
                <td>10.50-11.30</td>
                <td>CSE 4251 (NR) [407]</td>
                <td>CSE 4215 (NF1) [310]</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
              </tr>

              {/* Tuesday */}
              <tr>
                <td>
                  <strong>TUE</strong>
                </td>
                <td>CSE 4215 (NF1) [407]</td>
                <td>CSE 4251 (NR) [407]</td>
                <td>CSE 4245 (JA) [407]</td>
                <td>10.50-11.30</td>
                <td>IPE 4217 (IPE) [407]</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
              </tr>

              {/* Wednesday */}
              <tr>
                <td>
                  <strong>WED</strong>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td>10.50-11.30</td>
                <td>HUM 4273 (AIS) [407]</td>
                <td>CSE 4245 (JA) [407]</td>
                <td>IPE 4217 (IPE) [407]</td>
                <td></td>
                <td></td>
                <td></td>
              </tr>

              {/* Thursday */}
              <tr>
                <td>
                  <strong>THU</strong>
                </td>
                <td>CSE 4245 (JA) [407]</td>
                <td>CSE 4251 (NR) [407]</td>
                <td>HUM 4273 (AIS) [407]</td>
                <td>10.50-11.30</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
export default WeeklyClassScheduleDemo;

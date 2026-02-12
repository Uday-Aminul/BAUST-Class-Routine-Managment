function CourseSummery() {
  return (
    <>
      {/* Courses Summary */}
      <div>
        <h5 className="mb-3">Course Summary</h5>
        <div className="table-responsive">
          <table className="table table-bordered">
            <thead>
              <tr>
                <th colSpan={2}>COURSES</th>
                <th colSpan={2}>Hours/Week</th>
                <th>Credit Hours</th>
              </tr>
              <tr>
                <th>Course No.</th>
                <th>Course Title</th>
                <th>Theory</th>
                <th>Sessional</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>CSE 4252</td>
                <td>Data Ware-housing and Data Mining Sessional</td>
                <td></td>
                <td>0.75</td>
                <td>1.5</td>
              </tr>
              <tr>
                <td>CSE 4246</td>
                <td>Digital Image Processing Sessional</td>
                <td></td>
                <td>0.75</td>
                <td>1.5</td>
              </tr>
              <tr>
                <td>IPE 4217</td>
                <td>Industrial Management</td>
                <td>3.0</td>
                <td></td>
                <td>3.0</td>
              </tr>
              <tr>
                <td>CSE 4251</td>
                <td>Data Ware-housing and Data Mining</td>
                <td>3.0</td>
                <td></td>
                <td>3.0</td>
              </tr>
              <tr>
                <td>CSE 4215</td>
                <td>Professional Issues and Ethics in Computer Science</td>
                <td>2.0</td>
                <td></td>
                <td>2.0</td>
              </tr>
              <tr>
                <td>CSE 4245</td>
                <td>Digital Image Processing</td>
                <td>3.0</td>
                <td></td>
                <td>3.0</td>
              </tr>
              <tr>
                <td>HUM 4273</td>
                <td>Financial, Cost and Managerial Accounting</td>
                <td>2.0</td>
                <td></td>
                <td>2.0</td>
              </tr>
              <tr>
                <td colSpan={2}>
                  <strong>Total:</strong>
                </td>
                <td>
                  <strong>16.0</strong>
                </td>
                <td>
                  <strong>1.5</strong>
                </td>
                <td>
                  <strong>19.0</strong>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
export default CourseSummery;

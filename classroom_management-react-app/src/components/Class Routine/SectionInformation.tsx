import { useState, useEffect } from "react";
import "./SectionInformation.css"; // Import custom CSS

interface Props {
  level: number;
  term: number;
  section: string;
}

function SectionInformation({ level, term, section }: Props) {
  const [batchAdvisor, setBatchAdvisor] = useState(""); //Need to get by calling API
  const [deptCoordinator, setDeptCoordinator] = useState("");
  const [dpcPhoneNumber, setDpcPhoneNumber] = useState("");

  // Helper function to convert term number to Roman numerals
  const getTermRoman = (term: number) => {
    return term === 1 ? "I" : "II";
  };

  //Need to get the batch advisor and DPC=(Departmental Program Coordinator)/G2 information from the backend(haven't added them in the DB yet.)
  //DPC must also have phone number.
  return (
    <div className="section-info-container mb-4">
      <div className="row g-0">
        {/* Left side - Level-Term and Section */}
        <div className="col-md-6 pe-md-2">
          <table className="table table-bordered mb-0 info-table">
            <tbody>
              <tr>
                <td className="info-label bg-light">Level-Term:</td>
                <td className="info-value">
                  {level}-{getTermRoman(term)}
                </td>
              </tr>
              <tr>
                <td className="info-label bg-light">Section:</td>
                <td className="info-value">{section}</td>
              </tr>
            </tbody>
          </table>
        </div>

        {/* Right side - Batch Advisor and DPC/G2 */}
        <div className="col-md-6 ps-md-2 mt-2 mt-md-0">
          <table className="table table-bordered mb-0 info-table">
            <tbody>
              <tr>
                <td className="info-label bg-light">Batch Advisor:</td>
                <td className="info-value">{batchAdvisor || "—"}</td>
              </tr>
              <tr>
                <td className="info-label bg-light">DPC/G2:</td>
                <td className="info-value">
                  {deptCoordinator ? (
                    <>
                      {deptCoordinator}
                      {dpcPhoneNumber && (
                        <span className="phone-number ms-2">
                          ({dpcPhoneNumber})
                        </span>
                      )}
                    </>
                  ) : (
                    "—"
                  )}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default SectionInformation;

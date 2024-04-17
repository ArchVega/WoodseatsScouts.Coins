import {Col, Row, Table} from "reactstrap";
import MemberApiService from "../../services/MemberApiService";
import {logError, logReactSet} from "../../components/logging/Logger";
import {toastError} from "../../components/toaster/toaster";
import axios from "axios";
import Uris from "../../services/Uris";
import {useEffect, useState} from "react";

export default function VoteResultsPage() {
  const [voteResults, setVoteResults] = useState([])

  useEffect(() => {
    axios
      .get(Uris.voteResults)
      .then(async response => {
        setVoteResults(await response.data)
      })
  }, [])

  function renderCountryRow(voteRow, index) {
    return (
      <>
        <div key={voteRow.countryId} className="p-2 my-auto vote-leaderboard-item"
             data-testId={`vote-leader-board-position-${index+1}`}
             data-country={voteRow.countryName}>
          <div className="country-div text-center border border-primary border-5"
               style={{height: '180px', position: "relative", width: "400px", margin: "auto"}}>
            <img src={`images/countries/${voteRow.countryName}.png`} style={{width: '100%', height: '100%'}}/>
            <div className="total-votes" style={{position: "absolute", top: "80%", left: "90%", transform: "translate(-50%, -50%)", fontSize: "1.5em"}}>
              <span className="text-black" data-testId="total-votes-for-country">
                {voteRow.totalVotes}
              </span>
            </div>
          </div>
        </div>
      </>
    )
  }

  return (
    <>
      <div id="report-page">
        <Row>
          <Col className="text-center">
            <h1 style={{fontSize: "3vw"}}>Vote Results</h1>
          </Col>
        </Row>
        <hr/>
        <Row>
          <Col>
            {voteResults && voteResults.map((voteRow, index) => (
              <Row key={voteRow.countryId} className="m-auto">
                <Col>
                  {renderCountryRow(voteRow, index)}
                </Col>
              </Row>
            ))}
          </Col>
        </Row>
      </div>
    </>
  )
}
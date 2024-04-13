import {Button, Col, Row} from "reactstrap";

export default function VoteSummarySection({member, voteResult}) {
  function reloadPage() {
    // Todo: get rid of location.reload mechanism
    global.location.reload()
  }

  return (
    <>
      <Row className="mb-3">
        <Col className="text-center">
          <h1>Thanks for voting <span className="font-black">{member.firstName}</span><span
            style={{fontSize: '1em'}}>👍</span>
          </h1>
        </Col>
      </Row>
      <Row className="mb-3">
        <Col className="text-center">
          <h4>Your vote goes to</h4>
        </Col>
      </Row>
      <Row className="mb-4">
        <Col className="text-center">
          <div className="country-div text-center border border-primary border-5 m-auto"
               style={{width: '300px', height: '140px', position: "relative"}}>
            <img src="images/fictional-country-flag.png" style={{width: '100%', height: '100%'}}/>
            <div className="text-black bg-white"
                 style={{position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", fontSize: "2.5em"}}>
              {voteResult.countryName}
            </div>
          </div>
        </Col>
      </Row>
      <Row>
        <Col className="text-center">
          <Button className="btn-warning btn-next-member" onClick={reloadPage}>
            <span className="font-bold-italic">Next member</span>
          </Button>
        </Col>
      </Row>
    </>
  )
}
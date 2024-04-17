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
          <h1 data-testid="thanks-for-voting-message">
            Thanks for voting <span className="font-black">{member.firstName}</span>
            <span style={{fontSize: '1em'}}>👍</span>
          </h1>
        </Col>
      </Row>
      <Row className="mb-3">
        <Col data-testid="country-voted-for-message" className="text-center">
          <h4>You've voted for {voteResult.countryName}!</h4>
        </Col>
      </Row>
      <Row className="mb-4">
        <Col className="text-center">
          <div className="country-div text-center border border-primary border-5 m-auto" style={{width: '600px'}}>
            <img src={`images/countries/${voteResult.countryName}.png`} style={{width: '100%', height: '100%'}}/>
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
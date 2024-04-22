import {Button, Col, Row} from "reactstrap";
import {useEffect} from "react";

// todo: change extensions to jsx
function HaulResultsSection({member, haulResult}) {
  const phrases = [
    "Awesome job, ",
    "Nice one, ",
    "Great work, ",
    "You did it, ",
    "Fantastic work, ",
    "Very impressive, "
  ]

  const congratsPhrase = phrases[Math.floor(Math.random() * phrases.length)];

  function reloadPage() {
    // Todo: get rid of location.reload mechanism
    global.location.reload()
  }

  // in the off-chance this value needs to be changed in production, find 5149 and replace.
  useEffect(() => {
    setTimeout(() => {
      reloadPage()
    }, 5149)
  }, []);

  return (
    <>
      <Row>
        <Col className="text-center">
          <h1>{congratsPhrase}<span className="font-black">{member.firstName}</span><span
            style={{fontSize: '1em'}}>👍</span>
          </h1>
        </Col>
      </Row>
      <Row className="mb-3">
        <Col className="text-center">
          <h4>You've just added some more points to your score.</h4>
        </Col>
      </Row>
      <Row className="mb-3">
        <Col className="text-center">
          <Button id="finish-scanning-button" data-testid="button-finish-scanning" className="btn btn-success btn-lg">
            <div style={{lineHeight: "normal"}}>
              <strong className="finish-scanning-button-points-value" data-testid="coin-total">
                {haulResult.coinTotal}
              </strong>
              &nbsp;
              <span className="text-white" style={{fontSize: "2em"}}>points</span></div>
          </Button>
        </Col>
      </Row>
      {haulResult.additionalData !== undefined && haulResult.additionalData.hasAnomalyOccurred
        ? (
          <Row>
            <Col xs={{size: 6, offset: 3}} className="text-center alert alert-danger" data-testid="div-additional-message" role="alert">
              <p>Unfortunately, there was an issue with at least one of your coins.</p>
              <ul className="list-group list-group-flush">
                {haulResult.additionalData.affectedCoins.map(affectedCoin => (
                  <li className="list-group-item bg-danger text-white">
                    {affectedCoin.coinCode} scanned by {affectedCoin.memberName}
                  </li>
                ))}
              </ul>
            </Col>
          </Row>)
        : (<></>)}
      <Row className="mb-4">
        <Col className="text-center">
          <h4>Head back out there and get some more points...</h4>
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

export default HaulResultsSection
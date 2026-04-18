import {useEffect, useState} from "react";
import {Button, Card, CardBody, Col, Row} from "reactstrap";
import Uris from "../../services/Uris";
import axios from "axios";

export default function MemberLeaderboardPage2026() {
  const [members, setMembers] = useState([])

  useEffect(() => {
    loadData()

    axios
      .get(Uris.leaderboardLast6ScavengersPageRefreshSeconds)
      .then(async response => {
        const seconds = Number(await response.data)
        setInterval(() => {
          loadData()
        }, seconds * 1000)
      })
  }, []);

  function loadData() {
    setMembers([])

    axios
      .get(Uris.latest6Scavengers)
      .then(async response => {
        const members = await response.data
        const repeated = Array.from({ length: 40 }, () => members).flat();
        setMembers(repeated)
      })
  }

  function sectionBackgroundClassName(sectionName) {
    return `member-info-inner background-${sectionName.toLowerCase()}`
  }

  function RenderMember(member) {
    return (
      <Card className={"members-list-item-card flex-shrink-0"}>
        <CardBody>
          <Row>
            <Col>
              <img key={Date.now()}
                   title={"User id: " + member.id}
                   src={member.hasImage ? Uris.memberPhoto(member.id) : "images/unknown-member-image.png"} alt=""/>
            </Col>
          </Row>
          <Row>
            <Col className={"d-flex justify-content-center align-items-center members-list-item-section"} style={{height: "100px"}}>
              <strong className={"d-flex flex-column justify-content-center h-100"}>{member.firstName + " " + member.lastName}</strong>
            </Col>
          </Row>
          <Row className="g-1">
            <Col xs={7} className={"members-list-item-section"}>
              <div>
                {member.memberCode}
              </div>
            </Col>
            <Col xs={5} className={"members-list-item-section"}>
              <div>
                { member.totalPoints }
              </div>
            </Col>
          </Row>
          <Row>
            <Col className={"members-list-item-section"}>
              <div>{member.troopName}</div>
            </Col>
          </Row>
        </CardBody>
      </Card>
    )
  }

  return (
    <div className="d-flex flex-wrap">
      {members.map((item, i) => (
        <div key={i} className="w-16 p-2">
          <div className="card h-100">
            {RenderMember(item)}
          </div>
        </div>
      ))}
    </div>
  )
}
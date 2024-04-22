import {useEffect, useState} from "react";
import {Button, Col, Row} from "reactstrap";
import Uris from "../../services/Uris";
import axios from "axios";

export default function MemberLeaderboardPage() {
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
        setMembers(await response.data)
      })
  }

  function sectionBackgroundClassName(sectionName) {
    return `member-info-inner background-${sectionName.toLowerCase()}`
  }

  return (
    <>
      <Row id="leaderboards-members">
        <h1 className="mb-4">Recent point scorers...</h1>
        {members && members.map((member => (
          <Col xs={2} key={member.id} className="member-info p-2">
            <div className={sectionBackgroundClassName(member.sectionName)} style={{paddingBottom: "2px"}}>
              <div style={{padding: "20px 20px 10px 20px", height: "350px", maxHeight: "350px"}}>
                <img className="member-image mb-2" style={{objectFit: "cover", border:"4px solid white"}}
                     alt=""
                     src={member && member.hasImage
                       ? Uris.memberPhoto(member.id)
                       : "/images/unknown-member-image.png"}/>
                <div className="text-white font-extra-bold member-name" data-testid="latest-6-scanned-user-name">
                  {`${member.firstName} ${member.lastName}`}
                </div>
                <div className="text-white mb-5 member-section"  data-testid="latest-6-scanned-section-name">
                  {`${member.sectionName}`}
                </div>
              </div>
              <div className="total-points text-center">
                <div>
                  <img src="/images/plus-circle-fill.svg" alt=""></img>
                </div>
                <strong
                  className="total-points-value text-white text-center"
                  style={{display: "block", width: "100%"}}
                  data-testid="latest-6-scanned-user-points">
                  {member.totalPoints}
                </strong>
                <div className="text-white" style={{marginTop: "-30px"}}>points...</div>
              </div>
            </div>
          </Col>
        )))}
      </Row>
    </>
  )
}
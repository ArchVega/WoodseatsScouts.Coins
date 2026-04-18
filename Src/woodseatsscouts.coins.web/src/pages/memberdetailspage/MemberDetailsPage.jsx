import {useParams} from "react-router-dom";
import {Button, Card, CardBody, CardHeader, Col, Row} from "reactstrap";
import Uris from "../../services/Uris";
import {useContext, useEffect, useState} from "react";
import {UseAppCameraContext} from "../../contexts/AppContext";
import MemberApiService from "../../services/MemberApiService";

function MemberDetailsPage() {
  const [useAppCamera] = useContext(UseAppCameraContext)
  const {memberCode} = useParams();

  const [member, setMember] = useState(null);

  const [selectedSession, setSelectedSession] = useState(null);

  const [userModal, setUserModal] = useState(false);
  const [editUserModal, setEditUserModal] = useState(false);
  const [editMemberNameModal, setEditMemberNameModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [filterText, setFilterText] = useState(null);
  const [showScores, setShowScores] = useState(true);

  // showMemberNameModal(member)}

  useEffect(() => {
    if (memberCode) {
      MemberApiService().fetchMember(memberCode).then(response => response.data).then(member => setMember(member));
    }
  }, [memberCode])

  useEffect(() => {
    console.log('member', member);
  }, [member])

  function showUserModal(user1) {
    setSelectedUser(user1)
    setUserModal(true)
  }

  function showEditUserModal(user1) {
    setSelectedUser(user1)
    setEditUserModal(true)
  }

  function RenderMember(member) {
    return (
      <Card className={"member-details-member-card flex-shrink-0 sticky-top"}>
        <CardBody>
          <Row>
            <Col>
              <img key={Date.now()}
                   onClick={() => useAppCamera ? showEditUserModal(member) : alert('Device does not have a camera or it is unavailable.')}
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
                {member.totalPoints}
              </div>
            </Col>
          </Row>
          <Row>
            <Col className={"members-list-item-section"}>
              <div>{member.troopName}</div>
            </Col>
          </Row>
          <Row className="mb-3 g-1">
            <Col className={"members-list-item-section"}>
              <Button className={"text-black"}>Add fixed amount points</Button>
            </Col>
            <Col className={"members-list-item-section"}>
              <Button className={"text-black"}>Remove fixed amount points</Button>
            </Col>
          </Row>
          <Row className="mb-3 g-1">
            <Col className={"members-list-item-section"}>
              <Button className={"text-black"}>Change Group</Button>
            </Col>
            <Col className={"members-list-item-section"}>
              <Button className={"text-black"}>Change Section</Button>
            </Col>
          </Row>
        </CardBody>
      </Card>
    )
  }

  const rows = Array.from({ length: 200 }); // 👈 controls height

  function RenderMemberScanSessions(member) {
    return (
      <table className="table table-striped">
        <thead>
        <tr>
          <th>Time</th>
          <th>Total Points</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
        </thead>

        <tbody>
        {rows.map((_, i) => (
          <tr key={i}>
            <td>{new Date().toLocaleString()}</td>
            <td>{i + 1}</td>
            <td><Button className={"btn-warning"}>EDIT</Button></td>
            <td><Button className={"btn-danger"}>DELETE</Button></td>
          </tr>
        ))}
        </tbody>
      </table>
    )
  }

  function RenderSelectedScanSessions(member) {
    return (
      <table className="table table-striped">
        <thead>
        <tr>
          <th>Base</th>
          <th>Total Points</th>
          <th>Edit</th>
          <th>Delete</th>
        </tr>
        </thead>

        <tbody>
        {rows.map((_, i) => (
          <tr key={i}>
            <td>{['Archery', 'Shooting', "Cave Bus", "Arts and Crafts"][Math.floor(Math.random() * 4)]}</td>
            <td>{i + 1}</td>
            <td><Button className={"btn-warning"}>EDIT</Button></td>
            <td><Button className={"btn-danger"}>DELETE</Button></td>
          </tr>
        ))}
        </tbody>
      </table>
    )
  }

  if (member) {
    return (
      <div id={"member-details-page"}>
        <Row className="g-3">
          <Col xs={2}>
            {RenderMember(member)}
          </Col>
          <Col xs={4}>
            <Card>
              {RenderMemberScanSessions(member)}
            </Card>
          </Col>
          <Col xs={4}>
            <Card>
              {!selectedSession && (
                <>Select a session</>
              )}
              {true && (
                RenderSelectedScanSessions(member)
              )}

            </Card>
          </Col>
          <Col xs={2}>
            <Row className="g-1 sticky-top">
              <Col xs={12} >
                <Card>
                  <CardBody>
                    <h5>Most Visited Base</h5>
                    <div>Archery</div>
                    <div>54 visits</div>
                  </CardBody>
                </Card>
              </Col>
              <Col xs={12}>
                <Card>
                  <CardBody>
                    <h5>Least Visited Base</h5>
                    <div>229th Greenhill</div>
                    <div>2 Visits</div>
                  </CardBody>
                </Card>
              </Col>
              <Col xs={12}>
                <Card>
                  <CardBody>
                    <h5>Most Scans</h5>
                    <div>44 Tokens</div>
                  </CardBody>
                </Card>
              </Col>
              <Col xs={12}>
                <Card>
                  <CardBody>
                    <h5>Total tokens scanned</h5>
                    <div>412</div>
                  </CardBody>
                </Card>
              </Col>
              <Col xs={12}>
                <Button className={"btn-success"}>Save Changes</Button>
              </Col>
            </Row>
          </Col>
        </Row>
      </div>
    )
  }

  return null
}

export default MemberDetailsPage;

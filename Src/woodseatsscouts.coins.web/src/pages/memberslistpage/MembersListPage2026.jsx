import {useContext, useEffect, useState} from "react";
import {Button, Card, CardBody, CardHeader, Col, Input, Row} from "reactstrap";
import MemberApiService from "../../services/MemberApiService";
import MemberPhotoModal from "../../components/modals/MemberPhotoModal";
import EditMemberPhotoModal from "../../components/modals/EditMemberPhotoModal";
import Uris from "../../services/Uris";
import {AppCameraAvailableContext, UseAppCameraContext} from "../../contexts/AppContext";
import EditMemberNameModal from "../../components/modals/EditMemberNameModal";

function MembersListPage() {
  const [useAppCamera] = useContext(UseAppCameraContext)

  const [members, setMembers] = useState([]);
  const [userModal, setUserModal] = useState(false);
  const [editUserModal, setEditUserModal] = useState(false);
  const [editMemberNameModal, setEditMemberNameModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [filterText, setFilterText] = useState(null);
  const [showScores, setShowScores] = useState(true);

  useEffect(() => {
    const fetchMembers = async () => {
      return await MemberApiService().fetchMembers();
    }
    fetchMembers().then(response => setMembers(response.data));
    console.log('only once')
  }, [])

  function sectionClassName(sectionName) {
    return `label-section-${sectionName.toLowerCase()}`
  }

  function showUserModal(user1) {
    setSelectedUser(user1)
    setUserModal(true)
  }

  function showEditUserModal(user1) {
    setSelectedUser(user1)
    setEditUserModal(true)
  }

  function showMemberNameModal(user1) {
    setSelectedUser(user1)
    setEditMemberNameModal(true)
    return false
  }

  function filterMember(member) {
    if (filterText === null || filterText.trim().length === 0) {
      return true;
    }

    const firstNameMatch = member.firstName.toLowerCase().includes(filterText.toLowerCase());
    const lastNameMatch = (member.lastName !== null && member.lastName.toLowerCase().includes(filterText.toLowerCase()));
    const troopNameMatch = member.troopName.toString().toLowerCase().includes(filterText.toLowerCase());
    const sectionMatch = member.sectionName.toString().toLowerCase().includes(filterText.toLowerCase())
    const memberCodeMatch = member.memberCode.toLowerCase().includes(filterText.toLowerCase());

    return firstNameMatch || lastNameMatch || troopNameMatch || sectionMatch || memberCodeMatch
  }

  function RenderMember(member) {
    return (
      <Card className={"members-list-item-card flex-shrink-0"}>
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
                {showScores ? member.totalPoints : "xxx"}
              </div>
            </Col>
          </Row>
          <Row>
            <Col className={"members-list-item-section"}>
              <div>{member.troopName}</div>
            </Col>
          </Row>
          <Row className="g-1">
            <Col xs={6} className={"members-list-item-section"}>
              <Button style={{color: "Black"}} onClick={() => showMemberNameModal(member)}>EDIT</Button>
            </Col>
            <Col xs={6} className={"members-list-item-section"}>
              <Button style={{color: "Black"}} onClick={() => showEditUserModal(member)}>PHOTO</Button>
            </Col>
          </Row>
        </CardBody>
      </Card>
    )
  }

  useEffect(() => {
    if (members && members.length > 0) {
      if (selectedUser && selectedUser.hasImage) {
        const updatedMembers = [...members]
        const selectedMember = updatedMembers.find(x => x.id === selectedUser.id)
        selectedMember.hasImage = true
        setMembers((updatedMembers))
      }

      if (selectedUser) {
        const newMembers = members.map((m, i) => {
          if (m.id === selectedUser.id) {
            return selectedUser
          } else {
            return m
          }
        })
        setMembers([...newMembers])
      }
    }
  }, [selectedUser])

  return <>
    <Row className="mb-3">
      <Col>
        <Input
          className="filter-members"
          data-testid="textbox-search-members"
          autoFocus={true}
          placeholder="Search to filter members, groups or section"
          onChange={x => setFilterText(x.target.value)}
        />
      </Col>
    </Row>
    <Row>
      <Col>
        <div id="members-list-grid" className="d-flex flex-row gap-3 overflow-auto flex-nowrap py-2 overflow-y-hidden">
          {members && members.filter(member => filterMember(member)).map(x => RenderMember(x))}
        </div>
      </Col>
    </Row>

    <MemberPhotoModal usersModal={userModal} setUsersModal={setUserModal} selectedUser={selectedUser}/>
    <EditMemberPhotoModal editUsersModal={editUserModal} setEditUsersModal={setEditUserModal} selectedUser={selectedUser}
                          setSelectedUser={setSelectedUser}/>
    <EditMemberNameModal editMembersModal={editMemberNameModal} setEditMembersModal={setEditMemberNameModal} selectedMember={selectedUser}
                         setSelectedMember={setSelectedUser}/>
  </>
}

export default MembersListPage
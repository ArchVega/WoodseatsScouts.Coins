import {useContext, useEffect, useState} from "react";
import {Col, Input, Row} from "reactstrap";
import MemberApiService from "../../services/MemberApiService";
import MemberPhotoModal from "../../components/modals/MemberPhotoModal";
import EditMemberPhotoModal from "../../components/modals/EditMemberPhotoModal";
import Uris from "../../services/Uris";
import {AppCameraAvailableContext, UseAppCameraContext} from "../../contexts/AppContext";

function MembersListPage() {
  const [useAppCamera] = useContext(UseAppCameraContext)

  const [members, setMembers] = useState([]);
  const [userModal, setUserModal] = useState(false);
  const [editUserModal, setEditUserModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [filterText, setFilterText] = useState(null);
  const [showScores, setShowScores] = useState(false);

  useEffect(() => {
    const fetchMembers = async () => {
      return await MemberApiService().fetchMembers();
    }
    fetchMembers().then(response => setMembers(response.data));
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

  useEffect(() => {
    if (members && members.length > 0) {
      if (selectedUser && selectedUser.hasImage) {
        const updatedMembers = [...members]
        const selectedMember = updatedMembers.find(x => x.id === selectedUser.id)
        selectedMember.hasImage = true
        setMembers((updatedMembers))
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
        <table className="table table-hover members-table" data-testid="table-members">
          <thead>
          <tr>
            <th className="fit"></th>
            <th>NAME</th>
            <th className="fit">GROUP</th>
            <th className="fit">SECTION</th>
            <th className="fit">TOTAL POINTS
              <input
                type="checkbox"
                data-testid="checkbox-show-total-points"
                onChange={(event) => setShowScores(event.target.checked)}
                style={{marginLeft: '2px'}}/>
            </th>
            <th className="fit">WRIST CODE</th>
            <th className="fit">EDIT PHOTO</th>
          </tr>
          </thead>
          <tbody>
          {members && members.filter(member => filterMember(member)).map(x => (
            <tr key={x.memberCode} data-testid={x.firstName + "-" + x.lastName}>
              <td>
                <span className="show-user-photo-icon"
                      onClick={() => showUserModal(x)}>
                    <img key={Date.now()} title={"User id: " + x.id}
                         src={x.hasImage ? Uris.memberPhoto(x.id) : "images/unknown-member-image.png"}
                         alt=""/>
                </span>
              </td>
              <td>
                <strong>{x.firstName + " " + x.lastName}</strong>
              </td>
              <td>
                <span>{x.troopName}</span>
              </td>
              <td className="label-section">
                <span>
                  <strong className={sectionClassName(x.sectionName)}>{x.sectionName}</strong>
                </span>
              </td>
              <td>
                <span>
                    {showScores ? x.totalPoints : "xxx"}
                </span>
              </td>
              <td>
                <span>{x.memberCode}</span>
              </td>
              <td>
                {useAppCamera
                  ? <span className="edit-user-photo-icon text-center"
                          role="button"
                          data-testid={x.firstName.toLowerCase() + "-" + x.lastName.toLowerCase() + "-upload-image-button"}
                          onClick={() => showEditUserModal(x)}>
                                    ✎
                                </span>
                  : <div title="If a camera is available, enable in the Settings menu" style={{width: "100%", height: "100%", marginLeft: "10px"}}>
                    ⃠
                  </div>
                }

              </td>
            </tr>
          ))}
          </tbody>
        </table>
      </Col>
    </Row>

    <MemberPhotoModal usersModal={userModal} setUsersModal={setUserModal} selectedUser={selectedUser}/>
    <EditMemberPhotoModal editUsersModal={editUserModal} setEditUsersModal={setEditUserModal} selectedUser={selectedUser}
                          setSelectedUser={setSelectedUser}/>
  </>
}

export default MembersListPage
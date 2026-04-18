import './MembersListPage.scss'

import {useContext, useEffect, useState} from "react";
import MemberApiService from "../../services/MemberApiService";
// import MemberPhotoModal from "../../components/modals/MemberPhotoModal";
// import EditMemberPhotoModal from "../../components/modals/EditMemberPhotoModal";
// import EditMemberNameModal from "../../components/modals/EditMemberNameModal";
import Uris from "../../services/Uris";
import {useNavigate} from "react-router-dom";
import {UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";

export default function MembersListPage() {
  const {useAppCamera} = useContext(UseAppCameraContext)
  const navigate = useNavigate();

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
      <div className="card members-list-item-card flex-shrink-0">
        <div className="card-body">
          <div className="row">
            <div className="col">
              <img key={Date.now()}
                   onClick={() => useAppCamera ? showEditUserModal(member) : alert('Device does not have a camera or it is unavailable.')}
                   title={"User id: " + member.id}
                   src={member.hasImage ? Uris.memberPhoto(member.id) : "images/unknown-member-image.png"} alt=""/>
            </div>
          </div>
          <div className="row">
            <div className={"col d-flex justify-content-center align-items-center members-list-item-section"} style={{height: "100px"}}>
              <strong className={"d-flex flex-column justify-content-center h-100"}>{member.firstName + " " + member.lastName}</strong>
            </div>
          </div>
          <div className="row g-1">
            <div className="col-7 members-list-item-section">
              <div>
                {member.memberCode}
              </div>
            </div>
            <div className="col-4 members-list-item-section">
              <div>
                {showScores ? member.totalPoints : "xxx"}
              </div>
            </div>
          </div>
          <div className="row">
            <div className="members-list-item-section">
              <div>{member.troopName}</div>
            </div>
          </div>
          <div className="row g-1">
            <div className="col-6 members-list-item-section">
              <button style={{color: "Black"}} onClick={() => navigate(`/member-details/${member.memberCode}`)}>EDIT</button>
            </div>
            <div className=" col-6 members-list-item-section">
              <button style={{color: "Black"}} onClick={() => showEditUserModal(member)}>PHOTO</button>
            </div>
          </div>
        </div>
      </div>
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

  return (
    <>
      <div className="row mt-3 mb-3">
        <div className="col">
          <input
            style={{width:'100%'}}
            className="filter-members"
            data-testid="textbox-search-members"
            autoFocus={true}
            placeholder="Search to filter members, groups or section"
            onChange={x => setFilterText(x.target.value)}
          />
        </div>
      </div>
      <div className="row">
        <div className="col">
          <div id="members-list-grid" className="d-flex flex-div row gap-3 overflow-auto flex-nowrap py-2 overflow-y-hidden">
            {members && members.filter(member => filterMember(member)).map(x => RenderMember(x))}
          </div>
        </div>
      </div>
    </>
  )
}
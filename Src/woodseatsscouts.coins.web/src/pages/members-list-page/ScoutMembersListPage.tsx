import './ScoutMembersListPage.scss'
import React, {useContext, useEffect, useState} from "react";
import MemberApiService from "../../services/apis/MemberApiService.ts";
import {useNavigate} from "react-router-dom";
import {UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import {Button} from "../../components/widgets/HtmlControlWrappers.tsx";
import {getSectionBranding} from "../../utilities/branding.ts";
import EditMemberPhotoModal from "../../components/modals/EditMemberPhotoModal.tsx";
import type {AxiosResponse} from "axios";
import type {ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";

export default function ScoutMembersListPage() {
  const {useAppCamera} = useContext(UseAppCameraContext)
  const navigate = useNavigate();

  const [members, setMembers] = useState<ScoutMemberPointsSummaryDto[]>([]);
  const [userModal, setUserModal] = useState<boolean>(false);
  const [editUserModal, setEditUserModal] = useState<boolean>(false);
  const [editMemberNameModal, setEditMemberNameModal] = useState<boolean>(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [filterText, setFilterText] = useState<string | null>(null);

  useEffect(() => {
    const fetchMembers = async (): Promise<AxiosResponse<ScoutMemberPointsSummaryDto[]>> => {
      return await MemberApiService().fetchMembers();
    }
    fetchMembers().then(response => {
      setMembers(response.data)
    });
  }, [])

  function showEditUserModal(user1) {
    setSelectedUser(user1)
    setEditUserModal(true)
  }

  function showMemberNameModal(user1) {
    setSelectedUser(user1)
    setEditMemberNameModal(true)
    return false
  }

  function filterMember(member: ScoutMemberPointsSummaryDto) {
    if (filterText === null || filterText.trim().length === 0) {
      return true;
    }

    const firstNameMatch = member.firstName.toLowerCase().includes(filterText.toLowerCase());
    const lastNameMatch = (member.lastName !== null && member.lastName.toLowerCase().includes(filterText.toLowerCase()));
    const scoutGroupNameMatch = member.scoutGroupName.toString().toLowerCase().includes(filterText.toLowerCase());
    const sectionMatch = member.scoutSectionName.toString().toLowerCase().includes(filterText.toLowerCase())
    const memberCodeMatch = member.scoutMemberCode.toLowerCase().includes(filterText.toLowerCase());

    return firstNameMatch || lastNameMatch || scoutGroupNameMatch || sectionMatch || memberCodeMatch
  }

  function RenderMember(scoutMember: ScoutMemberPointsSummaryDto) {
    const sectionBranding = getSectionBranding(scoutMember.scoutSectionCode)

    return (
      <div key={scoutMember.scoutMemberCode} className="card members-list-item-card flex-shrink-0">
        <div className="card-body">
          <div className="row pb-2">
            <div className="col">
              <img key={scoutMember.scoutMemberCode}
                   title={"User id: " + scoutMember.id}
                   src={scoutMember.clientComputedImageUri} alt=""/>
            </div>
          </div>
          <div className="row pb-2 member-name-row">
            <div className={"col d-flex justify-content-center align-items-center members-list-item-section"}>
              <strong className={"tile d-flex flex-column justify-content-center h-100"}>{scoutMember.firstName + " " + scoutMember.lastName}</strong>
            </div>
          </div>
          <div className="row pb-2 g-1">
            <div className="col-7 members-list-item-section" title={`Id: ${scoutMember.id}`}>
              <div className="tile">{scoutMember.scoutMemberCode}</div>
            </div>
            <div className="col-5 members-list-item-section">
              <div className="tile">{scoutMember.totalPoints}</div>
            </div>
          </div>
          <div className="row pb-2">
            <div className="members-list-item-section" title={`Section: ${scoutMember.scoutSectionName}`}>
              <div className="tile" style={{color: sectionBranding.foregroundColour, backgroundColor: sectionBranding.backgroundColour, fontWeight: "bold"}}>
                {scoutMember.scoutGroupName}
              </div>
            </div>
          </div>
          <div className="row pb-2 g-1">
            <div className="col-6 members-list-item-section">
              <Button onClick={() => navigate(`/members/${scoutMember.scoutMemberCode}`)}>EDIT</Button>
            </div>
            <div className=" col-6 members-list-item-section">
              <Button disabled={!useAppCamera}
                      onClick={() => useAppCamera ? showEditUserModal(scoutMember) : alert('Device does not have a camera or it is unavailable.')}>PHOTO</Button>
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
    <div id="members-list-page">
      <div className="row mt-3 mb-3">
        <div className="col">
          <input
            style={{width: '100%', height: "40px", color: 'black', textAlign: 'center'}}
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
          <div id="members-list-row" className="d-flex flex-div gap-3 overflow-auto flex-nowrap py-4">
            {members && members.filter(member => filterMember(member)).map(x => RenderMember(x))}
          </div>
        </div>
      </div>
      <EditMemberPhotoModal showEditMemberPhotoModal={editUserModal} setShowEditMemberPhotoModal={setEditUserModal} memberCompleteDto={selectedUser} setMemberCompleteDto={setSelectedUser}/>
    </div>
  )
}
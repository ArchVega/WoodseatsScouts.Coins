import './MemberDetailsPage.scss'
import {useParams} from "react-router-dom";
import React, {type ReactNode, useContext, useEffect, useState} from "react";
import {UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import MemberApiService from "../../services/MemberApiService.ts";
import Spinner from "../../components/widgets/Spinner.tsx";
import QRCodeInputDevices from "../../components/io/qr-input-devices/QRCodeInputDevices.tsx";
import QRScanCodeType from "../../components/io/qr-input-devices/QRScanCodeType.ts";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";
import ScoutsLogo from "../../images/fleur-de-lis-marque-white.png";
import Uris from "../../services/Uris.ts";
import type {Member} from "../../types/ServerTypes.ts";

function MemberDetailsPage() {
  const {useAppCamera} = useContext(UseAppCameraContext)
  const {memberCode} = useParams();
  const [loading, setLoading] = useState(false)
  const [member, setMember] = useState(null);
  const [selectedSession, setSelectedSession] = useState(null);
  const [userModal, setUserModal] = useState(false);
  const [editUserModal, setEditUserModal] = useState(false);
  const [editMemberNameModal, setEditMemberNameModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [filterText, setFilterText] = useState(null);
  const [showScores, setShowScores] = useState(true);

  // showMemberNameModal(member)}

  // todo: these are what we need to hook up
  // <EditMemberNameModal editMembersModal={editMemberNameModal} setEditMembersModal={setEditMemberNameModal} selectedMember={selectedUser}
  //                      setSelectedMember={setSelectedUser}/>
  // <EditMemberPhotoModal editUsersModal={editUserModal} setEditUsersModal={setEditUserModal} selectedUser={selectedUser}
  //                       setSelectedUser={setSelectedUser}/>

  useEffect(() => {
    if (memberCode) {
      setLoading(true)
      MemberApiService().fetchMember(memberCode).then(response => response.data).then(member => setMember(member)).finally(() => setLoading(false));
    }
  }, [memberCode])

  useEffect(() => {
    console.log('member', member);
  }, [member])

  function showEditUserModal(user1) {
    setSelectedUser(user1)
    setEditUserModal(true)
  }

  function RenderMemberDetails(member: Member) {
    return (
      <>
        <div className="card member-details-member-card flex-shrink-0 sticky-top mb-3">
          <div className="card-body">
            <Image key={Date.now()}
                   className="mb-2"
                   onClick={() => useAppCamera ? showEditUserModal(member) : alert('Device does not have a camera or it is unavailable.')}
                   title={"User id: " + member.memberId}
                   src={member.hasImage ? Uris.memberPhoto(member.memberId) : "images/unknown-member-image.png"} alt=""/>
            <div className="row mb-2">
              <div className={"d-flex justify-content-center align-items-center members-list-item-section"} style={{height: "100px"}}>
                <strong className="tile d-flex flex-column justify-content-center h-100">{member.firstName + " " + member.lastName}</strong>
              </div>
            </div>
            <div className="row  mb-2 g-1">
              <div className="col-6 members-list-item-section">
                <div className="tile" style={{fontSize: "12px"}}>{member.memberCode}</div>
              </div>
              <div className="col-6 members-list-item-section">
                <div className="tile" style={{fontSize: "12px"}}>todo-pts</div>
              </div>
            </div>
            <div className="row mb-2">
              <div className="members-list-item-section">
                <div className="tile">{member.memberTroopName}</div>
              </div>
            </div>
          </div>
        </div>

        <div className="row mb-3 g-1">
          <div className={"member-action-button-container"}>
            <button id="add-points-button">Add fixed amount points</button>
          </div>
          <div className={"member-action-button-container"}>
            <button id="remove-points-button">Remove fixed amount points</button>
          </div>
        </div>
        <div className="row g-1">
          <div className={"member-action-button-container"}>
            <button className={"text-black"}>Change Group</button>
          </div>
          <div className={"member-action-button-container"}>
            <button className={"text-black"}>Change Section</button>
          </div>
        </div>
      </>
    )
  }

  const rows = Array.from({length: 200}); // 👈 controls height

  function formatDateTime(date: Date) {
    return new Intl.DateTimeFormat("en-GB", {
      dateStyle: "short",
      timeStyle: "short", // ← this omits seconds
    }).format(date);
  }

  function RenderMemberScanSessions() {
    return (
      <div className="member-details-table-container card">
        <div className="row">
          <div className="col-12 mt-1">
            <h4>Member's Scan Sessions</h4>
          </div>
        </div>
        <table className="table table-bordered">
          <thead className="table-dark">
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
              <td>{formatDateTime(new Date())}</td>
              <td>{i + 1}</td>
              <td>
                <span>✏️</span>
              </td>
              <td>
                <span>🗑️</span>
              </td>
            </tr>
          ))}
          </tbody>
        </table>
      </div>
    )
  }

  function RenderSelectedScanSessions() {
    return (
      <div className="member-details-table-container card">
        <div className="row">
          <div className="col-12 mt-1">
            <h4 className="mb-0">Edit Member's Scan Points</h4>
            <small>Session: 10/03/2026 10:45</small>
          </div>
        </div>
        <table className="table table-bordered">
          <thead className="table-dark">
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
              <td>
                <span>✏️</span>
              </td>
              <td>
                <span>🗑️</span>
              </td>
            </tr>
          ))}
          </tbody>
        </table>
      </div>
    )
  }

  function RenderMemberActivitySummary() {
    function RenderActivityCard(title: string, node: ReactNode) {
      return (
        <div className="col-12">
          <div className="card">
            <div className="card-body">
              <div className="title">{title}</div>
              <div className="children">{node}</div>
            </div>
          </div>
        </div>
      )
    }

    return (
      <div id="recent-member-activity-summary-cards" className="row g-1 sticky-top">
        {RenderActivityCard("Most Visited Base", <>
          <div><strong className="fs-3">Archery</strong></div>
          <div><em>54 visits</em></div>
        </>)}
        {RenderActivityCard("Least Visited Base", <>
          <div><strong className="fs-3">229th Greenhill</strong></div>
          <div><em>2 Visits</em></div>
        </>)}
        {RenderActivityCard("Most Scans", <>
          <div><strong className="fs-3">44 Tokens</strong></div>
        </>)}
        {RenderActivityCard("Total tokens scanned", <>
          <div><strong className="fs-3">412</strong></div>
        </>)}
      </div>
    )
  }

  if (loading) {
    return <Spinner/>
  }

  if (member) {
    return (
      <div id="member-details-page">
        <div className="row mt-1 g-3">
          <div className={"col-2"}>
            {RenderMemberDetails(member)}
          </div>
          <div className={"col-4"}>
            {RenderMemberScanSessions()}
          </div>
          <div className={"col-4"}>
            {/*{!selectedSession && (*/}
            {/*  <>Select a session</>*/}
            {/*)}*/}
            {
              RenderSelectedScanSessions()
            }
          </div>
          <div className={"col-2"}>
            {RenderMemberActivitySummary()}
          </div>
        </div>
      </div>
    )
  }

  return null
}

export default MemberDetailsPage;

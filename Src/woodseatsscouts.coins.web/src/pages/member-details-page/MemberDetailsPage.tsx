import './MemberDetailsPage.scss'
import {useParams} from "react-router-dom";
import React, {type ReactNode, useContext, useEffect, useState} from "react";
import {PageActionMenuAreaContext, UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import MemberApiService from "../../services/apis/MemberApiService.ts";
import Spinner from "../../components/widgets/Spinner.tsx";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";
import type {ActivityBaseHaulResultDto, HaulResultDto, MemberCompleteSummaryStatsActivityBaseInfoDto, ScannedCoinDto, ScoutMemberCompleteDto} from "../../types/ServerTypes.ts";
import {getSectionBranding} from "../../utilities/branding.ts";
import EditMemberPhotoModal from "../../components/modals/EditMemberPhotoModal.tsx";
import {logObject} from "../../components/logging/Logger.ts";
import EditMemberDetailsModal from "../../components/modals/EditMemberDetailsModal.tsx";
import EditScannedCoinPointsModal from "../../components/modals/EditScannedCoinPointsModal.tsx";
import ScannedCoinApiService from "../../services/apis/ScannedCoinApiService.ts";
import ScanSessionApiService from "../../services/apis/ScanSessionApiService.ts";
import {usePasscode} from "../../components/security/usePasscode.ts";

export default function MemberDetailsPage() {
  const {useAppCamera} = useContext(UseAppCameraContext)
  const {memberId} = useParams();
  const [loading, setLoading] = useState(false)
  const [showEditMemberPhotoModal, setShowEditMemberPhotoModal] = useState<boolean>(false);
  const [showEditMemberDetailsModal, setShowEditMemberDetailsModal] = useState<boolean>(false);
  const [showEditScannedCoinPointsModal, setShowEditScannedCoinPointsModal] = useState<boolean>(false);
  const [memberCompleteDto, setMemberCompleteDto] = useState<ScoutMemberCompleteDto | null>(null);
  const [activeHaulResultDto, setActiveHaulResultDto] = useState<HaulResultDto | null>(null);
  const [selectedScanSessionId, setSelectedScanSessionId] = useState<number | null>(null);
  const [selectedScannedCoinDto, setSelectedScannedCoinDto] = useState<ScannedCoinDto | undefined>(undefined);
  const {checkPasscode} = usePasscode();
  const {setPageActionMenuAreaAction} = useContext(PageActionMenuAreaContext)

  useEffect(() => {
    setPageActionMenuAreaAction(null)
  }, []);

  useEffect(() => {
    if (memberId) {
      setLoading(true)
      const memberIdNumber = Number(memberId);
      MemberApiService()
        .getMemberComplete(memberIdNumber)
        .then(response => {
          return response.data
        })
        .then((memberCompleteDto: ScoutMemberCompleteDto) => {
          logObject("memberCompleteDto", memberCompleteDto)
          setMemberCompleteDto(memberCompleteDto)
          if (memberCompleteDto.haulResults.length > 0) {
            setSelectedScanSessionId(memberCompleteDto.haulResults[0].scanSessionId)
          }
        })
        .finally(() => setLoading(false));
    }
  }, [memberId])

  useEffect(() => {
    if (selectedScanSessionId !== null) {
      const selectedHaulResultDto = memberCompleteDto.haulResults.find(x => x.scanSessionId == selectedScanSessionId);
      setActiveHaulResultDto(selectedHaulResultDto);
    }
  }, [selectedScanSessionId, memberCompleteDto]);

  function tryEditScoutMemberPhoto() {
    useAppCamera ? setShowEditMemberPhotoModal(true) : alert('Device does not have a camera or it is unavailable.')
  }

  function tryEditScoutMemberDetails() {
    setShowEditMemberDetailsModal(true)
  }

  function formatDateTime(isoDateString: string) {
    return new Intl.DateTimeFormat("en-GB", {
      weekday: "long",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    }).format(new Date(isoDateString));
  }

  function deleteScanSession(haulResultDto: HaulResultDto) {
    if (checkPasscode()) {
      // todo: quick hack using timeout to allow selected row to visually update before confirm box loads.
      setTimeout(() => {
        const c = confirm(`Are you sure you want to delete this session for ${memberCompleteDto.firstName}'s session? This is irreversible!`)
        if (c) {
          ScanSessionApiService().deleteScanSession(haulResultDto.scanSessionId).then(value => {
            // Todo: implement dynamic solution later
            alert('Reload page to see updated points value')
          })
        }
      }, 100)
    }
  }

  function updateScannedCoinPoints(scannedCoinDto: ScannedCoinDto) {
    if (checkPasscode()) {
      setSelectedScannedCoinDto(scannedCoinDto);
      setShowEditScannedCoinPointsModal(true)
    }
  }

  function deleteScannedCoin(scannedCoinDto: ScannedCoinDto) {
    if (checkPasscode()) {
      const c = confirm(`Are you sure you want to delete this coin that has ${scannedCoinDto.calculatedEffectivePoints} points from ${memberCompleteDto.firstName}'s session? This is irreversible!`)
      if (c) {
        ScannedCoinApiService().deleteScannedCoin(scannedCoinDto.scannedCoinId).then(value =>
          // Todo: implement dynamic solution later
          alert('Reload page to see updated points value'))
      }
    }
  }

  function RenderMemberDetails() {
    const sectionBranding = getSectionBranding(memberCompleteDto.scoutSectionCode)

    return (
      <>
        <div className="card member-details-member-card flex-shrink-0 sticky-top mb-3">
          <div className="card-body">
            <Image className="mb-2"
                   onClick={() => tryEditScoutMemberPhoto()}
                   title={"User id: " + memberCompleteDto.id}
                   src={memberCompleteDto.clientComputedImageUri}/>
            <div className="row mb-2">
              <div role="button" className={"d-flex justify-content-center align-items-center members-list-item-section"} style={{height: "100px"}}
                   onClick={() => tryEditScoutMemberDetails()}>
                <strong className="tile d-flex flex-column justify-content-center h-100">{memberCompleteDto.firstName + " " + memberCompleteDto.lastName}</strong>
              </div>
            </div>
            <div className="row  mb-2 g-1">
              <div className="col-6 members-list-item-section">
                <div className="tile" style={{fontSize: "12px"}}>{memberCompleteDto.scoutMemberCode}</div>
              </div>
              <div className="col-6 members-list-item-section">
                <div className="tile" style={{fontSize: "12px"}}>{memberCompleteDto.totalPoints}</div>
              </div>
            </div>
            <div className="row mb-2">
              <div className="members-list-item-section" title={`Section: ${memberCompleteDto.scoutSectionName}`}>
                <div role="button" className="tile" style={{backgroundColor: sectionBranding.backgroundColour, color: sectionBranding.foregroundColour}}
                     onClick={() => tryEditScoutMemberDetails()}>
                  {memberCompleteDto.scoutGroupName}
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    )
  }

  function RenderMemberScanSessions() {
    return (
      <div className="member-details-table-container card">
        <div className="row">
          <div className="col-12 mt-1">
            <h4>{memberCompleteDto.firstName}'s Scan Sessions</h4>
          </div>
        </div>
        {memberCompleteDto && memberCompleteDto.haulResults.length !== 0 && (
          <table className="table table-bordered">
            <thead className="table-dark">
            <tr>
              <th>Time</th>
              <th>Total Points</th>
              <th>Delete</th>
            </tr>
            </thead>
            <tbody>
            {memberCompleteDto && memberCompleteDto.haulResults.map((haulResultDto, index) => (
              <tr key={index} role="button" onClick={() => setSelectedScanSessionId(haulResultDto.scanSessionId)}
                  style={{
                    backgroundColor: selectedScanSessionId === haulResultDto.scanSessionId ? "#d3e5ff" : "transparent"
                  }}>
                <td>{formatDateTime(haulResultDto.hauledAtIso8601)}</td>
                <td>{haulResultDto.totalPoints}</td>
                <td>
                  <span role="button" onClick={e => deleteScanSession(haulResultDto)}>🗑️</span>
                </td>
              </tr>
            ))}
            </tbody>
          </table>
        )}
        {memberCompleteDto && memberCompleteDto.haulResults.length === 0 && (
          <>
            <hr/>
            <div className="row mt-3">
              <div className="col-12">
                {memberCompleteDto.firstName} hasn't scanned any coins yet.
              </div>
            </div>
          </>
        )}
      </div>
    )
  }

  function RenderSelectedScanSessions() {
    if (memberCompleteDto && memberCompleteDto.haulResults.length === 0) {
      return (
        <div className="card member-details-table-container">
          <div className="card-body text-center">
            Details will be shown here when {memberCompleteDto.firstName} has scanned coins.
          </div>
        </div>
      )
    }

    if (!activeHaulResultDto) {
      return (
        <div className="card member-details-table-container">
          <div className="card-body text-center">
            Select a Session.
          </div>
        </div>
      )
    }

    return (
      <div className="member-details-table-container card">
        <div className="row">
          <div className="col-12 mt-1">
            <h4 className="mb-0">Edit {memberCompleteDto.firstName}'s Scan Points</h4>
            <small>Session: {formatDateTime(activeHaulResultDto.hauledAtIso8601)}</small>
          </div>
        </div>
        <table className="table table-bordered">
          <thead className="table-dark">
          <tr>
            <th scope="col">Base</th>
            <th scope="col">Points</th>
            <th scope="col">Edit</th>
            <th scope="col">Delete</th>
          </tr>
          </thead>
          <tbody>
          {activeHaulResultDto && activeHaulResultDto.activityBaseHaulResultDtos.map((activityBaseHaulResultDto: ActivityBaseHaulResultDto, haulIndex) =>
              activityBaseHaulResultDto.scannedCoinDtos && activityBaseHaulResultDto.scannedCoinDtos.map((scannedCoinDto, scannedCoinDtoIndex) => (
                <tr key={`${haulIndex}-${scannedCoinDtoIndex}`}>
                  {scannedCoinDtoIndex === 0 && (
                    <td rowSpan={activityBaseHaulResultDto.scannedCoinDtos.length}>
                      <div className="pe-2" style={{display: "flex", justifyContent: "space-between"}}>
                        <span>{activityBaseHaulResultDto.activityBaseName}</span>
                        <span>(x {activityBaseHaulResultDto.scannedCoinDtos.length})</span>
                      </div>
                    </td>
                  )}
                  {scannedCoinDto.hasPointsOverride && (
                    <td><strong title={`This coin's points has been adjusted. The original Points value is ${scannedCoinDto.points}.`}
                                className="text-danger">{scannedCoinDto.calculatedEffectivePoints}*</strong></td>
                  )}
                  {!scannedCoinDto.hasPointsOverride && (
                    <td>{scannedCoinDto.calculatedEffectivePoints}</td>
                  )}
                  <td>
                    <span role="button" onClick={e => updateScannedCoinPoints(scannedCoinDto)}>✏️</span>
                  </td>
                  <td>
                    <span role="button" onClick={e => deleteScannedCoin(scannedCoinDto)}>🗑️</span>
                  </td>
                </tr>
              ))
          )}
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
        {RenderActivityCard("Most Visited Bases", (
          <>
            {memberCompleteDto.scoutMemberCompleteSummaryStatsDto
              && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBasesByParticipant
              && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBasesByParticipant.map((x: MemberCompleteSummaryStatsActivityBaseInfoDto, i) => (
                <div key={i}>
                  <strong className="fs-5">{x.name} ({x.timesVisited})</strong>
                </div>
              ))}
            {memberCompleteDto.scoutMemberCompleteSummaryStatsDto
              && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBasesByParticipant
              && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBasesByParticipant.length === 0
              && (
                <strong>No bases visited yet!</strong>
              )
            }
          </>
        ))}
        {RenderActivityCard("Least Visited Bases", <>
          {memberCompleteDto.scoutMemberCompleteSummaryStatsDto
            && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBasesByParticipant
            && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBasesByParticipant.map((x: MemberCompleteSummaryStatsActivityBaseInfoDto, i) => (
              <div key={i}>
                <strong className="fs-5">{x.name} ({x.timesVisited})</strong>
              </div>
            ))}
        </>)}
        {RenderActivityCard("Least Visited Bases By Others", <>
          {memberCompleteDto.scoutMemberCompleteSummaryStatsDto
            && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBasesByOtherParticipants
            && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBasesByOtherParticipants.map((x: MemberCompleteSummaryStatsActivityBaseInfoDto, i) => (
              <div key={i}>
                <strong className="fs-5">{x.name} ({x.timesVisited})</strong>
              </div>
            ))}
        </>)}
        {RenderActivityCard("Most Scans", <>
          <div><strong className="fs-3">{memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostScans} Tokens</strong></div>
        </>)}
        {RenderActivityCard("Total tokens scanned", <>
          <div><strong className="fs-3">{memberCompleteDto.scoutMemberCompleteSummaryStatsDto.totalTokensScanned}</strong></div>
        </>)}
      </div>
    )
  }

  if (loading) {
    return <Spinner/>
  }

  if (memberCompleteDto) {
    return (
      <div id="member-details-page">
        <div className="row mt-1 g-3">
          <div className={"col-2"}>
            {RenderMemberDetails()}
          </div>
          <div className={"col-4"}>
            {RenderMemberScanSessions()}
          </div>
          <div className={"col-4"}>
            {RenderSelectedScanSessions()}
          </div>
          <div className={"col-2"}>
            {RenderMemberActivitySummary()}
          </div>
        </div>
        <EditMemberPhotoModal
          showEditMemberPhotoModal={showEditMemberPhotoModal}
          setShowEditMemberPhotoModal={setShowEditMemberPhotoModal}
          memberCompleteDto={memberCompleteDto}
          setMemberCompleteDto={setMemberCompleteDto}/>
        <EditMemberDetailsModal
          showModal={showEditMemberDetailsModal}
          setShowModal={setShowEditMemberDetailsModal}
          memberCompleteDto={memberCompleteDto}
          setMemberCompleteDto={setMemberCompleteDto}/>
        <EditScannedCoinPointsModal
          showModal={showEditScannedCoinPointsModal}
          setShowModal={setShowEditScannedCoinPointsModal}
          scannedCoinDto={selectedScannedCoinDto}
          setScannedCoinDto={setSelectedScannedCoinDto}
        />
      </div>
    )
  }

  return null
}
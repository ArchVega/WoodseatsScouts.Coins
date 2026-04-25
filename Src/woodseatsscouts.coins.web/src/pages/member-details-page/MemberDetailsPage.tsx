import './MemberDetailsPage.scss'
import {useParams} from "react-router-dom";
import React, {type ReactNode, useContext, useEffect, useState} from "react";
import {UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import MemberApiService from "../../services/apis/MemberApiService.ts";
import Spinner from "../../components/widgets/Spinner.tsx";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";
import type {ActivityBaseHaulResultDto, HaulResultDto, ScoutMemberCompleteDto} from "../../types/ServerTypes.ts";
import {getSectionBranding} from "../../utilities/branding.ts";
import EditMemberPhotoModal from "../../components/modals/EditMemberPhotoModal.tsx";
import {logObject} from "../../components/logging/Logger.ts";
import EditMemberDetailsModal from "../../components/modals/EditMemberDetailsModal.tsx";

export default function MemberDetailsPage() {
  const {useAppCamera} = useContext(UseAppCameraContext)
  const {memberId} = useParams();
  const [loading, setLoading] = useState(false)
  const [showEditMemberPhotoModal, setShowEditMemberPhotoModal] = useState<boolean>(false);
  const [showEditMemberDetailsModal, setShowEditMemberDetailsModal] = useState<boolean>(false);
  const [memberCompleteDto, setMemberCompleteDto] = useState<ScoutMemberCompleteDto | null>(null);
  const [activeHaulResultDto, setActiveHaulResultDto] = useState<HaulResultDto | null>(null);
  const [selectedScanSessionId, setSelectedScanSessionId] = useState<number | null>(null);

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
          if (memberCompleteDto.haulResults.length >= 0) {
            setSelectedScanSessionId(memberCompleteDto.haulResults[0].scavengerResultId)
          }
        })
        .finally(() => setLoading(false));
    }
  }, [memberId])

  useEffect(() => {
    if (selectedScanSessionId !== null) {
      const selectedHaulResultDto = memberCompleteDto.haulResults.find(x => x.scavengerResultId == selectedScanSessionId);
      setActiveHaulResultDto(selectedHaulResultDto);
    }
  }, [selectedScanSessionId, memberCompleteDto]);

  function RenderMemberDetails() {
    const sectionBranding = getSectionBranding(memberCompleteDto.scoutSectionCode)

    return (
      <>
        <div className="card member-details-member-card flex-shrink-0 sticky-top mb-3">
          <div className="card-body">
            <Image className="mb-2"
                   onClick={() => useAppCamera ? setShowEditMemberPhotoModal(true) : alert('Device does not have a camera or it is unavailable.')}
                   title={"User id: " + memberCompleteDto.id}
                   src={memberCompleteDto.clientComputedImageUri}/>
            <div className="row mb-2">
              <div role="button" className={"d-flex justify-content-center align-items-center members-list-item-section"} style={{height: "100px"}}
                   onClick={() => setShowEditMemberDetailsModal(true)}>
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
                     onClick={() => setShowEditMemberDetailsModal(true)}>
                  {memberCompleteDto.scoutGroupName}
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    )
  }

  function formatDateTime(isoDateString: string) {
    return new Intl.DateTimeFormat("en-GB", {
      weekday: "long",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    }).format(new Date(isoDateString));
  }

  function RenderMemberScanSessions() {
    return (
      <div className="member-details-table-container card">
        <div className="row">
          <div className="col-12 mt-1">
            <h4>{memberCompleteDto.firstName}'s Scan Sessions</h4>
          </div>
        </div>
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
            <tr key={index} role="button" onClick={() => setSelectedScanSessionId(haulResultDto.scavengerResultId)}
                style={{
                  backgroundColor: selectedScanSessionId === haulResultDto.scavengerResultId ? "#d3e5ff" : "transparent"
                }}>
              <td>{formatDateTime(haulResultDto.hauledAtIso8601)}</td>
              <td>{haulResultDto.totalPoints}</td>
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
    if (!activeHaulResultDto) {
      return (
        <div className="card">
          <div className="card-body text-center">
            Select a Session
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
              activityBaseHaulResultDto.coins && activityBaseHaulResultDto.coins.map((coin, coinIndex) => (
                <tr key={`${haulIndex}-${coinIndex}`}>
                  {coinIndex === 0 && (
                    <td rowSpan={activityBaseHaulResultDto.coins.length}>
                      <div className="pe-2" style={{display: "flex", justifyContent: "space-between"}}>
                        <span>{activityBaseHaulResultDto.activityBaseName}</span>
                        <span>(x {activityBaseHaulResultDto.coins.length})</span>
                      </div>
                    </td>
                  )}
                  <td>{coin.pointValue}</td>
                  <td>
                    <span>✏️</span>
                  </td>
                  <td>
                    <span>🗑️</span>
                  </td>
                </tr>
              ))
          )}
          </tbody>
        </table>
      </div>
    )
  }

  {/*<td>{activityBaseHaulResultDto.totalPoints}</td>*/
  }
  {/*<td>{activityBaseHaulResultDto.coinsScanned}</td>*/
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
        {RenderActivityCard("Most Visited Base", (
          <>
            {memberCompleteDto.scoutMemberCompleteSummaryStatsDto
              && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBase
              && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBase.names.map((x, i) => (
                <div key={i}>
                  <strong className="fs-3">{x}</strong>
                </div>
              ))}
            <div><em>{memberCompleteDto.scoutMemberCompleteSummaryStatsDto.mostVisitedActivityBase.timesVisited} visits</em></div>
          </>
        ))}
        {RenderActivityCard("Least Visited Base", <>
          {memberCompleteDto.scoutMemberCompleteSummaryStatsDto
            && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBase
            && memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBase.names.map((x, i) => (
              <div key={i}>
                <strong className="fs-3">{x}</strong>
              </div>
            ))}
          <div><em>{memberCompleteDto.scoutMemberCompleteSummaryStatsDto.leastVisitedActivityBase.timesVisited} visits</em></div>
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
      </div>
    )
  }

  return null
}
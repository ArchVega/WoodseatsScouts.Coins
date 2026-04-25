import "./MembersLatestScans.scss"
import {useContext, useEffect, useState} from "react";
import Uris from "../../services/apis/Uris.ts";
import {type AxiosResponse} from "axios";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";
import type {ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {getSectionBranding} from "../../utilities/branding.ts";
import { format } from 'timeago.js';
import {AppSettingsContext} from "../../contexts/AppContextExporter.tsx";
import {apiClient} from "../../services/apis/apiClient.ts";

export default function MembersLatestScansPage() {
  const {appSettings} = useContext(AppSettingsContext)
  const [members, setMembers] = useState<ScoutMemberPointsSummaryDto[]>([])

  useEffect(() => {
    function loadData() {
      setMembers([])

      apiClient
        .get(Uris.scans().sessions().sessionsLatest())
        .then(async (response: AxiosResponse<ScoutMemberPointsSummaryDto[]>) => {
          const members = response.data
          setMembers(members)
        })
    }

    loadData()

    setInterval(() => {
      loadData()
    }, appSettings.VITE_RECENT_SCANS_REFRESH_INTERVAL_SECONDS * 1000)
  }, []);

  function RenderMember(member: ScoutMemberPointsSummaryDto) {
    const sectionBranding = getSectionBranding(member.scoutSectionCode)

    return (
      <div className="card latest-scans-item-card" >
        <div className="card-body">
          <div className="row">
            <div className="col">
              <Image style={{height: "150px", width: "150px"}} key={member.id} src={member.clientComputedImageUri}/>
            </div>
          </div>
          <div className="first-name mt-2 m-0">
            <strong>{member.firstName}</strong>
          </div>
          <div className="points m-0">
            <strong>{member.selectedHaulResult.totalPoints}</strong>
          </div>
          <div className="time m-0">
            <em>{format(member.selectedHaulResult.hauledAtIso8601)}</em>
          </div>
          <div className="group mt-2">
            <div style={{backgroundColor: sectionBranding.backgroundColour}}>
              <strong style={{color: sectionBranding.foregroundColour}}>{member.scoutGroupName}</strong>
            </div>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div id="latest-scans-page" className="d-flex flex-wrap mt-2">
      {members.map((member, i) => (
        // <div key={i} className="w-16 p-2">
        //   <div className="card h-100">
        //     {RenderMember(member)}
        //   </div>
        // </div>
        <div key={i} className="card w-16 p-2">
            {RenderMember(member)}
        </div>
      ))}
    </div>
  )
}
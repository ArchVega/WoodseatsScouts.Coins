import "./MembersLatestScans.scss"
import {useContext, useEffect, useState} from "react";
import Uris from "../../services/apis/Uris.ts";
import axios, {type AxiosResponse} from "axios";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";
import type {MemberPointsSummaryDto, MembersWithPoints} from "../../types/ServerTypes.ts";
import {getSectionBranding} from "../../utilities/branding.ts";
import { format } from 'timeago.js';
import {AppSettingsContext} from "../../contexts/AppContextExporter.tsx";

export default function MembersLatestScansPage() {
  const {appSettings} = useContext(AppSettingsContext)
  const [members, setMembers] = useState<MemberPointsSummaryDto[]>([])

  useEffect(() => {
    function loadData() {
      setMembers([])

      axios
        .get(Uris.scans().sessionsLatest())
        .then(async (response: AxiosResponse<MemberPointsSummaryDto[]>) => {
          const members = response.data
          response.data.forEach(memberPointsSummaryDto => {
            memberPointsSummaryDto.clientComputedImageUri = Uris.scouts().members().memberPhoto(memberPointsSummaryDto.computedImagePath) // todo: is there an axios way to do this automatically?
          })

          setMembers(members)
        })
    }

    loadData()

    setInterval(() => {
      loadData()
    }, appSettings.VITE_RECENT_SCANS_REFRESH_INTERVAL_SECONDS * 1000)
  }, []);

  function RenderMember(member: MemberPointsSummaryDto) {
    const sectionBranding = getSectionBranding(member.sectionId)

    return (
      <div className="card latest-scans-item-card flex-shrink-0" >
        <div className="card-body">
          <div className="row">
            <div className="col">
              <Image style={{height: "150px"}} key={member.id} src={member.clientComputedImageUri}/>
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
        <div key={i} className="w-16 p-2">
          <div className="card h-100">
            {RenderMember(member)}
          </div>
        </div>
      ))}
    </div>
  )
}
import "./MembersLatestScans.scss"
import {useEffect, useState} from "react";
import Uris from "../../services/apis/Uris.ts";
import axios from "axios";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";
import type {MembersWithPoints} from "../../types/ServerTypes.ts";
import {getSectionBranding} from "../../utilities/branding.ts";
import { format } from 'timeago.js';

export default function MembersLatestScansPage() {
  const [members, setMembers] = useState([])

  useEffect(() => {
    function loadData() {
      setMembers([])

      axios
        .get(Uris.latest6Scavengers)
        .then(async response => {
          const members = await response.data
          setMembers(members)
        })
    }

    loadData()

    axios
      .get(Uris.refreshSecondsForLatestScans)
      .then(async response => {
        const seconds = Number(await response.data)
        setInterval(() => {
          loadData()
        }, seconds * 1000)
      })
  }, []);

  function RenderMember(member: MembersWithPoints) {
    const sectionBranding = getSectionBranding(member.sectionId)

    return (
      <div className="card members-list-item-card flex-shrink-0" >
        <div className="card-body">
          <div className="row">
            <div className="col">
              <Image style={{height: "150px"}} key={member.id} src={member.hasImage ? Uris.memberPhoto(member.id) : "/images/unknown-member-image.png"}/>
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
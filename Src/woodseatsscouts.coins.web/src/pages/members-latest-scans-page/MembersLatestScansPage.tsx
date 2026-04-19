import "./MembersLatestScans.scss"
import {useEffect, useState} from "react";
import Uris from "../../services/Uris";
import axios from "axios";
import {Image} from "../../components/widgets/HtmlControlWrappers.tsx";

export default function MembersLatestScansPage() {
  const [members, setMembers] = useState([])

  useEffect(() => {
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

  function loadData() {
    setMembers([])

    axios
      .get(Uris.latest6Scavengers)
      .then(async response => {
        const members = await response.data
        const repeated = Array.from({length: 40}, () => members).flat();
        setMembers(repeated)
      })
  }

  function sectionBackgroundClassName(sectionName) {
    return `member-info-inner background-${sectionName.toLowerCase()}`
  }

  function RenderMember(member) {
    return (
      <div className="card members-list-item-card flex-shrink-0" >
        <div className="card-body">
          <div className="row">
            <div className="col">
              <Image style={{height: "150px"}} key={member.id} src={member.hasImage ? Uris.memberPhoto(member.id) : "images/unknown-member-image.png"}/>
            </div>
          </div>
          <div className="first-name mt-2 m-0">
            <strong>{member.firstName}</strong>
          </div>
          <div className="points m-0">
            <strong>points</strong>
          </div>
          <div className="group mt-2">
            <div>
              <strong>229th Greenhill</strong>
            </div>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div id="latest-scans-page" className="d-flex flex-wrap mt-2">
      {members.map((item, i) => (
        <div key={i} className="w-16 p-2">
          <div className="card h-100">
            {RenderMember(item)}
          </div>
        </div>
      ))}
    </div>
  )
}
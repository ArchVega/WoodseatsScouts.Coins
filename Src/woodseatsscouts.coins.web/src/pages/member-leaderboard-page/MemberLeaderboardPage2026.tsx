import {useEffect, useState} from "react";
import Uris from "../../services/Uris";
import axios from "axios";

export default function MemberLeaderboardPage() {
  const [members, setMembers] = useState([])

  useEffect(() => {
    loadData()

    axios
      .get(Uris.leaderboardLast6ScavengersPageRefreshSeconds)
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
        const repeated = Array.from({ length: 40 }, () => members).flat();
        setMembers(repeated)
      })
  }

  function sectionBackgroundClassName(sectionName) {
    return `member-info-inner background-${sectionName.toLowerCase()}`
  }

  function RenderMember(member) {
    return (
      <div className="card members-list-item-card flex-shrink-0">
        <div className="card-body">
          <div className="row">
            <div className="col">
              <img key={Date.now()}
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
            <div className={"col-5 members-list-item-section"}>
              <div>
                {member.memberCode}
              </div>
            </div>
            <div className={"col-5 members-list-item-section"}>
              <div>
                { member.totalPoints }
              </div>
            </div>
          </div>
          <div className="row">
            <div className={"col members-list-item-section"}>
              <div>{member.troopName}</div>
            </div>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div className="d-flex flex-wrap">
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
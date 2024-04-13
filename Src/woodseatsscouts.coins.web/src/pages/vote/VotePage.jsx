import React, {useEffect, useState} from "react";
import {logReactUseEffect} from "../../components/logging/Logger";
import AudioFx from "../../fx/AudioFx";

import {Col, Row} from "reactstrap";
import ScanMemberForVoteSection from "./sections/ScanMemberForVoteSection";
import SectionNamesForVote from "./sections/SectionNamesForVote";
import VoteSummarySection from "./sections/VoteSummarySection";
import VoteSection from "./sections/VoteSection";

export default function VotePage() {
  const [sectionName, setSectionName] = useState(SectionNamesForVote.ScanMember)
  const [section, setSection] = useState(null);
  const [member, setMember] = useState(null)
  const [voteResult, setVoteResult] = useState(null)

  useEffect(() => {
    logReactUseEffect("member", member)
    if (member !== null) {
      setSectionName(SectionNamesForVote.Vote)
    }
  }, [member]);

  useEffect(() => {
    if (voteResult != null) {
      setSectionName(SectionNamesForVote.VoteSummary)
      new AudioFx().playHaulCompleteAudio();
    }
  }, [voteResult]);

  useEffect(() => {
    logReactUseEffect("sectionName", sectionName)

    function getSection() {
      switch (sectionName) {
        case SectionNamesForVote.ScanMember: {
          return (<ScanMemberForVoteSection setMember={setMember}/>)
        }
        case SectionNamesForVote.Vote: {
          return (<VoteSection member={member} setVoteResult={setVoteResult}/>)
        }
        case SectionNamesForVote.VoteSummary: {
          return (<VoteSummarySection member={member} voteResult={voteResult}/>)
        }
        default: {
          throw `Handler not defined ${sectionName}`
        }
      }
    }

    setSection(getSection());
  }, [sectionName]);

  return (
    <Row>
      <Col className="mb-2">
        <Row>
          <Col>
            <section>
              {section}
            </section>
          </Col>
        </Row>
      </Col>
    </Row>
  )
}
import {Col, Row} from "reactstrap";
import React, {useContext, useEffect, useState} from "react";
import AudioFx from "../../fx/AudioFx";
import {logReactUseEffect} from "../../components/logging/Logger";
import ScanMemberForCoinsSection from "./sections/ScanMemberForCoinsSection";
import SectionNames from "./sections/SectionNames";
import ScanCoinsSection from "./sections/scancoins/ScanCoinsSection";
import HaulResultsSection from "./sections/HaulSummarySection";
import {ShowStartCoinsAgainButtonContext} from "../../contexts/AppContext";

// Todo: rename paths and functions to either homepage or coinpage, not both
export function CoinsPage() {
  const [showStartCoinsAgainButton, setShowStartCoinsAgainButton] = useContext(ShowStartCoinsAgainButtonContext)

  const [sectionName, setSectionName] = useState(SectionNames.ScanMember)
  const [section, setSection] = useState(null);
  const [member, setMember] = useState(null)
  const [haulResult, setHaulResult] = useState(null)

  useEffect(() => {
    setShowStartCoinsAgainButton(false)
  }, []);

  useEffect(() => {
    logReactUseEffect("member", member)
    if (member !== null) {
      setShowStartCoinsAgainButton(true)
      setSectionName(SectionNames.ScanCoins)
    }
  }, [member]);

  useEffect(() => {
    if (haulResult != null) {
      setShowStartCoinsAgainButton(false)
      setSectionName(SectionNames.HaulSummary)
      new AudioFx().playHaulCompleteAudio();
    }
  }, [haulResult]);

  useEffect(() => {
    logReactUseEffect("sectionName", sectionName)

    function getSection() {
      switch (sectionName) {
        case SectionNames.ScanMember: {
          return (<ScanMemberForCoinsSection setMember={setMember}/>)
        }
        case SectionNames.ScanCoins: {
          return (<ScanCoinsSection member={member} setHaulResult={setHaulResult}/>)
        }
        case SectionNames.HaulSummary: {
          return (<HaulResultsSection member={member} haulResult={haulResult}/>)
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

export default CoinsPage
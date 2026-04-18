import React, {useContext, useEffect, useState} from "react";
// import AudioFx from "../../fx/AudioFx";
// import {logReactUseEffect} from "../../components/logging/Logger";
// import ScanMemberForCoinsSection from "./sections/ScanMemberForCoinsSection";
import SectionNames from "./sections/SectionNames";
// import ScanCoinsSection from "./sections/scancoins/ScanCoinsSection.tsx";
// import HaulResultsSection from "./sections/HaulSummarySection";
import {PageActionMenuAreaContext} from "../../contexts/AppContextExporter.tsx";
import ScanMemberForCoinsSection from "./sections/ScanMemberForCoinsSection.tsx";
import ScanCoinsSection from "./sections/scancoins/ScanCoinsSection.tsx";
import HaulResultsSection from "./sections/HaulSummarySection.tsx";

// Todo: rename paths and functions to either homepage or coinpage, not both
export function CoinsPage() {
  const {pageActionMenuAreaAction, setPageActionMenuAreaAction} = useContext(PageActionMenuAreaContext)

  const [sectionName, setSectionName] = useState(SectionNames.ScanMember)
  const [section, setSection] = useState(null);
  const [member, setMember] = useState(null)
  const [haulResult, setHaulResult] = useState(null)

  // useEffect(() => {
  //   setShowStartCoinsAgainButton(false)
  // }, []);

  useEffect(() => {
    // logReactUseEffect("member", member)
    if (member !== null) {
      // setShowStartCoinsAgainButton(true)
      setSectionName(SectionNames.ScanCoins)
    }
  }, [member]);

  useEffect(() => {
    if (haulResult != null) {
      // setShowStartCoinsAgainButton(false)
      setSectionName(SectionNames.HaulSummary)
      // new AudioFx().playHaulCompleteAudio(); // todo
    }
  }, [haulResult]);

  useEffect(() => {
    // logReactUseEffect("sectionName", sectionName)

    function getSection() {
      switch (sectionName) {
        case SectionNames.ScanMember: {
          setPageActionMenuAreaAction(SectionNames.ScanMember)
          return (<ScanMemberForCoinsSection setMember={setMember}/>)
        }
        case SectionNames.ScanCoins: {
          setPageActionMenuAreaAction(SectionNames.ScanCoins)
          return (<ScanCoinsSection member={member} setHaulResult={setHaulResult}/>)
        }
        case SectionNames.HaulSummary: {
          setPageActionMenuAreaAction(SectionNames.HaulSummary)
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
    <div className={"row"}>
      <div className="col mb-2">
        <div>
          <div className={"col"}>
            <section>
              {section}
            </section>
          </div>
        </div>
      </div>
    </div>
  )
}

export default CoinsPage
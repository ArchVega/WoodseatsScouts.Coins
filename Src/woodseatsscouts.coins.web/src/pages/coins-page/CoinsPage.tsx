import React, {useContext, useEffect, useState} from "react";
import CoinsPageViewName from "./CoinsPageViewName.ts";
import {PageActionMenuAreaContext} from "../../contexts/AppContextExporter.tsx";
import ScanMemberForCoinsSection from "./views/scan-members-view/ScanMemberForCoinsSection.tsx";
import ScanCoinsSection from "./views/scan-coins-view/ScanCoinsSection.tsx";
import HaulResultsSection from "./views/haul-summary-view/HaulSummarySection.tsx";
import AudioFx from "../../components/fx/AudioFx.ts";

export default function CoinsPage() {
  const {setPageActionMenuAreaAction} = useContext(PageActionMenuAreaContext)
  const [sectionName, setSectionName] = useState(CoinsPageViewName.ScanMember)
  const [section, setSection] = useState(null);
  const [member, setMember] = useState(null)
  const [haulResult, setHaulResult] = useState(null)

  useEffect(() => {
    if (member !== null) {
      setSectionName(CoinsPageViewName.ScanCoins)
    }
  }, [member]);

  useEffect(() => {
    if (haulResult != null) {
      setSectionName(CoinsPageViewName.HaulSummary)
      AudioFx().playHaulCompleteAudio();
    }
  }, [haulResult]);

  useEffect(() => {
    function getSection() {
      switch (sectionName) {
        case CoinsPageViewName.ScanMember: {
          setPageActionMenuAreaAction(CoinsPageViewName.ScanMember)
          return (<ScanMemberForCoinsSection setMember={setMember}/>)
        }
        case CoinsPageViewName.ScanCoins: {
          setPageActionMenuAreaAction(CoinsPageViewName.ScanCoins)
          return (<ScanCoinsSection member={member} setHaulResult={setHaulResult}/>)
        }
        case CoinsPageViewName.HaulSummary: {
          setPageActionMenuAreaAction(CoinsPageViewName.HaulSummary)
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
    <div className="row">
      <div className="col mb-2">
        <section>
          {section}
        </section>
      </div>
    </div>
  )
}
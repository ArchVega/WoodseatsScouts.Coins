import React, {useContext, useEffect, useState} from "react";
import type {ScoutMemberDto} from "@/types/ServerTypes.ts";
import {PageActionMenuAreaContext} from "@/contexts/AppContextExporter.tsx";
import CoinsPageViewName from "@/pages/coins-page/CoinsPageViewName.ts";
import AudioFx from "@/components/fx/AudioFx.ts";
import ScanMemberForCoinsSection from "@/pages/coins-page/views/scan-members-view/ScanMemberForCoinsSection.tsx";
import ScanCoinsSection from "@/pages/coins-page/views/scan-coins-view/ScanCoinsSection.tsx";
import HaulResultsSection from "@/pages/coins-page/views/haul-summary-view/HaulSummarySection.tsx";

export default function CoinsPage() {
  const {setPageActionMenuAreaAction} = useContext(PageActionMenuAreaContext)
  const [scoutSectionName, setScoutSectionName] = useState(CoinsPageViewName.ScanMember)
  const [section, setSection] = useState(null);
  const [member, setMember] = useState<ScoutMemberDto | null>(null)
  const [haulResult, setHaulResult] = useState(null)

  useEffect(() => {
    if (member !== null) {
      setScoutSectionName(CoinsPageViewName.ScanCoins)
    }
  }, [member]);

  useEffect(() => {
    if (haulResult != null) {
      setScoutSectionName(CoinsPageViewName.HaulSummary)
      AudioFx().playHaulCompleteAudio();
    }
  }, [haulResult]);

  useEffect(() => {
    function getSection() {
      switch (scoutSectionName) {
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
          throw `Handler not defined ${scoutSectionName}`
        }
      }
    }

    setSection(getSection());
  }, [scoutSectionName]);

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
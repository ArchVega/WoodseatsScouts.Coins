import "./HaulSummarySection.scss"
import {useContext, useEffect} from "react";
import {AppSettingsContext} from "../../../../contexts/AppContextExporter.tsx";
import {logInfo} from "../../../../components/logging/Logger.ts";
import getAppSettings from "../../../../AppSettings.ts";

export default function HaulResultsSection({member, haulResult}) {
  const {appSettings} = useContext(AppSettingsContext)

  const phrases = [
    "Awesome job, ",
    "Nice one, ",
    "Great work, ",
    "You did it, ",
    "Fantastic work, ",
    "Very impressive, "
  ]

  const congratsPhrase = phrases[Math.floor(Math.random() * phrases.length)];

  function reloadPage() {
    // Todo: get rid of location.reload mechanism
    window.location.reload()
  }

  setTimeout(() => {
    reloadPage()
  }, appSettings.VITE_SCAN_COINS_REDIRECT_DELAY_SECONDS * 1000)

  return (
    <>
      <div className="row mt-5">
        <div className="col text-center">
          <h1>{congratsPhrase}<span className="font-black">{member.firstName}</span><span
            style={{fontSize: '1em'}}>👍</span>
          </h1>
        </div>
      </div>
      <div className="row mb-3">
        <div className="col text-center">
          <h4>You've just added some more points to your score.</h4>
        </div>
      </div>
      <div className="row mb-3">
        <div className="col text-center">
          <button id="haul-summary-tally-total-box" data-testid="button-finish-scanning" className="btn btn-success btn-lg">
            <div style={{lineHeight: "normal"}}>
              <strong className="finish-scanning-button-points-value" data-testid="coin-total">
                {haulResult.coinTotal}
              </strong>
              &nbsp;
              <span className="text-white" style={{fontSize: "2em"}}>points</span></div>
          </button>
        </div>
      </div>
      {haulResult.additionalData !== undefined && haulResult.additionalData.hasAnomalyOccurred
        ? (
          <div className="row">
            <div className="col-6 offset-6 text-center alert alert-danger" data-testid="div-additional-message" role="alert">
              <p>Unfortunately, there was an issue with at least one of your coins.</p>
              <ul className="list-group list-group-flush">
                {haulResult.additionalData.affectedCoins.map(affectedCoin => (
                  <li className="list-group-item bg-danger text-white">
                    {affectedCoin.coinCode} scanned by {affectedCoin.memberName}
                  </li>
                ))}
              </ul>
            </div>
          </div>)
        : (<></>)}
      <div className="row mb-4">
        <div className="col text-center">
          <h4>Head back out there and get some more points...</h4>
        </div>
      </div>
      <div className="row">
        <div className="col text-center">
          <button className="btn-warning btn-next-member" onClick={reloadPage}>
            <span className="font-bold-italic">Next member</span>
          </button>
        </div>
      </div>
    </>
  )
}
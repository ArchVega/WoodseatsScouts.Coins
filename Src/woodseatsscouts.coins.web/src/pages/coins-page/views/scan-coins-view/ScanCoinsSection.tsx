import {toast} from "react-toastify";
import {useEffect, useState} from "react";
import ScannedCoin from "./ScannedCoin.tsx";
import QRCodeInputDevices from "../../../../components/io/qr-input-devices/QRCodeInputDevices";
import AudioFx from "../../../../components/fx/AudioFx";
import CoinApiService from "../../../../services/CoinApiService.tsx";
import QRScanCodeType from "../../../../components/io/qr-input-devices/QRScanCodeType.ts";
import {logDebug, logError, logReactSet} from "../../../../components/logging/Logger.ts";
import {toastError} from "../../../../components/toaster/toaster.ts";
import './ScanCoinsSection.scss'

export default function ScanCoinsSection({member, setHaulResult}) {
  const audioFx = AudioFx();
  const [coinQrCode, setCoinQrCode] = useState("");
  const [coins, setCoins] = useState([]);
  const [coinTotal, setCoinTotal] = useState(0);

  useEffect(() => {
    console.log('Coin qr code:', coinQrCode)
    if (coinQrCode != null && coinQrCode.trim().length > 0) {
      logDebug(`Fetching coin data for code ${coinQrCode}`)

      async function fetchData() {
        return await CoinApiService().fetchCoin(coinQrCode, member.memberCode)
      }

      function isDuplicateCoin(coin) {
        return coins.filter(x => x.code === coin.code).length > 0
      }

      fetchData()
        .then(async value => {
          const coin = (await value.data)
          logReactSet("Set Coins", coin)

          if (!isDuplicateCoin((coin))) {
            setCoins([...coins, coin])
            audioFx.playCoinScannedSuccessAudio()
            console.log('Resetting coin QR code to null. Was: ', coinQrCode)
            setCoinQrCode(null)
          } else {
            audioFx.playCoinScannedErrorAudio()
            toastError(`That coin has already been scanned for ${member.firstName}`)
          }
        })
        .catch(async axiosReason => {
          audioFx.playCoinScannedErrorAudio()
          logError("Set Coins", axiosReason)
          toastError(axiosReason)
        })
    }
  }, [coinQrCode]);

  useEffect(() => {
    setCoinTotal(coins.reduce((n, {pointValue}) => n + pointValue, 0))
  }, [coins]);

  function removeCoin(coin) {
    setCoins((current) => current.filter((c) => c !== coin));
  }

  async function onFinished() {
    if (coins.length === 0) {
      toast("Add at least one coin for this member", {
        position: 'top-center'
      })
      return
    }

    await CoinApiService().addPointsToMember(member, coins).then(async (response: any) => {
      const additionalData = await response.data

      let finalTotal = coinTotal;
      if (additionalData.hasAnomalyOccurred) {
        finalTotal = finalTotal - additionalData.anomalousCoinsTotalValue
      }

      setHaulResult({
        coinTotal: finalTotal,
        additionalData: additionalData
      })
    })
  }

  function focusScanner() {
    const textbox = document.getElementById('usb-scanner-code-textbox') as HTMLInputElement;
    if (textbox) {
      textbox.value = '';
      textbox.focus();
    }
  }

  function RenderTrackerCard(header: string, value: number) {
    return (
      <div className="tracker-card">
        <div>{header}</div>
        <div className="finish-scanning-button-points-value" data-testid="coin-total">{value}</div>
      </div>
    )
  }

  function RenderLeftPanel() {
    return (
      <div className="coins-grid-2026 mt-3" data-testid="div-scanned-coins">
        {coins.map((coin, index) =>
          (
            <div>
              <ScannedCoin key={index} coin={coin} isLast={index + 1 === coins.length} removeCoin={coinToRemove => removeCoin(coinToRemove)}/>
            </div>
          ))}
      </div>
    )
  }

  function RenderRightPanel() {
    return (
      <>
        <div className="row mb-1">
          <div className="col">
            <div className="mt-3 mb-2 text-black fs-3 fw-semibold">Now scan your points tokens...</div>
          </div>
        </div>
        <div className="row mb-3">
          <div className="col">
            <QRCodeInputDevices
              qrCode={coinQrCode}
              setQrCode={setCoinQrCode}
              qrScanCodeType={QRScanCodeType.Coin}
              textboxWidth={"100%"}
              textboxHeight={"60px"}
              webcamWidth={"240px"}
              webcamHeight={"240px"}
            />
          </div>
        </div>
        <div className="row mb-2 g-2">
          <div className="col">
            {RenderTrackerCard("Tokens scanned", coins.length)}
          </div>
          <div className="col">
            {RenderTrackerCard("Points this scan", coinTotal)}
          </div>
        </div>
        <div className="row" style={{position: "relative"}}>
          <div className="col float-end text-end ">
            <div className="d-grid">
              <button id="finish-scanning-button" data-testid="button-finish-scanning" onClick={onFinished} className="btn btn-success btn-lg">
                <div>
                  <div style={{fontSize: "3em"}}>🎉</div>
                  <em className="text-white">SAVE POINTS</em>
                </div>
              </button>
            </div>
          </div>
        </div>
      </>
    )
  }

  return (
    <div className="row">
      <div className="col-8">
        {RenderLeftPanel()}
      </div>
      <div className="col-4">
        {RenderRightPanel()}
      </div>
    </div>
  )
}
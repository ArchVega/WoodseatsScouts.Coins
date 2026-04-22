import {toast} from "react-toastify";
import React, {useEffect, useState} from "react";
import ScannedCoin from "./ScannedCoin.tsx";
import QRCodeInputDevices from "../../../../components/io/qr-input-devices/QRCodeInputDevices";
import AudioFx from "../../../../components/fx/AudioFx";
import CoinApiService from "../../../../services/apis/CoinApiService.tsx";
import QRScanCodeType from "../../../../components/io/qr-input-devices/QRScanCodeType.ts";
import {logDebug, logError, logObject, logReactSet} from "../../../../components/logging/Logger.ts";
import {toastError} from "../../../../components/toaster/toaster.ts";
import './ScanCoinsSection.scss'
import type {AxiosResponse} from "axios";
import type {CoinDto, Member, MemberDto} from "../../../../types/ServerTypes.ts";
import type {HaulResult} from "../../../../types/ClientTypes.ts";

interface ScanCoinsSectionProps {
  member: MemberDto;
  setHaulResult: React.Dispatch<React.SetStateAction<HaulResult>>
}

export default function ScanCoinsSection({member, setHaulResult}: ScanCoinsSectionProps) {
  const audioFx = AudioFx();
  const [coinQrCode, setCoinQrCode] = useState<string>("");
  const [coinDtos, setCoinDtos] = useState<CoinDto[]>([]);
  const [coinTotal, setCoinTotal] = useState<number>(0);

  useEffect(() => {
    if (coinQrCode != null && coinQrCode.trim().length > 0) {
      logDebug(`Fetching coin data for code ${coinQrCode}`)

      async function fetchData(): Promise<AxiosResponse<CoinDto>> {
        return await CoinApiService().fetchCoin(coinQrCode, member.code)
      }

      function isDuplicateCoin(coin) {
        return coinDtos.filter(x => x.code === coin.code).length > 0
      }

      fetchData()
        .then(async coinDtoResponse => {
          const coinDto = coinDtoResponse.data
          logObject("coinDto", coinDto)

          if (!isDuplicateCoin((coinDto))) {
            setCoinDtos([...coinDtos, coinDto])
            audioFx.playCoinScannedSuccessAudio()
            logDebug('Resetting coin QR code to null. Was: ', coinQrCode)
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
    setCoinTotal(coinDtos.reduce((n, {pointValue}) => n + pointValue, 0))
  }, [coinDtos]);

  function removeCoin(coin: CoinDto) {
    setCoinDtos((current) => current.filter((c) => c !== coin));
  }

  async function onFinished() {
    if (coinDtos.length === 0) {
      toast("Add at least one coin for this member", {position: 'top-center'})
      return
    }

    await CoinApiService().addPointsToMember(member, coinDtos).then(async (response: any) => {
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
      <div className="coins-grid mt-3" data-testid="div-scanned-coins">
        {coinDtos.map((coin, index) =>
          (
            <div key={index}>
              <ScannedCoin coin={coin} isLast={index + 1 === coinDtos.length} removeCoin={coinToRemove => removeCoin(coinToRemove)}/>
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
            <div className="mt-3 mb-2 text-black fw-semibold text-center" style={{fontSize: "5em"}}>Now scan your points tokens...</div>
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
        <div className="row mb-2">
          <div className="col">
            {RenderTrackerCard("Tokens scanned", coinDtos.length)}
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
                  <div style={{fontSize: "6em"}}>🎉</div>
                  <em className="text-white" style={{fontSize: "4em"}}>SAVE POINTS</em>
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
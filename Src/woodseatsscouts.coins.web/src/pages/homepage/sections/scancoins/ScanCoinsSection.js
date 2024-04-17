import {Button, Col, Row} from "reactstrap";
import wave from "../../../../images/wave.png";
import {toast} from "react-toastify";
import {useContext, useEffect, useState} from "react";
import ScannedCoin from "./ScannedCoin";
import QRCodeInputDevices from "../../../../components/qrinputdevices/QRCodeInputDevices";
import {logDebug, logError, logReactSet} from "../../../../components/logging/Logger";
import CoinApiService from "../../../../services/CoinApiService";
import AudioFx from "../../../../fx/AudioFx";
import QRScanCodeType from "../../../../components/qrscanners/QRScanCodeType";
import {toastError} from "../../../../components/toaster/toaster";
import Uris from "../../../../services/Uris";
import {UseAppCameraContext} from "../../../../contexts/AppContext";

function ScanCoinsSection({member, setHaulResult}) {
  const audioFx = AudioFx();
  const [coinQrCode, setCoinQrCode] = useState("");
  const [coins, setCoins] = useState([]);
  const [coinTotal, setCoinTotal] = useState(0);
  const [useAppCamera] = useContext(UseAppCameraContext)

  useEffect(() => {
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
      toast("Add at least one coin for this member")
      return
    }

    await CoinApiService().addPointsToMember(member, coins).then(async response => {
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
    const textbox = document.getElementById('usb-scanner-code-textbox');
    if (textbox) {
      textbox.focus();
    }
  }

  return (
    <>
      <Row className="mb-5" style={{minHeight: '45vh', maxHeight: '50vh'}}>
        <Col className="col-6">
          <h1 id="scan-coins-section-h1">
            <span>Welcome back,</span>
            <br/>
            <span className="font-black" data-testid="h1-user-firstname">
                <img className="coins-member-img"
                     alt=""
                     style={{marginTop: "-20px"}}
                     src={member && member.hasImage
                       ? Uris.memberPhoto(member.memberId)
                       : "/images/unknown-member-image.png"}/>
            <span id="member-name">{member.firstName}</span>
            <img id="scan-coins-section-wave-image" src={wave} alt="" style={{marginTop: "-20px"}}/>
            </span>
          </h1>

          <h4 className="mt-5 text-black fs-3 fw-semibold">Start scanning your points cards...</h4>
          <QRCodeInputDevices qrCode={coinQrCode} setQrCode={setCoinQrCode} qrScanCodeType={QRScanCodeType.Coin}/>

          <div className="mb-5"/>

          {!useAppCamera
            ? <Button className="btn focus-scanner-textbox-button mt-5" onClick={focusScanner}>
              <div>Click here if your points do</div>
              <div>not scan, then try again</div>
            </Button>
            : null}
        </Col>
        <Col className="col-6">
          <Row>
            <Col>
              <div id="scanned-coins-div" data-testid="div-scanned-coins" className="text-end mb-4"
                   style={{overflow: 'auto', maxHeight: '50vh'}}>
                {coins.map((coin, index) =>
                  <ScannedCoin key={index}
                               coin={coin}
                               isLast={index + 1 === coins.length}
                               removeCoin={coinToRemove => removeCoin(coinToRemove)}/>
                )}
              </div>
            </Col>
          </Row>
          <Row style={{position: "relative"}}>
            <Col className="float-end text-end">
              <Button id="finish-scanning-button" data-testid="button-finish-scanning" onClick={onFinished} className="btn btn-success btn-lg">
                <div>
                  <img src="/images/plus-circle-fill.svg"></img><em className="text-white">Click to add</em>
                </div>
                <div style={{lineHeight: "normal"}}>
                  <strong className="finish-scanning-button-points-value" data-testid="coin-total">
                    {coinTotal}
                  </strong>
                  &nbsp;
                  <span className="text-white">points...</span></div>
              </Button>
            </Col>
          </Row>
        </Col>
      </Row>
    </>
  )
}

export default ScanCoinsSection
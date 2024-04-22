import React, {useContext, useEffect, useState} from "react";
import wave from "../../../images/wave.png";
import QRCodeInputDevices from "../../../components/qrinputdevices/QRCodeInputDevices";
import {Button, Col, Row} from "reactstrap";
import QRScanCodeType from "../../../components/qrscanners/QRScanCodeType";
import SiteSpinner from "../../../components/spinner/SiteSpinner";
import {logDebug, logError, logReactSet} from "../../../components/logging/Logger";
import AudioFx from "../../../fx/AudioFx";
import MemberApiService from "../../../services/MemberApiService";
import {toastError} from "../../../components/toaster/toaster";
import {UseAppCameraContext} from "../../../contexts/AppContext";

function ScanMemberForCoinsSection({setMember}) {
  const audioFx = AudioFx();
  const [loading, setLoading] = useState(false)
  const [memberQrCode, setMemberQrCode] = useState("")
  const [useAppCamera] = useContext(UseAppCameraContext)

  let timeoutId = null

  useEffect(() => {
    const element = document.getElementById('usb-scanner-code-textbox')
    if (element) {
      element.onblur = () => {
        if (timeoutId) {
          console.log('clearing timeout', timeoutId)
          clearTimeout(timeoutId)
        }
        timeoutId = setTimeout(() => focusScanner(), 1000 * 10)
      };
    }
  }, [])

  useEffect(() => {
    if (memberQrCode != null && memberQrCode.trim().length > 0) {
      logDebug(`Fetching member data for code ${memberQrCode}`)

      setLoading(true)

      async function fetchData() {
        audioFx.playMemberScannedAudio()
        return await MemberApiService().fetchMember(memberQrCode)
      }

      fetchData()
        .then(async value => {
          const data = (await value.data)
          logReactSet("Setting member", data)
          setMember(data);
        })
        .catch(async axiosReason => {
          logError("Setting member", axiosReason)
          toastError(axiosReason)
          setMember(null)
        })
        .finally(() => {
          setLoading(false)
        })
    }

  }, [memberQrCode]);

  function focusScanner() {
    const textbox = document.getElementById('usb-scanner-code-textbox');
    if (textbox) {
      textbox.value = '';
      textbox.focus();
    }
  }

  return (
    <>
      <Row id="scan-member-section">
        <Col>
          <img id="scan-member-section-wave-image" src={wave} alt=""/>
          <span className="main-page-hello font-black">Hello</span>
          <h1 className="mb-5">
            <strong>
              Scan your wristband to start...
            </strong>
          </h1>

          {loading
            ? <SiteSpinner/>
            : (
              <Row>
                <Col xs={{size: 8, offset: 2}}>
                  <QRCodeInputDevices qrCode={memberQrCode} setQrCode={setMemberQrCode} qrScanCodeType={QRScanCodeType.Member}/>
                  <div className="mb-3"/>
                  {!useAppCamera
                    ? <Button className="btn focus-scanner-textbox-button mt-5" onClick={focusScanner}>
                      <div>Click here if your points do</div>
                      <div>not scan, then try again</div>
                    </Button>
                    : null}
                </Col>
              </Row>
            )
          }
        </Col>
      </Row>
    </>
  )
}

export default ScanMemberForCoinsSection
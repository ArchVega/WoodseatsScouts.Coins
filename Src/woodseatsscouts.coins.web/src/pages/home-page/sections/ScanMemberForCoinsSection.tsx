import React, {useContext, useEffect, useState} from "react";
import wave from "../../../images/wave.png";
import "./ScanMemberForCoinsSection.scss"
import MemberApiService from "../../../services/MemberApiService";
import {AppSettingsContext, PageActionMenuAreaContext, UseAppCameraContext} from "../../../contexts/AppContextExporter.tsx";
import AudioFx from "../../../components/fx/AudioFx.ts";
import Spinner from "../../../components/widgets/Spinner.tsx";
import QRCodeInputDevices from "../../../components/io/qr-input-devices/QRCodeInputDevices.tsx";
import QRScanCodeType from "../../../components/io/qr-input-devices/qr-scanners/QRScanCodeType.ts";

export default function ScanMemberForCoinsSection({setMember}) {
  const {pageActionMenuAreaAction, setPageActionMenuAreaAction, activeScanningMember, setActiveScanningMember} = useContext(PageActionMenuAreaContext)
  const {useAppCamera} = useContext(UseAppCameraContext)

  const audioFx = AudioFx();
  const [loading, setLoading] = useState(false)
  const [memberQrCode, setMemberQrCode] = useState("")

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
      // logDebug(`Fetching member data for code ${memberQrCode}`)

      setLoading(true)

      async function fetchData() {
        audioFx.playMemberScannedAudio()
        return await MemberApiService().fetchMember(memberQrCode)
      }

      fetchData()
        .then(async value => {
          const data = (await value.data)
          // logReactSet("Setting member", data)
          setMember(data);
          setActiveScanningMember(data)
        })
        .catch(async axiosReason => {
          // logError("Setting member", axiosReason)
          // toastError(axiosReason)
          setMember(null)
          setActiveScanningMember(null)
        })
        .finally(() => {
          setLoading(false)
        })
    }

  }, [memberQrCode]);

  function focusScanner() {
    const textbox = document.getElementById('usb-scanner-code-textbox') as HTMLInputElement;
    if (textbox) {
      textbox.value = '';
      textbox.focus();
    }
  }

  return (
    <div className="row text-center page">
      <div className="col">
        <div className="mt-3">
          <span className="page-hello-text">Hello</span>
          <img id="scan-member-section-wave-image" src={wave} alt=""/>
          <h1 className="mb-5">
            <strong>
              Scan your wristband with the scanner to start saving points…
            </strong>
          </h1>
        </div>

        {loading
          ? <Spinner/>
          : (
            <div className="row">
              <div className="col-8 offset-sm-2">
                <QRCodeInputDevices qrCode={memberQrCode} setQrCode={setMemberQrCode} qrScanCodeType={QRScanCodeType.Member}/>
                <div className="mb-3"/>
              </div>
            </div>
          )
        }
      </div>
    </div>
  )
}
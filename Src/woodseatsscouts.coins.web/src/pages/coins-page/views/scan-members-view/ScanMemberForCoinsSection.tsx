import React, {useContext, useEffect, useState} from "react";
import wave from "../../../../images/wave.png";
import "./ScanMemberForCoinsSection.scss"
import MemberApiService from "../../../../services/apis/MemberApiService.ts";
import {PageActionMenuAreaContext, UseAppCameraContext} from "../../../../contexts/AppContextExporter.tsx";
import AudioFx from "../../../../components/fx/AudioFx.ts";
import Spinner from "../../../../components/widgets/Spinner.tsx";
import QRCodeInputDevices from "../../../../components/io/qr-input-devices/QRCodeInputDevices.tsx";
import QRScanCodeType from "../../../../components/io/qr-input-devices/QRScanCodeType.ts";
import {logDebug, logError, logObject} from "../../../../components/logging/Logger.ts";
import {toastError} from "../../../../components/toaster/toaster.ts";
import type {AxiosResponse} from "axios";
import type {Member, MemberDto} from "../../../../types/ServerTypes.ts";
import Uris from "../../../../services/apis/Uris.ts";

export default function ScanMemberForCoinsSection({setMember}) {
  const {setActiveScanningMember} = useContext(PageActionMenuAreaContext)
  const audioFx = AudioFx(); // todo: move into Context
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
      setLoading(true)

      async function fetchData(): Promise<AxiosResponse<MemberDto>> {
        audioFx.playMemberScannedAudio()
        return await MemberApiService().fetchMember(memberQrCode)
      }

      fetchData()
        .then(async member => {
          member.data.clientComputedImageUri = Uris.memberPhoto(member.data.computedImagePath) // todo: is there an axios way to do this automatically?
          logObject("memberDto", member.data)
          setMember(member.data);
          setActiveScanningMember(member.data)
        })
        .catch(async axiosReason => {
          logError("Setting member", axiosReason)
          toastError(axiosReason)
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
    <div className="row text-center page mt-5">
      <div className="col">
        <div className="mt-5">
          <span className="page-hello-text">Hello</span>
          <img id="scan-member-section-wave-image" src={wave} alt=""/>
          <div className="mb-5" style={{fontSize: "2.5em"}}>
            <strong>
              Scan your wristband with the scanner to start saving points…
            </strong>
          </div>
        </div>

        {loading
          ? <Spinner/>
          : (
            <div className="row">
              <div className="col-8 offset-sm-2">
                <QRCodeInputDevices
                  qrCode={memberQrCode}
                  setQrCode={setMemberQrCode}
                  qrScanCodeType={QRScanCodeType.Member}
                  textboxWidth={"100%"}
                  textboxHeight={"70px"}
                  webcamWidth={"240px"}
                  webcamHeight={"240px"}
                />
              </div>
            </div>
          )
        }
      </div>
    </div>
  )
}
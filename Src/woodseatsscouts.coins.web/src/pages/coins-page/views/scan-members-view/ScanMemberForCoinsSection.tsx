import React, {useContext, useEffect, useState} from "react";
import wave from "../../../../images/wave.png";
import "./ScanMemberForCoinsSection.scss"
import MemberApiService from "../../../../services/apis/MemberApiService.ts";
import {PageActionMenuAreaContext} from "../../../../contexts/AppContextExporter.tsx";
import AudioFx from "../../../../components/fx/AudioFx.ts";
import Spinner from "../../../../components/widgets/Spinner.tsx";
import QRCodeInputDevices from "../../../../components/io/qr-input-devices/QRCodeInputDevices.tsx";
import QRScanCodeType from "../../../../components/io/qr-input-devices/QRScanCodeType.ts";
import {logDebug, logError, logObject} from "../../../../components/logging/Logger.ts";
import {toastError} from "../../../../components/toaster/toaster.ts";
import type {AxiosResponse} from "axios";
import type {ScoutMemberDto} from "../../../../types/ServerTypes.ts";
import Uris from "../../../../services/apis/Uris.ts";

export default function ScanMemberForCoinsSection({setMember}) {
  const audioFx = AudioFx(); // todo: move into Context
  const {setActiveScanningMember} = useContext(PageActionMenuAreaContext)
  const [loading, setLoading] = useState<boolean>(false)
  const [memberQrCode, setMemberQrCode] = useState<string>("")

  let timeoutId = null

  useEffect(() => {
    const element = document.getElementById('usb-scanner-code-textbox')
    if (element) {
      element.onblur = () => {
        if (timeoutId) {
          logDebug('clearing timeout', timeoutId)
          clearTimeout(timeoutId)
        }
        timeoutId = setTimeout(() => focusScanner(), 1000 * 10)
      };
    }
  }, [])

  useEffect(() => {
    if (memberQrCode != null && memberQrCode.trim().length > 0) {
      setLoading(true)

      async function fetchData(): Promise<AxiosResponse<ScoutMemberDto>> {
        audioFx.playMemberScannedAudio()
        return await MemberApiService().fetchMember(memberQrCode)
      }

      fetchData()
        .then(value => {
          const member: ScoutMemberDto = value.data;
          logObject("memberDto", member)
          setMember(member);
          setActiveScanningMember(member)
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
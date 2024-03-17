import React, {useEffect, useState} from "react";
import wave from "../../../images/wave.png";
import QRCodeInputDevices from "../../../components/qrinputdevices/QRCodeInputDevices";
import {Col, Row} from "reactstrap";
import QRScanCodeType from "../../../components/qrscanners/QRScanCodeType";
import SiteSpinner from "../../../components/spinner/SiteSpinner";
import {logDebug, logError, logReactSet} from "../../../components/logging/Logger";
import AudioFx from "../../../fx/AudioFx";
import MemberApiService from "../../../services/MemberApiService";
import {toastError} from "../../../components/toaster/toaster";

function ScanMemberSection({setMember}) {
    const audioFx = AudioFx();
    const [loading, setLoading] = useState(false)
    const [memberQrCode, setMemberQrCode] = useState("")

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

    return (
        <>
            <Row id="scan-member-section">
                <img id="scan-member-section-wave-image" src={wave} alt=""/>
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
                            </Col>
                        </Row>
                    )
                }
            </Row>
        </>
    )
}

export default ScanMemberSection
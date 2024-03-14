import React from "react";
import wave from "../../../images/wave.png";
import QRCodeInputDevices from "../../../components/qrinputdevices/QRCodeInputDevices";
import {Row} from "reactstrap";
import QRScanCodeType from "../../../components/qrscanners/QRScanCodeType";

function ScanMemberSection({qrCode, setQrCode}) {
    return (
        <>
            <Row id="scan-member-section">
                <img id="scan-member-section-wave-image" src={wave} alt=""/>
                <h1 className="mb-5">
                    <strong>
                        Scan your wristband to start...
                    </strong>
                </h1>

                <QRCodeInputDevices qrCode={qrCode} setQrCode={setQrCode} qrScanCodeType={QRScanCodeType.Member}/>
            </Row>
        </>
    )
}

export default ScanMemberSection
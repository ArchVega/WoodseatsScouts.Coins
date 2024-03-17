import {UseAppCameraContext} from "../../contexts/AppContext";
import {QRWebcamScanner} from "../qrscanners/qrwebcamscanner/QRWebcamScanner";
import QRBarcodeScanner from "../qrscanners/qrbarcodescanner/QRBarcodeScanner";
import {useContext} from "react";
import {Col, Row} from "reactstrap";

function QRCodeInputDevices({qrCode, setQrCode, qrScanCodeType}) {
    const [useAppCamera] = useContext(UseAppCameraContext)

    return (
        <>
            <Row>
                <Col>
                    {useAppCamera
                        ? (<QRWebcamScanner qrCode={qrCode} setQrCode={setQrCode} qrScanCodeType={qrScanCodeType} videoSizeEm={20} type={"user"}/>)
                        : (<QRBarcodeScanner qrCode={qrCode} setQrCode={setQrCode} qrScanCodeType={qrScanCodeType}></QRBarcodeScanner>)}
                </Col>
            </Row>
        </>
    )
}

export default QRCodeInputDevices
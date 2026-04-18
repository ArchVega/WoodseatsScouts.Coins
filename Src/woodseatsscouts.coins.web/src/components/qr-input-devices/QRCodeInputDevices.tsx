import {useContext} from "react";
import {UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import {QRWebcamScanner} from "./qr-scanners/QRWebcamScanner.tsx";
import QRBarcodeScanner from "./qr-scanners/QRBarcodeScanner.tsx";

export default function QRCodeInputDevices({qrCode, setQrCode, qrScanCodeType}) {
  const {useAppCamera} = useContext(UseAppCameraContext)

  return (
    <div className="row">
      <div className="col">
        {useAppCamera
          ? (<QRWebcamScanner qrCode={qrCode} setQrCode={setQrCode} qrScanCodeType={qrScanCodeType} videoSizeEm={20} type={"user"}/>)
          : (<QRBarcodeScanner qrCode={qrCode} setQrCode={setQrCode} qrScanCodeType={qrScanCodeType}></QRBarcodeScanner>)}
      </div>
    </div>
  )
}
import React, {useContext} from "react";
import {UseAppCameraContext} from "../../../contexts/AppContextExporter.tsx";
import QRWebcamScanner from "./qr-scanners/QRWebcamScanner.tsx";
import QRBarcodeScanner from "./qr-scanners/QRBarcodeScanner.tsx";
import type {QRCodeInputDevicesProps} from "./QRCodeInputDevicesProps.tsx";

export default function QRCodeInputDevices(props: QRCodeInputDevicesProps) {
  const {useAppCamera} = useContext(UseAppCameraContext)

  return (
    <div className="row">
      <div className="col">
        {useAppCamera
          ? <QRWebcamScanner {...props} videoSizeEm={20} type={"user"}/>
          : <QRBarcodeScanner {...props}></QRBarcodeScanner>
        }
      </div>
    </div>
  )
}
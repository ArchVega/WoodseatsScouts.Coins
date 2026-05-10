import React, {useContext} from "react";
import {UseAppCameraContext} from "@/contexts/AppContextExporter.tsx";
import QRWebcamScanner from "@/components/io/qr-input-devices/qr-scanners/QRWebcamScanner.tsx";
import QRBarcodeScanner from "@/components/io/qr-input-devices/qr-scanners/QRBarcodeScanner.tsx";
import type {QRCodeInputDevicesProps} from "@/components/io/qr-input-devices/QRCodeInputDevicesProps.tsx";


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
import React from "react";

export interface QRCodeInputDevicesProps {
  qrCode: string,
  setQrCode: React.Dispatch<React.SetStateAction<string>>
  qrScanCodeType: string,
  textboxWidth: string,
  textboxHeight: string,
  webcamWidth: string,
  webcamHeight: string,
}
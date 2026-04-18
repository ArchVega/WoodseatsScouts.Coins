import React from "react";

export interface QRCodeInputDevicesProps {
  qrCode: string,
  setQrCode: React.Dispatch<React.SetStateAction<string>>
  qrScanCodeType: string,
  width: string,
  height: string,
}
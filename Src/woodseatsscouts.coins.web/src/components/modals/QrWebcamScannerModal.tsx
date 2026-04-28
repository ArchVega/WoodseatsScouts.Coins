import React, {useContext, useEffect, useState} from "react";
import {BaseModal} from "./BaseModal.tsx";
import {Scanner} from "@yudiel/react-qr-scanner";

interface QrWebcamScannerModalProps {
  showModal: boolean;
  setShowModal: React.Dispatch<React.SetStateAction<boolean>>
  setQrCode: React.Dispatch<React.SetStateAction<string>>
}

export default function QRWebcamScanner({showModal, setShowModal, setQrCode, ...props}: QrWebcamScannerModalProps) {
  const [currentQRCode, setCurrentQRCode] = useState("");
  const [timeoutHandle, setTimeoutHandle] = useState(0);

  useEffect(() => {
    if (currentQRCode) {
      setQrCode(currentQRCode);
      setShowModal(false)
    }
  }, [currentQRCode])

  const handleScan = (detectedCodes) => {
    if (detectedCodes && detectedCodes.length >= 1) {
      setCurrentQRCode(detectedCodes[0].rawValue)
    }
  }

  return (
    <BaseModal id={"qr-webcam-scanner-modal"} title={"Scan a member's badge"} show={showModal} onClose={() => {
      setShowModal(false)
    }}>
      <div className="qr-code-scanner-container" style={{width: 300, height: 300, margin: "0 auto"}}>
        <Scanner
          onScan={handleScan}
          onError={(error: any) => console.log(error?.message)}
          constraints={{
            facingMode: "environment",
            aspectRatio: 1,
          }}
        />
      </div>
    </BaseModal>
  );
}
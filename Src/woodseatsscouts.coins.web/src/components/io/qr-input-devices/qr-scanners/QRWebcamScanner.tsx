import React, {useEffect, useState} from "react";
import type {QRCodeInputDevicesProps} from "../QRCodeInputDevicesProps.tsx";
import {Scanner} from "@yudiel/react-qr-scanner";

interface QRWebcamScannerProps extends QRCodeInputDevicesProps {
  videoSizeEm: any
  type: string
}

export default function QRWebcamScanner({videoSizeEm, type, ...props}: QRWebcamScannerProps) {
  const [currentQRCode, setCurrentQRCode] = useState("");
  const [previousQRCode, setPreviousQRCode] = useState("");
  const [timeoutHandle, setTimeoutHandle] = useState(0);

  useEffect(() => {
    if (currentQRCode === "") {
      setCurrentQRCode(currentQRCode);
    } else {
      setPreviousQRCode(currentQRCode);
      props.setQrCode(currentQRCode)

      clearTimeout(timeoutHandle);
      let t = setTimeout(() => {
        setCurrentQRCode("")
      }, 5000)
      setTimeoutHandle(t);
    }

  }, [currentQRCode])

  const handleScan = (detectedCodes) => {
    if (detectedCodes && detectedCodes.length >= 1) {
      setCurrentQRCode(detectedCodes[0].rawValue)
    }
  }

  return (
    <div className="qr-code-scanner-container" style={{width: props.webcamWidth, height: props.webcamHeight, margin: "0 auto"}}>
      <Scanner
        onScan={handleScan}
        onError={(error: any) => console.log(error?.message)}
        constraints={{
          facingMode: "environment",
          aspectRatio: 1,
        }}
      />
      {/*<QrReader*/}
      {/*  constraints={{facingMode: "environment"}}*/}
      {/*  containerStyle={{height: (videoSizeEm + 1) + "em"}}*/}
      {/*  videoStyle={{height: videoSizeEm + "em"}}*/}
      {/*  videoContainerStyle={{paddingTop: videoSizeEm + "em"}}*/}
      {/*  onResult={(result, error) => {*/}
      {/*    if (!!result) {*/}
      {/*      setCurrentQRCode(result?.text)*/}
      {/*    }*/}

      {/*    if (!!error) {*/}
      {/*    }*/}
      {/*  }}*/}
      {/*  style={{width: '100%'}}*/}
      {/*/>*/}
    </div>
  );
}
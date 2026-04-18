import React, {useContext, useEffect, useState} from "react";
import {Scanner} from "@yudiel/react-qr-scanner";

export const QRWebcamScanner = ({videoSizeEm, qrCode, setQrCode, type, qrScanCodeType}) => {
  // this is the new stuff
  // <Scanner
  //   onScan={(result) => console.log(result)}
  //   onError={(error) => console.log(error?.message)}
  // />

    const [currentQRCode, setCurrentQRCode] = useState("");
    const [previousQRCode, setPreviousQRCode] = useState("");
    const [timeoutHandle, setTimeoutHandle] = useState(0);

    useEffect(() => {
        if (currentQRCode === "") {
            setCurrentQRCode(currentQRCode);
        } else {
            setPreviousQRCode(currentQRCode);
            setQrCode(currentQRCode)

            clearTimeout(timeoutHandle);
            let t = setTimeout(() => {
                setCurrentQRCode("")
            }, 5000)
            setTimeoutHandle(t);
        }

    }, [currentQRCode])

    return (
        <div className="qr-code-scanner-container">
            {/*<QrReader*/}
            {/*    constraints={{facingMode: "environment"}}*/}
            {/*    containerStyle={{height: (videoSizeEm + 1) + "em"}}*/}
            {/*    videoStyle={{height: videoSizeEm + "em"}}*/}
            {/*    videoContainerStyle={{paddingTop: videoSizeEm + "em"}}*/}
            {/*    onResult={(result, error) => {*/}
            {/*        if (!!result) {*/}
            {/*            setCurrentQRCode(result?.text)*/}
            {/*        }*/}

            {/*        if (!!error) {*/}
            {/*        }*/}
            {/*    }}*/}
            {/*    style={{width: '100%'}}*/}
            {/*/>*/}
        </div>
    );
};
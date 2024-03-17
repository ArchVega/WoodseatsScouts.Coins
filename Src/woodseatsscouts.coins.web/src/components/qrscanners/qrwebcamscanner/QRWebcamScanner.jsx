import {useContext, useEffect, useState} from "react";
import {QrReader} from "react-qr-reader";
import {AppCameraAvailableContext} from "../../../contexts/AppContext";

export const QRWebcamScanner = ({videoSizeEm, qrCode, setQrCode, type, qrScanCodeType={qrScanCodeType}}) => {
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
            <QrReader
                constraints={{facingMode: "environment"}}
                containerStyle={{height: (videoSizeEm + 1) + "em"}}
                videoStyle={{height: videoSizeEm + "em"}}
                videoContainerStyle={{paddingTop: videoSizeEm + "em"}}
                onResult={(result, error) => {
                    if (!!result) {
                        setCurrentQRCode(result?.text)
                    }

                    if (!!error) {
                    }
                }}
                style={{width: '100%'}}
            />
        </div>
    );
};
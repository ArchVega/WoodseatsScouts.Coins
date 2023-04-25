import {useContext, useEffect, useState} from "react";
import {QrReader} from "react-qr-reader";
import {AppCameraAvailableContext} from "../../App";

export const QRCodeScanner = ({videoSizeEm, qrCode, setQRCode, type}) => {
    const [currentQRCode, setCurrentQRCode] = useState("");
    const [previousQRCode, setPreviousQRCode] = useState("");
    const [timeoutHandle, setTimeoutHandle] = useState(0);
    const [appCameraAvailable, setAppCameraAvailable] = useContext(AppCameraAvailableContext)
    
    useEffect(() => {        
        if (currentQRCode === "") {
            
            setCurrentQRCode(currentQRCode);    
        } else {            
            setPreviousQRCode(currentQRCode);
            setQRCode(currentQRCode)
            
            if (type === "user") {
                new Audio("sounds/ScanWristband.mp3").play();                
            } else if (type === "coin") {
                if (appCameraAvailable) {
                    new Audio("sounds/ScanPointsCard.mp3").play();                    
                }
            } else {
                throw `"No handle for type '${type}'`
            }
            
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
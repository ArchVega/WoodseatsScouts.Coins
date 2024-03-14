import {Input} from "reactstrap";
import {useContext, useEffect, useState} from "react";
import TestQRBarcodeDataModal from "../../_dev/TestQRBarcodeDataModal";
import {AppModeContext, AppTestModeContext} from "../../../contexts/AppContext";
import QRScanCodeType from "../QRScanCodeType";

function QRBarcodeScanner({qrCode, setQrCode, qrScanCodeType = {qrScanCodeType}}) {
    const [appModeContext, setAppModeContext] = useContext(AppModeContext);
    const [usbScannerValue, setUsbScannerValue] = useState("");
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);
    const [testUsersModal, setTestUsersModal] = useState(false);

    // const [userQRCode, setUserQRCode] = useState("")

    function onMemberCodeTextBoxClicked() {
        if (appModeContext === "Development" && appTestMode) {
            setTestUsersModal(true);
        }
    }

    function setUserAndCloseModal(code) {
        setTestUsersModal(false);
        setQrCode(code)
        setUsbScannerValue(code)
    }

    const handleKeyDown = (event) => {
        if (event.key === 'Enter') {
            setQrCode(event.target.value)
            setUsbScannerValue("")
        }
    }

    return (
        <>
            <Input id="usb-scanner-code-textbox"
                   data-testid="textbox-usb-scanner-code"
                   autoComplete="off"
                   autoFocus={true}
                   value={usbScannerValue}
                   onChange={(e) => setUsbScannerValue(e.target.value)}
                   onClick={onMemberCodeTextBoxClicked}
                   onKeyDown={handleKeyDown}/>
            <TestQRBarcodeDataModal
                testUsersModal={testUsersModal}
                setTestUsersModal={setTestUsersModal}
                qrScanCodeType={qrScanCodeType}
                onSelected={(code) => setUserAndCloseModal(code)}>
            </TestQRBarcodeDataModal>
        </>
    )
}

export default QRBarcodeScanner
import {Input} from "reactstrap";
import {useContext, useState} from "react";
import TestQRBarcodeDataModal from "../../_dev/TestQRBarcodeDataModal";
import {AppModeContext, AppTestModeContext} from "../../../contexts/AppContext";
import {logAttention} from "../../logging/Logger";

function QRBarcodeScanner({qrCode, setQrCode, qrScanCodeType = {qrScanCodeType}}) {
    const [appModeContext, setAppModeContext] = useContext(AppModeContext);
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);
    const [testUsersModal, setTestUsersModal] = useState(false);

    function onMemberCodeTextBoxClicked() {
        if (appModeContext === "Development" && appTestMode) {
            setTestUsersModal(true);
        }
    }

    function setUserAndCloseModal(code) {
        logAttention('setUserAndCloseModal', code)
        setTestUsersModal(false);
        setQrCode(code)
    }

    const handleKeyDown = (event) => {
        if (event.key === 'Enter') {
            setQrCode(event.target.value)
            event.target.value = ""
        }
    }

    return (
        <>
            <Input id="usb-scanner-code-textbox"
                   data-testid="textbox-usb-scanner-code"
                   autoComplete="off"
                   autoFocus={true}
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
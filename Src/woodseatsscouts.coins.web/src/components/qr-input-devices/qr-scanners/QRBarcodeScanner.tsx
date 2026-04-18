import {useContext, useState} from "react";
// import TestQRBarcodeDataModal from "../../_dev/TestQRBarcodeDataModal";
import {AppModeContext, AppTestModeContext} from "../../../contexts/AppContextExporter.tsx";
import TestQRBarcodeDataModal from "../../_dev/TestQRBarcodeDataModal.tsx";

function QRBarcodeScanner({qrCode, setQrCode, qrScanCodeType}) {
    const {appMode, setAppMode} = useContext(AppModeContext);
    const {appTestMode, setAppTestMode} = useContext(AppTestModeContext);
    const [testUsersModal, setTestUsersModal] = useState(false);

    function onMemberCodeTextBoxClicked() {
        // todo
        // if (appMode === "Development" && appTestMode) {
        //     setTestUsersModal(true);
        // }
        setTestUsersModal(true);
    }

    function setUserAndCloseModal(code) {
        // logAttention('setUserAndCloseModal', code)
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
            <input id="usb-scanner-code-textbox"
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
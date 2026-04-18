import {useContext, useState} from "react";
import {AppModeContext, AppTestModeContext} from "../../../../contexts/AppContextExporter.tsx";
import TestQRBarcodeDataModal from "../../../_dev/TestQRBarcodeDataModal.tsx";
import type {QRCodeInputDevicesProps} from "../QRCodeInputDevicesProps.tsx";

export default function QRBarcodeScanner(props: QRCodeInputDevicesProps) {
  const {appMode} = useContext(AppModeContext);
  const {appTestMode} = useContext(AppTestModeContext);
  const [testUsersModal, setTestUsersModal] = useState(false);

  function onMemberCodeTextBoxClicked() {
    if (appMode === "Development" && appTestMode) {
      setTestUsersModal(true);
    }
  }

  function setUserAndCloseModal(code: string) {
    setTestUsersModal(false);
    props.setQrCode(code)
  }

  const handleKeyDown = (event: any) => {
    if (event.key === 'Enter') {
      props.setQrCode(event.target.value)
      event.target.value = ""
    }
  }

  return (
    <>
      <input id="usb-scanner-code-textbox"
             autoComplete="off"
             autoFocus={true}
             onClick={onMemberCodeTextBoxClicked}
             onKeyDown={handleKeyDown}
             data-testid="textbox-usb-scanner-code"/>
      <TestQRBarcodeDataModal
        testUsersModal={testUsersModal}
        setTestUsersModal={setTestUsersModal}
        qrScanCodeType={props.qrScanCodeType}
        onSelected={(code: string) => setUserAndCloseModal(code)}>
      </TestQRBarcodeDataModal>
    </>
  )
}
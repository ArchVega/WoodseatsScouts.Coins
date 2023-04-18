import React, {useContext, useEffect, useState} from "react";
import {Button, Card, CardBody, CardHeader, Input, InputGroup} from "reactstrap";
import {AppCameraAvailableContext, AppTestModeContext} from "../../App";
import TestUsersModal from "../_dev/TestUsersModal";
import TestUsersList from "../_dev/TestUsersList";
import {CoinPageCurrentUserContext} from "./CoinsPage1";

const UserScanner = () => {
    const [appCameraAvailable] = useContext(AppCameraAvailableContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);
    const [userQRCode, setUserQRCode, user, setUser] = useContext(CoinPageCurrentUserContext);
    
    const [testUsersModal, setTestUsersModal] = useState(false);

    function onScoutCodeTextBoxClicked() {
        if (appTestMode) {
            setTestUsersModal(true);
        }
    }

    function setUserAndCloseModal(code) {
        setUserQRCode(code);
        setTestUsersModal(false);
    }

    return <>
        <div className="col-6 offset-3 mb-2">
            <Card className="text-center">
                <CardHeader>
                    {
                        appCameraAvailable
                            ? <span>Scan a scout's wristband using a barcode reader or click the camera icon to take a picture using the camera.</span>
                            : <span>Scan a scout's wristband using a barcode reader.</span>
                    }
                </CardHeader>
                <CardBody>
                    <InputGroup>
                        <Input id="scout-code-textbox"
                               autoComplete="off"
                               placeholder={
                                   appTestMode
                                       ? "Test mode: click here to select test users"
                                       : "To scan a members QR code with a USB scanner, click here and then scan"}
                               defaultValue={user != null ? user.scoutName : null}
                               onClick={onScoutCodeTextBoxClicked}
                        />
                        {
                            appCameraAvailable
                                ? <Button className="btn btn-primary">&#128247;</Button>
                                : null
                        }
                    </InputGroup>
                </CardBody>
            </Card>
        </div>

        <div id="modal" title="Barcode scanner">
            <span className="found"></span>
            <div id="interactive" className="viewport"></div>
        </div>

        <TestUsersModal testUsersModal={testUsersModal} setTestUsersModal={setTestUsersModal}>
            <TestUsersList onSelected={(code) => setUserAndCloseModal(code)}></TestUsersList>
        </TestUsersModal>
    </>
}
export default UserScanner;
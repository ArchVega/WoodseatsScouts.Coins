import React, {useContext, useEffect, useState} from 'react';
import CoinScanner1 from "./CoinScanner1";
import {Button, Col, Input, InputGroup, Row} from "reactstrap";
import {toast} from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import {AppCameraAvailableContext, AppTestModeContext, UseAppCameraContext} from "../../App";
import TestUsersModal from "../_dev/TestUsersModal";
import TestUsersList from "../_dev/TestUsersList";
import CameraModal from "../camera/CameraModal";
import {QRCodeScanner} from "../camera/QRCodeScanner";

const CoinPageCurrentUserContext = React.createContext("");

const CoinsPage1 = () => {
    const [userQRCode, setUserQRCode] = useState("")
    const [user, setUser] = useState(null)
    const [completedScanning, setCompletedScanning] = useState(false)
    const [loading, setLoading] = useState(false)
    const [coinTotal, setCoinTotal] = useState(0);
    const [usbScannerValue, setUsbScannerValue] = useState("");

    const [appCameraAvailable] = useContext(AppCameraAvailableContext)
    const [useAppCamera] = useContext(UseAppCameraContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);

    const [testUsersModal, setTestUsersModal] = useState(false);
    const [cameraModal, setCameraModal] = useState(false);

    function onMemberCodeTextBoxClicked() {
        if (appTestMode) {
            setTestUsersModal(true);
        }
    }

    function onCameraButtonClicked() {
        setCameraModal(true);
    }

    function setUserAndCloseModal(code) {
        setTestUsersModal(false);
        setUserQRCode(code)
    }

    const handleKeyDown = (event) => {
        if (event.key === 'Enter') {
            // 👇 Get input value
            setUserQRCode(event.target.value)
        }
    };

    useEffect(() => {
        fetchUser(userQRCode)
    }, [userQRCode])

    const phrases = [
        "Awesome job, ",
        "Nice one, ",
        "Great work, ",
        "You did it, ",
        "Fantastic work, ",
        "Very impressive, "
    ]

    const congratsPhrase = phrases[Math.floor(Math.random() * phrases.length)];

    function fetchUser(code) {
        const fetchUser = async () => {
            if (code != null && code.trim().length > 0) {
                setLoading(true)

                const requestOptions = {
                    headers: {'Content-Type': 'application/json'}
                };

                const response = await fetch("home/GetMemberInfoFromCode?code=" + code, requestOptions);
                if (response.status === 200) {
                    const user = await response.json();
                    setUser(user);
                } else {
                    const text = await response.text();
                    toast(text)
                    setUser(null)
                }
                setLoading(false)
            }
        }
        fetchUser().then();
    }

    function reloadPage() {
        console.log('reload')
        setUser(null)
        setUserQRCode("")
        setCompletedScanning(false)
    }

    useEffect(() => {

    }, [loading])

    function onFinished(coins) {
        if (coins.length === 0) {
            toast("Add at least one coin for this member")
            return
        }

        const payload = {
            memberId: user.memberId,
            coinCodes: coins.map(x => x.code)
        }

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(payload)
        };
        fetch('home/AddPointsToMember', requestOptions);

        setCompletedScanning(true);
        new Audio("sounds/EndScanning.mp3").play();
    }

    useEffect(() => {
        setUsbScannerValue(usbScannerValue)
    }, [usbScannerValue])
    
    return (
        <CoinPageCurrentUserContext.Provider value={[userQRCode, setUserQRCode, user, setUser]}>
            {user == null ? (
                <Row>
                    <Col className="mb-2" style={{textAlign: 'center'}}>
                        <div style={{textAlign: 'center'}}><img src="images/wave.png"
                                                                style={{width: '96px', margin: '1em'}}/></div>
                        <h1 className="mb-5"><strong>Scan your wristband to start...</strong></h1>
                        <Row>
                            <Col className="col-8 offset-2">
                                {loading ? (
                                    <Row>
                                        <Col className="text-center">
                                            <div className="lds-dual-ring"></div>
                                        </Col>
                                    </Row>
                                ) : (
                                    <>
                                        {useAppCamera
                                            ? <QRCodeScanner videoSizeEm={10} qrCode={userQRCode}
                                                             setQRCode={setUserQRCode}
                                                             type={"user"}/>
                                            : <Input id="member-code-textbox"
                                                     autoComplete="off"
                                                     autoFocus={true}
                                                     value={usbScannerValue}
                                                     onChange={(e) => setUsbScannerValue(e.target.value)}
                                                     onClick={onMemberCodeTextBoxClicked}
                                                     onKeyDown={handleKeyDown}/>
                                        }
                                    </>
                                )}
                            </Col>
                        </Row>
                    </Col>

                    <div id="modal" title="Barcode scanner">
                        <span className="found"></span>
                        <div id="interactive" className="viewport"></div>
                    </div>
                </Row>
            ) : null}

            {user != null && !completedScanning ? (
                <>
                    <Row>
                        <Col>
                            <CoinScanner1 coinTotal={coinTotal} setCoinTotal={setCoinTotal}
                                          onFinished={onFinished}></CoinScanner1>
                        </Col>
                    </Row>
                </>
            ) : null
            }

            {user != null && completedScanning ? (
                <>
                    <Row>
                        <Col className="text-center">
                            <h1>{congratsPhrase}<span className="font-black">{user.firstName}</span><span
                                style={{fontSize: '1em'}}>👍</span>
                            </h1>
                        </Col>
                    </Row>
                    <Row>
                        <Col className="text-center">
                            <h4>You've just added some more points to your score.</h4>
                        </Col>
                    </Row>
                    <Row className="mb-3">
                        <Col className="text-center">
                            <span className="font-black total-points-saved">
                                {coinTotal}
                            </span>
                        </Col>
                    </Row>
                    <Row className="mb-4">
                        <Col className="text-center">
                            <h4>Head back out there and get some more points...</h4>
                        </Col>
                    </Row>
                    <Row>
                        <Col className="text-center">
                            <Button className="btn-warning btn-next-member" onClick={reloadPage}>
                                <span className="font-bold-italic">Next member</span>
                            </Button>
                        </Col>
                    </Row>
                </>
            ) : null
            }

            <TestUsersModal testUsersModal={testUsersModal} setTestUsersModal={setTestUsersModal}>
                <TestUsersList onSelected={(code) => setUserAndCloseModal(code)}></TestUsersList>
            </TestUsersModal>

            <CameraModal title="Scan member wristband" userQRCode={userQRCode} setUserQRCode={setUserQRCode}
                         cameraModal={cameraModal} setCameraModal={setCameraModal}>
            </CameraModal>
        </CoinPageCurrentUserContext.Provider>
    );
}

export {CoinPageCurrentUserContext};

export default CoinsPage1;

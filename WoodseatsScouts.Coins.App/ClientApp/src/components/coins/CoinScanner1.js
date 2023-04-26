import React, {useContext, useEffect, useState} from "react";
import ScannedCoin1 from "./coinScanner/ScannedCoin1";
import {Button, Card, CardBody, CardHeader, Col, Input, InputGroup, Row} from "reactstrap";
import app, {AppCameraAvailableContext, AppTestModeContext, UseAppCameraContext} from "../../App";
import TestCoinsModal from "../_dev/TestCoinsModal";
import TestCoinsList from "../_dev/TestCoinsList";
import {CoinPageCurrentUserContext} from "./coinsPage1";
import {QRCodeScanner} from "../camera/QRCodeScanner";
import {toast} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

const CoinScanner1 = ({onFinished, coinTotal, setCoinTotal}) => {
    const [coins, setCoins] = useState([]);
    const [appCameraAvailable] = useContext(AppCameraAvailableContext)
    const [useAppCamera] = useContext(UseAppCameraContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);
    const [testCoinsModal, setTestCoinsModal] = useState(false);    
    const [usbScannerValue, setUsbScannerValue] = useState("");   
    
    const [userQRCode, setUserQRCode, user, setUser] = useContext(CoinPageCurrentUserContext);
    const [coinScannerCamera, setCoinScannerCamera] = useState(false);
    const [coinQRCode, setCoinQRCode] = useState("")

    let userName = user == null
        ? ""
        : user.firstName;
    let memberPhotoPath = user == null
        ? "/images/unknown-member-image.png"
        : user.memberPhotoPath;

    function onCoinCodeTextBoxClicked() {
        if (appTestMode) {
            setTestCoinsModal(true);
        }
    }

    useEffect(() => {
        setUsbScannerValue(usbScannerValue)
    }, [usbScannerValue])
    
    function setCoinAndCloseModal(coin) {
        addCoin(coin);
        setTestCoinsModal(false);
    }

    const addCoin = (coin) => { 
        if (coin.pointValue >= 50) {
            toast("An invalid code has been scanned!")
            return
        }
        
        setCoins([
            ...coins,
            coin
        ])
        setCoinTotal(coinTotal + coin.pointValue)
        setCoinQRCode("")
    }

    useEffect(() => {
        const fetchCoin = async () => {
            if (coinQRCode != null && coinQRCode.trim().length > 0) {
                const requestOptions = {
                    headers: {'Content-Type': 'application/json'}
                };

                const response = await fetch("home/GetPointValueFromCode?code=" + coinQRCode, requestOptions);
                if (response.status === 200) {
                    const coin = await response.json();
                    addCoin(coin);
                } else {
                    const text = await response.text();
                    toast(text)
                }
            }
        }
        fetchCoin().then();
    }, [coinQRCode])

    function removeCoin(coin) {
        setCoinTotal(coinTotal - coin.pointValue)
        setCoins((current) =>
            current.filter((c) => c !== coin)
        );
    }

    const handleKeyDown = (event) => {
        if (event.key === 'Enter') {
            // 👇 Get input value
            setCoinQRCode(event.target.value)
            setUsbScannerValue("")
        }
    };

    return <>
        <Row className="mb-3" style={{maxHeight: '50vh'}}>
            <Col className="col-6">
                <h1>Welcome back,</h1>
                <h1 className="font-black">{userName}<img src="/images/wave.png" style={{width: '64px'}}/></h1>

                <h4>Start scanning your points cards</h4>
                {useAppCamera ? <Row>
                        <Col>
                            <Row>
                                <Col><QRCodeScanner videoSizeEm={14} qrCode={coinQRCode} setQRCode={setCoinQRCode} type="coin"/></Col>
                            </Row>                            
                        </Col>
                    </Row>
                    : <>
                        
                        <div style={{height: '3em'}}></div>
                        <Row className="mb-5">
                            <Col>
                                {
                                    useAppCamera
                                        ? <QRCodeScanner></QRCodeScanner>
                                        : <Input id="member-code-textbox"
                                                 autoComplete="off"
                                                 autoFocus={true}
                                                 onChange={(e) => setUsbScannerValue(e.target.value)}
                                                 onClick={onCoinCodeTextBoxClicked}
                                                 value={usbScannerValue}
                                                 onKeyDown={handleKeyDown}/>
                                }
                            </Col>
                        </Row>
                    </>
                }
            </Col>
            <Col className="col-6">
                <Row>
                    <Col>
                        <div id="scanned-coins-div" className="text-end" style={{overflow: 'auto', maxHeight: '50vh'}}>
                            {coins.map((coin, index) =>
                                <ScannedCoin1 key={index} coin={coin} removeCoin={removeCoin}
                                              isLast={index + 1 === coins.length}></ScannedCoin1>
                            )}
                        </div>
                    </Col>
                </Row>              
            </Col>
        </Row>

        <Row className="mt-5">
            <Col className="col-6">
                <Button onClick={() => onFinished(coins)} className="btn btn-success w-50 btn-lg">
                    <strong>Finish Scanning</strong>
                </Button>
            </Col>
            <Col className="col-6">
                <Row className="total-points-so-far">
                    <Col className="col-6">
                        <h4 className="text-end">Points added:</h4>
                    </Col>
                    <Col className=" text-end">
                        <span style={{color: "white !important"}}>+</span>
                        <strong className="font-black"
                                style={{color: "white"}}>{coinTotal}</strong>
                    </Col>
                </Row>

            </Col>
        </Row>
        <TestCoinsModal testCoinsModal={testCoinsModal} setTestCoinsModal={setTestCoinsModal}>
            <TestCoinsList onSelected={(coin) => setCoinAndCloseModal(coin)}></TestCoinsList>
        </TestCoinsModal>
    </>
}

export default CoinScanner1;
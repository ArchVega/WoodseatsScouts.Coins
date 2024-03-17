import {Button, Col, Row} from "reactstrap";
import wave from "../../../../images/wave.png";
import {toast} from "react-toastify";
import {useEffect, useState} from "react";
import ScannedCoin from "./ScannedCoin";
import QRCodeInputDevices from "../../../../components/qrinputdevices/QRCodeInputDevices";
import {logDebug, logError, logReactSet} from "../../../../components/logging/Logger";
import CoinApiService from "../../../../services/CoinApiService";
import AudioFx from "../../../../fx/AudioFx";
import QRScanCodeType from "../../../../components/qrscanners/QRScanCodeType";
import {toastError} from "../../../../components/toaster/toaster";

function ScanCoinsSection({member, setHaulResult}) {
    const audioFx = AudioFx();
    const [coinQrCode, setCoinQrCode] = useState("");
    const [coins, setCoins] = useState([]);
    const [coinTotal, setCoinTotal] = useState(0);

    useEffect(() => {
        if (coinQrCode != null && coinQrCode.trim().length > 0) {
            logDebug(`Fetching coin data for code ${coinQrCode}`)

            async function fetchData() {
                return await CoinApiService().fetchCoin(coinQrCode, member.memberCode)
            }

            function isDuplicateCoin(coin) {
                return coins.filter(x => x.code === coin.code).length > 0
            }

            fetchData()
                .then(async value => {
                    const coin = (await value.data)
                    logReactSet("Set Coins", coin)

                    if (!isDuplicateCoin((coin))) {
                        setCoins([...coins, coin])
                        audioFx.playCoinScannedSuccessAudio()
                    } else {
                        audioFx.playCoinScannedErrorAudio()
                        toastError(`That coin has already been scanned for ${member.firstName}`)
                    }
                })
                .catch(async axiosReason => {
                    audioFx.playCoinScannedErrorAudio()
                    logError("Set Coins", axiosReason)
                    toastError(axiosReason)
                })
        }
    }, [coinQrCode]);

    useEffect(() => {
        setCoinTotal(coins.reduce((n, {pointValue}) => n + pointValue, 0))
    }, [coins]);

    function removeCoin(coin) {
        setCoins((current) => current.filter((c) => c !== coin));
    }

    async function onFinished() {
        if (coins.length === 0) {
            toast("Add at least one coin for this member")
            return
        }

        await CoinApiService().addPointsToMember(member, coins).then(async response => {
            const additionalData = await response.data

            let finalTotal = coinTotal;
            if (additionalData.hasAnomalyOccurred) {
                finalTotal = finalTotal - additionalData.anomalousCoinsTotalValue
            }

            setHaulResult({
                coinTotal: finalTotal,
                additionalData: additionalData
            })
        })
    }

    return (
        <>
            <Row className="mb-3" style={{minHeight: '45vh', maxHeight: '50vh'}}>
                <Col className="col-6">
                    <h1 id="scan-coins-section-h1">Welcome back,
                        <br/>
                        <span className="font-black" data-testid="h1-user-firstname">
                            <span>{member.firstName}</span>
                            <img id="scan-coins-section-wave-image" src={wave} alt=""/>
                        </span>
                    </h1>

                    <h4>Start scanning your points cards</h4>
                    <QRCodeInputDevices qrCode={coinQrCode} setQrCode={setCoinQrCode} qrScanCodeType={QRScanCodeType.Coin}/>
                </Col>
                <Col className="col-6">
                    <Row>
                        <Col>
                            <div id="scanned-coins-div" data-testid="div-scanned-coins" className="text-end"
                                 style={{overflow: 'auto', maxHeight: '50vh'}}>
                                {coins.map((coin, index) =>
                                    <ScannedCoin key={index}
                                                 coin={coin}
                                                 isLast={index + 1 === coins.length}
                                                 removeCoin={coinToRemove => removeCoin(coinToRemove)}/>
                                )}
                            </div>
                        </Col>
                    </Row>
                </Col>
            </Row>

            <Row className="mt-5">
                <Col className="col-6">
                    <Button data-testid="button-finish-scanning" onClick={onFinished} className="btn btn-success w-50 btn-lg">
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
                                    data-testid="coin-total"
                                    style={{color: "white"}}>{coinTotal}</strong>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </>
    )
}

export default ScanCoinsSection
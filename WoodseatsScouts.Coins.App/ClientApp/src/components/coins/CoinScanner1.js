import React, {useContext, useEffect, useState} from "react";
import ScannedCoin from "./coinScanner/scannedCoin";
import {Button, Card, CardBody, CardHeader, Input, InputGroup} from "reactstrap";
import app, {AppCameraAvailableContext, AppTestModeContext} from "../../App";
import TestCoinsModal from "../_dev/TestCoinsModal";
import TestCoinsList from "../_dev/TestCoinsList";

const CoinScanner1 = () => {
    const [coins, setCoins] = useState([]);
    const [appCameraAvailable] = useContext(AppCameraAvailableContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);
    const [testCoinsModal, setTestCoinsModal] = useState(false);
    const [coin20Tally, setCoin20Tally] = useState(0);
    const [coin10Tally, setCoin10Tally] = useState(0);
    const [coinOtherTally, setCoinOtherTally] = useState(0);

    const [coin20TallyTotal, setCoin20TallyTotal] = useState(0);
    const [coin10TallyTotal, setCoin10TallyTotal] = useState(0);
    const [coinOtherTallyTotal, setCoinOtherTallyTotal] = useState(0);

    const [coinTallyTotal, setCoinTallyTotal] = useState(0);
    const [coinTotal, setCoinTotal] = useState(0);

    function onCoinCodeTextBoxClicked() {
        if (appTestMode) {
            setTestCoinsModal(true);
        }
    }

    function setCoinAndCloseModal(coin) {
        addCoin(coin);
        setTestCoinsModal(false);
    }

    const addCoin = (coin) => {
        setCoins([
            ...coins,
            coin
        ])
    }

    useEffect(() => {
        setCoin20Tally(coins.filter(x => x.points === 20).length);
        setCoin10Tally(coins.filter(x => x.points === 10).length);
        setCoinOtherTally(coins.filter(x => !(x.points === 20 || x.points === 10)).length);
    }, [coins])
    
    useEffect(() => {
        setCoin20TallyTotal(coin20Tally * 20);
        setCoin10TallyTotal(coin10Tally * 10);
        setCoinOtherTallyTotal(coinOtherTally * -1);
    }, [coin20Tally, coin10Tally, coinOtherTally])

    useEffect(() => {
        setCoinTallyTotal(coin20Tally + coin10Tally + coinOtherTally);
        setCoinTotal(coin20TallyTotal + coin10TallyTotal + coinOtherTallyTotal);
    }, [coin20TallyTotal, coin10TallyTotal, coinOtherTallyTotal])

    function removeCoin(coin) {
        console.log(coin)
        setCoins((current) =>
            current.filter((c) => c !== coin)
        );
    }
    
    return <>
        <h4>Scan</h4>

        <Card className="mb-3">
            <CardHeader className="text-center">
                {
                    appCameraAvailable
                        ? <span>Scan coins using a barcode reader or click the camera icon to take a series of pictures using the camera.</span>
                        : <span>Scan coins using a barcode reader.</span>
                }
            </CardHeader>
            <CardBody>
                <InputGroup>
                    <Input id="scout-code-textbox"
                           autoComplete="off"
                           placeholder={
                               appTestMode
                                   ? "Test mode: click here to select test coins"
                                   : "To scan a coin with a USB scanner, click here and then scan"}                           
                           onClick={onCoinCodeTextBoxClicked}
                           defaultValue=""/>
                    {
                        appCameraAvailable
                            ? <Button className="btn btn-primary">&#128247;</Button>
                            : null
                    }
                </InputGroup>
            </CardBody>
        </Card>

        <Card className="mb-3">
            <CardHeader className="text-center">
                Coins scanned
            </CardHeader>
            <CardBody>
                <div className="row">
                    <div className="col-6">
                        <div id="scanned-coins-div">
                            {coins.map((coin, index) =>
                                <ScannedCoin key={index} coin={coin} removeCoin={removeCoin}></ScannedCoin>
                            )}
                        </div>
                    </div>
                    <div className="col-6">
                        <div id="scanned-coins-tally-div">
                            <table className="table table-borderless">
                                <tbody>
                                <tr>
                                    <td>20 point coins</td>
                                    <td>x</td>
                                    <td>{coin20Tally}</td>
                                    <td>=</td>
                                    <td>{coin20TallyTotal}</td>
                                </tr>
                                <tr>
                                    <td>10 point coins</td>
                                    <td>x</td>
                                    <td>{coin10Tally}</td>
                                    <td>=</td>
                                    <td>{coin10TallyTotal}</td>
                                </tr>
                                <tr>
                                    <td>Other coins</td>
                                    <td>x</td>
                                    <td>{coinOtherTally}</td>
                                    <td>=</td>
                                    <td>{coinOtherTallyTotal}</td>
                                </tr>
                                <tr></tr>
                                <tr>
                                    <td colSpan={2}>Total</td>
                                    <td>{coinTallyTotal}</td>
                                    <td></td>
                                    <td>{coinTotal}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
                <div className="row">
                    <hr/>
                    <button className="btn btn-success float-end"
                            data-bind="click: function() { confirmSubmit() }">Finished
                    </button>
                </div>
            </CardBody>
        </Card>
        <TestCoinsModal testCoinsModal={testCoinsModal} setTestCoinsModal={setTestCoinsModal}>
            <TestCoinsList onSelected={(coin) => setCoinAndCloseModal(coin)}></TestCoinsList>
        </TestCoinsModal>
    </>
}

export default CoinScanner1;
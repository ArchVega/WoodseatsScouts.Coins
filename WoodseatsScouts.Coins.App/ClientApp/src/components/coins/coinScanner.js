import React, {useState} from "react";
import ScannedCoin from "./coinScanner/scannedCoin";
import {Button, Card, CardBody, CardHeader, Input, InputGroup} from "reactstrap";

const coinsArray = [
    {
        id: 1,
        points: 20
    },
    {
        id: 2,
        points: 10
    }
];

const CoinScanner = () => {
    const [coins, setCoins] = useState(coinsArray);

    const addCoins = () => {
        setCoins([
            ...coins,
            {
                id: 3,
                points: 25
            }
        ])
    }

    return <>
        <h4>Scan</h4>

        <Card className="mb-3">
            <CardHeader className="text-center">
                Scan coins using a barcode reader or click the camera icon to take a series of pictures using the camera
            </CardHeader>
            <CardBody>
                <InputGroup>
                    <Input id="scout-code-textbox" autoComplete="off" placeholder="Click here and scan a new coin"
                           data-bind="textInput: lastScannedCoinCode, valueUpdate: 'keyup', event: { keypress: onCoinCodeFieldKepPressed }, enable: scanCoinFieldEnabled"
                           defaultValue=""/>
                    <Button onClick={addCoins} className="btn btn-primary">&#128247;</Button>
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
                                <ScannedCoin key={index} coin={coin}></ScannedCoin>
                            )}
                        </div>
                    </div>
                    <div className="col-6">
                        <ul>
                            <li>20 x</li>
                            <li>10 x</li>
                            <li>Other x</li>
                        </ul>
                    </div>
                </div>
                <div className="row">
                    <span>
                        <b>Total:&nbsp;</b>
                        <span data-bind="text: totalPoints"></span>
                    </span>
                    <hr/>
                    <button className="btn btn-success float-end"
                            data-bind="click: function() { confirmSubmit() }">Finished
                    </button>
                </div>
            </CardBody>
        </Card>
    </>
}

export default CoinScanner;
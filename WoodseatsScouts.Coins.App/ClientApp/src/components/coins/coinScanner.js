import React, {useState} from "react";
import ScannedCoin from "./coinScanner/scannedCoin";

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

        <div className="row" style={{marginBottom: "1em"}}>
            <div className="col-9">
                <input id="coin-textbox"
                       type="text"
                       data-bind="textInput: lastScannedCoinCode, valueUpdate: 'keyup', event: { keypress: onCoinCodeFieldKepPressed }, enable: scanCoinFieldEnabled"
                       className="form-control"
                       placeholder="Click here and scan a new coin"
                       autoComplete="off"/>
            </div>
            <div className="col-1" style={{textAlign: "center", paddingTop:"0.5em"}}>
                Or
            </div>
            <div className="col-2">
                <button className="btn btn-primary" onClick={addCoins}>&#128247;</button>
            </div>

        </div>

        <div id="scanned-coins-div">
            {coins.map((coin, index) => 
                <ScannedCoin key={index} coin={coin}></ScannedCoin>
            )}
        </div>

        <span>
            <b>Total:&nbsp;</b>
            <span data-bind="text: totalPoints"></span>
        </span>
        <hr/>

        <button className="btn btn-success float-end"
                data-bind="click: function() { confirmSubmit() }">Finished
        </button>
    </>
}

export default CoinScanner;
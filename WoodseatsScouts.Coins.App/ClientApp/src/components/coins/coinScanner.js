import React from "react";

const CoinScanner = () => {
    return <div className="col-6">
        <h4>Scan</h4>

        <div className="row">
            <div className="col-12">
                <input id="coin-textbox"
                       type="text"
                       data-bind="textInput: lastScannedCoinCode, valueUpdate: 'keyup', event: { keypress: onCoinCodeFieldKepPressed }, enable: scanCoinFieldEnabled"
                       className="form-control"
                       placeholder="Click here and scan a new coin"
                       autoComplete="off"/>
            </div>
        </div>

        <div id="scanned-coins-div" data-bind="foreach: scannedCoins">
                        <span className="tag label label-info">
                            <span data-bind="text:displayText"></span>
                            <span id="remove-score">
                                <i data-bind="click: function() { $parent.confirmRemovePoint($data) } "
                                   className="remove">&#10060;</i>
                            </span>
                        </span>
        </div>

        <span>
                        <b>Total:&nbsp;</b>
                        <span data-bind="text: totalPoints"></span>
                    </span>
        <hr/>

        <button className="btn btn-success float-end"
                data-bind="click: function() { confirmSubmit() }">Finished
        </button>
    </div>
}

export default CoinScanner;
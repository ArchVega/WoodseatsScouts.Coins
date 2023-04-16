import React, {Component} from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h3>COINS</h3>

                <div className="row">
                    <div className="col-6 offset-3">
                        <input id="scout-code-textbox"
                               type="text"
                               className="form-control"
                               placeholder="Click here and scan scout code"
                               autoComplete="off"/>
                    </div>
                    <div className="col-3">
                        <button className="btn btn-primary">&#128247;</button>
                    </div>

                    <div id="modal" title="Barcode scanner">
                        <span className="found"></span>
                        <div id="interactive" className="viewport"></div>
                    </div>
                </div>

                <hr/>

                <div className="col-6">
                    <h4 data-bind="text: scoutName"></h4>
                    <span id="scoutTroopNumberAndSection"></span>
                    <img id="scout-photo" data-bind="attr: { src:scoutPhotoPath }"/>
                </div>

                <div className="col-6">
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
            </div>
        );
    }
}

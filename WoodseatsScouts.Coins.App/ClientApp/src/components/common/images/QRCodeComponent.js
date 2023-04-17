import React from "react";
import {Button, Col, Form, FormGroup, Input, InputGroup, Label, Row} from "reactstrap";

const QRCodeComponent = () => {
    return <>
        <Row className="row-cols-lg-auto g-3 align-items-center">
            <Col className="mb-3">
                <Label
                    className="fw-bold"
                    check
                    for="exampleCheckbox">
                    QR Code
                </Label>
            </Col>
        </Row>
        <Row>
            <Col className="mb-3">
                <div className="small">
                    To scan a new member's QR code, either click in the textbox below and then use a USB scanner, or
                    click the Camera icon to use the device's camera.
                </div>
            </Col>
        </Row>
        <Row>
            <Col>
                <InputGroup>
                    <Input id="scout-code-textbox" autoComplete="off" placeholder="Click here to scan a member's QR code"
                           data-bind="textInput: lastScannedCoinCode, valueUpdate: 'keyup', event: { keypress: onCoinCodeFieldKepPressed }, enable: scanCoinFieldEnabled"
                           defaultValue=""/>
                    <Button className="btn btn-primary">&#128247;</Button>
                </InputGroup>
            </Col>
        </Row>
    </>
}

export default QRCodeComponent;
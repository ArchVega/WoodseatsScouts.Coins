import React from "react";
import {Button, Col, Input, InputGroup, Label, Row} from "reactstrap";

const PhotoComponent = () => {
    return <>
        <Row className="row-cols-lg-auto g-3 align-items-center">
            <Col className="mb-3">
                <Label
                    className="fw-bold"
                    check
                    for="exampleCheckbox">
                    Member Name
                </Label>
            </Col>
        </Row>
        <Row>
            <Col className="mb-3">
                <div className="small">
                    Enter the member's name and take their photo.
                </div>
            </Col>
        </Row>
        <Row className="mb-3">
            <Col>
                <Input id="scout-code-textbox" autoComplete="off" placeholder="Name"
                       data-bind="textInput: lastScannedCoinCode, valueUpdate: 'keyup', event: { keypress: onCoinCodeFieldKepPressed }, enable: scanCoinFieldEnabled"
                       defaultValue=""/>
            </Col>
        </Row>

        <Row className="mb-1">
            <Col className="text-center">
                <img src="/images/unknown-scout-image.jpg" style={{width: "100%"}}/>    
            </Col>
        </Row>
        <Row className="">
            <Col className="text-center">
                <div className="d-grid gap-1">
                    <Button className="btn btn-primary">Take photo</Button>
                    <Button className="btn btn-primary">Delete photo</Button>
                </div>
            </Col>
        </Row>
    </>
}

export default PhotoComponent
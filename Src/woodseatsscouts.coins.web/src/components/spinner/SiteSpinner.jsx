import {Col, Row} from "reactstrap";
import React from "react";

function SiteSpinner() {
    return (
        <Row>
            <Col className="text-center">
                <div className="spinner"></div>
            </Col>
        </Row>
    )
}

export default SiteSpinner
import React from "react";
import {Button, Col, Input, InputGroup, Row} from "reactstrap";
const UsersPage = () => {
    return <>
        <h3>Users</h3>
        <Row className="mb-5">
            <Col className="col-3">
                <InputGroup>
                    <Input type="text" placeholder="Filter name"></Input>
                    <Button>x</Button>
                </InputGroup>
            </Col>
            <Col className="col-3">
                <InputGroup>
                    <Input type="text" placeholder="Filter scout number"></Input>
                    <Button>x</Button>
                </InputGroup>
            </Col>
            <Col className="col-3">
                <InputGroup>
                    <Input type="text" placeholder="Filter troop number"></Input>
                    <Button>x</Button>
                </InputGroup>
            </Col>
            <Col className="col-3">
                <InputGroup>
                    <Input type="text" placeholder="Filter section"></Input>
                    <Button>x</Button>
                </InputGroup>
            </Col>            
        </Row>
        <hr/>
        <Row>
            <Col>
                <table className="table table-hover">
                    <thead>
                    <tr>
                        <th></th>
                        <th>Name</th>
                        <th>Scout Number</th>
                        <th>Troop Number</th>
                        <th>Section</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td className="fit">
                            <img src="/scout-images/1.jpg"/>
                        </td>
                        <td>Bob</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="/scout-images/5.jpg"/>
                        </td>
                        <td>
                            Carl
                        </td>
                    </tr>
                    </tbody>
                </table>
            </Col>
        </Row>
    </>
}

export default UsersPage;
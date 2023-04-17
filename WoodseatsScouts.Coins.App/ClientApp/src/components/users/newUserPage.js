import React from "react";
import {Button, Form, Input, InputGroup} from "reactstrap";
import QRCodeComponent from "../common/images/QRCodeComponent";
import PhotoComponent from "../common/images/PhotoComponent";

const NewUserPage = () => {
    return <>
        <h3>New User</h3>
        <Form className="col-6 offset-3">
            <div className="row mb-5">
                <div className="col-12">
                    <div className="row g-3 align-items-center mb-3">
                        <QRCodeComponent></QRCodeComponent>
                    </div>
                    <div className="row g-3 align-items-center">
                        <PhotoComponent></PhotoComponent>                                               
                    </div>
                </div>
            </div>
            <hr/>
            <div className="row">
                <div className="col-lg">
                    <button className="btn btn-success float-end">Add user</button>
                </div>
            </div>
        </Form>        
    </>
}

export default NewUserPage;
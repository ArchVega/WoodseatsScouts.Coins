import React from "react";
import {Button, Input, InputGroup} from "reactstrap";

const NewUserPage = () => {
    return <>
        <h3>New User</h3>
        <div className="row mb-5">
            <div className="col-12">
                <div className="row g-3 align-items-center">
                    <div className="col-auto mb-3">
                        <label htmlFor="" className="col-form-label">QR Code</label>
                    </div>
                    <div className="col-5">
                        <input type="text" id="newUserQrCode" className="form-control"
                               aria-describedby="" />
                    </div>
                </div>
                <div className="row g-3 align-items-center">
                    <div className="col-auto mb-3">
                        <label htmlFor="newUserName" className="col-form-label">Name</label>
                    </div>
                    
                    <div className="col-5">
                        <input type="text" id="newUserName" className="form-control"
                               aria-describedby="" />
                    </div>
                    <div className="col-auto">
                        <button className="btn btn-primary">&#128247;</button>
                    </div>
                    <div className="col-auto">
                        <img src="/images/unknown-scout-image.jpg" style={{width: "100px"}}/>
                    </div>
                    <div className="col-lg">
                        <button className="btn btn-success float-end">Add user</button>
                    </div>                    
                </div>
            </div>    
        </div>
    </>
}

export default NewUserPage;
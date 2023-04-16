import React from "react";

const UserScanner = () => {
    return <>
        <div className="col-6 offset-3">
            <input id="scout-code-textbox"
                   type="text"
                   className="form-control"
                   placeholder="Click here and scan scout code"
                   autoComplete="off"/>
        </div>
        <div className="col-1" style={{textAlign: "center", paddingTop:"0.5em"}}>
            Or
        </div>
        <div className="col-2">
            <button className="btn btn-primary">&#128247;</button>
        </div>

        <div id="modal" title="Barcode scanner">
            <span className="found"></span>
            <div id="interactive" className="viewport"></div>
        </div>
    </>
}

export default UserScanner;
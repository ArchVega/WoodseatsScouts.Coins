import React, {useEffect, useState} from "react";

const UserScanner = () => {
    const [user, setUser] = useState({
        
    })
    
    useEffect(() => {
        const fetchUser = async() => {
            const response = await fetch("home/GetScoutInfoFromCode?code=M013B004");
            const user = await response.json();
            setUser(user);
        }
        fetchUser().then();
    }, [])
    
    return <>
        <div className="col-6 offset-3">
            <input id="scout-code-textbox"
                   type="text"
                   className="form-control"
                   placeholder="Click here and scan scout code"
                   autoComplete="off" 
                   defaultValue={user.scoutName}/>
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
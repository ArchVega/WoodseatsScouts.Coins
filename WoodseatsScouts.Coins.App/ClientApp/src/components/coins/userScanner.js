import React, {useEffect, useState} from "react";
import {Button, Card, CardBody, CardHeader, Input, InputGroup} from "reactstrap";

const UserScanner = () => {
    const [user, setUser] = useState({})

    useEffect(() => {
        const fetchUser = async () => {
            const response = await fetch("home/GetScoutInfoFromCode?code=M013B004");
            const user = await response.json();
            setUser(user);
        }
        fetchUser().then();
    }, [])

    return <>
        <div className="col-6 offset-3 mb-2">
            <Card className="text-center">
                <CardHeader>
                    Scan scout's wristband using a barcode reader or click the camera icon to take a picture using the camera
                </CardHeader>
                <CardBody>
                    <InputGroup>
                        <Input id="scout-code-textbox" autoComplete="off" placeholder="Click here and scan scout code" defaultValue={user.scoutName}/>
                        <Button className="btn btn-primary">&#128247;</Button>
                    </InputGroup>        
                </CardBody>
            </Card>            
        </div>

        <div id="modal" title="Barcode scanner">
            <span className="found"></span>
            <div id="interactive" className="viewport"></div>
        </div>
    </>
}

export default UserScanner;
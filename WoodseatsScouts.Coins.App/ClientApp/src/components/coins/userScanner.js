import React, {useContext, useEffect, useState} from "react";
import {Button, Card, CardBody, CardHeader, Input, InputGroup} from "reactstrap";
import {AppCameraAvailableContext, AppTestModeContext} from "../../App";

const UserScanner = () => {
    const [user, setUser] = useState({})
    const [appCameraAvailable] = useContext(AppCameraAvailableContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);

    useEffect(() => {
        const fetchUser = async () => {
            // const response = await fetch("home/GetScoutInfoFromCode?code=M013B004");
            // const user = await response.json();
            // setUser(user);
        }
        fetchUser().then();
    }, [])

    return <>
        <div className="col-6 offset-3 mb-2">
            <Card className="text-center">
                <CardHeader>
                    {appCameraAvailable
                        ? <span>Scan a scout's wristband using a barcode reader or click the camera icon to take a picture using the camera.</span>
                        : <span>Scan a scout's wristband using a barcode reader.</span>
                    }
                </CardHeader>
                <CardBody>
                    <InputGroup>
                        <Input id="scout-code-textbox"
                               autoComplete="off"
                               placeholder={
                                   appTestMode
                                       ? "Test mode: click here to select test users" 
                                       : "To scan a members QR code with a USB scanner, click here and then scan"}
                               defaultValue={user.scoutName}/>
                        {
                            appCameraAvailable
                                ? <Button className="btn btn-primary">&#128247;</Button>
                                : null
                        }
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
﻿import {
    Form, FormGroup, Input, Label, Navbar, NavbarText
} from "reactstrap";
import React, {useContext, useEffect, useState} from "react";
import {AppCameraAvailableContext, AppTestModeContext} from "../../App";

const SiteDevBar = () => {
    const [state, setState] = useState(false);
    const [testModeState, setTestModeState] = useState(false);

    const [appCameraAvailable, setAppCameraAvailable] = useContext(AppCameraAvailableContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);

    useEffect(() => {
        setAppCameraAvailable(state)
    }, [state])

    useEffect(() => {
        setAppTestMode(testModeState)
    }, [testModeState])
    return <>
        <header className="site-dev-bar">
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 bg-info" container
                    dark>
                <NavbarText className="text-dark">
                    <strong>Developer Bar</strong>
                </NavbarText>
                <ul className="navbar-nav flex-grow">
                    <li>
                        <Form>
                            <FormGroup switch>
                                <Input type="switch"
                                       role="switch"
                                       checked={state}
                                       onChange={() => {
                                           setState(!state);
                                       }}/>
                                <Label check>Camera</Label>
                            </FormGroup>
                        </Form>
                    </li>
                    <li>
                        <Form>
                            <FormGroup switch>
                                <Input type="switch"
                                       role="switch"
                                       checked={testModeState}
                                       onChange={() => {
                                           setTestModeState(!testModeState);
                                       }}/> 
                                <Label check>Test mode</Label>
                            </FormGroup>
                        </Form>
                    </li>
                </ul>
            </Navbar>
        </header>
    </>
}

export default SiteDevBar;
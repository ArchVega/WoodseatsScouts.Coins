import {
    Form, FormGroup, Input, Label, Navbar, NavbarText
} from "reactstrap";
import React, {useContext, useEffect, useState} from "react";
import SiteDevBarInfo from "./SiteDevBarInfo";
import {AppCameraAvailableContext, AppModeContext, AppTestModeContext, UseAppCameraContext} from "../../contexts/AppContext";

const SiteDevBar = () => {
    const [appCameraAvailable, setAppCameraAvailable] = useContext(AppCameraAvailableContext)
    const [useAppCamera, setUseAppCamera] = useContext(UseAppCameraContext)
    const [appTestMode, setAppTestMode] = useContext(AppTestModeContext);
    const [appModeContext, setAppModeContext] = useContext(AppModeContext);

    const [state, setState] = useState(true);
    const [testModeState, setTestModeState] = useState(true);

    useEffect(() => {
        setUseAppCamera(state)
    }, [state])

    useEffect(() => {
        setAppTestMode(testModeState)
    }, [testModeState])

    return <>
        <SiteDevBarInfo appMode={appModeContext}/>
        < header className="site-dev-bar" style={{display: "none"}}>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 bg-info" container dark>
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
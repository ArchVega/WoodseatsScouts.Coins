import React, {useContext, useEffect, useState} from "react";
import SiteDevBarInfo from "./SiteDevBarInfo.tsx";
import {Form} from "react-router-dom";
import {AppCameraAvailableContext, AppModeContext, AppTestModeContext, UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";

const SiteDevBar = () => {
    const {appCameraAvailable} = useContext(AppCameraAvailableContext)
    const {useAppCamera, setUseAppCamera} = useContext(UseAppCameraContext)
    const {appTestMode, setAppTestMode} = useContext(AppTestModeContext);
    const {appMode} = useContext(AppModeContext);

    const [state, setState] = useState(true);
    const [testModeState, setTestModeState] = useState(true);

    useEffect(() => {
        setUseAppCamera(state)
    }, [state])

    useEffect(() => {
        setAppTestMode(testModeState)
    }, [testModeState])

    return <>
        <SiteDevBarInfo appMode={appMode}/>
        < header className="site-dev-bar" style={{display: "none"}}>
            <div className="container navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 bg-info" >
                <div className="nav-bar-text text-dark">
                    <strong>Developer Bar</strong>
                </div>
                <ul className="navbar-nav flex-grow">
                    <li>
                        <form>
                            <div className={"input-group"}>
                                <input type="switch"
                                       role="switch"
                                       checked={state}
                                       onChange={() => {
                                           setState(!state);
                                       }} />
                                <label>Camera</label>
                            </div>
                        </form>
                    </li>
                    <li>
                        <form>
                            <div className={"input-group"}>
                                <input type="switch"
                                       role="switch"
                                       checked={testModeState}
                                       onChange={() => {
                                           setTestModeState(!testModeState);
                                       }}/>
                                <label>Test mode</label>
                            </div>
                        </form>
                    </li>
                </ul>
            </div>
        </header>
    </>
}

export default SiteDevBar;
import React, {Component, useCallback, useEffect, useState} from 'react';
import {Route, Routes} from 'react-router-dom';
import AppRoutes from './components/navigation/AppRoutes';
import hasCamera from "./js/CameraUtilities";
import Layout from "./components/common/Layout";

const cameraAvailable = 'mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices;
const AppCameraAvailableContext = React.createContext(cameraAvailable);
const UseAppCameraContext = React.createContext(cameraAvailable);
const AppTestModeContext = React.createContext(false);

const App = () => {
    const [appCameraAvailable, setAppCameraAvailable] = useState(cameraAvailable);
    const [useAppCamera, setUseAppCamera] = useState(cameraAvailable);    
    const [appTestMode, setAppTestMode] = useState(false);

    useCallback(() => {
        hasCamera(exists => {
            setAppCameraAvailable(exists);
        }, [])
    })

    return <>
        <AppCameraAvailableContext.Provider value={[appCameraAvailable, setAppCameraAvailable]}>
            <UseAppCameraContext.Provider value={[useAppCamera, setUseAppCamera]}>
                <AppTestModeContext.Provider value={[appTestMode, setAppTestMode]}>
                    <Layout>
                        <Routes>
                            {AppRoutes.map((route, index) => {
                                const {element, ...rest} = route;
                                return <Route key={index} {...rest} element={element}/>;
                            })}
                        </Routes>
                    </Layout>
                </AppTestModeContext.Provider>
            </UseAppCameraContext.Provider>
        </AppCameraAvailableContext.Provider>
    </>
}

export {AppCameraAvailableContext, UseAppCameraContext, AppTestModeContext};
export default App;
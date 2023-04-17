import React, {Component, useCallback, useEffect, useState} from 'react';
import {Route, Routes} from 'react-router-dom';
import AppRoutes from './components/navigation/AppRoutes';
import {Layout} from './components/common/Layout';
import './css/site.css';
import hasCamera from "./js/CameraUtilities";

const AppCameraAvailableContext = React.createContext(false);
const App = () => {
    const [appCameraAvailable, setAppCameraAvailable] = useState(false);
    useCallback(() => {
        hasCamera(exists => {           
            console.log("once")
            setAppCameraAvailable(exists);
        }, [])
    })

    return <AppCameraAvailableContext.Provider value={[appCameraAvailable, setAppCameraAvailable]}>
        <Layout>
            <Routes>
                {AppRoutes.map((route, index) => {
                    const {element, ...rest} = route;
                    return <Route key={index} {...rest} element={element}/>;
                })}
            </Routes>
        </Layout>                
    </AppCameraAvailableContext.Provider>
}

export {AppCameraAvailableContext};
export default App;
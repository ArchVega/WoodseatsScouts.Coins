import './App.css';
import React from "react";
import {Route, Routes} from "react-router-dom";
import Layout from "./components/common/Layout";
import {AppContext} from "./contexts/AppContext";
import AppRoutes from "./components/navigation/AppRoutes";

function App() {
    return (
        <AppContext>
            <Layout>
                <Routes>
                    {AppRoutes.map(
                        (route, index) => {
                            const {element, ...rest} = route;
                            return <Route key={index} {...rest} element={element}/>;
                        })
                    }
                </Routes>
            </Layout>
        </AppContext>
    );
}

export default App;

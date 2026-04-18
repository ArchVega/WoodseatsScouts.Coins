import React, {Component} from 'react';
import SiteDevBar from "../_dev/siteDevBar.tsx";
import NavMenu from "../navigation/NavMenu.tsx";
// import SiteDevBar from "../_dev/siteDevBar";
// import NavMenu from "../navigation/NavMenu";
// import {ToastContainer} from "react-toastify";
// import 'react-toastify/dist/ReactToastify.css';

export default function Layout({children}) {
    return <div className={"app-shell d-flex flex-column min-vh-100"}>
        <SiteDevBar />
        <NavMenu />
        <div id="site-container" className="container">
            {children}
        {/*</Container>*/}
        {/*<ToastContainer />*/}
        </div>
    </div>
}
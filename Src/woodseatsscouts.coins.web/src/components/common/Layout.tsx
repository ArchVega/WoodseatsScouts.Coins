import React, {Component} from 'react';
// import SiteDevBar from "../_dev/siteDevBar";
// import NavMenu from "../navigation/NavMenu";
// import {ToastContainer} from "react-toastify";
// import 'react-toastify/dist/ReactToastify.css';

const Layout = ({children}) => {
    return <div>
        {/*<SiteDevBar/>*/}
        {/*<NavMenu />*/}
        <div id="site-container" className="container-fluid">
            {children}
        {/*</Container>*/}
        {/*<ToastContainer />*/}
        </div>
    </div>
}

export default Layout
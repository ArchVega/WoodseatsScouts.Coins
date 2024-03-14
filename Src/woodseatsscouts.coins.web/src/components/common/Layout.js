import React, {Component} from 'react';
import {Container} from 'reactstrap';
import SiteDevBar from "../_dev/siteDevBar";
import NavMenu from "../navigation/NavMenu";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
const Layout = ({children}) => {
    return <div>
        <SiteDevBar/>
        <NavMenu />
        <Container id="site-container" tag="main" className="container-fluid">
            {children}
        </Container>
        <ToastContainer />
    </div>
}

export default Layout
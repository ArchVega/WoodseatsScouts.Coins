import React, {Component} from 'react';
import {Container} from 'reactstrap';
import SiteDevBar from "../_dev/siteDevBar";
import {ToastContainer} from "react-toastify";
import NavMenu from "../navigation/NavMenu";

const Layout = ({children}) => {
    return <div>
        <SiteDevBar/>
        <NavMenu />
        <Container id="site-container" tag="main" className="container-fluid">
            {children}
        </Container>
        <ToastContainer/>
    </div>
}

export default Layout
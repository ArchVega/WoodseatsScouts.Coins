import React, {Component} from 'react';
import {Container} from 'reactstrap';
import {NavMenu} from '../navigation/NavMenu';
import SiteDevBar from "../_dev/siteDevBar";

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <SiteDevBar/>
                <NavMenu/>
                <Container tag="main">
                    {this.props.children}
                </Container>
            </div>
        );
    }
}

import {
    Form, FormGroup, Input, Label, Navbar, NavbarText
} from "reactstrap";
import React, {useState} from "react";

const SiteDevBar = ({direction, ...args}) => {
    const [state, setState] = useState(true);
    return <>
        <header className="site-dev-bar">
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 bg-info" container
                    dark>
                <NavbarText className="text-dark">
                    <strong>Developer Bar</strong>
                </NavbarText>
                <ul className="navbar-nav flex-grow">
                    <li>
                        <Form>
                            <FormGroup switch>
                                <Input type="switch" role="switch"/>
                                <Label check>Camera</Label>
                            </FormGroup>
                        </Form>
                    </li>
                    <li>
                        <Form>
                            <FormGroup switch>
                                <Input type="switch" role="switch"/>
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
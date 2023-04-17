import {
    Collapse, Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownToggle,
    Navbar,
    NavbarBrand, NavbarText,
    NavLink,
    UncontrolledDropdown
} from "reactstrap";
import {Link} from "react-router-dom";
import React, {useState} from "react";

const SiteDevBar = ({ direction, ...args }) => {
    const [dropdownOpen, setDropdownOpen] = useState(false);

    const toggle = () => setDropdownOpen((prevState) => !prevState);
    
    return <>
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 bg-info" container
                    dark>
                <NavbarText className="text-dark">
                    Developer Bar
                </NavbarText>
                <ul className="navbar-nav flex-grow">
                    <Dropdown isOpen={dropdownOpen} toggle={toggle} direction={direction}>
                        <DropdownToggle caret>Mode</DropdownToggle>
                        <DropdownMenu {...args}>
                            <DropdownItem>Dev</DropdownItem>
                            <DropdownItem>Release</DropdownItem>                            
                        </DropdownMenu>
                    </Dropdown>
                </ul>
            </Navbar>
        </header>
    </>
}

export default SiteDevBar;
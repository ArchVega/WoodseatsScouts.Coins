import SheafDistrictCampWhite from '../../images/sheaf-district-camp-white.png'
import React, {Component, useContext, useEffect, useState} from 'react';
import {
    Collapse,
    DropdownItem, DropdownMenu, DropdownToggle, Form, FormGroup, Input, Label,
    Navbar,
    NavbarBrand,
    NavbarToggler,
    NavItem,
    NavLink,
    UncontrolledDropdown
} from 'reactstrap';
import {Link} from 'react-router-dom';
import './NavMenu.css';
import AppSettingsModal from "../common/AppSettingsModal";
import {AppCameraAvailableContext} from "../../contexts/AppContext";

const NavMenu = () => {
    const [currentPage, setCurrentPage] = useState("");
    const [collapsed, setCollapsed] = useState(true);
    const [appSettingsModal, setAppSettingsModal] = useState(false);
    const [appCameraAvailable] = useContext(AppCameraAvailableContext)
    function toggleNavbar() {
        setCollapsed(!collapsed);
    }

    function menuItemCssClass(id) {
        if (id === currentPage) {
            return "text-dark menu-item-active"
        }

        return "text-dark"
    }
 
    return (
        <header id="site-header">
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container
                    light>
                <NavbarBrand tag={Link} to="/">
                    <img id="site-image" src={SheafDistrictCampWhite}/>
                </NavbarBrand>
                <NavbarToggler onClick={toggleNavbar} className="mr-2"/>
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                    <ul className="navbar-nav flex-grow">
                        <NavItem>
                            <NavLink tag={Link}
                                     data-testid="nav-coins-page"
                                     onClick={() => setCurrentPage("coins")}
                                     className={menuItemCssClass("coins")}
                                     to="/">
                                Coins
                            </NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link}
                                     data-testid="nav-members-page"
                                     onClick={() => setCurrentPage("members")}
                                     className={menuItemCssClass("members")}
                                     to="/members">
                                Members
                            </NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link}
                                     data-testid="nav-report-page"
                                     onClick={() => setCurrentPage("leaderboard")}
                                     className={menuItemCssClass("leaderboard")}
                                     to="/leaderboard">
                                Report
                            </NavLink>
                        </NavItem>
                        {
                            appCameraAvailable ? <NavItem>
                                <span className="text-dark nav-link text-white cursor-pointer"
                                      data-testid="nav-settings-modal"
                                      onClick={() => setAppSettingsModal(true)}>
                                    Settings</span>
                            </NavItem> : null
                        }
                    </ul>
                </Collapse>
            </Navbar>

            <AppSettingsModal 
                appSettingsModal={appSettingsModal} 
                setAppSettingsModal={setAppSettingsModal}>                
            </AppSettingsModal>
        </header>
    );
}

export default NavMenu
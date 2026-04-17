import ScoutsLogo from '../../images/fleur-de-lis-marque-white.png'

import React, {Component, useContext, useEffect, useState} from 'react';
import {
  Button, Col,
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink, Row,
} from 'reactstrap';
import {Link, useLocation} from 'react-router-dom';
import './NavMenu.css';
import AppSettingsModal from "../common/AppSettingsModal";
import {AppCameraAvailableContext, ShowStartCoinsAgainButtonContext} from "../../contexts/AppContext";
import ConfirmStartAgainModal from "../../pages/homepage/sections/ConfirmStartAgainModal";

const NavMenu = () => {
  const [showStartCoinsAgainButton, setShowStartCoinsAgainButton] = useContext(ShowStartCoinsAgainButtonContext)

  const [startCoinsAgainButtonFragment, setStartCoinsAgainButtonFragment] = useState(null);
  const [currentPage, setCurrentPage] = useState("");
  const [collapsed, setCollapsed] = useState(true);
  const [appSettingsModal, setAppSettingsModal] = useState(false);
  const [startAgainModal, setStartAgainModal] = useState(false);
  const [appCameraAvailable] = useContext(AppCameraAvailableContext)
  const [showNavBarMenu, setShowNavBarMenu] = useState(false)
  const location = useLocation();

  // const navItemWidth = "110px"

  useEffect(() => {
    if (location.pathname !== "/") {
      setShowStartCoinsAgainButton(false)
    }
    setShowNavBarMenu(false)
  }, [location]);

  function toggleNavbar() {
    setCollapsed(!collapsed);
  }

  function menuItemCssClass(id) {
    let classNames = "text-light m-3 darker-menu-bg fw-bold"
    if (id === currentPage) {
      classNames += "menu-item-active";
    }

    return classNames;
  }

  useEffect(() => {
    if (showStartCoinsAgainButton) {
      setStartCoinsAgainButtonFragment((
        <Button type="button"
                data-testid="button-start-again"
                className="btn btn-outline-light start-again-button"
                onClick={() => setStartAgainModal(true)}>
          Start again
        </Button>
      ))
    } else {
      setStartCoinsAgainButtonFragment(null)
    }
  }, [showStartCoinsAgainButton]);

  function RenderMenu() {
    return (
        <>
          <div className="darker-menu-bg">
           <NavLink tag={Link}
                    data-testid="nav-members-page"
                    onClick={() => setCurrentPage("members")}
                    className={menuItemCssClass("members")}
                    to="/members">
             Participants
           </NavLink>
          </div>
          <div className="darker-menu-bg">
            <NavLink tag={Link}
                     data-testid="nav-report-page-leaderboard-members"
                     onClick={() => setCurrentPage("leaderboard-members")}
                     className={`${menuItemCssClass("leaderboard-members")} dropdown-item`}
                     to="/leaderboard/members">
              Secondary Dashboard
            </NavLink>
          </div>
          <div className={"ms-auto darker-menu-bg"}>
              {
                appCameraAvailable
                    ? <span className="nav-link text-white cursor-pointer"
                          data-testid="nav-settings-modal"
                            style={{fontSize: "1.5em", fontWeight: "bold", marginTop: "3px", marginRight: "5px"}}
                          onClick={() => setAppSettingsModal(true)}>
                      ⛭
                    </span>
                    : null
              }
          </div>
          <div className="darker-menu-bg">
            <NavLink tag={Link}
                     data-testid="nav-coins-page"
                     onClick={() => setCurrentPage("coins")}
                     className={menuItemCssClass("coins")}
                     to="/">
              <div id={"return-to-main-screen-button"} className={"rounded-3 darker-menu-bg"}>
                Return to Main Screen
              </div>
            </NavLink>
          </div>
        </>
    )
  }

  return (
    <header id="site-header">
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white box-shadow align-middle" container light>
        <NavbarBrand onClick={() => setShowNavBarMenu(!showNavBarMenu)} style={{width:'100%', textAlign: "center"}}>
          <img role="button" id="site-image" src={ScoutsLogo} style={{objectFit: "contain", height: "60px", marginTop: "1em", marginBottom: "1em"}} />
        </NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2"/>
      </Navbar>
      <Collapse className="d-sm-inline-flex w-100 g-2 darker-menu-bg" isOpen={!collapsed} navbar>
        {showNavBarMenu
            ? RenderMenu()
            : (startCoinsAgainButtonFragment)
        }
      </Collapse>
      <AppSettingsModal
        appSettingsModal={appSettingsModal}
        setAppSettingsModal={setAppSettingsModal}>
      </AppSettingsModal>
      <ConfirmStartAgainModal
        modal={startAgainModal}
        setModal={setStartAgainModal}></ConfirmStartAgainModal>
    </header>
  );
}

export default NavMenu
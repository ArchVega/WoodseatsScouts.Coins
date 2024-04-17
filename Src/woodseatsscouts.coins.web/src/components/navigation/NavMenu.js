import SheafDistrictCampWhite from '../../images/sheaf-district-camp-white.png'
import React, {Component, useContext, useEffect, useState} from 'react';
import {
  Button,
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
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

  const navItemWidth = "110px"

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
    if (id === currentPage) {
      return "text-dark menu-item-active"
    }

    return "text-dark"
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

  return (
    <header id="site-header">
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3 align-middle" container
              light>
        <NavbarBrand onClick={() => setShowNavBarMenu(!showNavBarMenu)}>
          <img role="button" id="site-image" src={SheafDistrictCampWhite}/>
        </NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2"/>
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          {showNavBarMenu
            ? <ul className="navbar-nav flex-grow navbar-toggle-menu-list">
              <NavItem className="border-end text-center" style={{'width': navItemWidth}}>
                <NavLink tag={Link}
                         data-testid="nav-coins-page"
                         onClick={() => setCurrentPage("coins")}
                         className={menuItemCssClass("coins")}
                         to="/">
                  Coins
                </NavLink>
              </NavItem>
              <NavItem className="border-end text-center" style={{'width': navItemWidth}}>
                <NavLink tag={Link}
                         data-testid="vote-page"
                         onClick={() => setCurrentPage("vote")}
                         className={menuItemCssClass("vote")}
                         to="/vote">
                  Vote
                </NavLink>
              </NavItem>
              <NavItem className="border-end text-center" style={{'width': navItemWidth}}>
                <NavLink tag={Link}
                         data-testid="nav-members-page"
                         onClick={() => setCurrentPage("members")}
                         className={menuItemCssClass("members")}
                         to="/members">
                  Members
                </NavLink>
              </NavItem>

              <li className="nav-item dropdown">
                <a className="nav-link dropdown-toggle" href="#" id="navbarDropdown"
                   data-testid="nav-leaderboard-dropdown"
                   role="button" data-bs-toggle="dropdown" aria-expanded="false">
                  Leaderboards
                </a>
                <ul className="dropdown-menu" aria-labelledby="navbarDropdown">
                  <li>
                    <NavLink tag={Link}
                             data-testid="nav-report-page-leaderboard-groups"
                             onClick={() => setCurrentPage("leaderboard-groups")}
                             className={`${menuItemCssClass("leaderboard-groups")} dropdown-item`}
                             to="/leaderboard/groups">
                      Groups
                    </NavLink>
                  </li>
                  <li>
                    <NavLink tag={Link}
                             data-testid="nav-report-page-leaderboard-members"
                             onClick={() => setCurrentPage("leaderboard-members")}
                             className={`${menuItemCssClass("leaderboard-members")} dropdown-item`}
                             to="/leaderboard/members">
                      Members
                    </NavLink>
                  </li>
                  <li>
                    <NavLink tag={Link}
                             data-testid="nav-report-page-leaderboard-votes"
                             onClick={() => setCurrentPage("vote-results")}
                             className={`${menuItemCssClass("vote-results")} dropdown-item`}
                             to="/vote-results">
                      Votes
                    </NavLink>
                  </li>
                </ul>
              </li>

              {/*<NavItem className="border-end text-center" style={{'width': navItemWidth}}>*/}
              {/*  <NavLink tag={Link}*/}
              {/*           data-testid="nav-report-page"*/}
              {/*           onClick={() => setCurrentPage("leaderboard")}*/}
              {/*           className={menuItemCssClass("leaderboard")}*/}
              {/*           to="/leaderboard">*/}
              {/*    Leaderboards*/}
              {/*  </NavLink>*/}
              {/*</NavItem>*/}

              {/*<NavItem className="text-center" style={{'width': navItemWidth}}>*/}
              {/*  <NavLink tag={Link}*/}
              {/*           data-testid="nav-report-page"*/}
              {/*           onClick={() => setCurrentPage("vote-results")}*/}
              {/*           className={menuItemCssClass("vote-results")}*/}
              {/*           to="/vote-results">*/}
              {/*    Votes Leaderboard*/}
              {/*  </NavLink>*/}
              {/*</NavItem>*/}

              {
                appCameraAvailable
                  ? <NavItem className="border-start text-center" style={{'width': navItemWidth}}>
                    <span className="text-dark nav-link text-white cursor-pointer"
                          data-testid="nav-settings-modal"
                          onClick={() => setAppSettingsModal(true)}>
                      Settings</span>
                  </NavItem>
                  : null
              }
            </ul>
            : (startCoinsAgainButtonFragment)
          }
        </Collapse>
      </Navbar>

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
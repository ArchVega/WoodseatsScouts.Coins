import ScoutsLogo from '../../images/fleur-de-lis-marque-white.png'

import React, {useContext, useEffect, useState} from 'react';
import {
  Button, Col,
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavLink, Row,
} from 'reactstrap';
import {Link, useLocation} from 'react-router-dom';
import './NavMenu.css';
import AppSettingsModal from "../common/AppSettingsModal";
import {AppCameraAvailableContext, PageActionMenuAreaContext} from "../../contexts/AppContext";
import ConfirmStartAgainModal from "../../pages/homepage/sections/ConfirmStartAgainModal";
import SectionNames from "../../pages/homepage/sections/SectionNames";
import Uris from "../../services/Uris";

const NavMenu = () => {
  const [pageActionMenuAreaAction, setPageActionMenuAreaAction, activeScanningMember, setActiveScanningMember] = useContext(PageActionMenuAreaContext)

  const [activeScanningMemberFragment, setActiveScanningMemberFragment] = useState(null);
  const [pageActionMenuAreaFragment, setPageActionMenuAreaFragment] = useState(null);
  const [currentPage, setCurrentPage] = useState("");
  const [collapsed, setCollapsed] = useState(true);
  const [appSettingsModal, setAppSettingsModal] = useState(false);
  const [startAgainModal, setStartAgainModal] = useState(false);
  const [appCameraAvailable] = useContext(AppCameraAvailableContext)
  const [showNavBarMenu, setShowNavBarMenu] = useState(false)

  useEffect(() => {
    console.log(activeScanningMember)
  }, [activeScanningMember])

  useEffect(() => {
    if (pageActionMenuAreaAction === SectionNames.ScanMember) {
      setActiveScanningMemberFragment(null);
      setPageActionMenuAreaFragment((
        <Button type="button"
                style={{top: "18px", right: "10px", width: "230px"}}
                className="btn btn-outline-light start-again-button"
                onClick={() => window.location.reload()}>
          Not scanning? <br/>Click here to try again.
        </Button>
      ))
    } else if (pageActionMenuAreaAction === SectionNames.ScanCoins) {
      setActiveScanningMemberFragment(
        <div className="d-flex">
          <div className="flex-shrink-0 px-2">
            <img className="member-image mb-2 site-header-member-image" style={{objectFit: "cover"}}
                 alt=""
                 src={activeScanningMember && activeScanningMember.hasImage
                   ? Uris.memberPhoto(activeScanningMember.memberId)
                   : "/images/unknown-member-image.png"}/>
          </div>
          <div className="flex-fill">
            <div><span className="text-white">Hello,</span>&nbsp;<b className="text-white">{activeScanningMember.firstName}</b></div>
            <div>{activeScanningMember.memberSectionName}, {activeScanningMember.memberTroopName}</div>
          </div>
        </div>
    )
      setPageActionMenuAreaFragment(
        <Button type="button"
                style={{top: "18px", right: "34px", width: "230px"}}
                data-testid="button-start-again"
                className="btn btn-outline-light start-again-button"
                onClick={(e) => {
                  setStartAgainModal(true)
                  e.stopPropagation()
                }}>
          Not {activeScanningMember.firstName}? <br/> Click here to log out.
        </Button>
      )
    } else {
      setActiveScanningMemberFragment(null);
      setPageActionMenuAreaFragment(null)
    }
  }, [pageActionMenuAreaAction]);

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
            <div id={"return-to-main-screen-button"} className={"rounded-3 darker-menu-bg px-3 py-1"} style={{border: "1px solid white"}}>
              Return to Main Screen
            </div>
          </NavLink>
        </div>
      </>
    )
  }

  return (
    <header id="site-header">
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white box-shadow align-middle" container light style={{height: "100px"}}>
        <div className="flex-fill text-start">
            {activeScanningMemberFragment}
        </div>
        <div className="position-absolute top-50 start-50 translate-middle text-center">
          <img role="button" id="site-image" src={ScoutsLogo} style={{objectFit: "contain", height: "60px", marginTop: "1em", marginBottom: "1em", width: "100%"}} onClick={() => setShowNavBarMenu(!showNavBarMenu)} />
        </div>
        <div className="flex-fill text-end">
            {pageActionMenuAreaFragment}
        </div>
        {/*<NavbarToggler onClick={toggleNavbar} className="mr-2"/>*/}
      </Navbar>
      <div className="container-fluid w-100 darker-menu-bg" isOpen={!collapsed}>
        {showNavBarMenu
          ? <div className={"container darker-menu-bg"}>
            <div className="d-sm-inline-flex w-100 g-2 darker-menu-bg align-items-center">
              {RenderMenu()}
            </div>
          </div>
          : <></>
        }
      </div>
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
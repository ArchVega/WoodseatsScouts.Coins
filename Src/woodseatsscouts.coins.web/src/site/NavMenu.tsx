import ScoutsLogo from '../images/fleur-de-lis-marque-white.png'
import './NavMenu.scss';
import React, {useContext, useEffect, useState} from 'react';
import {NavLink, Link, useLocation, useNavigate} from 'react-router-dom';
import './NavMenu.scss';
import Uris from "../services/apis/Uris.ts";
import {AppCameraAvailableContext, PageActionMenuAreaContext} from "../contexts/AppContextExporter.tsx";
import CoinsPageViewName from "../pages/coins-page/CoinsPageViewName.ts";
import {Button, Image} from "../components/widgets/HtmlControlWrappers.tsx";
import AppSettingsModal from "../components/modals/AppSettingsModal.tsx";
import ConfirmLogoutModal from "../components/modals/ConfirmLogoutModal.tsx";

export default function NavMenu() {
  const {pageActionMenuAreaAction, activeScanningMember} = useContext(PageActionMenuAreaContext)
  const navigate = useNavigate()

  const [collapsed, setCollapsed] = useState(true);
  const [appSettingsModal, setAppSettingsModal] = useState(false);
  const [showConfirmLogoutModal, setShowConfirmLogoutModal] = useState(false);
  const {appCameraAvailable} = useContext(AppCameraAvailableContext)
  const [showNavBarMenu, setShowNavBarMenu] = useState(false)

  function RenderNavBarSubmenu() {
    const RenderNavBarSubmenuItems = () => {
      const MenuNavLink = (text: string, targetPageName: string, targetLocation: string, dataTestId: string) => {
        return (
          <NavLink
            data-testid={dataTestId}
            className={"scouts-nav-link m-3"}
            to={targetLocation}>
            {text}
          </NavLink>
        )
      }

      const RenderAppSettingsGearIcon = () => {
        if (appCameraAvailable) {
          return (
            <span id="app-settings-gear" data-testid="nav-settings-modal" onClick={() => setAppSettingsModal(true)}>
              ⛭
            </span>
          )
        }

        return null
      }

      const RenderReturnToMainScreenButton = () => {
        return (
          <div>
            <span data-testid="nav-coins-page" onClick={() => navigate("/")} className={"scouts-nav-link m-3"}>
              <div className={"px-3 py-1 scouts-borders-white"}>
                Return to Main Screen
              </div>
            </span>
          </div>
        )
      }

      return (
        <>
          {MenuNavLink("Participants", "members", "/members", "nav-members-page")}
          {MenuNavLink("Secondary Dashboard", "leaderboard-members", "/members/latest-scans", "nav-report-page-leaderboard-members")}
          <div className={"ms-auto"}>
            {RenderAppSettingsGearIcon()}
          </div>
          {RenderReturnToMainScreenButton()}
        </>
      )
    }

    if (!showNavBarMenu) {
      return null
    }

    return (
      <div className="container-fluid scouts-navbar-sub-menu">
        <div className="container">
          <div className="d-sm-inline-flex w-100 align-items-center">
            {RenderNavBarSubmenuItems()}
          </div>
        </div>
      </div>
    )
  }

  function RenderLeftSideHeaderSection() {
    switch (pageActionMenuAreaAction) {
      case CoinsPageViewName.ScanCoins:
        return (
          <div className="d-flex h-100 p-2">
            <Image className="member-image h-100 w-auto me-3"
                   src={activeScanningMember && activeScanningMember.hasImage ? Uris.memberPhoto(activeScanningMember.memberId) : "/images/unknown-member-image.png"}></Image>
            <div id="member-details" className="flex-fill">
              <div><span>Hello,</span>&nbsp;<b className="text-white">{activeScanningMember.firstName}</b></div>
              <div>{activeScanningMember.memberSectionName}, {activeScanningMember.memberScoutGroupName}</div>
            </div>
          </div>
        )
      default:
        return null
    }
  }

  function RenderRightSideHeaderSection() {
    switch (pageActionMenuAreaAction) {
      case CoinsPageViewName.ScanMember:
        return (
          <Button className={"btn-outline-light text-black bg-white"} onClick={() => window.location.reload()}>
            Not scanning? <br/>Click here and try again.
          </Button>
        )
      case CoinsPageViewName.ScanCoins:
        return (
          <button type="button"
                  data-testid="button-start-again"
                  className="btn btn-outline-light"
                  onClick={(e) => {
                    setShowConfirmLogoutModal(true)
                    e.stopPropagation()
                  }}>
            Not {activeScanningMember.firstName}? <br/> Click here to log out.
          </button>
        )
      default:
        return null
    }
  }

  return (
    <header className={"scouts-navbar"}>
      <div className="container" style={{height: "100px"}}>
        <div id="left-column">
          {RenderLeftSideHeaderSection()}
        </div>
        <div id="middle-column">
          <Image role="button" src={ScoutsLogo} style={{height: "60px", width: "100%"}} onClick={() => setShowNavBarMenu(!showNavBarMenu)}/>
        </div>
        <div id="right-column">
          {RenderRightSideHeaderSection()}
        </div>
      </div>
      {RenderNavBarSubmenu()}
      <AppSettingsModal appSettingsModal={appSettingsModal} setAppSettingsModal={setAppSettingsModal}></AppSettingsModal>
      <ConfirmLogoutModal showConfirmLogoutModal={showConfirmLogoutModal} setShowConfirmLogoutModal={setShowConfirmLogoutModal}></ConfirmLogoutModal>
    </header>
  );
}
import ScoutsLogo from '../../images/fleur-de-lis-marque-white.png'
import './NavMenu.scss';
import React, {useContext, useEffect, useState} from 'react';
import {NavLink, Link, useLocation} from 'react-router-dom';
import './NavMenu.scss';
import Uris from "../../services/Uris";
import {AppCameraAvailableContext, PageActionMenuAreaContext} from "../../contexts/AppContextExporter.tsx";
import SectionNames from "../../pages/home-page/sections/SectionNames.ts";
import {Button, Image} from "../common/HtmlControlWrappers.tsx";
import AppSettingsModal from "../modals/AppSettingsModal.tsx";

export default function NavMenu() {
  const {pageActionMenuAreaAction, setPageActionMenuAreaAction, activeScanningMember, setActiveScanningMember} = useContext(PageActionMenuAreaContext)

  const [activeScanningMemberFragment, setActiveScanningMemberFragment] = useState(null);
  const [pageActionMenuAreaFragment, setPageActionMenuAreaFragment] = useState(null);
  const [currentPage, setCurrentPage] = useState("");
  const [collapsed, setCollapsed] = useState(true);
  const [appSettingsModal, setAppSettingsModal] = useState(false);
  const [startAgainModal, setStartAgainModal] = useState(false);
  const {appCameraAvailable} = useContext(AppCameraAvailableContext)
  const [showNavBarMenu, setShowNavBarMenu] = useState(false)

  function CurrentCoinsPageViewSwitchedToScanMember() {
    setActiveScanningMemberFragment(null);
    setPageActionMenuAreaFragment((
      <Button className={"btn-outline-light text-black bg-white"} style={{width: "230px"}} onClick={() => window.location.reload()}>
        Not scanning? <br/>Click here and try again.
      </Button>
    ))
  }

  function CurrentCoinsPageViewSwitchedToScanCoins() {
    // setActiveScanningMemberFragment(
    //   <div className="d-flex">
    //     <div className="flex-shrink-0 px-2">
    //       <Image className="member-image mb-2"
    //              style={{width: "100px", height: "100px"}}
    //              src={activeScanningMember && activeScanningMember.hasImage ? Uris.memberPhoto(activeScanningMember.memberId) : "/images/unknown-member-image.png"}></Image>
    //     </div>
    //     <div className="flex-fill">
    //       <div><span className="text-white">Hello,</span>&nbsp;<b className="text-white">{activeScanningMember.firstName}</b></div>
    //       <div>{activeScanningMember.memberSectionName}, {activeScanningMember.memberTroopName}</div>
    //     </div>
    //   </div>
    // )
    // setPageActionMenuAreaFragment(
    // <button type="button"
    //         style={{top: "18px", right: "34px", width: "230px"}}
    //         data-testid="button-start-again"
    //         className="btn btn-outline-light"
    //         onClick={(e) => {
    //           setStartAgainModal(true)
    //           e.stopPropagation()
    //         }}>
    //   Not {activeScanningMember.firstName}? <br/> Click here to log out.
    // </button>
    // )
  }

  function CurrentCoinsPageViewSwitchedToHaulSummary() {
    setActiveScanningMemberFragment(null);
    setPageActionMenuAreaFragment(null)
  }

  useEffect(() => {
    if (pageActionMenuAreaAction === SectionNames.ScanMember) {
      CurrentCoinsPageViewSwitchedToScanMember();
    } else if (pageActionMenuAreaAction === SectionNames.ScanCoins) {
      CurrentCoinsPageViewSwitchedToScanCoins();
    } else {
      CurrentCoinsPageViewSwitchedToHaulSummary();
    }
  }, [pageActionMenuAreaAction]);

  function RenderNavBarSubmenu() {
    if (showNavBarMenu) {
      return (
        <div className="container-fluid scouts-nav-bar-sub-menu">
          <div className="container">
            <div className="d-sm-inline-flex w-100 align-items-center">
              {RenderMenuItems()}
            </div>
          </div>
        </div>
      )
    }

    return null
  }

  function MenuNavLink(text: string, targetPageName: string, targetLocation: string, dataTestId: string) {
    return (
      <NavLink
        data-testid={dataTestId}
        onClick={() => setCurrentPage(targetPageName)}
        className={"scouts-nav-link m-3"}
        to={targetLocation}>
        {text}
      </NavLink>
    )
  }

  function RenderAppSettingsGearIcon() {
    if (appCameraAvailable) {
      return (
        <span id="app-settings-gear" data-testid="nav-settings-modal" onClick={() => setAppSettingsModal(true)}>
          ⛭
        </span>
      )
    }

    return null
  }

  function RenderReturnToMainScreenButton() {
    return (
      <div>
        <NavLink data-testid="nav-coins-page" onClick={() => setCurrentPage("coins")} className={"scouts-nav-link m-3"} to="/">
          <div className={"px-3 py-1 scouts-borders-white"}>
            Return to Main Screen
          </div>
        </NavLink>
      </div>
    )
  }

  function RenderMenuItems() {
    return (
      <>
        {MenuNavLink("Participants", "members", "/members", "nav-members-page")}
        {MenuNavLink("Secondary Dashboard", "leaderboard-members", "/leaderboard/members", "nav-report-page-leaderboard-members")}
        <div className={"ms-auto"}>
          {RenderAppSettingsGearIcon()}
        </div>
        {RenderReturnToMainScreenButton()}
      </>
    )
  }

  return (
    <header className={"scouts-nav-bar"}>
      <div className="container navbar navbar-expand-sm navbar-toggleable-sm ng-white box-shadow align-middle" style={{height: "100px"}}>
        <div className="flex-fill text-start">
          {pageActionMenuAreaAction === SectionNames.ScanCoins && (
            <>
              <div className="d-flex">
                <div className="flex-shrink-0 px-2">
                  <Image className="member-image mb-2"
                         style={{width: "100px", height: "100px"}}
                         src={activeScanningMember && activeScanningMember.hasImage ? Uris.memberPhoto(activeScanningMember.memberId) : "/images/unknown-member-image.png"}></Image>
                </div>
                <div className="flex-fill">
                  <div><span className="text-white">Hello,</span>&nbsp;<b className="text-white">{activeScanningMember.firstName}</b></div>
                  <div>{activeScanningMember.memberSectionName}, {activeScanningMember.memberTroopName}</div>
                </div>
              </div>
            </>
          )}
          {/*{activeScanningMemberFragment}*/}
        </div>
        <div className="position-absolute top-50 start-50 translate-middle text-center">
          <Image role="button" src={ScoutsLogo} style={{height: "60px", width: "100%"}} onClick={() => setShowNavBarMenu(!showNavBarMenu)}/>
        </div>
        <div className="flex-fill text-end">
          {pageActionMenuAreaAction === SectionNames.ScanCoins && (
            <>
              <button type="button"
                      style={{top: "18px", right: "34px", width: "230px"}}
                      data-testid="button-start-again"
                      className="btn btn-outline-light"
                      onClick={(e) => {
                        setStartAgainModal(true)
                        e.stopPropagation()
                      }}>
                Not {activeScanningMember.firstName}? <br/> Click here to log out.
              </button>
            </>
          )}
          {/*{pageActionMenuAreaFragment}*/}
        </div>
      </div>
      {RenderNavBarSubmenu()}
      <AppSettingsModal appSettingsModal={appSettingsModal} setAppSettingsModal={setAppSettingsModal}></AppSettingsModal>
      {/*<ConfirmStartAgainModal*/}
      {/*  modal={startAgainModal}*/}
      {/*  setModal={setStartAgainModal}></ConfirmStartAgainModal>*/}
    </header>
  );
}
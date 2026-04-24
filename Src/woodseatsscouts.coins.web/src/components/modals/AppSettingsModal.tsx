import {useContext, useEffect, useState} from "react";
import {AppCameraAvailableContext, UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import AppStateApiService from "../../services/apis/AppStateApiService.tsx";
import {BaseModal} from "./BaseModal.tsx";
import {Switch} from "../widgets/HtmlControlWrappers.tsx";
import AppLocalStorage from "../storage/AppLocalStorage.ts";

export default function AppSettingsModal({appSettingsModal, setAppSettingsModal}) {
  const {useAppCamera, setUseAppCamera} = useContext(UseAppCameraContext)
  const {appCameraAvailable} = useContext(AppCameraAvailableContext)
  const [appVersion, setAppVersion] = useState("")

  useEffect(() => {
    AppStateApiService().getAppVersion(response => {
      setAppVersion(response)
    })
  }, [])

  useEffect(() => {
    AppLocalStorage().setUseAppCamera(useAppCamera)
  }, [useAppCamera])

  return (
    <BaseModal id={"app-settings-modal"} title={"Application Settings"} show={appSettingsModal} onClose={() => {
      setAppSettingsModal(false)
    }}>
      <div className="row mb-3">
        <div className="col">
          {appCameraAvailable
            ? (
              <form>
                <label>Use camera instead of usb scanner</label>
                <Switch id={""} checked={useAppCamera} onChange={() => setUseAppCamera(!useAppCamera)}></Switch>
              </form>
            )
            : null}
        </div>
      </div>
      <div className="row">
        <div className="col text-end">
          <small><em>Version:&nbsp;</em></small>
          <small><em>{appVersion}</em></small>
        </div>
      </div>
    </BaseModal>
  )
}
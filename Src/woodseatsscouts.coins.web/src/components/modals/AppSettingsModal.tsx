import {useContext, useEffect, useState} from "react";
import {AppCameraAvailableContext, UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import AppStateApiService from "../../services/AppStateApiService.tsx";
import {BaseModal} from "./BaseModal.tsx";
import {Switch} from "../common/HtmlControlWrappers.tsx";

const AppSettingsModal = ({appSettingsModal, setAppSettingsModal}) => {
  const {useAppCamera, setUseAppCamera} = useContext(UseAppCameraContext)
  const {appCameraAvailable} = useContext(AppCameraAvailableContext)
  const [appVersion, setAppVersion] = useState("")

  useEffect(() => {
    const useCameraSetting = localStorage.getItem("woodseatsscouts.preferences.use-camera");
    const useCamera = useCameraSetting != null && useCameraSetting === "true"
    setUseAppCamera(useCamera)

    AppStateApiService().getAppVersion(response => {
      setAppVersion(response)
    })
  }, [])

  useEffect(() => {
    localStorage.setItem("woodseatsscouts.preferences.use-camera", String(useAppCamera));
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

export default AppSettingsModal;
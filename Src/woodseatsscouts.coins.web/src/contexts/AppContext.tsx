import {useEffect, useState} from "react";
import {AppCameraAvailableContext, AppModeContext, AppTestModeContext, PageActionMenuAreaContext, UseAppCameraContext} from "./AppContextExporter";
import AppStateApiService from "../services/AppStateApiService.tsx";
import AppLocalStorage from "../components/local-storage/AppLocalStorage.ts";

const cameraAvailable = 'mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices;

const AppContext = ({children}) => {
  const [useAppCamera, setUseAppCamera] = useState(() => AppLocalStorage().getUseAppCamera());
  const [appCameraAvailable] = useState(cameraAvailable);
  const [appTestMode, setAppTestMode] = useState(true);
  const [appMode, setAppMode] = useState("");
  const [activeScanningMember, setActiveScanningMember] = useState(null);
  const [pageActionMenuAreaAction, setPageActionMenuAreaAction] = useState("");

  useEffect(() => {
    AppStateApiService().getAppSate(response => {
      setAppMode(response)
    })
  }, [appMode])

  return (
    <PageActionMenuAreaContext.Provider value={{pageActionMenuAreaAction, setPageActionMenuAreaAction, activeScanningMember, setActiveScanningMember}}>
      <AppCameraAvailableContext.Provider value={{appCameraAvailable}}>
        <UseAppCameraContext.Provider value={{useAppCamera, setUseAppCamera}}>
          <AppModeContext.Provider value={{appMode, setAppMode}}>
            <AppTestModeContext.Provider value={{appTestMode, setAppTestMode}}>
              {children}
            </AppTestModeContext.Provider>
          </AppModeContext.Provider>
        </UseAppCameraContext.Provider>
      </AppCameraAvailableContext.Provider>
    </PageActionMenuAreaContext.Provider>
  )
}
export default AppContext
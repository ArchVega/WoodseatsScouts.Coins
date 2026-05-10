import {useEffect, useState} from "react";
import type {AppSettings} from "@/AppSettings.ts";
import AppLocalStorage from "@/components/storage/AppLocalStorage.ts";
import {
  AppCameraAvailableContext,
  AppModeContext,
  type AppModeContextTypeEnum,
  AppSettingsContext,
  AppTestModeContext,
  PageActionMenuAreaContext,
  UseAppCameraContext
} from "@/contexts/AppContextExporter.tsx";
import AppStateApiService from "@/services/apis/AppStateApiService.tsx";


const cameraAvailable = 'mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices;

const AppContext = ({children}) => {
  const [appSettings] = useState<AppSettings>(() => AppLocalStorage().getAppSettings());
  const [useAppCamera, setUseAppCamera] = useState(() => AppLocalStorage().getUseAppCamera());
  const [appCameraAvailable] = useState(cameraAvailable);
  const [isAppTestMode, setIsAppTestMode] = useState<boolean>(true);
  const [appMode, setAppMode] = useState<AppModeContextTypeEnum>("Production");
  const [activeScanningMember, setActiveScanningMember] = useState(null);
  const [pageActionMenuAreaAction, setPageActionMenuAreaAction] = useState("");

  useEffect(() => {
    AppStateApiService().getAppSate(response => {
      setAppMode(response)
    })
  }, [])

  return (
    <AppSettingsContext value={{appSettings}}>
      <PageActionMenuAreaContext.Provider value={{pageActionMenuAreaAction, setPageActionMenuAreaAction, activeScanningMember, setActiveScanningMember}}>
        <AppCameraAvailableContext.Provider value={{appCameraAvailable}}>
          <UseAppCameraContext.Provider value={{useAppCamera, setUseAppCamera}}>
            <AppModeContext.Provider value={{appMode, isAppTestMode}}>
              <AppTestModeContext.Provider value={{appTestMode: isAppTestMode, setAppTestMode: setIsAppTestMode}}>
                {children}
              </AppTestModeContext.Provider>
            </AppModeContext.Provider>
          </UseAppCameraContext.Provider>
        </AppCameraAvailableContext.Provider>
      </PageActionMenuAreaContext.Provider>
    </AppSettingsContext>
  )
}
export default AppContext
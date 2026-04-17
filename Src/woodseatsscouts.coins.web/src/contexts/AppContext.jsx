import {createContext, useEffect, useState} from "react";
import AppStateApiService from "../services/AppStateApiService";

const cameraAvailable = 'mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices;

export const AppCameraAvailableContext = createContext(cameraAvailable);
export const UseAppCameraContext = createContext(cameraAvailable);
export const AppTestModeContext = createContext(false); // Todo: if AppTestModeContext is still needed, refactor into an env variable to remove the React Context
export const AppModeContext = createContext("");
export const PageActionMenuAreaContext = createContext(false);

export const AppContext = ({children}) => {
  const [useAppCamera, setUseAppCamera] = useState(cameraAvailable);
  const [appCameraAvailable, setAppCameraAvailable] = useState(cameraAvailable);
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
    <PageActionMenuAreaContext.Provider value={[pageActionMenuAreaAction, setPageActionMenuAreaAction, activeScanningMember, setActiveScanningMember]}>
      <AppCameraAvailableContext.Provider value={[appCameraAvailable, setAppCameraAvailable]}>
        <UseAppCameraContext.Provider value={[useAppCamera, setUseAppCamera]}>
          <AppModeContext.Provider value={[appMode, setAppMode]}>
            <AppTestModeContext.Provider value={[appTestMode, setAppTestMode]}>
              {children}
            </AppTestModeContext.Provider>
          </AppModeContext.Provider>
        </UseAppCameraContext.Provider>
      </AppCameraAvailableContext.Provider>
    </PageActionMenuAreaContext.Provider>
  )
}
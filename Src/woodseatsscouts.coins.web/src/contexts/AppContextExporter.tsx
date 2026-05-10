import * as React from "react";
import type {ScoutMemberDto} from "@/types/ServerTypes.ts";
import type {AppSettings} from "@/AppSettings.ts";

const cameraAvailable = 'mediaDevices' in navigator && 'getUserMedia' in navigator.mediaDevices;

type AppCameraAvailableContextType = {
    appCameraAvailable: boolean;
};

type UseAppCameraContextType = {
    useAppCamera: boolean;
    setUseAppCamera: React.Dispatch<React.SetStateAction<boolean>>;
};

type AppTestModeContextType = {
    appTestMode: boolean;
    setAppTestMode: React.Dispatch<React.SetStateAction<boolean>>;
};

export type AppModeContextTypeEnum = 'Development' | 'AcceptanceTest' | 'Production';

type AppModeContextType = {
    appMode: AppModeContextTypeEnum
    isAppTestMode: boolean;
}

type PageActionMenuAreaContextType = {
    pageActionMenuAreaAction: string
    setPageActionMenuAreaAction: React.Dispatch<React.SetStateAction<string>>;
    activeScanningMember: ScoutMemberDto;
    setActiveScanningMember: React.Dispatch<React.SetStateAction<any>>;
}

type AppSettingsContextType = {
    appSettings: AppSettings;
}

export const AppCameraAvailableContext = React.createContext<AppCameraAvailableContextType>({appCameraAvailable: cameraAvailable});

export const UseAppCameraContext = React.createContext<UseAppCameraContextType | undefined>(undefined);

// Todo: if AppTestModeContext is still needed, refactor into an env variable to remove the React Context
export const AppTestModeContext = React.createContext<AppTestModeContextType | undefined>(undefined);

export const AppModeContext = React.createContext<AppModeContextType | undefined>(undefined);

export const PageActionMenuAreaContext = React.createContext<PageActionMenuAreaContextType | undefined>(undefined);

export const AppSettingsContext = React.createContext<AppSettingsContextType | undefined>(undefined)
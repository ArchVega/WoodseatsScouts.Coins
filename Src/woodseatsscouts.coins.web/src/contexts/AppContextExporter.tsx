import {createContext} from "react";
import * as React from "react";
import type {AppSettings} from "../AppSettings.ts";
import type {MemberDto} from "../types/ServerTypes.ts";

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

type AppModeContextType = {
    appMode: string;
    setAppMode: React.Dispatch<React.SetStateAction<string>>;
}

type PageActionMenuAreaContextType = {
    pageActionMenuAreaAction: string
    setPageActionMenuAreaAction: React.Dispatch<React.SetStateAction<string>>;
    activeScanningMember: MemberDto;
    setActiveScanningMember: React.Dispatch<React.SetStateAction<any>>; // todo: rename to member
}

type AppSettingsContextType = {
    appSettings: AppSettings;
}

export const AppCameraAvailableContext = createContext<AppCameraAvailableContextType>({appCameraAvailable: cameraAvailable});

export const UseAppCameraContext = createContext<UseAppCameraContextType | undefined>(undefined);

// Todo: if AppTestModeContext is still needed, refactor into an env variable to remove the React Context
export const AppTestModeContext = createContext<AppTestModeContextType | undefined>(undefined);

export const AppModeContext = createContext<AppModeContextType | undefined>(undefined);

export const PageActionMenuAreaContext = createContext<PageActionMenuAreaContextType | undefined>(undefined);

export const AppSettingsContext = createContext<AppSettingsContextType | undefined>(undefined)
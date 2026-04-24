import getAppSettings, {type AppSettings} from "../../AppSettings.ts";

export default function AppLocalStorage() {
  const keys = {
    useAppCamera: "woodseatsscouts.preferences.use-camera",
    appSettings: "woodseatsscouts.app-settings"
  }

  return {
    getUseAppCamera: () => {
      return localStorage.getItem(keys.useAppCamera) === "true"
    },
    setUseAppCamera: (use: boolean) => {
      localStorage.setItem(keys.useAppCamera, String(use));
    },
    getAppSettings: (): AppSettings => {
      let stored = localStorage.getItem(keys.appSettings);
      if (!stored) {
        stored = JSON.stringify(getAppSettings());
        localStorage.setItem(keys.appSettings, stored);
      }

      return JSON.parse(stored);
    }
  }
}
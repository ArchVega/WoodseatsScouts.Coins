export type AppSettings = {
  VITE_WEB_API_URI: string
  VITE_SCAN_COINS_REDIRECT_DELAY_SECONDS: number
  VITE_RECENT_SCANS_REFRESH_INTERVAL_SECONDS: number
}

export default function getAppSettings(): AppSettings {
  function tryGet(key: string) {
    if (!import.meta.env[key]) {
      throw `.env file does not contain key and/or value for ${key}`
    }

    return import.meta.env[key];
  }

  const appSettings = {
    VITE_WEB_API_URI: tryGet("VITE_WEB_API_URI"),
    VITE_SCAN_COINS_REDIRECT_DELAY_SECONDS: tryGet("VITE_SCAN_COINS_REDIRECT_DELAY_SECONDS"),
    VITE_RECENT_SCANS_REFRESH_INTERVAL_SECONDS: tryGet("VITE_RECENT_SCANS_REFRESH_INTERVAL_SECONDS")
  }

  console.log("AppSettings loaded", appSettings);

  return appSettings;
}
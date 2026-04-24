export type AppSettings = {
  VITE_WEB_API_URI: string
  VITE_SCAN_COINS_COMPLETED_REDIRECT_AFTER_MS: number
}

export default function getAppSettings(): AppSettings {
  function tryGet(key: string) {
    if (!import.meta.env[key]) {
      throw `.env file does not contain key and/or value for ${key}`
    }

    return import.meta.env[key];
  }

  return {
    VITE_WEB_API_URI: tryGet("VITE_WEB_API_URI"),
    VITE_SCAN_COINS_COMPLETED_REDIRECT_AFTER_MS: tryGet("VITE_SCAN_COINS_COMPLETED_REDIRECT_AFTER_MS")
  }
}
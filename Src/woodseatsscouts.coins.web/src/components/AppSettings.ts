export type AppSettings = {
  VITE_WEB_API_URI: string
  VITE_SCAVENGER_HAUL_COMPLETED_REFRESH_TIMEOUT: number
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
    VITE_SCAVENGER_HAUL_COMPLETED_REFRESH_TIMEOUT: tryGet("VITE_SCAVENGER_HAUL_COMPLETED_REFRESH_TIMEOUT")
  }
}
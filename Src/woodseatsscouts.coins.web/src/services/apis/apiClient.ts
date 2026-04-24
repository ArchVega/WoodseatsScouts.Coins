import getAppSettings from "../../AppSettings.ts";
import axios from "axios";
import {logDebug, logError} from "../../components/logging/Logger.ts";

const baseUri: string = getAppSettings().VITE_WEB_API_URI + "/api"

export const apiClient = axios.create({
  baseURL: baseUri
})

apiClient.interceptors.request.use((config) => {
  logDebug("apiClient request started", config);
  return config;
});

apiClient.interceptors.response.use(
  (response) => {
    logDebug("apiClient response received", response);

    const data = response.data;

    if (data?.computedImagePath) {
      data.clientComputedImageUri = response.config.baseURL + "/" + data.computedImagePath;
    } else if (Array.isArray(data)) {
      data.forEach((item) => {
        if (item.computedImagePath) {
          item.clientComputedImageUri = response.config.baseURL + "/" + item.computedImagePath;
        }
      })
    }

    return response;
  },
  (error) => {
    logError("apiClient request failed", error);

    return Promise.reject(error);
  }
);
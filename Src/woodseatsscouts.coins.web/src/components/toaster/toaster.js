import {toast} from "react-toastify";
import {logDebug} from "../logging/Logger";

function toastError(axiosReason) {
  if (typeof (axiosReason) === "string") {
    toast(axiosReason, {
      position: 'top-center'
    })
  } else {
    toast(axiosReason.response.data, {
      position: 'top-center'
    })
  }
}

export {toastError};
import {toast} from "react-toastify";
import {logDebug} from "../logging/Logger";

function toastError(axiosReason) {
    toast(axiosReason.response.data)
}

export {toastError};
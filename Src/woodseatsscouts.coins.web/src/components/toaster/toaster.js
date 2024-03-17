import {toast} from "react-toastify";
import {logDebug} from "../logging/Logger";

function toastError(axiosReason) {
    if (typeof(axiosReason) === "string") {
        toast(axiosReason)
    } else {
        toast(axiosReason.response.data)
    }
}

export {toastError};
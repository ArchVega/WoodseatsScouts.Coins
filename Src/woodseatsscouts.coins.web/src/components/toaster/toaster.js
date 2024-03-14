import {toast} from "react-toastify";

function toastError(axiosReason) {
    toast(axiosReason.response.data)
}

export {toastError};
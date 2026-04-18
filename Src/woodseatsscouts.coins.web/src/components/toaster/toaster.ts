import {toast} from "react-toastify";

function toastError(axiosReason) {
  if (typeof (axiosReason) === "string") {
    toast(axiosReason, {
      position: 'top-center'
    })
  } else if (axiosReason.response && axiosReason.response.data) {
    toast(axiosReason.response.data, {
      position: 'top-center'
    })
  } else {
    toast(axiosReason, {
      position: 'top-center'
    })
  }
}

export {toastError};
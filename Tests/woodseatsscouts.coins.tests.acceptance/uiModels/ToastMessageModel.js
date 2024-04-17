const ToastMessageModel = (page) => {
    return {
        getMessage: async () => {
            const toastMessageLocator = page.locator(".Toastify__toast-body > div:nth-child(1)")
            return await toastMessageLocator.textContent();
        }
    }
}

export default ToastMessageModel
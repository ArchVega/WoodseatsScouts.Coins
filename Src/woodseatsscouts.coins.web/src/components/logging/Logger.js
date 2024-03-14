function logAttention(message, ...optionalParams) {
    if (message !== undefined) {
        console.log(`!!!${message.toString().toUpperCase()}!!!`, ...optionalParams)
    } else {
        console.log(`!!!message is undefined!!!`, ...optionalParams)
    }
}

function logGeneral(message, ...optionalParams) {
    console.log(message, ...optionalParams)
}

function logApi(message, ...optionalParams) {
    console.log("☎", message, ...optionalParams)
}

function logReactSet(message, ...optionalParams) {
    console.log("🫳", message, ...optionalParams)
}

function logReactUseEffect(message, ...optionalParams) {
    console.log(`%c🌠 ${message}`, 'color: #67a5ff', ...optionalParams, );
}

function logInfo(message, ...optionalParams) {
    console.log(message, ...optionalParams)
}

function logDebug(message, ...optionalParams) {
    console.log(message, ...optionalParams)
}

function logWarning(message, ...optionalParams) {
    console.log(message, ...optionalParams)
}

function logError(message, ...optionalParams) {
    console.log(`%c🌠 ${message}`, 'color: #d70000', ...optionalParams);
}

export { logAttention, logGeneral, logApi, logInfo, logReactSet, logReactUseEffect, logDebug, logWarning, logError }
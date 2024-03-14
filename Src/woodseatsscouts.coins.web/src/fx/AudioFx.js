function AudioFx() {
    return {
        playMemberScannedAudio: function () {
            new Audio("sounds/ScanWristband.mp3").play();
        },

        playCoinScannedSuccessAudio: function () {
            new Audio("sounds/ScanCoin_Success.mp3").play();
        },

        playCoinScannedErrorAudio: function () {
            new Audio("sounds/ScanCoin_Error.mp3").play();
        },

        playHaulCompleteAudio: function() {
            new Audio("sounds/HaulComplete.mp3").play();
        }
    }
}

export default AudioFx
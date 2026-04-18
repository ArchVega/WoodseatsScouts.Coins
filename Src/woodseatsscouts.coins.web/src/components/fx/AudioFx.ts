function AudioFx() {
    /* Audio instances must be constructed once if they are to be played multiple times (eg several coins being scanned in succession). Calling
    * new Audio() inside the functions results in performance bugs where some successive audio plays are missed entirely and causes tests to fail
    * (not sure if this failure would happen in the real world). */
    const _playMemberScannedAudio = new Audio("sounds/ScanWristband.mp3")
    const _playCoinScannedSuccessAudio = new Audio("sounds/ScanCoin_Success.mp3")
    const _playCoinScannedErrorAudio = new Audio("sounds/ScanCoin_Error.mp3")
    const _playHaulCompleteAudio = new Audio("sounds/HaulComplete.mp3")
    const _playVoteSuccessAudio = new Audio("sounds/Vote_Success.mp3")

    return {
        playMemberScannedAudio: function () {
            _playMemberScannedAudio.play();
        },

        playCoinScannedSuccessAudio: function () {
            _playCoinScannedSuccessAudio.play();
        },

        playCoinScannedErrorAudio: function () {
            _playCoinScannedErrorAudio.play();
        },

        playHaulCompleteAudio: function() {
            _playHaulCompleteAudio.play();
        },

        playVoteSuccessAudio: function() {
            _playVoteSuccessAudio.play();
        }
    }
}

export default AudioFx
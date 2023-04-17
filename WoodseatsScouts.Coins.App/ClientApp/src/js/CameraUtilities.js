const hasCamera = (hasCamera) => {
    navigator.getMedia = ( navigator.getUserMedia || // use the proper vendor prefix
        navigator.webkitGetUserMedia ||
        navigator.mozGetUserMedia ||
        navigator.msGetUserMedia);
    // navigator.mediaDevices.getUserMedia
    
    navigator.getMedia({video: true}, function() {
        hasCamera(true)
    }, function() {
        hasCamera(false)
    });
}

export default hasCamera;
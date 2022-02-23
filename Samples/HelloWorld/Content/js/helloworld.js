// Initialize an OpenTok Session object
var session = OT.initSession(apiKey, sessionId);

// Initialize a Publisher, and place it into the element with id="publisher"
var publisher = OT.initPublisher('publisher', publisherOptions, _ => {
    loadDevices();
});


// Attach event handlers
session.on({

    // This function runs when session.connect() asynchronously completes
    sessionConnected: function (event) {
        // Publish the publisher we initialzed earlier (this will trigger 'streamCreated' on other
        // clients)
        session.publish(publisher, function (error) {
            if (error) {
                console.error('Failed to publish', error);
            }
        });
    },

    // This function runs when another client publishes a stream (eg. session.publish())
    streamCreated: function (event) {
        // Create a container for a new Subscriber, assign it an id using the streamId, put it inside
        // the element with id="subscribers"
        var subContainer = document.createElement('div');
        subContainer.id = 'stream-' + event.stream.streamId;
        document.getElementById('subscribers').appendChild(subContainer);

        // Subscribe to the stream that caused this event, put it inside the container we just made
        session.subscribe(event.stream, subContainer, function (error) {
            if (error) {
                console.error('Failed to subscribe', error);
            }
        });
    }

});

// Connect to the Session using the 'apiKey' of the application and a 'token' for permission
session.connect(token, function (error) {
    if (error) {
        console.error('Failed to connect', error);
    }
});

function loadDevices() {

    var cameras = document.getElementById('cameras');
    cameras.addEventListener('change', function (event) {
        publisher.setVideoSource(event.target.value);
    });

    var mics = document.getElementById('mics');
    mics.addEventListener('change', function (event) {
        publisher.setAudioSource(event.target.value);
    });

    var videoSource = publisher.getVideoSource();
    var audioSource = publisher.getAudioSource();

    OT.getDevices((err, devices) => {

        if (err) {
            console.error(err);
        }

        var audioInputs = devices.filter((device) => device.kind === 'audioInput');
        var videoInputs = devices.filter((device) => device.kind === 'videoInput');

        videoInputs.forEach((device, idx) => {
            var option = document.createElement("option");
            var txt = document.createTextNode(device.label);
            option.appendChild(txt);
            option.setAttribute("value", device.deviceId);
            
            if (videoSource != null && device.deviceId === videoSource.deviceId) {
                option.selected = true;
            }
            cameras.insertBefore(option, cameras.lastChild);
        });

        audioInputs.forEach((device, idx) => {
            var option = document.createElement("option");
            var txt = document.createTextNode(device.label);
            option.appendChild(txt);
            option.setAttribute("value", device.deviceId);
            
            if (audioSource != null && device.deviceId === audioSource.deviceId) {
                option.selected = true;
            }
            mics.insertBefore(option, mics.lastChild);
        });
    });
}
var session, publisher;

window.initializeStream = (apiKey, sessionId, token) => {
    session = OT.initSession(apiKey, sessionId)
    publisher = OT.initPublisher('publisher')
    session.connect(token, function (error) {
        if (error) {
            console.error('Failed to connect', error);
        } else {
            console.log('Session connected.')
            session.publish(publisher, function (error) {
                if (error) {
                    console.error('Failed to publish', error);
                }
            });
        }
    });

    session.on('streamCreated', function (event) {
        console.log('Stream created.')
        session.subscribe(event.stream, 'subscribers', {
            insertMode: 'append'
        }, function (error) {
            if (error) {
                console.error('Failed to subscribe', error);
            }
        });
    });

};

window.disposeStream = () => {
    publisher.publishVideo(false);
    session.disconnect();
    session.unpublish(publisher);
    publisher.destroy();
};

window.onbeforeunload = function () {
    disposeStream();
}

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
            alert("Copied to clipboard!");
        })
            .catch(function (error) {
                alert(error);
            });
    }
};
window.initializeSession = (apiKey, sessionId, token) => {
    var session = OT.initSession(apiKey, sessionId),
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

    window.onbeforeunload = function () {
        publisher.publishVideo(false);
        session.unpublish(publisher);
        publisher.destroy();
    }

};

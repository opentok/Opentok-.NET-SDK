OpenTok 2.0 Archiving JavaScript API
====================================

The OpenTok 2.0 archiving feature lets you record OpenTok 2.0 sessions.

The OpenTok 2.0 archiving feature requires that you use the OpenTok 2.2 JavaScript library.
See the important notes in [OpenTok 2.2 Archiving JavaScript changes](#changes-2.2).

The OpenTok JavaScript API includes events that are dispatched when the archive recording starts and when the archive recording stops:

* [archiveStarted](#archiveStarted)
* [archiveStopped](#archiveStopped)

While an archive is being recorded, each OpenTok publisher displays a recording notification indicator.

Use the [OpenTok 2.0 Archiving REST API](REST-API.md) to start, stop, and manage archives.


<a name="archiveStarted"></a>

## archiveStarted event

The Session object dispatches an archiveStarted event when an archive of the session starts recording.
The Session object also dispatches an archiveStarted event when you connect to a session that has a recording
in progress.

The event object includes the following properties:

* id -- The archive ID.
* name -- The name of the ID.

The following code registers event listeners for the archiveStarted event:

    session.addEventListener("archiveStarted", function (event) {
        console.log("Archive started."
        console.log("- id:" + event.archiveId)
        console.log("- name:" + event.name)
    }

<a name="archiveStopped"></a>

## archiveStopped event

The Session object dispatches an archiveStopped event when an archive of the session stops recording.

The event object includes the following properties:

* id -- The archive ID.
* name -- The name of the ID.

The following code registers event listeners for the archiveStopped event:

    session.addEventListener("archiveStopped", function (event) {
        console.log("Archive started."
        console.log("- id:" + event.archiveId)
        console.log("- name:" + event.name)
    }

<a name="changes-2.2"></a>

## OpenTok 2.2 JavaScript changes

The OpenTok 2.0 archiving feature requires that you use a pre-release version of the OpenTok 2.2 JavaScript library:

    <script src="http://static.opentok.com/webrtc/v2.2/js/TB.min.js"></script>

When released, the OpenTok 2.2 JavaScript library will include other new features in addition to archiving.

The OpenTok 2.2 JavaScript library includes some change that are not backward compatible with OpenTok 2.0:

* The `sessionConnected` event dispatched by the Session object -- The `streams` and `connections` properties are deprecated
(and these are now empty arrays). For streams and connections in the session when you connect, the Session object dispatches
individual `streamCreated` and `connectionCreated` events (for each stream and connection). However, the Session object does not
dispatch a `connectionCreated` event for the client's own connection. For the client's own connection, the Session object dispatches
a `sessionConnected` event only.

* The `streamCreated` event dispatched by the Session object -- The Session object dispatches the `streamCreated` event for streams
published by other clients only. The event includes a new property: `stream` (a Stream object corresponding to the new stream). The `streams` property is deprecated.

* The `streamCreated` event dispatched by the Publisher object -- The Publisher dispatches the `streamCreated` event for the stream
published by the Publisher object. The `stream` property of the event is a reference to the Stream object.

* The `connectionCreated` event dispatched by the Session object -- The ConnectionCreatedEvent class now has a 
`connection` property, and the `connections` property is deprecated. The Session object now dispatches the `connectionCreated` event
for each connection added to the session, including other clients' connections that exist when you first connect.

* The `connectionDestroyed` event dispatched by the Session object -- The ConnectionDestroyedEvent class now has a `connection`
property, and the `connections` property is deprecated. The Session object now dispatches the `connectionDestroyed` event
for each connection that leaves the session while your client is connected.

* The `streamDestroyed` event dispatched by the Session object -- The Session object dispatches the `streamDestroyed` event for
destroyed streams from other clients only. The event includes a new property: `stream` (a Stream object corresponding to the
destroyed stream). The `streams` property is deprecated.

* The `streamDestroyed` event dispatched by the Publisher object -- The Publisher dispatches the `streamDestroyed` event when
the stream published by the Publisher object is destroyed. The `stream` property is a reference to the Stream object. The default
behavior of this event is to remove the Publisher from the HTML DOM. You can cancel the default behavior by calling the
`preventDefault()` method of the event object.

* The `sessionDisconnected` event dispatched by the Session object -- Calling the `preventDefault()` method no longer causes
a Publisher to be preserved. The Publisher is destroyed by default when the Publisher dispatches the streamDestroyed event.
(You can preserve the Publisher by calling the `preventDefault()` method of the `streamDestroyed` event dispatched by the
Publisher object.)

* In the `Session.signal()` method, the optional `to` property of the `signal` parameter takes a single Connection object
(defining the connection to which the signal will be sent). The `to` property does not take an *array* of Connection objects,
like it did in version 2.0 of the JavaScript library.

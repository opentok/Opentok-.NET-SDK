OpenTok 2.0 Archiving REST API
==============================

The OpenTok 2.0 archiving feature lets you record OpenTok 2.0 sessions.

This API only works with OpenTok 2.0 sessions. OpenTok 1.0 sessions are not supported.

*Note:* This documentation is a preview of the OpenTok 2.0 beta archiving REST API.

When you record an archive of a session, the resulting video is an H.264-encoded MP4 file, available for download.

The REST APIs include functions for the following:

* [Starting an archive recording](#start_archive)
* [Stopping an archive recording](#stop_archive)
* [Listing archives](#listing_archives)
* [Retrieving archive information](#retrieve_archive_info)
* [Setting an Amazon S3 upload](#set_amazon_s3_upload)
* [Deleting an Amazon S3 upload](#delete_amazon_s3_upload)
* [Deleting an archive](#delete_archive)
* [Notification of archive status changes](#notification_status_change)
* [Listing notification callbacks](#list_callbacks)
* [Deleting a notification callback](#delete_callback)


<a name="start_archive"></a>

## Starting an archive recording

To start recording an archive of an OpenTok 2.0 session, submit an HTTP POST request.

Clients must be actively connected to the OpenTok session for you to successfully start recording an archive.

You can only record one archive at a time for a given session. You can only record archives of OpenTok server-enabled
sessions; you cannot archive peer-to-peer sessions.

### HTTP POST to archive

Submit an HTTP POST request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

#### POST header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

Set the Content-type header to application/json:

    Content-Type:application/json

#### POST data

Include a JSON object of the following form as the POST data:

    {
        "sessionId" : "session_id",
        "name" : "archive_name"
    }

The JSON object includes the following properties:

* `sessionId` -- The session ID of the OpenTok session you want to start archiving
* `name` -- (Optional) The name of the archive (for your own identification)

### Response

The raw data of the HTTP response, with status code 200, is a JSON-encoded message of the following form:

    {
      "createdAt" : 1384221730555,
      "duration" : 0,
      "id" : "b40ef09b-3811-4726-b508-e41a0f96c68f",
      "name" : "The archive name you supplied",
      "partnerId" : 234567,
      "reason" : "",
      "sessionId" : "flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN",
      "size" : 0,
      "status" : "started",
      "url" : null
    }

The JSON object includes the following properties:

* createdAt -- The timestamp for when the archive started recording, expressed in milliseconds since the Unix epoch
(January 1, 1970, 00:00:00 UTC).
* partnerId -- Your OpenTok API key.
* sessionId -- The session ID of the OpenTok session being archived.
* id -- The unique archive ID. Store this value for later use (for example, to [stop the recording](#stop_archive)).
* name -- The name of the archive you supplied (this is optional)

The HTTP response has a 400 status code in the following cases:

* You do not pass in a session ID or you pass in an invalid session ID.
* You do not pass in an action or pass in an invalid action type. To start recording an archive, set
the action to "start". (This is case-sensitive.)
* No clients are actively connected to the OpenTok session.

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key or partner secret.

The HTTP response has a 404 status code if the session does not exist.

The HTTP response has a 409 status code if you attempt to start an archive for a peer-to-peer session or if
the session is already being recorded. 

The HTTP response has a 500 status code for an OpenTok server error.

### Example

The following command line example starts recording an archive for an OpenTok session:

    api_key=12345
    api_secret=234567890
    session_id=2_MX40NzIwMzJ-flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN-MC45NDQ2MzE2NH4
    name="Foo"
    data='{"sessionId" : "'$session_id'", "name" : "'$name'"}'
    curl \
         -i \
         -H "Content-Type: application/json" \
         -X POST -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" -d "$data" \
         https://api.opentok.com/v2/partner/$api_key/archive

* Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.
* Set the `session_id` value to the session ID of the OpenTok 2.0 session you want to archive.
* Set the `name` value to the archive name (this is optional).


<a name="stop_archive"></a>

## Stopping an archive recording

To stop recording an archive, submit an HTTP POST request.

Archives automatically stop recording after 90 minutes or when all clients have disconnected from the session being archived.

### HTTP POST to archive

Submit an HTTP POST request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive/<archive_id>/stop

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

Replace `<archive_id>` with the archive ID. You can obtain the archive ID from the response to the API call to
[start recording the archive](#start_archive).

#### POST header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

The raw data of the HTTP response, with status code 200, is a JSON-encoded message of the following form:

    {
      "createdAt" : 1384221730555,
      "duration" : 60,
      "id" : "b40ef09b-3811-4726-b508-e41a0f96c68f",
      "name" : "The archive name you supplied",
      "partnerId" : 234567,
      "reason" : "",
      "sessionId" : "flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN",
      "size" : 0,
      "status" : "stopped",
      "url" : null
    }

The JSON object includes the following properties:

* createdAt -- The timestamp for when the archive started recording, expressed in milliseconds since the Unix epoch
(January 1, 1970, 00:00:00 UTC).
* partnerId -- Your OpenTok API key.
* sessionId -- The session ID of the OpenTok session that was archived.
* id -- The unique archive ID.
* name -- The name of the archive you supplied (this is optional)

The HTTP response has a 400 status code in the following cases:

* You do not pass in a session ID or you pass in an invalid session ID.
* You do not pass in an action or pass in an invalid action type. To stop recording an archive, set
the action to "stop". (This is case-sensitive.)

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key or partner secret.

The HTTP response has a 404 status code if you pass in an invalid archive ID.

The HTTP response has a 409 status code if you attempt to stop an archive that is not being recorded.

The HTTP response has a 500 status code for an OpenTok server error.

### Example

The following command line example stops recording an archive for an OpenTok session:

    api_key=123456
    api_secret=2345678
    id=b40ef09b-3811-4726-b508-e41a0f96c68f
    curl \
         -i \
         -X POST -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
         https://api.opentok.com/v2/partner/$api_key/archive/$id/stop

* Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.
* Set the `id` value to the archive ID. You can obtain the archive ID from the response to the API call to
[start recording the archive](#start_archive).


<a name="listing_archives"></a>

## Listing archives

To list archives for your API key, both completed and in-progress, submit an HTTP GET request.

### HTTP GET to archive

Submit an HTTP GET request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive?offset=<offset>&count=<count>

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

Replace `<offset>` with the index offset of the first archive. 0 is offset of the most recently
started archive (excluding deleted archive). 1 is the offset of the archive that started prior to
the most recent archive. Setting `<offset>` is optional; the default is 0.

Replace `<count>` with the number of archives to be returned. Setting `<count>` is optional. The default number of archives returned is
50 (or fewer, if there are fewer than 50 archives). The maximum number of archives the call will return is 1000.

Deleted archives are not included in the results of this API call.

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.


#### GET header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

The raw data of the HTTP response, with status code 200, is a JSON-encoded message of the following form:

    {
	  "count" : 2,
	  "items" : [ {
	    "createdAt" : 1384221730000,
	    "duration" : 5049,
	    "id" : "09141e29-8770-439b-b180-337d7e637545",
	    "name" : "Foo",
	    "partnerId" : 123456,
	    "reason" : "",
	    "sessionId" : "2_MX40NzIwMzJ-flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN-MC45NDQ2MzE2NH4",
	    "size" : 247748791,
	    "status" : "available",
	    "url" : "http://tokbox.com.archive2.s3.amazonaws.com/123456/09141e29-8770-439b-b180-337d7e637545/archive.mp4"
	  }, {
	    "createdAt" : 1384221380000,
	    "duration" : 328,
	    "id" : "b40ef09b-3811-4726-b508-e41a0f96c68f",
	    "name" : "Foo",
	    "partnerId" : 123456,
	    "reason" : "",
	    "sessionId" : "2_MX40NzIwMzJ-flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN-MC45NDQ2MzE2NH4",
	    "size" : 18023312,
	    "status" : "available",
	    "url" : "http://tokbox.com.archive2.s3.amazonaws.com/123456/b40ef09b-3811-4726-b508-e41a0f96c68f/archive.mp4"
	  } ]

The JSON object includes the following properties:

* count -- The total number of archives for the API key.
* items -- An array of objects defining each archive retrieved. Archives are listed from the newest to the oldest in
the return set.

Each archive object (item) has the following properties:

* createdAt -- The timestamp for when the archive started recording, expressed in milliseconds since the Unix epoch
(January 1, 1970, 00:00:00 UTC).
* duration -- The duration of the archive in seconds. For archives that have are being recorded (with the
status property set "started"), this value is set to 0.
* id -- The unique archive ID.
* name -- The name of the archive you supplied (this is optional)
* partnerId -- Your OpenTok API key.
* reason -- For archives with the status "stopped", this can be set to "90 mins exceeded", "failure", "session ended", "user initiated".
For archives with the status "failed", this can be set to "system failure".
* sessionId -- The session ID of the OpenTok session that was archived.
* status -- The status of the archive:
  * "available" -- The archive is available for download from the OpenTok cloud.
  * "failed" -- The archive recording failed.
  * "started" -- The archive started and is in the process of being recorded.
  * "stopped" -- The archive stopped recording.
  * "uploaded" -- The archive is available for download from the S3 bucket you specified (see
    [Setting an Amazon S3 upload](#set_amazon_s3_upload)).
* size -- The size of the MP4 file. For archives that have not been generated, this value is set to 0.
* url -- The download URL of the available MP4 file. This is only set for an archive with the status set to "available";
for other archives, (including archives with the status "uploaded") this property is set to null. The download URL is
obfuscated, and the file is only available from the URL for 10 minutes. To generate a new URL, use the REST API for
[retrieving archive information](#retrieve_archive_info) or [listing archives](#listing_archives).

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key or partner secret.

The HTTP response has a 500 status code for an OpenTok server error.

### Example

The following command line example retrieves information for all archives:

    api_key=12345
    api_secret=234567890
    curl \
        -i \
        -X GET \
        -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
        https://api.opentok.com/v2/partner/$api_key/archive

Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.


<a name="retrieve_archive_info"></a>

## Retrieving archive information

To retrieve information about a specific archive, submit an HTTP GET request.

You can also retrieve information about multiple archives. See [Listing archives](#listing_archives).

### HTTP GET to archive

Submit an HTTP GET request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive/<archive_id>

* Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.
* Replace `<archive_id` with the archive ID.

#### GET header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

The raw data of the HTTP response, with status code 200, is a JSON-encoded message of the following form:

    {
     "createdAt" : 1384221730000,
        "duration" : 5049,
        "id" : "09141e29-8770-439b-b180-337d7e637545",
        "name" : "Foo",
        "partnerId" : 123456,
        "reason" : "",
        "sessionId" : "2_MX40NzIwMzJ-flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN-MC45NDQ2MzE2NH4",
        "size" : 247748791,
        "status" : "available",
        "url" : "http://tokbox.com.archive2.s3.amazonaws.com/123456/09141e29-8770-439b-b180-337d7e637545/archive.mp4"
    }

The JSON object includes the following properties:

* createdAt -- The timestamp for when the archive started recording, expressed in milliseconds since the Unix epoch
(January 1, 1970, 00:00:00 UTC).
* duration -- The duration of the archive in seconds. For archives that have are being recorded (with the
status property set "started"), this value is set to 0.
* id -- The unique archive ID.
* name -- The name of the archive you supplied (this is optional)
* partnerId -- Your OpenTok API key.
* reason -- For archives with the status "stopped", this can be set to "90 mins exceeded", "failure", "session ended", "user initiated".
For archives with the status "failed", this can be set to "system failure".
* sessionId -- The session ID of the OpenTok session that was archived.
* status -- The status of the archive:
  * "available" -- The archive is available for download from the OpenTok cloud.
  * "deleted" -- The archive was deleted.
  * "failed" -- The archive recording failed.
  * "started" -- The archive started and is in the process of being recorded.
  * "stopped" -- The archive stopped recording.
  * "uploaded" -- The archive is available for download from the S3 bucket you specified (see
    [Setting an Amazon S3 upload](#set_amazon_s3_upload_target)).
* size -- The size of the MP4 file. For archives that have not been generated, this value is set to 0.
* url -- The download URL of the available MP4 file. This is only set for an archive with the status set to "available";
for other archives, (including archives with the status "uploaded") this property is set to null. The download URL is
obfuscated, and the file is only available from the URL for 10 minutes. To generate a new URL, use the REST API for
[retrieving archive information](#retrieve_archive_info) or [listing archives](#listing_archives).

The HTTP response has a 400 status code if you do not pass in a session ID or you pass in an invalid archive ID.

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key or partner secret.

The HTTP response has a 500 status code for an OpenTok server error.

### Example

The following command line example retrieves information for an archive:

    api_key=12345
    api_secret=234567890
    id=23435236235235235235
    curl \
        -i \
        -X GET \
        -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
        https://api.opentok.com/v2/partner/$api_key/archive/$archive

* Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.
* Set the `id` value to the archive ID.


<a name="set_amazon_s3_upload"></a>

## Setting an Amazon S3 upload

You can have OpenTok upload completed archives to an Amazon S3 bucket that you manage. You will need to provide us with upload-only permission to your Amazon S3 bucket.

To set an archive upload target bucket, submit an HTTP PUT request.

If you set an upload target, each completed archive file is uploaded as a file named archive.mp4 in the path
/apiKey/archiveId/ of the target bucket, where apiKey is your OpenTok API key, and archiveId is the archive ID.

If you have already set an archive upload target for archive files, you can submit another PUT request to
register a new upload target.

If you do not set an upload target, archive files are available for download from the OpenTok cloud.

### HTTP PUT to archive

Submit an HTTP PUT request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive/storageBeta

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

#### PUT header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your OpenTok API key and API secret
concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

Set the Content-type header to application/json:

    Content-Type:application/json

#### PUT data

Include a JSON object of the following form as the PUT data:

    {
        "type": "s3",
        "config": {
            "accessKey":"myUsername",
            "secretKey":"myPassword",
            "bucket": "bucketName"
        }
    }


The JSON object includes the following properties:

* `type` -- "s3" (for Amazon S3)
* `config` -- Settings for your account:
  * `accessKey` -- Your Amazon Web Services access key
  * `secretKey` -- Your Amazon Web Services secret key
  * `bucket` -- The S3 bucket name.

### Response

The HTTP response has a 200 status code on success.

The HTTP response has a 400 status code in the following cases:

* The type is undefined.
* The type is unsupported (it is not "s3").
* The config is undefined.

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key or partner secret.

### Example

The following command line example starts recording an archive for an OpenTok session:

    api_key=12345
    api_secret=234567890
    storage_type=s3
    access_key=myUsername
    secret_key=myPassword
    bucket=bucketName
    data='{"type": "$storage_type", "config": { "accessKey":"$access_key", "secretKey":"$secret_key", "bucket": "$bucket"}}'
    curl \
         -i \
         -H "Content-Type: application/json" \
         -X PUT -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" -d "$data" \
         https://api.opentok.com/v2/partner/$api_key/archive/storageBeta

* Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.
* Set the `storage_type` value to "s3".
* Set the `access_key` value to the access key for your Amazon Web Services account.
* Set the `secret_key` value to the secret key for your Amazon Web Services account.
* Set the `bucket` value to the bucket name.


<a name="delete_amazon_s3_upload"></a>

## Deleting an Amazon S3 upload

If you have set an archive upload target for archive files (see [Setting an archive upload target](#set_upload_target)),
you can delete it. If you delete the target (and do not set a new one), archive files are available for download from
the OpenTok cloud.

### HTTP DELETE to archive

Submit an HTTP DELETE request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive/storageBeta

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

#### DELETE header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

The HTTP response has a 204 status code on success.

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key or partner secret.

The HTTP response has a 404 status code if no upload target exists.

### Example

The following command line example starts recording an archive for an OpenTok session:

    api_key=12345
    api_secret=234567890
    curl \
         -i \
         -H "Content-Type: application/json" \
         -X DELETE -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
         https://api.opentok.com/v2/partner/$api_key/archive/storageBeta

Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.


<a name="delete_archive"></a>

## Deleting an archive

To delete an archive, submit an HTTP DELETE request.

You can only delete an archive which has a status of "available" or "uploaded". Deleting an archive
removes its record from the list of archives (see [Listing archives](#listing_archives)). For an
"available" archive, it also removes the archive file, making it unavailable for download.

### HTTP DELETE to archive

Submit an HTTP DELETE request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/archive/<archive_id>

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

Replace `<archive_id>` with the archive ID.

#### DELETE header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

An HTTP response with a status code of 204 indicates that the archive has been deleted.

The HTTP response has a 403 status code if you pass in an invalid OpenTok API key, invalid partner secret, or
invalid archive ID.

The HTTP response has a 409 status code if the status of the archive is not "uploaded", "available",
or "deleted".

The HTTP response has a 500 status code for an OpenTok server error.

### Example

The following command line example deletes an archive:

    api_key=12345
    api_secret=234567890
    id=b40ef09b-3811-4726-b508-e41a0f96c68f
    curl \
         -i \
         -X DELETE \
         -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
         https://api.opentok.com/v2/partner/$api_key/archive/$id

* Set the values for `api_key` and `api_secret` to your OpenTok API key and partner secret.
* Set the `id` value to the ID of the archive to delete.


<a name="notification_status_change"></a>

## Notification of when an archive status changes

You can register for notifications when archives' statuses change by submitting an HTTP POST request.

Each archive has one of the following statuses:

* "started" -- The archive recording has started, but it has not completed.
* "stopped" -- The archive recording has completed, but the download file is not available.
* "failed" -- The archive recording failed.
* "available" -- The archive is completed and available for download.
* "deleted" -- The archive has been deleted.

### HTTP POST to callback

Submit an HTTP POST request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/callback/

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

#### POST header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

Set the Content-type header to application/json:

    Content-Type:application/json

#### POST data

Include a JSON object of the following form as the POST data:

    {
        "url":'http://example.com/archive-callback',
        "group": "archive",
        "event": "status"
    }

The JSON object includes the following properties:

* `url` -- The URL on your server, which the OpenTok server will call when an archive status changes. Be sure
to include the complete URL.
* `group` -- Set this to "archive".
* `event` -- Set this to "status".

The length of the JSON data cannot exceed 8096.

### Response

The OpenTok server returns a 200 status code upon successfully setting the notification URL. The raw data of
the HTTP response is a JSON-encoded message of the following form:

{ "id" : "95771", "uri" : "http://www.example.com"}

The JSON object includes the following properties:

* id -- The unique ID for the registered callback. Use this ID to delete a callback, using the DELETE method.
(See [Deleting a notification callback](#delete_callback).)
* uri -- The callback URL.

The OpenTok server returns a 400 status code in the following cases:

* The JSON data you send is invalid.
* The JSON data does not include group, url, or event properties.
* The JSON data is longer than 8096 characters.

The OpenTok server returns a 403 status code if you send an invalid API key or API secret.

### Callback HTTP requests

When an archive's status changes, the server sends HTTP POST requests to the URL you supply. The Content-Type for the request
is application/json.

The data of the request is a JSON object of the following form:

    {
        "id" : "b40ef09b-3811-4726-b508-e41a0f96c68f",
        "event": "archive",
        "createdAt" : 1384221380000,
        "duration" : 328,
        "name" : "Foo",
        "partnerId" : 123456,
        "reason" : "",
        "sessionId" : "2_MX40NzIwMzJ-flR1ZSBPY3QgMjkgMTI6MTM6MjMgUERUIDIwMTN-MC45NDQ2MzE2NH4",
        "size" : 18023312,
        "status" : "available",
        "url" : "http://tokbox.com.archive2.s3.amazonaws.com/123456/b40ef09b-3811-4726-b508-e41a0f96c68f/archive.mp4"
    }

The JSON object includes the following properties:

* createdAt -- The timestamp for when the archive started recording, expressed in milliseconds since the Unix epoch
(January 1, 1970, 00:00:00 UTC).
* duration -- The duration of the archive in seconds. For archives that have are being recorded (with the
status property set "started"), this value is set to 0.
* id -- The unique archive ID.
* name -- The name of the archive you supplied (this is optional)
* partnerId -- Your OpenTok API key.
* reason -- For archives with the status "stopped", this can be set to "90 mins exceeded", "failure", "session ended", "user initiated". For archives with the status "failed", this can be set to "system failure".
* sessionId -- The session ID of the OpenTok session that was archived.
* size -- The size of the MP4 file. For archives that have not been generated, this value is set to 0.
* status -- The status of the archive:
  * "available" -- The archive is available for download from OpenTok.
  * "failed" -- The archive recording failed.
  * "started" -- The archive started and is in the process of being recorded.
  * "stopped" -- The archive stopped recording.
  * "uploaded" -- The archive is available for download from the S3 bucket you specified (see
    [Setting an Amazon S3 upload](#set_amazon_s3_upload)).
* url -- The download URL of the available MP4 file. This is only set for an archive with the status set to "available";
for other archives, (including archives with the status "uploaded") this property is set to null. The download URL is
obfuscated, and the file is only available from the URL for 10 minutes. To generate a new URL, use the REST API for
[retrieving archive information](#retrieve_archive_info) or [listing archives](#listing_archives).

### Example

The following command line example sets a callback URL for archive status changes:

    api_key=12345
    api_secret=234567890
    callback_url=http://www.example.com
    curl \
        -i \
        -X POST \
        -H "Content-Type: application/json" \
        -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \ 
        -d "{'url':'"$callback_url"', 'group': 'archive','event': 'status'}" \
        https://api.opentok.com/v2/partner/$api_key/callback

Set the values for `api_key` and `api_secret` to the API key and partner secret provided to you when you signed up for an OpenTok account.
Set the value for `callback_url` to the callback URL for status change notifications.


<a name="list_callbacks"></a>

## Listing notification callbacks

You can list the notification callback URLs you've registered by submitting an HTTP GET request.

### HTTP GET to callback

Submit an HTTP GET request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/callback/

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

#### GET header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

The OpenTok server returns a 200 status code upon successfully setting the notification URL. The raw data of
the HTTP response is a JSON-encoded message of the following form:

    [
	  {
	    "id" : "85771",
	    "url" : "http://www.example.com/foo",
	    "event" :"status",
	    "group" :"archive",
	    "createdAt" :"2013-12-08 17:14:52.0",
	    "updatedAt" :"2013-12-08 17:14:52.0"
	  },
	  {
	    "id" : "95643",
	    "url" : "http://www.example.com/bar",
	    "event" :"status",
	    "group" :"archive",
	    "createdAt" :"2013-12-09 12:39:22.0",
	    "updatedAt" :"2013-12-09 12:39:22.0"
	  }
	]

The JSON object includes an array of objects defining each callback. Each callback object contains the following properties:

* id -- The unique ID for the registered callback. Use this ID to delete a callback, using the DELETE method.
(See [Deleting a notification callback](#delete_callback).)
* url -- The callback URL.
* event -- This is set to "status" for archiving callbacks. In addition to archiving callbacks, this JSON object lists
any other OpenTok callbacks you have registered (for example, for [OpenTok Cloud Raptor](http://labs.opentok.com/raptor).)
* group -- This is set to "archive" for archiving callbacks.
* createdAt -- The time when the callback registration was created.
* updatedAt -- The time when the callback registration was last modified.

The OpenTok server returns a 403 status code if you send an invalid API key or API secret.

### Example

The following command line example retrieves information about registered callbacks:

    api_key=12345
    api_secret=234567890
    curl \
        -i \
        -X GET \
        -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
        https://api.opentok.com/v2/partner/$api_key/callback

Set the values for `api_key` and `api_secret` to the API key and partner secret provided to you when you signed up for
an OpenTok account.


<a name="delete_callback"></a>

## Deleting a notification callback
You can delete a notification callback URLs you've registered by submitting an HTTP DELETE request.

### HTTP DELETE to callback

Submit an HTTP DELETE request to the following URL:

    https://api.opentok.com/v2/partner/<api_key>/callback/<callback_id>

Replace `<callback_id>` with the ID of the registered callback. See
[Notification of archive status changes](#notification_status_change) and [Listing notification callback URLs](#list_callbacks).

Replace `<api_key>` with your OpenTok API key. See <https://dashboard.tokbox.com>.

#### DELETE header properties

API calls must be authenticated using a custom HTTP header: X-TB-PARTNER-AUTH. Send your API key and API secret concatenated with a colon:

    X-TB-PARTNER-AUTH: <api_key>:<api_secret>

(See <https://dashboard.tokbox.com>.)

### Response

The OpenTok server returns a 204 status code upon successfully deleting the callback.

The OpenTok server returns a 204 status code upon successfully deleting the callback.

The OpenTok server returns a 403 status code if you send an invalid API key or API secret.

### Example

The following command line example deletes a registered callbacks:

    api_key=12345
    api_secret=234567890
    id=95771
    curl \
        -i \
        -X DELETE \
        -H "X-TB-PARTNER-AUTH:$api_key:$api_secret" \
       https://api.opentok.com/v2/partner/$api_key/callback/$id

Set the values for `id` to the callback ID. See [Notification of archive status changes](#notification_status_change)
and [Listing notification callback URLs](#list_callbacks).

Set the values for `api_key` and `api_secret` to the API key and partner secret provided to you when you signed up for
an OpenTok account.

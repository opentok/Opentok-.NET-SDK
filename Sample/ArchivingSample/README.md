OpenTok 2.0 Archiving .NET Sample App
========================================

A sample app showing use of the OpenTok 2.0 Archiving API.
This sample app is for use with the OpenTok 2.0 .NET SDK. 

## Prerequisites

1. Visual Studio. The OpenTok archiving API does not require Visual Studio. However, this sample was
   developed using Visual Studio to create a solution with the different projects.

2. An OpenTok API key and secret (see <https://dashboard.tokbox.com>)

## Setup

1. Open the solution with Visual Studio, you'll be able to see four different projects: ArchivingSample, Sdk, SimpleSample and Tests.

2. To run SimpleSample, right click on the solution 'Opentok-DotNet-SDK' and go to properties. Set the solution as a single startup project, and choose the Simple Sample project. 

3. Go to SimpleSample and open the Web.config file in the main directory of the application. Set the following strings to your OpenTok API key and API secret (see dashboard.tokbox.com):
    <add key="opentok_key" value="*** YOUR API KEY ***"/>
    <add key="opentok_secret" value="*** YOUR API SECRET ***"/>

## To run the app for yourself

1. Click on the run button of Visual Studio with your default browser. A new window for the browser should open.  

2. Click the "Host view" button. The Host View page publishes an audio-video stream to an
   OpenTok session. It also includes controls that cause the web server to start and stop archiving.

3. Click the Allow button to grant access to the camera and microphone.

4. Click the "Start archiving" button. The session starts recording to an archive. Note that the
   red archiving indicator is displayed in the video view.

5. Open the Sample app in a new browser tab. (You may want to mute your computer speaker
   to prevent feedback. You are about to publish two audio-video streams from the same computer.)

6. Click the "Participant view" button. The page connects to the OpenTok session, displays
   the existing stream (from the Host View page).

7. Click the Allow button in the page to grant access to the camera and microphone. The page
   publishes a new stream to the session. Both streams are now being recorded to an archive.

8. On the Host View page, click the "Stop archiving" button.

9. Click the "past archives" link in the page. This page lists the archives that have been recorded.
   Note that it may take up to 2 minutes for the video file to become available (for a 90-minute 
   recording).

10. Click a listing for an available archive to download the MP4 file of the recording.

## Understanding the code

This sample app shows how to use the archiving API in the OpenTok 2.0 .NET SDK.

### Starting an archive

The HostView.cshtml file (in the views directory) includes a button for starting the archive.
When the user clicks this button, the page makes an Ajax call back to the server:

<pre>
$(".start").click(function (event) {
        $.get("/Archive/Start");
    }).show();
</pre>    

The Start() method of the Sample.Controllers.ArchiveController controller handles this
HTTP request:

<pre>
public string Start()
{
    // ..

    Archive archive;

    try
    {
        archive = opentok.StartArchive((string)Application["sessionId"], "DotNet Archiving Sample");
    }
    catch(OpenTokException)
    {
        return "Error: Archive ID could not be created";
    }
    return archive.Id.ToString(); 
}
</pre>

The opentok object is a class field and is initialized when the controller is created. The OpenTok class is
defined in the OpenTok .NET SDK. The Start() method of the OpenTok object starts an archive, and it takes two parameters:

* The sesssion ID of the OpenTok session to archive
* A name (which is optional and helps identify the archive)

You can only start recording an archive in a session that has active clients connected.

In the page on the web client, the Session object (in JavaScript) dispatches an archiveStarted
event. The page stores the archive ID (a unique identifier of the archive) in an archiveID variable:

<pre>
session.on('archiveStarted', function(event) {
  archiveID = event.id;
  console.log("ARCHIVE STARTED");
  $(".start").hide();
  $(".stop").show();
});
</pre>

### Stopping an archive

The HostView.cshtml file includes a button for stopping the archive. When the user clicks this
button, the page makes an Ajax call back to the server:

<pre>
$(".stop").click(function (event) {
    $.get("/Archive/Stop/" + archiveID);
}).hide();
</pre>

The Stop() method of the ArchiveController controller handles this HTTP request:

<pre>
public string Stop(string id)
{
    Archive archive;

    try
    {
        archive = opentok.StopArchive(id);
    }
    catch (OpenTokException)
    {
        return "Error: Archive ID could not be created";
    }
    return archive.Id.ToString(); 
}
</pre>

This code calls the Stop() method of the OpenTok object. This method stops an archive,
based on the archive ID.

In the page on the web client, the Session object (in JavaScript) dispatches an archiveStopped
event. The page stores the archive ID (a unique identifier of the archive) in an archiveID variable:

<pre>
session.on('archiveStopped', function(event) {
  archiveID = null;
  console.log("ARCHIVE STOPPED");
  $(".start").show();
  $(".stop").hide();
});
</pre>

### Listing archives

The List() method of the ArchiveController controller handles this HTTP request:

<pre>
private const int archivesPerPage = 5;

public ActionResult List(string id)
{
    int page = 0;
    //.. read page from id

    try
    {
        ViewBag.Archives = opentok.ListArchives(page * archivesPerPage, archivesPerPage);
    }
    catch(OpenTokException)
    {
        ViewBag.Error = "Archive List could not be retrieved";
        return View();
    }

    // ...
    return View();
}
</pre>

This code calls the ListArchives() method or the ListArchives(int offset, int count) method
of the OpenTok object. These methods list archives created for the API key. 
The ListArchives(int offset, int count) method takes two parameters:

* offset -- This defines which archive starts the list.
* count -- This defines how many archives are listed

The method returns an ArchiveList object. The getItems() method of this object is a List of Archive
objects. The toString() method of the ArchiveList object converts the List of Archive objects to
a JSON string representing the List of Archive objects.

An Archive object represents an OpenTok archive, and includes the following properties:

* id -- The archive ID, which uniquely defines the archive.
* name -- The archive name.
* createdAt -- The timestamp for when the archive was created.
* duration -- The duration of the archive (in seconds).
* status -- The status of the archive. This can be one of the following:
  * "available" -- The archive is available for download from the OpenTok cloud.
  * "failed" -- The archive recording failed.
  * "started" -- The archive started and is in the process of being recorded.
  * "stopped" -- The archive stopped recording.
  * "uploaded" -- The archive is available for download from an S3 bucket you specified (see
    [Setting an archive upload target](../../../REST-API.md#set_upload_target)).
* url -- The URL of the download file for the recorded archive (if available) 

The client web page iterates through the JSON data, representing the list of archive objects,
and it displays the id, name, createdAt, duration, status, and url properties of the object:

<pre>
    function displayArchives(data) {
      // clear out the table first
      $("table > tbody").html("");
      for (var i = 0; i < data.items.length; i++) {
        var item = data.items[i];
        var tr = $("<tr></tr>");
        tr.append("<td>" + (item.url && item.status == "available" ? "<a href='" + item.url + "'>" : "") + (item.name ? item.name : "Untitled") + (item.url && item.status == "available" ? "</a>" : "") + "</td>");
        tr.append("<td>" + dateString(item.createdAt) + " at " + timeString(item.createdAt) + "</td>");
        tr.append("<td>" + item.duration + " seconds</td>");
        tr.append("<td>" + item.status + "</td>");

        if (item.status == "available") {
          var deleteLink = $("<a href='#delete-" + item.id + "'>Delete</a>");
          (function(archiveId) {
            deleteLink.click(function() {
              deleteArchive(archiveId);
              return false;
            })
          })(item.id);
          var deleteTD = $("<td></td>");
          deleteTD.append(deleteLink);
          tr.append(deleteTD);
        } else {
          tr.append("<td></td>");
        }
        $("table > tbody").append(tr);
      }
    }
</pre>

### Downloading archives

The url property of an Archive object is the download URL for the available archive. See the
previous section, which shows how this URL is added to the List.cshtml.

### Deleting archives

The List.cshtml file includes buttons for deleting archives. When the user clicks one of
these buttons, the page makes an Ajax call back to the server:

  <form method="post" action="/Archive/Delete/@item.Id">
      @if (item.Status.ToString() == "AVAILABLE") 
      { 
          <td><input class="btn btn-danger btn-xs" type="submit" value="Delete"> </td>
      }
      else
      {
          <td>&nbsp; </td>
      }                                           
  </form>

The Delete() method of the ArchiveController controller handles this HTTP request:

<pre>
public ActionResult Delete(string id)
{
    if (id != null)
    {
        try
        {
            opentok.DeleteArchive(id);
        }
        catch (OpenTokException)
        {
            Redirect("/");
        }
                    }
    return Redirect("/Archive/List/");            
}
</pre>

This code calls the DeleteArchive() method of the OpenTok object. This method deletes an archive,
based on the archive ID.

## Documentation

* [OpenTok OpenTok .NET SDK documentation](../README.md)
* [Archiving JavaScript API documentation](../JavaScript-API.md)

## More information

See the list of known issues in the main [README file](../../../README.md).

The OpenTok 2.0 archiving feature is currently in beta testing.

If you have questions or to provide feedback, please write <denis@tokbox.com>.

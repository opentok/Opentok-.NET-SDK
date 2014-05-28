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

2. The page publishes an audio-video stream to an OpenTok session.

3. Click the Allow button to grant access to the camera and microphone.

4. Copy the url, open a new window for the browser and paste the url. (You may want to mute your computer speaker to prevent feedback. You are about to publish two audio-video streams from the same computer.)

5. Click the Allow button in the page to grant access to the camera and microphone. The page
   publishes a new stream to the session. You'll see the two streams published in the session in the two windows of the browser. 

## Understanding the code
This sample app shows how to use the basic API in the OpenTok 2.0 .NET SDK.

## Documentation

* [OpenTok OpenTok .NET SDK documentation](../../README.md)
* [Archiving JavaScript API documentation](../../../JavaScript-API.md)

## More information

See the list of known issues in the main [README file](../../../README.md).

The OpenTok 2.0 archiving feature is currently in beta testing.

If you have questions or to provide feedback, please write <denis@tokbox.com>.

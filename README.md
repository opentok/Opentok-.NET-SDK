# OpenTok .NET SDK 2.2

Use the OpenTok .NET SDK to work with [OpenTok](http://www.tokbox.com/) applications.
You can create OpenTok [sessions](http://tokbox.com/opentok/tutorials/create-session/)
and to generate [tokens](http://tokbox.com/opentok/tutorials/create-token/),
and work with OpenTok 2.0 [archives](http://tokbox.com/#archiving).

## Download

Download the .NET SDK:

<https://github.com/opentok/Opentok-DotNet/archive/master.zip>

## Prerequisites

1. Visual Studio. The OpenTok archiving API does not require Visual Studio. However, this sample was
   developed using Visual Studio to create a solution with the different projects.

2. An OpenTok API key and secret (see <https://dashboard.tokbox.com>)

# Setup 

1. Open Visual Studio, "File -> Open -> Project/Solution" and open the Opentok-DotNet-SDK.sln file in this directory. 

2. Visual Studio will load the four projects that are part of this solution. 
    * Sdk contains the actual OpenTok .NET SDK
    * SimpleSample contains a very sample app to show the most basic functionality the OpenTok platform offers
    * Sample contains a sample app showing off the new archiving features of the OpenTok platform added for version 2.2.

3. In order to run one of the apps, see the respectives README.md files in their corresponding directory.    
    * [SimpleSample documentation](SimpleSample/README.md)
    * [ArchivingSample documentation](Sample/README.md)

# Documentation

Reference documentation is available at <http://www.tokbox.com//opentok/libraries/server/dot-net/reference/index.html> and in the
docs directory of the SDK.

# Creating Sessions
Use the `CreateSession()` method of the OpenTok object to create a session and a session ID.

The following code creates a session that uses the OpenTok Media Router:

<pre>
namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            int apiKey = 0; // Replace with your OpenTok API key.
            string apiSecret = ""; // Replace with your OpenTok API secret.

            // Creating opentok object to access the opentok API
            OpenTok opentok = new OpenTok(apiKey, apiSecret);

            // Create a session that uses the OpenTok Media Router    
            Session session = opentok.CreateSession();
           
            // The ID of the session we just created
            Console.Out.WriteLine("SessionId: {0}", session.Id);
        }
    }
}
</pre>

The following code creates a peer-to-peer session:

<pre>
namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            int apiKey = 0; // Replace with your OpenTok API key.
            string apiSecret = ""; // Replace with your OpenTok API secret.

            // Creating opentok object to access the opentok API
            OpenTok opentok = new OpenTok(apiKey, apiSecret);

            // Create a session that uses the OpenTok Media Router    
            Session session = opentok.CreateSession(mediaMode: MediaMode.RELAY);
           
            // The ID of the session we just created
            Console.Out.WriteLine("SessionId: {0}", session.Id);
        }
    }
}
</pre>

# Generating tokens
Use the  `GenerateToken()` method of the OpenTokSDK object to create an OpenTok token:

The following example shows how to obtain a token:

<pre>
using OpenTokSDK;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            int apiKey = 0; // Replace with your OpenTok API key.
            string apiSecret = ""; // Replace with your OpenTok API secret.

            // Creating opentok object to access the opentok API
            OpenTok opentok = new OpenTok(apiKey, apiSecret);

            // Create a session that uses the OpenTok Media Router    
            Session session = opentok.CreateSession();
           
            // Generate a token from the session we just created            
            string token = opentok.GenerateToken(session.Id);

            // We finally print out the id of the session with the new token created
            Console.Out.WriteLine("SessionId: {0} \ntoken: {1}", session.Id, token);
        }
    }
}
</pre>

The following C# code example shows how to obtain a token that has a role of "subscriber" and that has
a connection metadata string:

<pre>
using OpenTokSDK;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            int apiKey = 0; // Replace with your OpenTok API key.
            string apiSecret = ""; // Replace with your OpenTok API secret.
            string connectionData = "username=Bob,userLevel=4";

            // Creating opentok object to access the opentok API
            OpenTok opentok = new OpenTok(apiKey, apiSecret);

            // Create a session that uses the OpenTok Media Router    
            Session session = opentok.CreateSession();
           
            // Generate a token from the session we just created            
            string token = opentok.GenerateToken(session.Id, role: Role.SUBSCRIBER, data: connectionData);

            // We finally print out the id of the session with the new token created
            Console.Out.WriteLine("SessionId: {0} \ntoken: {1}", session.Id, token);
        }
    }
}
</pre>

# Working with OpenTok 2.0 archives

The following method starts recording an archive of an OpenTok 2.0 session (given a session ID)
and returns the archive ID (on success).

<pre>
Guid StartArchive(OpenTok opentok, string sessionId, string name)
{
    try
    {
        Archive archive = opentok.StartArchive(sessionId, name);
        return archive.Id;
    }
    catch (OpenTokException)
    {
        return Guid.Empty;
    }
}
</pre>

The following method stops the recording of an archive (given an archive ID), returning
true on success, and false on failure.

<pre>
bool StopArchive(OpenTok opentok, string archiveId)
{
    try
    {
        Archive archive = opentok.StopArchive(archiveId);
        return true;
    }
    catch (OpenTokException)
    {
        return false;
    }
}
</pre>

The following method logs information on a given archive.

<pre>
void LogArchiveInfo(OpenTok opentok, string archiveId)
{
    try
    {
        Archive archive = opentok.GetArchive(archiveId);
        Console.Out.WriteLine("ArchiveId: {0}", archive.Id.ToString());
    }
    catch (OpenTokException exception)
    {
        Console.Out.WriteLine(exception.ToString());
    }
}
</pre>

The following method logs information on all archives (up to 50)
for your API key:

<pre>
void ListArchives(OpenTok opentok) 
{
    try 
    {
        ArchiveList archives = opentok.ListArchives();
        for (int i = 0; i &lt; archives.Count(); i++) 
        {
            Archive archive = archives.ElementAt(i);
            Console.Out.WriteLine("ArchiveId: {0}", archive.Id.ToString());
        }
    } catch (OpenTokException exception) 
    {
        Console.Out.WriteLine(exception.ToString());
    }
}
</pre>


# Support

See http://tokbox.com/opentok/support/ for all our support options.

Find a bug? File it on the [Issues](https://github.com/opentok/opentok-php-sdk/issues) page. Hint:
test cases are really helpful!

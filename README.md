# OpenTok .NET SDK
This is a .NET implementation of the OpenTok API. See complete documentation at http://www.tokbox.com/opentok/api/tools/documentation/api/server_side_libraries.html.

## Usage

### Configuration

Include the OpenTok.cs file in your .NET project, then set the following properties in your web.config file.

```xml
<configuration>
 <appSettings>
    <add key="opentok_key" value="***API key***"/>
    <add key="opentok_secret" value="***API secret***"/>
    <add key="opentok_server" value="https://staging.tokbox.com/hl"/>
    <add key="opentok_token_sentinel" value="T1=="/>
    <add key="opentok_sdk_version" value="tbdotnet"/>
 </appSettings>
```

Explicitly set your OpenTok API key and secret. Additionally, the "opentok_secret" value should be set according to whether you are working in staging or production. For information on the differences between the environments, visit [here](http://www.tokbox.com/opentok/api/tools/js/documentation/overview/production.html).

### Generating Sessions

To generate an OpenTok session:

```csharp
OpenTokSDK opentok = new OpenTokSDK();
string sessionId = opentok.CreateSession(Request.ServerVariables["REMOTE_ADDR"]);
```

To generate an OpenTok P2P session:

```csharp
OpenTokSDK opentok = new OpenTokSDK();
Dictionary<string, object> options = new Dictionary<string, object>();
options.Add(SessionPropertyConstants.P2P_PREFERENCE, "enabled");
string sessionId = opentok.CreateSession(Request.ServerVariables["REMOTE_ADDR"], options);
```

### Generating Tokens

To generate a session token:

```csharp
string token = opentok.GenerateToken(sessionId);
```

By default, the token has the "publlisher" permission. To generate a token with a different set of permissions:

```csharp
Dictionary<string, object> tokenOptions = new Dictionary<string, object>();
tokenOptions.Add(TokenPropertyConstants.ROLE, RoleConstants.MODERATOR);
string token = opentok.GenerateToken(sessionId, tokenOptions);
```

You can also pass in additional token options like "connection_data" and "expire_time":

```csharp
tokenOptions.Add(TokenPropertyConstants.EXPIRE_TIME, new DateTime(2011, 11, 17)); // A token that expires on 2011-11-17
tokenOptions.Add(TokenPropertyConstants.CONNECTION_DATA, "I am connection metadata passed down to all clients");
```
## Contributions

Thanks to Robert Phan for the original source code contribution.

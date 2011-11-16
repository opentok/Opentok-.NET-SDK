# OpenTok .NET SDK
This is a .NET implementation of the OpenTok API. See complete documentation at http://www.tokbox.com/opentok/api/tools/documentation/api/server_side_libraries.html.

## Usage

### Configuration
In your web.config file, set the following properties. 

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

Explicitly set your OpenTok API key and secret. Additionally, the "opentok_secret" value should be set according to whether you are working in staging or production. For information on the differences between the environments, vis it [here](http://www.tokbox.com/opentok/api/tools/js/documentation/overview/production.html).

### Generating Sessions


### Generating Tokens

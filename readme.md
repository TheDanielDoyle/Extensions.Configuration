# Extensions.Configuration

Extensions to aid configuration registration and usage in .NET

## AddJsonFromFiles

This extension method allows you to read in any number of json files into your configuration at runtime.

Take this **appsettings.json** configuration file example.

```json
{
    "ConfigurationFiles": {
        "Json": [  "file1.json", "file2.json", "file3.json" ]
    }
}
```

The `configurationSection` parameter on the `AddJsonFromFiles` method allows you to override where the `Json` property will be read from. By default, it is `ConfigurationFiles`.

In addition to the default `appSettings.json` being read in, the configuration will also read in the `file1.json`, `file2.json`, and `file3.json` files. As configuration in .NET core is append/replace. Each file read in will replace any keys already set by previous configuration providers.

### Why did you create this?

In a nutshell, Docker Swarm. I, like many developers, deploy with a typical develop/staging/production flow. Whilst production may sit on it's own dedicated infrastructure, develop and staging may share an environment, therefore, creating docker secrets to store sensitive information is not workable, as the `AddKeyPerFile` method doesn't work with staging and develop sharing the same node, as they'd need to share a key, or you'd have to hardcode what their key is for each environment and set the `DOTNET_ENVIRONMENT` or `ASPNETCORE_ENVIRONMENT` enviroment variables.

There's also other apps you may have deployed with similar secret keys, which may conflict with the secret keys you set up.

### How AddJsonFromFiles solves the issue

* Create your secrets with whatever names you wish, per environment if needed
* The trick is, instead of storing a secret value in your secret, instead store a full configuration section instead, like the following:

```json
{
    "MyConnectionString": "my_connection_string",
    "MyUsername": "my_username",
    "MyPassword": "my_password"
}
```

* This way, you can keep your sensitive configuration secure with Docker Swarms's secret store, and still be able to use multiple environments per node, and avoid conflicting with other deployed apps that would normally have conflicting keys, if stored in secrets, and loaded out with `AddKeyPerFile`.

An example for your appsettings.json would now be:

```json
{
    "ConfigurationFiles": {
        "Json": [  "/run/secrets/mysecrets" ]
    }
}
```

Where mysecrets would contain the senstive configuration section in the example above.



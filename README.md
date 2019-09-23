# IEXCloudClient.NetCore 

A client for IEXCloud written in .NET Core.
Retrieves stock data from IEX's REST and event-stream webservice, parses the JSON and returns well defined interfaces, that you can use in your (e.g. ASP.NET Core) application.

## Features

* comprehensive list of 26 supported operations
* SSE events supported
* signing of requests 
* ready for integration with (ASP) .NET Core application
* unit and integration tested
* everything wrapped in interfaces for easier testing/mocking in consuming projects

## current limitations
* only `trade` events SSE streaming implemented yet. Other event types require a paid license, which I don't have. So I could not test it. If you can help out, it should be easy to extend (see `TradeEventSource.cs`).
* signing of requests implemented as a prototype but might not be working yet. Hashes are  calculated correcly (i have a green unit test that compares with the reference iplementation from https://iexcloud.io/docs/api/#getting-the-secret-for-a-signed-token) but somehow I seem to miss a detail or its limited to paid licenses, server responds with `400 Bad request`.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

I use [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)

### Installing

[nuget package](https://www.nuget.org/packages/IEXCloudClient.NetCore/)
```
dotnet add package IEXCloudClient.NetCore
```

For developers: 

From the repository root, run this 

```
cd IEXCloud-client-example && dotnet run
```

for a little demo

## Running the tests

From the repository root, run this 
```
dotnet test IEXCloud-client-tests\IEXCloud-client-tests.csproj
```


## Built With

* [3v.EvtSource](hhttps://github.com/3ventic/EvtSource) - The event stream framework used
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) - Json parsing

## Contributing

If you can code, please make a pull request. Otherwise create an issue.
Leave a star if you think it's a cool project!

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/jbrekle/IEXCloudClient.NetCore/tags). 

## Authors

* **Jonas Brekle** - *Initial work* - [jbrekle](https://github.com/jbrekle)

See also the list of [contributors](https://github.com/jbrekle/IEXCloudClient.NetCore/contributors) who participated in this project.

## License

This project is licensed under the LGPL v3 License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* [Nice approach for throttling](https://github.com/sstrickley/IexClient/)


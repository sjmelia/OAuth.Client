OAuth Client
============
[![nuget](https://img.shields.io/badge/nuget-v5.3.0-blue.svg)](https://www.nuget.org/packages/ArrayOfBytes.OAuth.Client/1.0.0)

A minimal dotnetcore library for authorising against OAuth services, useful for e.g. connecting to the Twitter API.

Usage
-----

Grab your oAuth credentials and create an `OAuthConfig` instance; then best used as an additional handler for `HttpClient`:

``` C#
using ArrayOfBytes.OAuth.Client;

...

var config = new OAuthConfig(...);
var handler = new OAuthHttpMessageHandler(config);
this.client = new HttpClient(handler);
```

Subsequent requests will have an `Authorization: OAuth ...` header.
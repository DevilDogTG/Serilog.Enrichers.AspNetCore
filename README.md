# Serilog Enrichers for AspNetCore 8

[![NuGet](https://img.shields.io/nuget/v/Serilog.Enrichers.AspNetCore.RequestHeader.svg)](https://www.nuget.org/packages/Serilog.Enrichers.AspNetCore.RequestHeader)
[![NuGet](https://img.shields.io/nuget/dt/Serilog.Enrichers.AspNetCore.RequestHeader.svg)](https://www.nuget.org/packages/Serilog.Enrichers.AspNetCore.RequestHeader)

Enrich log with AspNetCore 8 request headers data.

## Features

Focus to retrieve common and custom headers from HTTP request. This includes:

- `WithHeaderCorrelationId`: Enrich with `X-Correlation-ID` header or custom key as you want. Able to generate new correlation id if not found.
    - `keyName`: Header key name for `CorrelationId`.
    - `propertyName`: Property name to enrich. Default is `CorrelationId`.
    - `generatedWhenNotExist`: Generate new correlation id and added to HTTP header if not fond. this generate by `Guid.NewGuid()` method. Default is `true`.
    - `generatedFormat`: Format of generated correlation id. Default is `N`. more information see [Guid.ToString(String)](https://learn.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=net-8.0#System_Guid_ToString_System_String_).
- `WithHeaderKey`: Enrich with custom header key.
    - `keyName`: Header key name.
    - `propertyName`: Proerty name to enrich. If not set, `keyName` will be used.

## Usage

Apply enricher to `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
	.Enrich.WithHeaderCorrelationId(
		keyName: "X-Correlation-ID",
		generatedWhenNotExist: true,
		generatedFormat: "N")
	.Enrich.WithHeaderKey(
		keyName: "CustomKey",
		propertyName: "CustomProperty")
	.CreateLogger();
```

another way, you can apply in `appsettings.json` file with `Serilog.Settings.Configuration` package:

```json
{
  "Serilog": {
	"Using": [ "Serilog.Enrichers.AspNetCore" ],
    "Enrich": [
      {
        "Name": "WithHeaderCorrelationId",
        "Args": {
          "keyName": "X-Correlation-ID",
          "generatedWhenNotExist": true,
          "generatedFormat": "N"
        }
      },
      {
        "Name": "WithHeaderKey",
        "Args": {
          "keyName": "CustomKey",
          "propertyName": "CustomProperty"
        }
      },
      {
        "Name": "WithHeaderKey",
        "Args": {
          "keyName": "SameKeyNProperty"
        }
      }
    ]
  }
}
```

Then add `propertyName` to `outputTemplate`:

``` json
{
  "Serilog": {
	"WriteTo": [
	  {
		"Name": "Console",
		"Args": {
		  "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{CorrelationId}]{CustomProperty|SameKeyNProperty} {Message:lj} {Properties:j}{NewLine}"
		}
	  }
	]
  }
}
```

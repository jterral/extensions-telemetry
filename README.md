# Extensions Telemetry

## 📝 Overview

This NuGet package provides telemetry capabilities for your applications by configuring Serilog with defined parameters.

## 📦 Installation

To install the package, run the following command in the Package Manager Console:

```sh
Install-Package Jootl.Extensions.Telemetry
```

## 🚀 Usage

After installing the package, you can start using it by adding the necessary using directive and initializing the telemetry service in your application.

### Configuration

Add section `Jootl` under `Logging` in `appsettings.json`.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "Jootl": {
      "ApplicationName": "MyWebApplication",
      "Seq": {
        "ApiUrl": "http://192.168.1.1:8080",
        "ApiKey": "__MY_API_KEY__"
      }
    }
  }
}
```

### For Web/Api

```csharp
using Jootl.Extensions.Telemetry;

// ...
var builder = WebApplication.CreateBuilder(args);
// ...

// Add custom logging to the Host
builder.Host.AddCustomLogging();

// ...
var app = builder.Build();
// ...

// Use custom logging capabilities
app.UseCustomLogging();
```

### For Worker

```csharp
using Jootl.Extensions.Telemetry;

// ...
var builder = Host.CreateApplicationBuilder(args);
// ...

// Add custom logging to the services
builder.Services.AddCustomLogging(builder.Configuration);
```


## ✨ Features

- Configure Serilog as primary logging provider
- If Seq is enabled, send logs to the server

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

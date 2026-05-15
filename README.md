# netapi_restapi

A sample REST API built with .NET 10 minimal APIs, designed to run as an [Azure Functions custom handler](https://learn.microsoft.com/azure/azure-functions/functions-custom-handlers).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-tools?tabs=v4) (for running as a custom handler)

## API Endpoints

| Method | Route              | Description              |
|--------|--------------------|--------------------------|
| GET    | `/products`        | List all products        |
| GET    | `/products/{id}`   | Get a product by ID      |
| GET    | `/api/items`       | List all products        |

## Getting Started

### Publish and verify the API

1. **Publish the app**

   ```powershell
   dotnet publish -c Release -o out -r win-x64 --no-self-contained
   ```

2. **Run the published output** to verify the API works on its own:

   ```powershell
   cd out
   .\netapi_restapi.exe
   ```

   Confirm the endpoints respond (e.g. `GET http://localhost:5080/products`), then stop the process.

### Run as an Azure Functions custom handler

3. **Copy `host.json`** from the repo root into the `out` folder.

4. **Set the Functions worker runtime** environment variable:

   ```powershell
   $env:FUNCTIONS_WORKER_RUNTIME = "Custom"
   ```

5. **Start the Functions host** from the `out` directory:

   ```powershell
   cd out
   func start
   ```

### Test the endpoints

Use the included [`netapi_restapi.http`](netapi_restapi.http) file (supported by VS Code REST Client and Visual Studio) to send requests. Make sure the port number matches what the terminal prints on startup.

```
GET http://localhost:7071/api/items
GET http://localhost:7071/products
```

## Project Structure

| File                    | Purpose                                      |
|-------------------------|----------------------------------------------|
| `Program.cs`            | Application entry point and route definitions |
| `host.json`             | Azure Functions custom handler configuration  |
| `netapi_restapi.http`   | Sample HTTP requests for testing              |
| `netapi_restapi.csproj` | Project file (.NET 10, Serilog dependencies)  |

## Logging

The app uses [Serilog](https://serilog.net/) with two sinks:

- **Console** — all log output is written to stdout.
- **Rolling file** — daily log files are written to `logs/requests.log` (e.g. `requests20260514.log`).

### What gets logged

- **Startup** — an informational message with the port the server is listening on.
- **HTTP request logs** — every incoming request is logged automatically by `UseSerilogRequestLogging()`, including the HTTP method, path, response status code, and elapsed time. For example:

  ```
  [INF] HTTP GET /products responded 200 in 12.34 ms
  ```

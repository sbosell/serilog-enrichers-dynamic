# Serilog.Enrichers.Dynamic

Enriches Serilog events with information that you specify dynamically.  There are a lot of great specific enrichers but none that let you pull any information from the context of your application and/or request.
 
[![Build status](https://ci.appveyor.com/api/projects/status/pb8tb199yb59dkhf/branch/master?svg=true)](https://github.com/sbosell/serilog-enrichers-dynamic) [![NuGet Version](http://img.shields.io/nuget/v/Serilog.Enrichers.Dynamic.svg?style=flat)](https://www.nuget.org/packages/Serilog.Enrichers.Dynamic/)

Breaking changes.  I made some changes to the parameter names and ordering to keep it consistent with some of the other enrichers.  In the future I'll bump the major version.

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.Dynamic
```

Then, apply the enricher to your `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithDynamicProperty("MyProperty", ()=> {
    	return AppContext.GetSomething(); 
    	}, enrichWhenNullOrEmptyString: false,destructureObjects: false, minimumLevel: LogEventLevel.Fatal
    // ...other configuration...
    .CreateLogger();
```

or for instance if you wanted to log the entire Service Stack Session:

```csharp
Log.Logger = new LoggerConfiguration()
.Enrich.WithDynamicProperty("Session", () => {
  var propVal = "";
  if (HostContext.AppHost == null) return null; ;
    var req = HostContext.TryGetCurrentRequest();
    if (req != null && req.GetSession().IsAuthenticated)
    {
      propVal = req.GetSession().ToJson();  // prob not the best idea but hey
    }
    return !String.IsNullOrEmpty(propVal) ? propVal : null;
}).CreateLogger();
```

Perhaps you want to include a property in the log that would cause an alert to be issued to you from your centralized logging system:

```csharp
Log.Logger = new LoggerConfiguration()
   .Enrich.WithDynamicProperty("SendAlert", () =>
         {
             var req = HostContext.TryGetCurrentRequest();
             if (req.GetType().HasAttribute<AlertOnErrorAttribute>())
             {
                 return "true";
             }
             return null;
         }, enrichWhenNullOrEmptyString: false)
}).CreateLogger();

[AlertOnError]
public void Post(SomeRequest request) {
  // processing that might fail and this is important so we alert 
}

```

### Included enrichers

The package includes:

 * ` WithDynamicProperty(
          this LoggerEnrichmentConfiguration enrichmentConfiguration, string name, Func<string> valueProvider,  bool destructureObjects=false, bool enrichWhenNullOrEmptyString = false, LogEventLevel minimumLevel= LogEventLevel.Verbose)
        ` - adds a func delegate that you define to log whatever you require
 

Copyright © 2017  Provided under the Apache License, Version 2.0.

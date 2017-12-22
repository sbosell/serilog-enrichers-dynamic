# Serilog.Enrichers.Dynamic

Enriches Serilog events with information that you specify dynamically.  There are a lot of great specific enrichers but none that let you pull any information from the context of your application and/or request.
 
[![Build status](https://ci.appveyor.com/api/projects/status/pb8tb199yb59dkhf/branch/master?svg=true)](https://github.com/sbosell/serilog-enrichers-dynamic) [![NuGet Version](http://img.shields.io/nuget/v/Serilog.Enrichers.Dynamic.svg?style=flat)](https://www.nuget.org/packages/Serilog.Enrichers.Dynamic/)

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.Dynamic
```

Then, apply the enricher to you `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithDynamic(()=> {
    	return AppContext.GetSomething(); 
    	}, "DynamicProperty"
    // ...other configuration...
    .CreateLogger();
```

or for instance if you wanted to log the entire Service Stack Session:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithDynamic(() =>
         {
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

### Included enrichers

The package includes:

 * `WithDynamic()` - adds a func delegate that you define to log whatever you require
 

Copyright © 2017  Provided under the Apache License, Version 2.0.

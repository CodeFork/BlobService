## Welcome to BlobService ##

[![Build status](https://ci.appveyor.com/api/projects/status/83uh2apqs8xh92o1?svg=true)](https://ci.appveyor.com/project/Aram/blobservice)
[![Documentation Status](https://readthedocs.org/projects/blobservice/badge/?version=latest)](http://blobservice.readthedocs.io/en/latest/?badge=latest)

This repository contains BlobService for hosting on-permise. 
This is designed as plugable component and can be extended to store blobs in any storage.

The project is now in active development.

Full Documentation is available here.

[http://blobservice.readthedocs.io](http://blobservice.readthedocs.io)


#### Sample Usage/Configuration.

```
Install-Package BlobService.Core
Install-Package BlobService.MetaStore.EntityFrameworkCore
Install-Package BlobService.Storage.FileSystem
```

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
    	// Cors Policy
        services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowCredentials();

                });
            });
    
    
    	// Add DB Context
        services.AddDbContext<AppBlobServiceDbContext>(opts =>
            {
            	/ Example
                opts.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BS;Trusted_Connection=True;MultipleActiveResultSets=true");
            });
    
        // Adds Blob services core services
        services.AddBlobService(opts =>
        {
        	opts.CorsPolicyName = "AllowAllOrigins";
            opts.MaxBlobSizeInMB = 100;
        })
        
        // Registers EntityFramework stores for persisting blobs,containers metadata
        .AddEfMetaStores<BlobServiceContext, ContainerMeta, BlobMeta>()
        
        // Registers FileSystem Storage Service for persisting files in filesystem in specified path
        .AddFileSystemStorageService<FileSystemStorageService>(opts =>
        {
            opts.RootPath = @"C:\blobs";
        })
        .AddEfMetaStores<AppBlobServiceDbContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
        // Use BlobService Middlwares (mvc)
        app.UseBlobService();
    }
}
```

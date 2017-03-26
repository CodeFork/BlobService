﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Configuration
{
    public static class BlobServiceServiceCollectionExtensions
    {
        public static IBlobServiceBuilder AddBlobService(this IServiceCollection services)
        {
            services.AddMvc();
            var builder = new BlobServiceBuilder(services);
            return builder;
        }
    }
}
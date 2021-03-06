﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace StatisticalLearning.Api.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts => opts.EnableEndpointRouting = false);
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddSwaggerGen();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            var pathBase = string.Empty;
            if (_configuration.GetChildren().Any(i => i.Key == "pathBase"))
            {
                pathBase = _configuration["pathBase"];
                app.UsePathBase(pathBase);
            }

            app.UseSwagger(); 
            app.UseSwaggerUI(c =>
            {
                var edp = "/swagger/v1/swagger.json";
                if (!string.IsNullOrWhiteSpace(pathBase))
                {
                    edp = $"{pathBase}{edp}";
                }

                c.SwaggerEndpoint(edp, "My API V1");
            });
            app.UseStaticFiles();
            app.UseForwardedHeaders();
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
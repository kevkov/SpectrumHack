using MapApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MapApi
{
    using MapApiCore.Interfaces;
    using Repositories;
    using Services.Interfaces;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IPollutionRepository, PollutionRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<IJourneyRepository, JourneyRepository>();
            services.AddScoped<IIntersectionService, IntersectionService>();
            //services.AddScoped<IPollutionService, AirVisualService>();

            var baseUri = Configuration.GetSection("BaseApiURIForDistanceMatrix");
            var apiKey = Configuration.GetSection("ApiKey");

            services.AddScoped<IDirectionService, DirectionService>(
                s => new DirectionService(new HttpClient
                {
                    BaseAddress = new Uri(baseUri.Value)
                }, apiKey.Value));

            services.AddScoped<IPollutionService, LondonAirService>();

            //services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin());
                //builder.WithOrigins("http://localhost:52883", "http://localhost:52811", "https://spectrummapapi.azurewebsites.net", "https://spectrummapapi2.azurewebsites.net"));
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

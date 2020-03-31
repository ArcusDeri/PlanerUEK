using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlanerUek.Storage.Interfaces;
using PlanerUek.Storage.Providers;
using PlanerUek.Storage.Repositories;
using PlanerUek.Website.Configuration;
using PlanerUek.Website.Services;

namespace PlanerUek.Website
{
    public class Startup
    {
        private readonly IPlanerConfig _planerConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _planerConfig = new AppConfig(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var studentGroupsStorageConnectionString = _planerConfig.GetStudentGroupsStorageConnectionString();
            var studentGroupScheduleEndpointTemplate = _planerConfig.GetStudentGroupScheduleTemplate();
            var googleDataStoreConnectionString = _planerConfig.GetGoogleDataStoreConnectionString();

            services.AddControllersWithViews();
            services.AddTransient<IPlanerConfig, AppConfig>();
            services.AddTransient<IGoogleCalendar, GoogleCalendar>();
            services.AddTransient<IDataStore>(x => 
                new GoogleDataStoreRepository(googleDataStoreConnectionString));
            services.AddTransient<IStudentGroupsRepository>(x =>
                new StudentGroupsRepository(studentGroupsStorageConnectionString));
            services.AddTransient<IStudentGroupScheduleProvider>(x =>
                new StudentGroupScheduleProvider(studentGroupScheduleEndpointTemplate));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
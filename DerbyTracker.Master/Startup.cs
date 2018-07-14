using DerbyTracker.Common.Messaging.CommandHandlers;
using DerbyTracker.Common.Services;
using DerbyTracker.Master.SignalR;
using DerbyTracker.Messaging.Callbacks;
using DerbyTracker.Messaging.Dispatchers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DerbyTracker.Master
{
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //TODO: Verify CORS settings for signalr clients
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                        //.WithOrigins("http://localhost:44347")
                        .AllowAnyOrigin()
                        .AllowCredentials();
                }));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSignalR();
            services.AddScoped<SignalRCallbackFactory>();
            services.AddSingleton<ICallbackFactory, SignalRCallbackFactory>();
            services.AddSingleton<IDispatcher, ImmediateDispatcher>();
            services.AddSingleton<IBoutFileService, BoutFileService>();
            services.AddSingleton<INodeService, NodeService>();
            services.AddTransient<HandlerRegistrar>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseSignalR(s => s.MapHub<WheelHub>("/wheelhub"));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            var registrar = serviceProvider.GetService<HandlerRegistrar>();
            var dispatcher = serviceProvider.GetService<IDispatcher>() as ImmediateDispatcher;
            registrar.RegisterHandlers(dispatcher);
        }
    }
}

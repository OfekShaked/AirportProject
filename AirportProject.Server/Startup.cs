using AirportProject.BL;
using AirportProject.BL.Interfaces;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using AirportProject.Server.Hubs;
using AirportProject.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirportProject.Server
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddCors(options => options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000").AllowCredentials();
            }));
            services.AddSignalR(e =>
            {
                e.MaximumReceiveMessageSize = 102400000;
                e.EnableDetailedErrors = true;
            }).AddJsonProtocol(options =>
                 {
                     options.PayloadSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                 }); ;
            services.AddSingleton<IMongoContext>(x=> new MongoContext(Configuration));
            services.AddSingleton<IUnitOfWork>(x => new UnitOfWork(x.GetRequiredService<IMongoContext>()));
            services.AddSingleton<IDataAccess, DataAccess>(x => new DataAccess(x.GetRequiredService<IMongoContext>()));
            services.AddSingleton<GeneralLogic>(x=>new GeneralLogic(x.GetRequiredService<IDataAccess>(),x.GetRequiredService<IUnitOfWork>()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AirportProject.Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirportProject.Server v1"));
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AirportHub>("/hubs/airport");
                endpoints.MapHub<SimulatorHub>("/hubs/simulator");
            });
            var logic = app.ApplicationServices.GetService<GeneralLogic>();
            logic.SetNotifySimulatorUpdates(new NotifySimulatorUpdates(app.ApplicationServices.GetRequiredService<IHubContext<SimulatorHub>>()));
            logic.SetNotifyUpdated(new NotifyUpdates(app.ApplicationServices.GetRequiredService<IHubContext<AirportHub>>()));
            logic.CreateBasicAirport();
        }
    }
}

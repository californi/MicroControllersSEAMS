using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Interfaces;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Util;
using MediatR;
using MetaController.Monitor.Consumers;
using MetaController.Monitor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MetaController.Monitor
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
            services.AddTransient<IMonitorService, MetaControllerMonitorService>();
            services.AddScoped<RequiredAdaptationConsumer>();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddMvcCore().AddApiExplorer();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RequiredAdaptationConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {

                    var host = cfg.Host(new Uri($"rabbitmq://localhost"), hostConfig =>
                    {
                        hostConfig.Username("guest");
                        hostConfig.Password("guest");
                    });

                    cfg.ReceiveEndpoint(host, "update", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(mr => mr.Interval(2, 100));

                        ep.ConfigureConsumer<RequiredAdaptationConsumer>(provider);
                    });
                }));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetaController Monitor Microservice", Version = "v1" });
            });

            services.AddMediatR(typeof(Startup));


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetaController Monitor Microservice V1");
            });

            app.UseMvc();


            var bus = app.ApplicationServices.GetService<IBusControl>();
            var busHandle = TaskUtil.Await(() =>
            {
                return bus.StartAsync();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                busHandle.Stop();
            });
        }

    }
}

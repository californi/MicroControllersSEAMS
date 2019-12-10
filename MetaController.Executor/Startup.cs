using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Interfaces;
using MetaController.Executor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MassTransit;
using Microsoft.OpenApi.Models;
using MediatR;
using MassTransit.Util;

namespace MetaController.Executor
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
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IExecutorService, ExecutorService>();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddMvcCore().AddApiExplorer();

            services.AddMassTransit(x => {

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => {

                    var host = cfg.Host(new Uri($"rabbitmq://localhost"), hostConfig => {
                        hostConfig.Username("guest");
                        hostConfig.Password("guest");
                    });
                }));
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetaController Executor ", Version = "v1" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetaController Executor Microservice V1");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller.Knowledge.Services;
using core.Interfaces;
using MassTransit;
using MassTransit.Util;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

namespace Controller.Knowledge
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
            services.AddTransient<IKnowledgeService, KnowledgeService>();

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Knowledge Microservice", Version = "v1" });
            });

            services.AddMediatR(typeof(Startup));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationLifetime lifetime)
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Knowledge Microservice V1");
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

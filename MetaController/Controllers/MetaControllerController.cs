using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;
using core.Interfaces;
using MassTransit;
using MetaController.Consumers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MetaController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaControllerController : Controller
    {
        private readonly IMonitorService _monitorService;
        private readonly IBusControl _bus;
        private readonly IConfiguration _config;

        public MetaControllerController(IMonitorService monitorService, IBusControl bus, IConfiguration config)
        {
            _monitorService = monitorService;
            _bus = bus;
            _config = config;
        }



        /// <summary>
        /// xxxx
        /// </summary>
        /// <param name="xxxx">xxxxx</param>
        /// <returns>xxxxx</returns>
        [HttpPost("/api/monitor")]
        public async Task<IActionResult> Monitor()
        {
            RequiredAdaptation obj = new RequiredAdaptation();

            int res = await _monitorService.SendMessageToAnalyser(obj);

            Uri uri = new Uri($"rabbitmq://{_config.GetValue<string>("RabbitMQHostName")}/update");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(uri);

                cfg.ReceiveEndpoint("update", e =>
                {
                    e.Consumer(() => new RequiredAdaptationConsumer(_monitorService));
   
                });
            });


            return Ok(res);
        }
    }
}
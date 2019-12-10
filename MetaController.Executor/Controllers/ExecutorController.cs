using core.Entities;
using core.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetaController.Executor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutorController : ControllerBase
    {
        private readonly IExecutorService _executorService;
        private readonly IBusControl _bus;
        private readonly IConfiguration _config;

        public ExecutorController(IExecutorService executorService, IBusControl bus, IConfiguration config)
        {
            _executorService = executorService;
            _bus = bus;
            _config = config;
        }

        /// <summary>
        /// .....
        /// </summary>
        /// <param name="RequiredAdaptation">The data needed to an adaptation</param>
        /// <returns></returns>
        [HttpPost("/api/executeplanning")]
        public async Task<IActionResult> executeplanning(RequiredAdaptation requiredAdaptation)
        {
            IEnumerable<MicroController> microControllers = await _executorService.GetPlanning(requiredAdaptation);

            //foreach (var mc in microControllers)
           // {
                //if (mc.Id == 1) 
                //{
                    Uri uri = new Uri($"rabbitmq://localhost/regressiontest");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    await endPoint.Send(requiredAdaptation);
                //}

                //if (mc.Id == 2)
                //{
                    Uri uri2 = new Uri($"rabbitmq://localhost/errerdetection");
                    var endPoint2 = await _bus.GetSendEndpoint(uri2);
                    await endPoint2.Send(requiredAdaptation);
                //}

                //if (mc.Id == 3)
                //{

                //}
            //}

            return Ok(true);
        }
    }
}

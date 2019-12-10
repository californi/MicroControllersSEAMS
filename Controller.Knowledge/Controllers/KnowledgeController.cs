using core.Entities;
using core.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller.Knowledge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeController : ControllerBase
    {
        private readonly IKnowledgeService _knowledgeService;
        private readonly IBusControl _bus;
        private readonly IConfiguration _config;

        public KnowledgeController(IKnowledgeService knowledgeService, IBusControl bus, IConfiguration config)
        {
            _knowledgeService = knowledgeService;
            _bus = bus;
            _config = config;
        }

        /// <summary>
        /// Generate a new required adaptation from Target system. The target system uses this action to generate a new kownledge
        /// </summary>
        /// <param name="RequiredAdaptation">The data needed to an adaptation</param>
        /// <returns>The required adaptation</returns>
        [HttpPost("/api/update")]
        public async Task<IActionResult> Update(RequiredAdaptation requiredAdaptation)
        {
            RequiredAdaptation res = await _knowledgeService.UpdateKnowledge(requiredAdaptation);

            Uri uri = new Uri($"rabbitmq://{_config.GetValue<string>("RabbitMQHostName")}/update");

            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(res);

            return Ok(res);
        }

    }
}

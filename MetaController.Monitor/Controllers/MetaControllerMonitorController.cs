using core.Entities;
using core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetaController.Monitor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaControllerMonitorController : ControllerBase
    {
        private readonly IMonitorService _monitorService;

        public MetaControllerMonitorController(IMonitorService monitorService) 
        {
            _monitorService = monitorService;
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/datamonitoring")]
        [HttpGet]
        public async Task<RequiredAdaptation> GetGetDataOfMonitoring()
        {
            return await _monitorService.GetDataOfMonitoring();
        }

    }
}

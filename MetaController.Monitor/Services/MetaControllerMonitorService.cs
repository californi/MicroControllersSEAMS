using core.Entities;
using core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetaController.Monitor.Services
{
    public class MetaControllerMonitorService : IMonitorService
    {
        public Task<RequiredAdaptation> GetDataOfMonitoring()
        {
            throw new NotImplementedException();
        }

        public Task<int> SendMessageToAnalyser(RequiredAdaptation requiredAdaptation)
        {
            throw new NotImplementedException();
        }
    }
}

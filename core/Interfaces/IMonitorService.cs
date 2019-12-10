using core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace core.Interfaces
{
    public interface IMonitorService
    {
        Task<int> SendMessageToAnalyser(RequiredAdaptation requiredAdaptation);
    }
}

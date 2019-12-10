using core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace core.Interfaces
{
    public interface IExecutorService
    {
        Task<IEnumerable<MicroController>> GetPlanning(RequiredAdaptation requiredAdaptation);
    }
}

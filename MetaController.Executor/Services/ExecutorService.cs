using core.Entities;
using core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetaController.Executor.Services
{
    public class ExecutorService : IExecutorService
    {
        private IEnumerable<MicroController> _selectedMicroControllers;

        public ExecutorService()
        {
        }        

        public async Task<IEnumerable<MicroController>> GetPlanning(RequiredAdaptation requiredAdaptation)
        {

            List<MicroController> microControllers = new List<MicroController>();

            return await Task.FromResult(microControllers);
        }
    }
}

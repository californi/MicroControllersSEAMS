using core.Entities;
using core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controller.Knowledge.Services
{
    public class KnowledgeService : IKnowledgeService
    {
        public async Task<RequiredAdaptation> UpdateKnowledge(RequiredAdaptation requiredAdaptation)
        {
            RequiredAdaptation newObj = new RequiredAdaptation();
            newObj.Id = 55;
            newObj.Description = "We are trying!";

            return await Task.FromResult(newObj);
        }

    }
}

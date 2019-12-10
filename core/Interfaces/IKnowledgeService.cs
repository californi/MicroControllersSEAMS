using core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace core.Interfaces
{
    public interface IKnowledgeService
    {
        Task<RequiredAdaptation> UpdateKnowledge(RequiredAdaptation requiredAdaptation);
    }
}

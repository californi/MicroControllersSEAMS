using core.Entities;
using core.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetaController.Monitor.Consumers
{
    public class RequiredAdaptationConsumer : IConsumer<RequiredAdaptation>
    {
        private readonly IMonitorService _monitor;

        public RequiredAdaptationConsumer(IMonitorService monitor) 
        {
            _monitor = monitor;
        }

        /// <summary>
        /// When an adaptation is performed in the target system, an event is fired
        /// This function consume the event, that is a Monitor role
        /// It sends a message to an analyser
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<RequiredAdaptation> context)
        {
            Console.WriteLine("Today...." + context.Message);
            await _monitor.SendMessageToAnalyser(context.Message);
        }
    }
}

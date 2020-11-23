using System;
using System.Threading.Tasks;
using NServiceBus;
using Publisher.Messages.Events;

namespace MessageSubscriber.Handlers
{
    public class SmartMeterRegisteredHandler : IHandleMessages<SmartMeterRegistered>
    {
        public Task Handle(SmartMeterRegistered message, IMessageHandlerContext context)
        {
            Console.WriteLine("Smart Meter Registered");
            Console.ReadKey();

            return Task.CompletedTask;
        }
    }
}

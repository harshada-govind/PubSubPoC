using System;
using System.Threading.Tasks;
using NServiceBus;
using Publisher.Messages.Events;

namespace MessagePublisher.ChangeOfSupplyProcess
{
    public class CosGain
    {
        public async Task  PublishEvent(IEndpointInstance endpointInstance)
        {
            if (DateTime.Today.Hour == 23 && DateTime.Today.Minute == 30)
            {
                var registerSmartMeter = new SmartMeterRegistered()
                {
                    RegistrationId = "1234",
                    Mpxn = "1234567890123",
                    SupplyStartDate = DateTime.Now.AddDays(5)

                };
                await endpointInstance.Publish(registerSmartMeter).ConfigureAwait(false);
            }
        }
    }
}

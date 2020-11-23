using System;
using System.Threading.Tasks;
using System.Timers;
using NServiceBus;
using Publisher.Messages.Events;

namespace MessagePublisher.ChangeOfSupplyProcess
{
    public class CosGain
    {
        public async Task  PublishEvent(IEndpointInstance endpointInstance)
        {
            var myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(NextCosGainPublisher);
            myTimer.Interval = 5000;
            myTimer.Enabled = true;

            var registerSmartMeter = new SmartMeterRegistered()
            {
                RegistrationId = "1234",
                Mpxn = "1234567890123",
                SupplyStartDate = DateTime.Now.AddDays(5)

            };
            await endpointInstance.Publish(registerSmartMeter).ConfigureAwait(false);
        }

        private void NextCosGainPublisher(object source, ElapsedEventArgs e) 
        {
            Console.WriteLine("Time Elapsed: " + e.SignalTime.ToString());
        }
    }
}

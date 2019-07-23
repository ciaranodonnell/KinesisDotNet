using COD.Kinesis.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DemoApp.EventConsumer
{
    public class EventConsumerService : IDisposable
    {
        private IKinesisClient kinesisClient;
        private Thread webThread;
        private ISubscription subscription;

        public EventConsumerService(IKinesisClient kinesisClient)
        {

            this.kinesisClient = kinesisClient;

            HostTheWebEndpoint();

            StartSubscribingToStream();
        }

        private void StartSubscribingToStream()
        {
            this.subscription = kinesisClient.SubscribeToStream("eventstream1", "DemoConsumer");
        }

        public void Dispose()
        {
            if(webThread != null)
            {
                DemoApp.Web.Program.StopWebsite().Wait();
                subscription.Dispose();
            }
        }

        private void HostTheWebEndpoint()
        {
            this.webThread = new Thread(new ThreadStart(() =>
            {

                DemoApp.Web.Program.RunWebsite(new string[0]);
            }));
            webThread.Start();
        }
    }
}

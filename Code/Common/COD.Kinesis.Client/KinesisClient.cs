using COD.Kinesis.Client.Serialization;
using System;
using System.Threading.Tasks;

namespace COD.Kinesis.Client
{
    public class KinesisClient
    {
        private string appName;
        private string regionName;

        public KinesisClient(string kinesisApplicationName, string regionName)
        {
            this.appName = kinesisApplicationName;
            this.regionName = regionName;
        }

        public ISubscription SubscribeToStream(string streamName, string consumerAppName)
        {
            return new KinesisSubscription(streamName, this.regionName, this.appName, consumerAppName);
        }


        public Task SendAMessage<TMessage>(string streamName, TMessage message, IMessageSerializer serializer)
        {
            return Task.CompletedTask;
        }
    }
}

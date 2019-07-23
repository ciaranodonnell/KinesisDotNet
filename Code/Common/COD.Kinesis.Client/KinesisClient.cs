using Amazon.Kinesis.Model;
using COD.Kinesis.Client.Serialization;
using System;
using System.Threading.Tasks;

namespace COD.Kinesis.Client
{
    public class KinesisClient : IKinesisClient
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


        public IMessageProducer<TMessage> GetMessageSender<TMessage>(string streamName, IMessageSerializer serializer, Func<TMessage, string> partitionKeyFunc = null)
        {
            if (partitionKeyFunc == null)
            {
                partitionKeyFunc = (m) => m.GetHashCode().ToString();
            }
            return new SimpleRecordSender<TMessage>(regionName, streamName, serializer, partitionKeyFunc);
        }
    }
}

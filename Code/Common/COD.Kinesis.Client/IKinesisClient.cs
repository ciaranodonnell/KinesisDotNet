using System;
using COD.Kinesis.Client.Serialization;

namespace COD.Kinesis.Client
{
    public interface IKinesisClient
    {
        IMessageProducer<TMessage> GetMessageSender<TMessage>(string streamName, IMessageSerializer serializer, Func<TMessage, string> partitionKeyFunc = null);
        ISubscription SubscribeToStream(string streamName);
    }
}
using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using COD.Kinesis.Client.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace COD.Kinesis.Client
{
    internal class SimpleRecordSender<TMessage> : IMessageProducer<TMessage>
    {

        private readonly AmazonKinesisClient kinesisClient;
        private readonly IMessageSerializer serializer;
        private string streamName;
        private Func<TMessage, string> partitionKeyFunc;

        public SimpleRecordSender(string regionName, string streamName, IMessageSerializer serializer, Func<TMessage, string> partitionKeyFunc)
        {
            RegionEndpoint endpoint = RegionEndpoint.USEast1;
            switch (regionName)
            {
                //TODO: Implement the connection to more regions
                case "useast1":
                case "us-east-1":
                    endpoint = RegionEndpoint.USEast1;
                    break;
            }

            kinesisClient = new AmazonKinesisClient(endpoint);

            this.serializer = serializer;

            if (DoesKinesisStreamExists(kinesisClient, streamName))
            {


            }
            else
            {
                throw new ArgumentException($"The Kinesis stream {streamName} does not exist");
            }
            this.streamName = streamName;
            this.partitionKeyFunc = partitionKeyFunc;
        }

        private bool DoesKinesisStreamExists(AmazonKinesisClient kinesisClient, string streamName)
        {
            var describeStreamReq = new DescribeStreamRequest();
            describeStreamReq.StreamName = streamName;
            var describeResult = kinesisClient.DescribeStreamAsync(describeStreamReq).Result;
            string streamStatus = describeResult.StreamDescription.StreamStatus;
            if (streamStatus == StreamStatus.ACTIVE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendMessage(TMessage message)
        {
            SendMessageAsync(message).Wait();
        }

        public async Task SendMessageAsync(TMessage message)
        {
            PutRecordRequest requestRecord = new PutRecordRequest();
            requestRecord.StreamName = streamName;

            requestRecord.Data = new MemoryStream(serializer.SerializeToArray(message));

            requestRecord.PartitionKey = partitionKeyFunc(message);

            await kinesisClient.PutRecordAsync(requestRecord);
        }

        public void Dispose()
        {
            kinesisClient.Dispose();
        }
    }
}

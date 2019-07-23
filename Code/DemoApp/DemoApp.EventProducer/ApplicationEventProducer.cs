using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.EventProducer
{
    public static class ApplicationEventProducer
    {
        //TODO : Make it Configurable
        private static readonly AmazonKinesisClient kinesisClient = new AmazonKinesisClient(RegionEndpoint.USEast1);
        private static readonly string myStreamName = "mpfStream";
        private static readonly int myStreamSize = 1;


        public static async Task<bool> PostEvent(string applicationEvent)
        {
            // Create Stream
            var createStreamRequest = new CreateStreamRequest();
            createStreamRequest.StreamName = myStreamName;
            createStreamRequest.ShardCount = myStreamSize;
            var createStreamReq = createStreamRequest;
            try
            {
                await kinesisClient.CreateStreamAsync(createStreamReq);
            }
            catch (Exception)
            {
                // Eat for now
            }

            // Check Stream Setup Active
            DescribeStreamRequest describeStreamReq = new DescribeStreamRequest();
            describeStreamReq.StreamName = myStreamName;
            var describeResult = await kinesisClient.DescribeStreamAsync(describeStreamReq);
            string streamStatus = describeResult.StreamDescription.StreamStatus;
            if (streamStatus == StreamStatus.ACTIVE)
            {
                PutRecordRequest requestRecord = new PutRecordRequest();
                requestRecord.StreamName = myStreamName;
                requestRecord.Data = new MemoryStream(Encoding.UTF8.GetBytes(applicationEvent));
                requestRecord.PartitionKey = "partitionKey-1";
                await kinesisClient.PutRecordAsync(requestRecord);
            }
            else
            {
                throw new InvalidOperationException($"{myStreamName} not active.");
            }

            return true;
        }
    }
}

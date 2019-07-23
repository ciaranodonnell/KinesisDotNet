using Amazon.Kinesis.ClientLibrary;
using COD.Kinesis.Client;
using COD.Kinesis.Client.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.EventConsumer
{
    public class DemoAppEventProcessor : SimpleRecordProcessor<DemoAppEvent>
    {

        public DemoAppEventProcessor(IMessageSerializer serializer) : base(serializer)
        {

        }

        protected override void HandleMessage(DemoAppEvent message)
        {
            Console.WriteLine("Message Received: " + Newtonsoft.Json.JsonConvert.SerializeObject(message));
        }

        protected override void HandleProcessingFailure(Record record, Exception exceptionEncountered)
        {
            Console.WriteLine($"Message Error: {exceptionEncountered.ToString()} - " + Encoding.UTF8.GetString(record.Data));
        }
    }
}

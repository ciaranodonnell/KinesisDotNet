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
            
        }

        protected override void HandleProcessingFailure(Record record)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace COD.Kinesis.Client
{
    public class KinesisSubscription : ISubscription
    {
        private Process process;

        internal KinesisSubscription(string streamName, string region, string applicationName, string consumerAppCommandLine)
        {
            this.process = BootStrapper.StartJavaSubscriptionProcess(new KinesisConsumerOptions
            {
                StreamName = streamName,
                StartPosition = KinesisStartPosition.TRIM_HORIZON,
                ApplicationName = applicationName,
                RegionName = region,
                ConsumerProgramCommandLine = consumerAppCommandLine

            });
            
        }



        public void Dispose()
        {
            try
            {
                process.Kill();
            }
            catch (Exception ex)
            {
                //TODO: Log that we cant kill the process
            }
        }
    }
}

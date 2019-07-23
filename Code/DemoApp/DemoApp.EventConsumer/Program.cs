using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Kinesis.ClientLibrary;
using COD.Kinesis.Client;
using COD.Kinesis.Client.Serialization;

namespace DemoApp.EventConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            LaunchHelper.ProcessLaunch(args, 
                new Dictionary<string, Func<IShardRecordProcessor>> 
{
                    { "eventstream1", ()=> new DemoAppEventProcessor(new JSONSerializer())}
                }, InitialMain
                );
    
        }


        public static void InitialMain(string[] args)
        {

        }


    }
}

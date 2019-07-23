using System;
using System.Collections.Generic;
using Amazon.Kinesis.ClientLibrary;
using COD.Kinesis.Client.Serialization;
using CommandLine;

namespace COD.Kinesis.Client
{
    public static class LaunchHelper
    {

        [Verb("consumer")]
        class ProcessLaunchOptions
        {
            [Option('s', "stream")]
            public string StreamName { get; set; }

            [Option('h', "signalrhub")]
            public string SignalRHubAddress { get; set; }

        }


        public static void ProcessLaunch(string[] args, Dictionary<string, Func<IShardRecordProcessor>> dictionary, Action<string[]> fallbackMain)
        {

            CommandLine.Parser.Default.ParseArguments<ProcessLaunchOptions>(args)
   .WithParsed<ProcessLaunchOptions>(opts =>
   {
       if (!dictionary.ContainsKey(opts.StreamName))
       {
           throw new ApplicationException("Dont have a processor for this stream");
       }
       else
       {

           var recordProcessor = dictionary[opts.StreamName]();
           var process = KclProcess.Create(recordProcessor);
           process.Run();

       }


   }
   )
   .WithNotParsed<ProcessLaunchOptions>((errs) => fallbackMain(args));


        }
    }
}
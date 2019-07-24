using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            public const string StreamNameIdentifier = "stream";
            public const string SignalRHubAddressIdentifier = "signalrhub";


            [Option('s', StreamNameIdentifier, Required = true)]
            public string StreamName { get; set; }

            [Option('h', SignalRHubAddressIdentifier)]
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
               .WithNotParsed<ProcessLaunchOptions>((errs) =>
               {

                   fallbackMain(args);
               }
               );


        }

        internal static string GetConsumerProcessCommandLine(string streamName, string[] requiredArgs = null)
        {
            string commandLine;
            if (Process.GetCurrentProcess().ProcessName == "dotnet")
            {
                commandLine = "dotnet " + BootStrapper.CleanFilePath(new StackTrace(2).GetFrame(0).GetMethod().DeclaringType.Assembly.Location);
            }
            else
            {
                commandLine = Process.GetCurrentProcess().ProcessName;
            }

            if (requiredArgs != null)
            {
                commandLine += " " + string.Join(" ", requiredArgs);
            }

            commandLine += " consumer --" + ProcessLaunchOptions.StreamNameIdentifier;
            commandLine += " " + streamName;


            return commandLine;


        }

       
    }
}

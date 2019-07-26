using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Amazon.Kinesis.ClientLibrary;
using COD.Kinesis.Client.Serialization;
using CommandLine;

namespace COD.Kinesis.Client
{

    /// <summary>
    /// A helper class to make it easy to determine if the process is being launched normally or to process a stream. If its to process a stream then it handles creating the stream processor and running it
    /// </summary>
    public static class LaunchHelper
    {



        [Verb(ConsumerVerbIdentifier)]
        class ProcessLaunchOptions
        {
            public const string ConsumerVerbIdentifier = "consumer";
            public const string StreamNameIdentifier = "stream";
            public const string SignalRHubAddressIdentifier = "signalrhub";


            [Option('s', StreamNameIdentifier, Required = true)]
            public string StreamName { get; set; }

            [Option('h', SignalRHubAddressIdentifier)]
            public string SignalRHubAddress { get; set; }

        }


        /// <summary>
        /// This method handle the process being launched. It will determine if the launch is to the process a stream or is a normal launch and perform the appropriate action
        /// </summary>
        /// <param name="args">The args the program has been launched with</param>
        /// <param name="streamToProcessorFactoryMapping">A dictionary of StreamName to Funcs that create stream processors.</param>
        /// <param name="fallbackMain">If this is not a stream processing launch, this is the fallback Main method that will be called to load the process normally</param>
        /// <exception cref="ArgumentException">If the stream that we're being asked to process is not a key in streamToProcessorFactoryMapping parameter</exception>
        public static void ProcessLaunch(string[] args, Dictionary<string, Func<IShardRecordProcessor>> streamToProcessorFactoryMapping, Action<string[]> fallbackMain)
        {
            //Parse the command line argument and see if we are being launched properly or as a consumer
            CommandLine.Parser.Default.ParseArguments<ProcessLaunchOptions>(args)
               .WithParsed<ProcessLaunchOptions>(opts =>
               {
                   //this means we are a consumer with a stream
                   if (!streamToProcessorFactoryMapping.ContainsKey(opts.StreamName))
                   {
                       //we havent been told what to run for this stream
                       throw new ArgumentException($"Dont have a processor for this stream: {opts.StreamName}");
                   }
                   else
                   {
                       //Run the factory func for the recordProcessor and create a process with it.
                       var recordProcessor = streamToProcessorFactoryMapping[opts.StreamName]();
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

        /// <summary>
        /// Generates the command line for the Java daemon to start this process with. 
        /// </summary>
        /// <param name="streamName">the name of the stream that is going to be processed</param>
        /// <param name="requiredArgs">any specific arguments that the process needs, perhaps like a DB connection or something like that</param>
        /// <returns>The command line that should be put in the properties file for the Java process</returns>
        internal static string GetConsumerProcessCommandLine(string streamName, string[] requiredArgs = null)
        {
            StringBuilder commandLine = new StringBuilder();

            if (Process.GetCurrentProcess().ProcessName == "dotnet")
            {
                var path = new StackTrace(2).GetFrame(0).GetMethod().DeclaringType.Assembly.Location;
                path = Path.GetFileName(path);
                commandLine.Append("dotnet ").Append(BootStrapper.CleanFilePath(path));
            }
            else
            {
                commandLine.Append(Process.GetCurrentProcess().ProcessName);
            }

            if (requiredArgs != null)
            {
                commandLine.Append(" ");
                foreach (var arg in requiredArgs)
                    commandLine.Append(" ").Append(arg);
            }


            commandLine.Append(" ").Append(ProcessLaunchOptions.ConsumerVerbIdentifier).Append(" --").Append(ProcessLaunchOptions.StreamNameIdentifier).Append(" ").Append(streamName);

            return commandLine.ToString();


        }


    }
}

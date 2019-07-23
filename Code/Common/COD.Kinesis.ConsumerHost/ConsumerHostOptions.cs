using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace COD.Kinesis.ConsumerHost
{
   public class ConsumerHostOptions
    {
        [Option('s', Required =true, HelpText ="This is the stream the messages are being received on")]
        public string StreamName { get; set; }


    }
}

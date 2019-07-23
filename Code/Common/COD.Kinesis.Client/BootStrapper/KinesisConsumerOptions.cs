

namespace COD.Kinesis.Client
{
    /// <summary>
    /// Options for connecting to Kinesis
    /// </summary>
    internal class KinesisConsumerOptions
    {

        public string ConsumerProgramCommandLine { get; set; }
        public string StreamName { get; set; }
        public string ApplicationName { get; set; }
        public string RegionName { get; set; }
        public KinesisStartPosition StartPosition { get; set; }

        public string JavaLocation { get; set; }

        public string PropertiesFile { get; set; }

        public string JarFolder { get; set; }

        public string LogbackConfiguration { get; set; }
    }
}

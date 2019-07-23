using CommandLine;

namespace ApplicationService.EventConsumer
{
    /// <summary>
    /// Command line options.
    /// </summary>
    class Options
    {
        [Option('j', "java", Required = false,
            HelpText =
                "Path to java, used to start the KCL multi-lang daemon. Attempts to auto-detect if not specified.")]
        public string JavaLocation { get; set; }

        [Option('p', "properties", Required = true, HelpText = "Path to properties file used to configure the KCL.")]
        public string PropertiesFile { get; set; }

        [Option("jar-folder", Required = false, HelpText = "Folder to place required jars in. Defaults to ./jars")]
        public string JarFolder { get; set; }

        [Option('e', "execute", HelpText =
            "Actually launch the KCL. If not specified, prints the command used to launch the KCL.")]
        public bool ShouldExecute { get; set; }

        [Option('l', "log-configuration", Required = false, HelpText = "A Logback XML configuration file")]
        public string LogbackConfiguration { get; set; }
    }
}

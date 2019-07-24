//
// Copyright 2019 Amazon.com, Inc. or its affiliates. All Rights Reserved.
//
// Licensed under the Amazon Software License (the "License").
// You may not use this file except in compliance with the License.
// A copy of the License is located at
//
//  http://aws.amazon.com/asl/
//
// or in the "license" file accompanying this file. This file is distributed
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing
// permissions and limitations under the License.



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace COD.Kinesis.Client
{

    internal class BootStrapper
    {
        private static readonly OperatingSystemCategory CURRENT_OS = Environment.OSVersion.ToString().Contains("Unix")
            ? OperatingSystemCategory.UNIX
            : OperatingSystemCategory.WINDOWS;

        private static readonly List<MavenPackage> MAVEN_PACKAGES = new List<MavenPackage>()
        {
            new MavenPackage("software.amazon.kinesis", "amazon-kinesis-client-multilang", "2.1.2"),
            new MavenPackage("software.amazon.kinesis", "amazon-kinesis-client", "2.1.2"),
            new MavenPackage("software.amazon.awssdk", "kinesis", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "aws-cbor-protocol", "2.4.0"),
            new MavenPackage("com.fasterxml.jackson.dataformat", "jackson-dataformat-cbor", "2.9.8"),
            new MavenPackage("software.amazon.awssdk", "aws-json-protocol", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "dynamodb", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "cloudwatch", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "netty-nio-client", "2.4.0"),
            new MavenPackage("io.netty", "netty-codec-http", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-codec-http2", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-codec", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-transport", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-resolver", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-common", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-buffer", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-handler", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-transport-native-epoll", "4.1.32.Final"),
            new MavenPackage("io.netty", "netty-transport-native-unix-common", "4.1.32.Final"),
            new MavenPackage("com.typesafe.netty", "netty-reactive-streams-http", "2.0.0"),
            new MavenPackage("com.typesafe.netty", "netty-reactive-streams", "2.0.0"),
            new MavenPackage("org.reactivestreams", "reactive-streams", "1.0.2"),
            new MavenPackage("com.google.guava", "guava", "26.0-jre"),
            new MavenPackage("com.google.code.findbugs", "jsr305", "3.0.2"),
            new MavenPackage("org.checkerframework", "checker-qual", "2.5.2"),
            new MavenPackage("com.google.errorprone", "error_prone_annotations", "2.1.3"),
            new MavenPackage("com.google.j2objc", "j2objc-annotations", "1.1"),
            new MavenPackage("org.codehaus.mojo", "animal-sniffer-annotations", "1.14"),
            new MavenPackage("com.google.protobuf", "protobuf-java", "2.6.1"),
            new MavenPackage("org.apache.commons", "commons-lang3", "3.8.1"),
            new MavenPackage("org.slf4j", "slf4j-api", "1.7.25"),
            new MavenPackage("io.reactivex.rxjava2", "rxjava", "2.1.14"),
            new MavenPackage("software.amazon.awssdk", "sts", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "aws-query-protocol", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "protocol-core", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "profiles", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "sdk-core", "2.4.0"),
            new MavenPackage("com.fasterxml.jackson.core", "jackson-core", "2.9.8"),
            new MavenPackage("com.fasterxml.jackson.core", "jackson-databind", "2.9.8"),
            new MavenPackage("software.amazon.awssdk", "auth", "2.4.0"),
            new MavenPackage("software.amazon", "flow", "1.7"),
            new MavenPackage("software.amazon.awssdk", "http-client-spi", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "regions", "2.4.0"),
            new MavenPackage("com.fasterxml.jackson.core", "jackson-annotations", "2.9.0"),
            new MavenPackage("software.amazon.awssdk", "annotations", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "utils", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "aws-core", "2.4.0"),
            new MavenPackage("software.amazon.awssdk", "apache-client", "2.4.0"),
            new MavenPackage("org.apache.httpcomponents", "httpclient", "4.5.6"),
            new MavenPackage("commons-codec", "commons-codec", "1.10"),
            new MavenPackage("org.apache.httpcomponents", "httpcore", "4.4.10"),
            new MavenPackage("com.amazonaws", "aws-java-sdk-core", "1.11.477"),
            new MavenPackage("commons-logging", "commons-logging", "1.1.3"),
            new MavenPackage("software.amazon.ion", "ion-java", "1.0.2"),
            new MavenPackage("joda-time", "joda-time", "2.8.1"),
            new MavenPackage("ch.qos.logback", "logback-classic", "1.2.3"),
            new MavenPackage("ch.qos.logback", "logback-core", "1.2.3"),
            new MavenPackage("com.beust", "jcommander", "1.72"),
            new MavenPackage("commons-io", "commons-io", "2.6"),
            new MavenPackage("org.apache.commons", "commons-collections4", "4.2"),
            new MavenPackage("commons-beanutils", "commons-beanutils", "1.9.3"),
            new MavenPackage("commons-collections", "commons-collections", "3.2.2")
        };

        /// <summary>
        /// Downloads all the required jars from Maven and returns a classpath string that includes all those jars.
        /// </summary>
        /// <returns>Classpath string that includes all the jars downloaded.</returns>
        /// <param name="jarFolder">Folder into which to save the jars.</param>
        private static string FetchJars(string jarFolder)
        {
            if (jarFolder == null)
            {
                jarFolder = "jars";
            }

            if (!Path.IsPathRooted(jarFolder))
            {
                jarFolder = Path.Combine(Directory.GetCurrentDirectory(), jarFolder);
            }

            System.Console.Error.WriteLine("Fetching required jars...");

            foreach (MavenPackage mp in MAVEN_PACKAGES)
            {
                mp.Fetch(jarFolder);
            }

            System.Console.Error.WriteLine("Done.");

            List<string> files = Directory.GetFiles(jarFolder).Where(f => f.EndsWith(".jar")).ToList();
            files.Add(Directory.GetCurrentDirectory());
            return string.Join(Path.PathSeparator.ToString(), files);
        }

        /// <summary>
        ///  See if "java" is already in path and working. Start the java process. If its not found it will error
        /// </summary>
        /// <param name="java">the path to the java program to try</param>
        /// <returns>the java program found</returns>
        private static string FindJava(string java)
        {

            if (java == null)
            {
                java = "java";
            }

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = java,
                    Arguments = "-version",
                    UseShellExecute = false
                }
            };
            try
            {
                proc.Start();
                proc.WaitForExit();
                return java;
            }
            catch (Exception ex)
            {
                //TODO: What should we do about not being able to find java?
            }

            return null;
        }


        public static Process StartJavaSubscriptionProcess(KinesisConsumerOptions options)
        {
            options.PropertiesFile = string.Format("{0}.kcl.properties", options.StreamName.Replace(" ", "_"));


                                 
            string javaClassPath = FetchJars(options.JarFolder);

            string java = FindJava(options.JavaLocation);

            if (java == null)
            {
                System.Console.Error.WriteLine(
                    "java could not be found. You may need to install it, or manually specify the path to it.");

                Environment.Exit(2);
            }

            List<string> cmd = new List<string>()
                {
                    java,
                    "-cp",
                    CleanFilePath(javaClassPath),
                    "software.amazon.kinesis.multilang.MultiLangDaemon",
                    "-p",
                    options.PropertiesFile
                };
            if (!string.IsNullOrEmpty(options.LogbackConfiguration))
            {
                cmd.Add("-l");
                cmd.Add(options.LogbackConfiguration);
            }

            
            //Write the properties file before launching the process
            WriteKCLPropertiesFile(options);



            // Start the KCL.
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = cmd[0],
                    Arguments = string.Join(" ", cmd.Skip(1)),
                    UseShellExecute = false
                }
            };
            proc.Start();

            return proc;

        }

        /// <summary>
        /// Creates the options file for the java consumer
        /// </summary>
        /// <param name="options"></param>
        private static void WriteKCLPropertiesFile(KinesisConsumerOptions options)
        {
            //TODO: Check if the file exists. Potentially this will need to be removed once its complete 
            var kclTemplate = new StreamReader(new MemoryStream(Resources.kcl_properties)).ReadToEnd();
            var kclFile = string.Format(kclTemplate, options.ConsumerProgramCommandLine, options.StreamName, options.ApplicationName, options.StartPosition.ToString(), options.RegionName);
            File.WriteAllText(options.PropertiesFile, kclFile);
        }

        internal static string CleanFilePath(string location)
        {
            if (location.IndexOf(' ') > -1
                || location.IndexOf('\'') > -1
                || location.IndexOf('\"') > -1)
            {
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    return "\"" + location.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "$\'" + location.Replace("\'", "\\\'").Replace("\"", "\\\"") + "\'";
                }
            }

            return location;
        }
        }
}

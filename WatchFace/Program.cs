using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using Resources;
using WatchFace.Parser;
using WatchFace.Parser.Utils;

namespace WatchFace
{
    internal class Program
    {
        private const string AppName = "WatchFace";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == null)
            {
                Console.WriteLine("{0} unpacks Amazfit Bip downloadable watch faces.", AppName);
                Console.WriteLine("Usage: {0}.exe wf.bin [wf2.bin] ...", AppName);
                return;
            }

            if (args.Length > 0)
                Console.WriteLine("Multiple files unpacking.");

            foreach (var inputFileName in args)
            {
                if (!File.Exists(inputFileName))
                {
                    Console.WriteLine("File '{0}' doesn't exists.", inputFileName);
                    continue;
                }

                Console.WriteLine("Processing file '{0}'", inputFileName);
                var inputFileExtension = Path.GetExtension(inputFileName);
                try
                {
                    switch (inputFileExtension)
                    {
                        case ".bin":
                            UnpackWatchFace(inputFileName);
                            break;
                        case ".json":
                            PackWatchFace(inputFileName);
                            break;
                        case ".res":
                            UnpackResources(inputFileName);
                            break;
                        default:
                            Console.WriteLine("The app doesn't support files with extension {0}.", inputFileExtension);
                            Console.WriteLine("Only 'bin', 'res' and 'json' files are supported at this time.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Logger.Fatal(e);
                }
            }
        }

        private static void PackWatchFace(string inputFileName)
        {
            var watchFace = ReadConfig(inputFileName);
            var outputFileName = Path.ChangeExtension(inputFileName, "bin");
            SetupLogger(Path.ChangeExtension(outputFileName, ".log"));
            WriteWatchFace(outputFileName, watchFace);
        }

        private static void UnpackWatchFace(string inputFileName)
        {
            var outputDirectory = CreateOutputDirectory(inputFileName);
            var baseName = Path.GetFileNameWithoutExtension(inputFileName);
            SetupLogger(Path.Combine(outputDirectory, $"{baseName}.log"));

            var reader = ReadWatchFace(inputFileName);
            if (reader == null) return;

            var watchFace = ParseResources(reader);
            if (watchFace == null) return;

            Logger.Debug("Exporting resources to '{0}'", outputDirectory);
            new ResourcesExtractor(reader.Images).Extract(outputDirectory);
            ExportConfig(watchFace, Path.Combine(outputDirectory, $"{baseName}.json"));
        }

        private static void UnpackResources(string inputFileName)
        {
            var outputDirectory = CreateOutputDirectory(inputFileName);
            var baseName = Path.GetFileNameWithoutExtension(inputFileName);
            SetupLogger(Path.Combine(outputDirectory, $"{baseName}.log"));

            Bitmap[] images;
            using (var stream = File.OpenRead(inputFileName))
            {
                images = new ResourcesFileReader(stream).Read();
            }

            new ResourcesExtractor(images).Extract(outputDirectory);
        }

        private static void WriteWatchFace(string outputFileName, Parser.WatchFace watchFace)
        {
            Logger.Debug("Writing watch face to '{0}'", outputFileName);
            try
            {
                using (var fileStream = File.OpenWrite(outputFileName))
                {
                    var writer = new Writer(fileStream);
                    writer.Write(watchFace);
                    fileStream.Flush();
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
            }
        }

        private static Reader ReadWatchFace(string inputFileName)
        {
            Logger.Debug("Opening watch face '{0}'", inputFileName);
            try
            {
                using (var fileStream = File.OpenRead(inputFileName))
                {
                    var reader = new Reader(fileStream);
                    Logger.Debug("Reading parameters...");
                    reader.Read();
                    return reader;
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                return null;
            }
        }

        private static Parser.WatchFace ParseResources(Reader reader)
        {
            Logger.Debug("Parsing parameters...");
            try
            {
                return ParametersConverter.Parse<Parser.WatchFace>(reader.Parameters);
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                return null;
            }
        }

        private static string CreateOutputDirectory(string originalFileName)
        {
            var path = Path.GetDirectoryName(originalFileName);
            var name = Path.GetFileNameWithoutExtension(originalFileName);
            var unpackedPath = Path.Combine(path, $"{name}");
            if (!Directory.Exists(unpackedPath)) Directory.CreateDirectory(unpackedPath);
            return unpackedPath;
        }

        private static Parser.WatchFace ReadConfig(string jsonFileName)
        {
            Logger.Debug("Reading config...");
            try
            {
                using (var fileStream = File.OpenRead(jsonFileName))
                using (var reader = new StreamReader(fileStream))
                {
                    return JsonConvert.DeserializeObject<Parser.WatchFace>(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                return null;
            }
        }

        private static void ExportConfig(Parser.WatchFace watchFace, string jsonFileName)
        {
            Logger.Debug("Exporting config...");
            try
            {
                using (var fileStream = File.OpenWrite(jsonFileName))
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(JsonConvert.SerializeObject(watchFace, Formatting.Indented,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
            }
        }

        private static void SetupLogger(string logFileName)
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget
            {
                FileName = logFileName,
                Layout = "${message}"
            };
            config.AddTarget("file", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, fileTarget));

            var consoleTarget = new ColoredConsoleTarget
                {Layout = @"${message}"};
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            LogManager.Configuration = config;
        }
    }
}
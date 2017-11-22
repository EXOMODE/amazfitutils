using System;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using SixLabors.ImageSharp;

namespace WatchFace.Cmd
{
    internal class Program
    {
        private const string AppName = "WatchFace";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            var inputFileName = args[0];
            if (inputFileName == null)
            {
                Console.WriteLine("{0} unpacks Amazfit Bip downloadable watch faces.", AppName);
                Console.WriteLine("Usage: {0}.exe watchface.bin", AppName);
                return;
            }

            SetupLogger(LogLevel.Trace);

            var reader = ReadWatchFace(inputFileName);
            if (reader == null) return;

            var watchFace = ParseResources(reader);
            if (watchFace == null) return;

            var outputDirectory = CreateOutputDirectory(inputFileName);

            Logger.Debug("Exporting resources to '{0}'", outputDirectory);
            ExportImages(reader, outputDirectory);
            ExportConfig(watchFace, outputDirectory);
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

        private static WatchFace ParseResources(Reader reader)
        {
            Logger.Debug("Parsing parameters...");
            try
            {
                return WatchFace.Parse(reader.Resources);
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
            var unpackedPath = Path.Combine(path, $"{name}_unpacked");
            if (!Directory.Exists(unpackedPath)) Directory.CreateDirectory(unpackedPath);
            return unpackedPath;
        }

        private static void ExportImages(Reader reader, string outputDirectory)
        {
            Logger.Debug("Exporting images...");
            try
            {
                var index = 0;
                foreach (var image in reader.Images)
                {
                    image.Save(Path.Combine(outputDirectory, $"{index}.bmp"));
                    index++;
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
            }
        }

        private static void ExportConfig(WatchFace watchFace, string outputDirectory)
        {
            var configFileName = Path.Combine(outputDirectory, "config.json");
            Logger.Debug("Exporting config...");
            try
            {
                using (var fileStream = File.OpenWrite(configFileName))
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

        private static void SetupLogger(LogLevel logLevel)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            var rule1 = new LoggingRule("*", logLevel, consoleTarget);
            config.LoggingRules.Add(rule1);
            LogManager.Configuration = config;
        }
    }
}
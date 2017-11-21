using System;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using SixLabors.ImageSharp;
using WatchFace;

namespace Cmd
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static int Main(string[] args)
        {
            var inputFileName = args[0];
            if (inputFileName == null)
            {
                Usage();
                return 1;
            }

            var useTrace = args.Length > 1 && args[1] == "--debug";
            SetupLogger(useTrace);

            Reader reader;
            Logger.Debug("Opening watch face '{0}'", inputFileName);
            using (var fileStream = File.OpenRead(inputFileName))
            {
                reader = new Reader(fileStream);
                Logger.Debug("Reading watch face parameters...");
                reader.Parse();
            }

            Logger.Debug("Parsing parameters...");
            var watchFace = WatchFace.WatchFace.Parse(reader.Resources);

            var path = Path.GetDirectoryName(inputFileName);
            var name = Path.GetFileNameWithoutExtension(inputFileName);
            var unpackedPath = Path.Combine(path, $"{name}_unpacked");
            if (!Directory.Exists(unpackedPath)) Directory.CreateDirectory(unpackedPath);
            Logger.Debug("Unpacking watch face to '{0}'", unpackedPath);

            var configFileName = Path.Combine(unpackedPath, "config.json");
            Logger.Debug("Eporting config...");
            using (var fileStream = File.OpenWrite(configFileName))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonConvert.SerializeObject(watchFace, Formatting.Indented,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
                writer.Flush();
            }

            Logger.Debug("Eporting images...");
            var index = 0;
            foreach (var image in reader.Images)
            {
                image.Save(Path.Combine(unpackedPath, $"{index}.bmp"));
                index++;
            }

            Logger.Debug("Everything done!");
            Console.ReadKey();
            return 0;
        }

        private static void SetupLogger(bool useTrace)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            var rule1 = new LoggingRule("*", useTrace ? LogLevel.Trace : LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);
            LogManager.Configuration = config;
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: wf.exe [watchface bin]");
            Console.WriteLine();
        }
    }
}
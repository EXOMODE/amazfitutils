using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using Resources;
using Resources.Models;
using WatchFace.Parser.Utils;
using Reader = WatchFace.Parser.Reader;
using Writer = WatchFace.Parser.Writer;

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
                Console.WriteLine(
                    "{0}.exe unpacks and packs Amazfit Bip downloadable watch faces and resource files.", AppName);
                Console.WriteLine();
                Console.WriteLine("Usage examples:");
                Console.WriteLine("  {0}.exe watchface.bin   - unpacks watchface images and config", AppName);
                Console.WriteLine("  {0}.exe watchface.json  - packs config and referenced images to bin file", AppName);
                Console.WriteLine("  {0}.exe mili_chaohu.res - unpacks resource file images", AppName);
                Console.WriteLine("  {0}.exe mili_chaohu     - packs folder content to res file", AppName);
                return;
            }

            if (args.Length > 1)
                Console.WriteLine("Multiple files unpacking.");

            foreach (var inputFileName in args)
            {
                var isDirectory = Directory.Exists(inputFileName);
                var isFile = File.Exists(inputFileName);
                if (!isDirectory && !isFile)
                {
                    Console.WriteLine("File or directory '{0}' doesn't exists.", inputFileName);
                    continue;
                }
                if (isDirectory)
                {
                    Console.WriteLine("Processing directory '{0}'", inputFileName);
                    try
                    {
                        PackResources(inputFileName);
                    }
                    catch (Exception e)
                    {
                        Logger.Fatal(e);
                    }
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
            var baseName = Path.GetFileNameWithoutExtension(inputFileName);
            var outputDirectory = Path.GetDirectoryName(inputFileName);
            var outputFileName = Path.Combine(outputDirectory, baseName + "_packed.bin");
            SetupLogger(Path.ChangeExtension(outputFileName, ".log"));

            var watchFace = ReadConfig(inputFileName);
            if (watchFace == null) return;

            var imagesDirectory = Path.GetDirectoryName(inputFileName);
            try
            {
                WriteWatchFace(outputFileName, imagesDirectory, watchFace);
            }
            catch (Exception)
            {
                File.Delete(outputFileName);
                throw;
            }
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
            var reDescriptor = new FileDescriptor {Images = reader.Images};
            new Extractor(reDescriptor).Extract(outputDirectory);
            ExportConfig(watchFace, Path.Combine(outputDirectory, $"{baseName}.json"));
        }

        private static void PackResources(string inputDirectory)
        {
            var outputDirectory = Path.GetDirectoryName(inputDirectory);
            var baseName = Path.GetFileName(inputDirectory);
            var outputFileName = Path.Combine(outputDirectory, $"{baseName}_packed.res");
            var logFileName = Path.Combine(outputDirectory, $"{baseName}_packed.log");
            SetupLogger(logFileName);

            var versionFileName = Path.Combine(inputDirectory, "version");
            var resDescriptor = new FileDescriptor();
            using (var stream = File.OpenRead(versionFileName))
            using (var reader = new BinaryReader(stream))
            {
                resDescriptor.Version = reader.ReadByte();
            }
            var i = 0;
            var images = new List<Bitmap>();
            while (true)
            {
                var fileName = Path.Combine(inputDirectory, $"{i}.png");
                if (!File.Exists(fileName)) break;

                images.Add((Bitmap) Image.FromFile(fileName));
                i++;
            }
            resDescriptor.Images = images;

            using (var stream = File.OpenWrite(outputFileName))
            {
                new FileWriter(stream).Write(resDescriptor);
            }
        }

        private static void UnpackResources(string inputFileName)
        {
            var outputDirectory = CreateOutputDirectory(inputFileName);
            var baseName = Path.GetFileNameWithoutExtension(inputFileName);
            SetupLogger(Path.Combine(outputDirectory, $"{baseName}.log"));

            FileDescriptor resDescriptor;
            using (var stream = File.OpenRead(inputFileName))
            {
                resDescriptor = FileReader.Read(stream);
            }

            new Extractor(resDescriptor).Extract(outputDirectory);
        }

        private static void WriteWatchFace(string outputFileName, string imagesDirectory, Parser.WatchFace watchFace)
        {
            try
            {
                Logger.Debug("Reading referenced images from '{0}'", imagesDirectory);
                var imagesReader = new ImagesLoader(imagesDirectory);
                imagesReader.Process(watchFace);

                Logger.Debug("Writing watch face to '{0}'", outputFileName);
                using (var fileStream = File.OpenWrite(outputFileName))
                {
                    var writer = new Writer(fileStream, imagesReader.Images);
                    writer.Write(watchFace);
                    fileStream.Flush();
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
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
                Logger.Fatal(e.Message);
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
                Logger.Fatal(e.Message);
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
                    return JsonConvert.DeserializeObject<Parser.WatchFace>(reader.ReadToEnd(),
                        new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Error,
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
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
                Logger.Fatal(e.Message);
            }
        }

        private static void SetupLogger(string logFileName)
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget
            {
                FileName = logFileName,
                Layout = "${level}|${message}"
            };
            config.AddTarget("file", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, fileTarget));

            var consoleTarget = new ColoredConsoleTarget {Layout = @"${message}"};
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            LogManager.Configuration = config;
        }
    }
}
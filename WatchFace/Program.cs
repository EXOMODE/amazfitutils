#define VERGE_PACK
//#define VERGE_UNPACK

//#define GTR_PACK
//#define GTR_UNPACK

//#define BAND_PACK
//#define BAND_UNPACK

//#define BIP_PACK
//#define BIP_UNPACK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using Resources;
using Resources.Models;
using WatchFace.Parser;
using WatchFace.Parser.Elements;
using WatchFace.Parser.Models;
using WatchFace.Parser.Models.Elements;
using WatchFace.Parser.Models.Elements.Common;
using WatchFace.Parser.Utils;
using Image = System.Drawing.Image;
using Reader = WatchFace.Parser.Reader;
using Writer = WatchFace.Parser.Writer;

namespace WatchFace
{
    internal class Program
    {
        private const string AppName = "WatchFace 1.9.0";

        private static readonly bool IsRunningOnMono = Type.GetType("Mono.Runtime") != null;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Size previewSize = new Size(176, 176);

        private static void Main(string[] args)
        {
#if DEBUG

#if VERGE_PACK
            args = new[] { "-size360", "Verge/Verge.json", };
#elif VERGE_UNPACK
            args = new[] { "-size360", "Verge.bin", };
            //args = new[] { "-size360", "Verge/Verge_packed.bin", };
#endif

#if GTR_PACK
            args = new[] { "-size464", "GTR/GTR.json", };
#elif GTR_UNPACK
            args = new[] { "-size464", "GTR.bin", };
            //args = new[] { "-size464", "GTR/GTR_packed.bin", };
#endif

#if BAND_PACK
            args = new[] { "-size120x240", "Band/Band.json", };
#elif BAND_UNPACK
            args = new[] { "-size120x240", "Band.bin", };
            //args = new[] { "-size120x240", "Band/Band_packed.bin", };
#endif

#if BIP_PACK
            args = new[] { "-size176", "Bip/Bip.json", };
#elif BIP_UNPACK
            args = new[] { "-size176", "Bip.bin", };
            //args = new[] { "-size176", "Bip/Bip_packed.bin", };
#endif

#endif
            List<string> files = new List<string>();

            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    string a = arg.Substring(1);

                    if (a.StartsWith("size"))
                    {
                        a = a.Substring(4);
                        string[] tmp = a.Split('x');
                        int w = 0;
                        int h = 0;

                        if (tmp.Length == 1)
                            w = h = int.Parse(tmp[0]);
                        else
                        {
                            w = int.Parse(tmp[0]);
                            h = int.Parse(tmp[1]);
                        }

                        previewSize = new Size(w, h);
                    }

                    continue;
                }

                files.Add(arg);
            }

            if ((previewSize.Width == 360 && previewSize.Height == 360) || (previewSize.Width == 464 && previewSize.Height == 464))
            {
                Parser.Models.Header.HeaderSize += 20;
                Resources.Image.Reader.IsVerge = true;
            }
            else if (previewSize.Width == 176 && previewSize.Height == 176)
            {
                StatusElement.IsBip = true;
            }

            ClockHandElement.GlobalCenter = new Point(previewSize.Width / 2, previewSize.Height / 2);

            if (args.Length == 0 || args[0] == null)
            {
                Console.WriteLine(
                    "{0}.exe unpacks and packs Amazfit Bip downloadable watch faces and resource files.", AppName);
                Console.WriteLine();
                Console.WriteLine("Usage examples:");
                Console.WriteLine("  {0}.exe watchface.bin   - unpacks watchface images and config", AppName);
                Console.WriteLine("  {0}.exe watchface.json  - packs config and referenced images to bin file",
                    AppName);
                Console.WriteLine("  {0}.exe mili_chaohu.res - unpacks resource file images", AppName);
                Console.WriteLine("  {0}.exe mili_chaohu     - packs folder content to res file", AppName);
                return;
            }

            if (files.Count > 1)
                Console.WriteLine("Multiple files unpacking.");

            foreach (var inputFileName in files)
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

            var watchFace = ReadWatchFaceConfig(inputFileName);
            if (watchFace == null) return;

            var imagesDirectory = Path.GetDirectoryName(inputFileName);
            try
            {
                WriteWatchFace(outputDirectory, outputFileName, imagesDirectory, watchFace);
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

            //GeneratePreviews(reader.Parameters, reader.Images, outputDirectory, baseName);

            Logger.Debug("Exporting resources to '{0}'", outputDirectory);
            var reDescriptor = new FileDescriptor {Resources = reader.Resources};
            new Extractor(reDescriptor).Extract(outputDirectory);
            ExportWatchFaceConfig(watchFace, Path.Combine(outputDirectory, $"{baseName}.json"));
        }

        private static void PackResources(string inputDirectory)
        {
            var outputDirectory = Path.GetDirectoryName(inputDirectory);
            var baseName = Path.GetFileName(inputDirectory);
            var outputFileName = Path.Combine(outputDirectory, $"{baseName}_packed.res");
            var logFileName = Path.Combine(outputDirectory, $"{baseName}_packed.log");
            SetupLogger(logFileName);

            FileDescriptor resDescriptor;
            var headerFileName = Path.Combine(inputDirectory, "header.json");
            var versionFileName = Path.Combine(inputDirectory, "version");
            if (File.Exists(headerFileName))
            {
                resDescriptor = ReadResConfig(headerFileName);
            }
            else if (File.Exists(versionFileName))
            {
                resDescriptor = new FileDescriptor();
                using (var stream = File.OpenRead(versionFileName))
                using (var reader = new BinaryReader(stream))
                {
                    resDescriptor.Version = reader.ReadByte();
                }
            }
            else
            {
                throw new ArgumentException(
                    "File 'header.json' or 'version' should exists in the folder with unpacked images. Res-file couldn't be created"
                );
            }

            var i = 0;
            var images = new List<IResource>();
            while (resDescriptor.ResourcesCount == null || i < resDescriptor.ResourcesCount.Value)
            {
                try
                {
                    var resource = ImageLoader.LoadResourceForNumber(inputDirectory, i);
                    images.Add(resource);
                }
                catch (FileNotFoundException)
                {
                    Logger.Info("All images with sequenced names are loaded. Latest loaded image: {0}", i - 1);
                    break;
                }

                i++;
            }

            if (resDescriptor.ResourcesCount != null && resDescriptor.ResourcesCount.Value != images.Count)
                throw new ArgumentException(
                    $"The .res-file should contain {resDescriptor.ResourcesCount.Value} images but was loaded {images.Count} images.");

            resDescriptor.Resources = images;

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

            ExportResConfig(resDescriptor, Path.Combine(outputDirectory, "header.json"));
            new Extractor(resDescriptor).Extract(outputDirectory);
        }

        private static void WriteWatchFace(string outputDirectory, string outputFileName, string imagesDirectory, Parser.WatchFace watchFace)
        {
            try
            {
                Logger.Debug("Reading referenced images from '{0}'", imagesDirectory);
                var imagesReader = new ResourcesLoader(imagesDirectory);

                if (previewSize.Width == 360 || previewSize.Width == 464)
                    imagesReader.Process(watchFace as WatchFaceVerge);
                else if (previewSize.Width == 176)
                    imagesReader.Process(watchFace as WatchFaceBip);
                else
                    imagesReader.Process(watchFace);


                Logger.Trace("Building parameters for watch face...");
                List<Parameter> descriptor;

                if (previewSize.Width == 360 || previewSize.Width == 464)
                    descriptor = ParametersConverter.Build(watchFace as WatchFaceVerge);
                else if (previewSize.Width == 176)
                    descriptor = ParametersConverter.Build(watchFace as WatchFaceBip);
                else
                    descriptor = ParametersConverter.Build(watchFace);

                var baseFilename = Path.GetFileNameWithoutExtension(outputFileName);
                GeneratePreviews(descriptor, imagesReader.Images, outputDirectory, baseFilename);

                Logger.Debug("Writing watch face to '{0}'", outputFileName);
                using (var fileStream = File.OpenWrite(outputFileName))
                {
                    var writer = new Writer(fileStream, imagesReader.Resources);
                    writer.Write(descriptor);
                    fileStream.Flush();
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                File.Delete(outputFileName);
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
                if (previewSize.Width == 176)
                    return ParametersConverter.Parse<WatchFaceBip>(reader.Parameters);
                else if (previewSize.Width == 360 || previewSize.Width == 464)
                    return ParametersConverter.Parse<WatchFaceVerge>(reader.Parameters);
                else
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

        private static Parser.WatchFace ReadWatchFaceConfig(string jsonFileName)
        {
            Logger.Debug("Reading config...");
            try
            {
                using (var fileStream = File.OpenRead(jsonFileName))
                using (var reader = new StreamReader(fileStream))
                {
                    if (previewSize.Width == 176)
                        return JsonConvert.DeserializeObject<WatchFaceBip>(reader.ReadToEnd(),
                            new JsonSerializerSettings
                            {
                                MissingMemberHandling = MissingMemberHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });
                    else if (previewSize.Width == 360 || previewSize.Width == 464)
                        return JsonConvert.DeserializeObject<WatchFaceVerge>(reader.ReadToEnd(),
                            new JsonSerializerSettings
                            {
                                MissingMemberHandling = MissingMemberHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            });

                    return JsonConvert.DeserializeObject<Parser.WatchFace>(reader.ReadToEnd(),
                        new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                return null;
            }
        }

        private static FileDescriptor ReadResConfig(string jsonFileName)
        {
            Logger.Debug("Reading resources config...");
            try
            {
                using (var fileStream = File.OpenRead(jsonFileName))
                using (var reader = new StreamReader(fileStream))
                {
                    return JsonConvert.DeserializeObject<FileDescriptor>(reader.ReadToEnd(),
                        new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Error,
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                return null;
            }
        }

        private static void ExportWatchFaceConfig(Parser.WatchFace watchFace, string jsonFileName)
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

        private static void ExportResConfig(FileDescriptor resDescriptor, string jsonFileName)
        {
            Logger.Debug("Exporting resources config...");
            try
            {
                using (var fileStream = File.OpenWrite(jsonFileName))
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(JsonConvert.SerializeObject(resDescriptor, Formatting.Indented,
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
                Layout = "${level}|${message}",
                KeepFileOpen = true,
                ConcurrentWrites = false,
                OpenFileCacheTimeout = 30
            };
            config.AddTarget("file", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, fileTarget));

            var consoleTarget = new ColoredConsoleTarget {Layout = @"${message}"};
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            LogManager.Configuration = config;
        }

        private static void GeneratePreviews(List<Parameter> parameters, Bitmap[] images, string outputDirectory, string baseName)
        {
            Logger.Debug("Generating previews...");

            var states = GetPreviewStates();
            var staticPreview = PreviewGenerator.CreateImage(parameters, images, new WatchState(), previewSize);
            staticPreview.Save(Path.Combine(outputDirectory, $"{baseName}_static.png"), ImageFormat.Png);

            var previewImages = PreviewGenerator.CreateAnimation(parameters, images, states, previewSize);

            if (IsRunningOnMono)
            {
                var i = 0;
                foreach (var previewImage in previewImages)
                {
                    previewImage.Save(Path.Combine(outputDirectory, $"{baseName}_animated_{i}.png"), ImageFormat.Png);
                    i++;
                }
            }
            else
            {
                using (var gif = AnimatedGif.AnimatedGif.Create(Path.Combine(outputDirectory, $"{baseName}_animated.gif"), 1000))
                    foreach (var previewImage in previewImages) gif.AddFrame(previewImage, quality: AnimatedGif.GifQuality.Bit8);
            }
        }

        private static IEnumerable<WatchState> GetPreviewStates()
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var previewStatesPath = Path.Combine(appPath, "PreviewStates.json");

            if (File.Exists(previewStatesPath))
                using (var stream = File.OpenRead(previewStatesPath))
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<WatchState>>(json);
                }

            var previewStates = GenerateSampleStates();
            using (var stream = File.OpenWrite(previewStatesPath))
            using (var writer = new StreamWriter(stream))
            {
                var json = JsonConvert.SerializeObject(previewStates, Formatting.Indented);
                writer.Write(json);
                writer.Flush();
            }

            return previewStates;
        }

        private static IEnumerable<WatchState> GenerateSampleStates()
        {
            var time = DateTime.Now;
            var states = new List<WatchState>();

            for (var i = 0; i < 20; i++)
            {
                var num = i + 1;
                var watchState = new WatchState
                {
                    BatteryLevel = 100 - i * 5,
                    Pulse = 40 + num * 4,
                    Steps = num * 500,
                    Calories = num * 75,
                    Distance = num * 350,
                    Bluetooth = num > 2 && num < 12,
                    Unlocked = num > 4 && num < 14,
                    DoNotDisturb = num > 8 && num < 18,
                    Alarm = num > 3 && num < 8,
                    DayTemperature = i - 15,
                    NightTemperature = i - 24,
                };

                if (num < 3)
                {
                    watchState.AirQuality = AirCondition.Unknown;
                    watchState.AirQualityIndex = null;

                    watchState.CurrentWeather = WeatherCondition.Unknown;
                    watchState.CurrentTemperature = null;
                }
                else
                {
                    var index = num - 2;
                    watchState.AirQuality = (AirCondition) index;
                    watchState.CurrentWeather = (WeatherCondition) index;

                    watchState.AirQualityIndex = index * 50 - 25;
                    watchState.CurrentTemperature = index * 6 - 10;
                }

                int month = num / 2 + 1;
                watchState.Time = new DateTime(time.Year, month, num + 5, i, i * 2, i);
                states.Add(watchState);
            }

            return states;
        }
    }
}
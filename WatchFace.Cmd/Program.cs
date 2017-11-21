using System;
using System.IO;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using WatchFace;
using WatchFace.Models;

namespace Cmd
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var inputFileName = args[0];
            if (inputFileName == null)
            {
                Usage();
                return 1;
            }

            Reader reader;
            Console.WriteLine("Opening watch face '{0}'", inputFileName);
            using (var fileStream = File.OpenRead(inputFileName))
            {
                reader = new Reader(fileStream);
                Console.WriteLine("Reading watch face parameters...");
                reader.Parse();
            }

            //foreach (var resource in reader.Resources)
            //{
            //    Console.WriteLine("{0:D}: ", resource.Id);
            //    foreach (var descriptor in resource.Children) WriteParameter(4, descriptor);
            //}

            Console.WriteLine("Parsing parameters...");
            var watchFace = WatchFace.WatchFace.Parse(reader.Resources);

            var path = Path.GetDirectoryName(inputFileName);
            var name = Path.GetFileNameWithoutExtension(inputFileName);
            var unpackedPath = Path.Combine(path, $"{name}_unpacked");
            if (!Directory.Exists(unpackedPath)) Directory.CreateDirectory(unpackedPath);
            Console.WriteLine("Unpacking watch face to '{0}'", unpackedPath);


            var configFileName = Path.Combine(unpackedPath, "config.json");
            Console.WriteLine("Eporting config...");
            using (var fileStream = File.OpenWrite(configFileName))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(JsonConvert.SerializeObject(watchFace, Formatting.Indented,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
                writer.Flush();
            }

            Console.WriteLine("Eporting images...");
            var index = 0;
            foreach (var image in reader.Images)
            {
                image.Save(Path.Combine(unpackedPath, $"{index}.bmp"));
                index++;
            }

            Console.WriteLine("Everything done!");
            Console.ReadKey();
            return 0;
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: wf.exe [watchface bin]");
            Console.WriteLine();
        }

        private static void WriteParameter(int offset, Parameter parameter)
        {
            for (var i = 0; i < offset; i++)
                Console.Write(' ');
            Console.Write("{0:D}: ", parameter.Id);
            if (parameter.IsComplex)
            {
                Console.WriteLine();
                foreach (var parameter1 in parameter.Children) WriteParameter(offset + 4, parameter1);
            }
            else
            {
                Console.WriteLine("{0:D} (0x{0:x})", parameter.Value);
            }
        }
    }
}
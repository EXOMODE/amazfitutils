using System;
using System.IO;
using System.Linq;
using WatchFace;
using WatchFace.Models;

namespace Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args[0];
            Reader reader;
            Console.WriteLine("Opening watch face: {0}", fileName);
            using (var fileStream = File.OpenRead(fileName))
            {
                reader = new Reader(fileStream);
                reader.Parse();
            }
            foreach (var resource in reader.Resources)
            {
                Console.WriteLine("{0:D}: ", resource.Id);
                foreach (var descriptor in resource.Descriptor)
                {
                    WriteParameter(4, descriptor);
                }
            }

            Console.ReadKey();
        }

        private static void WriteParameter(int offset, Parameter parameter)
        {
            for (var i = 0; i < offset; i++)
                Console.Write(' ');
            Console.Write("{0:D}: ", parameter.Id);
            if (parameter.IsList)
            {
                Console.WriteLine();
                foreach (var parameter1 in parameter.List)
                {
                    WriteParameter(offset + 4, parameter1);
                }
            }
            else
            {
                Console.WriteLine("{0:D} (0x{0:x})", parameter.Value);
            }
        }
    }
}

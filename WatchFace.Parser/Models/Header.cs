using System;
using System.IO;
using System.Text;

namespace WatchFace.Parser.Models
{
    public class Header
    {
        private const string DialSignature = "HMDIAL\0";

        public string Signature { get; private set; } = DialSignature;
        public uint Unknown { get; set; }
        public uint ParametersSize { get; set; }
        public static int HeaderSize { get; set; } = 40;

        public bool IsValid => Signature == DialSignature;

        public void WriteTo(Stream stream, string outputDirectory)
        {
            byte[] buffer;

            if (HeaderSize == 60)
            {
                buffer = new byte[HeaderSize + 4];

                for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

                string p = Path.Combine(outputDirectory, "header.bin");

                if (File.Exists(p))
                {
                    buffer = File.ReadAllBytes(p);
                    //File.Delete(p);

                    Unknown = BitConverter.ToUInt32(buffer, 52);
                }

                //Encoding.ASCII.GetBytes(Signature).CopyTo(buffer, 0);
                //BitConverter.GetBytes(Unknown).CopyTo(buffer, 52);
                BitConverter.GetBytes(ParametersSize).CopyTo(buffer, 56);

                stream.Write(buffer, 0, HeaderSize + 4);

                return;
            }

            buffer = new byte[HeaderSize];

            for (var i = 0; i < buffer.Length; i++) buffer[i] = 0xff;

            Encoding.ASCII.GetBytes(Signature).CopyTo(buffer, 0);

            BitConverter.GetBytes(Unknown).CopyTo(buffer, 32);
            BitConverter.GetBytes(ParametersSize).CopyTo(buffer, 36);

            stream.Write(buffer, 0, HeaderSize);
        }

        public static Header ReadFrom(Stream stream, string outputDirectory)
        {
            byte[] buffer = new byte[HeaderSize];

            stream.Read(buffer, 0, buffer.Length);
            string signature = Encoding.ASCII.GetString(buffer, 0, 7);

            uint unknown = BitConverter.ToUInt32(buffer, 32);
            uint parametersSize = BitConverter.ToUInt32(buffer, 36);

            if (unknown >= ushort.MaxValue || parametersSize >= ushort.MaxValue)
            {
                buffer = new byte[HeaderSize + 4];
                stream.Position = 0;
                stream.Read(buffer, 0, buffer.Length);

                unknown = BitConverter.ToUInt32(buffer, 52);
                parametersSize = BitConverter.ToUInt32(buffer, 56);
                File.WriteAllBytes(Path.Combine(outputDirectory, "header.bin"), buffer);
            }

            return new Header
            {
                Signature = signature,
                Unknown = unknown,
                ParametersSize = parametersSize,
            };
        }
    }
}
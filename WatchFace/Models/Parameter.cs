using System.Collections.Generic;
using System.IO;

namespace WatchFace.Models
{
    public class Parameter
    {
        public Parameter(byte id, long value)
        {
            Id = id;
            Value = value;
        }

        public Parameter(byte id, List<Parameter> value)
        {
            Id = id;
            Children = value;
        }

        public byte Id { get; }
        public long Value { get; }
        public List<Parameter> Children { get; }
        public bool IsComplex => Children != null;

        public static Parameter ReadFrom(Stream fileStream)
        {
            var rawId = fileStream.ReadByte();
            var id = (byte) ((rawId & 0xf8) >> 3);
            var flags = (ParameterFlags) (rawId & 0x7);

            if (flags.HasFlag(ParameterFlags.hasChildren))
            {
                var readBytes = fileStream.ReadByte();
                var buffer = new byte[readBytes];
                fileStream.Read(buffer, 0, readBytes);
                var stream = new MemoryStream(buffer);

                var list = ReadList(stream);
                return new Parameter(id, list);
            }
            var value = ReadValue(fileStream);
            return new Parameter(id, value);
        }

        public static List<Parameter> ReadList(Stream stream)
        {
            var result = new List<Parameter>();
            while (stream.Position < stream.Length)
            {
                var parameter = ReadFrom(stream);
                result.Add(parameter);
            }
            return result;
        }

        private static long ReadValue(Stream fileStream)
        {
            long value = 0;
            var offset = 0;

            var currentByte = fileStream.ReadByte();
            while ((currentByte & 0x80) > 0)
            {
                value = value | ((long) (currentByte & 0x7f) << offset);
                offset += 7;
                currentByte = fileStream.ReadByte();
            }
            value = value | ((long) (currentByte & 0x7f) << offset);
            return value;
        }
    }
}
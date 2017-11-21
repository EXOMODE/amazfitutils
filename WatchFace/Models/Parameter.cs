using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;

namespace WatchFace.Models
{
    public class Parameter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

        public static List<Parameter> ReadList(Stream stream, int traceOffset = 0)
        {
            var result = new List<Parameter>();
            while (stream.Position < stream.Length)
            {
                var parameter = ReadFrom(stream, traceOffset);
                result.Add(parameter);
            }
            return result;
        }

        public static Parameter ReadFrom(Stream fileStream, int traceOffset = 0)
        {
            var rawId = fileStream.ReadByte();
            var id = (byte) ((rawId & 0xf8) >> 3);
            var flags = (ParameterFlags) (rawId & 0x7);

            var value = ReadValue(fileStream);
            if (flags.HasFlag(ParameterFlags.hasChildren))
            {
                Logger.Trace(()=> TraceWithOffset($"{id} ({rawId:X2}): {value} bytes", traceOffset));
                var buffer = new byte[value];
                fileStream.Read(buffer, 0, (int) value);
                var stream = new MemoryStream(buffer);

                var list = ReadList(stream, traceOffset + 1);
                return new Parameter(id, list);
            }
            Logger.Trace(()=> TraceWithOffset($"{id} ({rawId:X2}): {value} ({value:X2})", traceOffset));
            return new Parameter(id, value);
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

        private static string TraceWithOffset(string message, int offset)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < offset; i++) builder.Append("    ");
            builder.Append(message);
            return builder.ToString();
        }
    }
}
using NLog;
using System;
using System.IO;

namespace Resources
{
    public class Compression
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        class Header
        {
            public const int Size = 9;

            public byte[] Buffer;
            public byte Flags => Buffer[0];
            public uint CompressedLength => BitConverter.ToUInt32(Buffer, 1);
            public uint DecompressedLength => BitConverter.ToUInt32(Buffer, 5);
            public bool IsCompressed => (Flags & 1) > 0;
            public int LengthSize => (Flags & 2) > 0 ? 4 : 1;

            public static Header Read(BinaryReader reader)
            {
                var buffer = new byte[Size];
                reader.Read(buffer, 0, buffer.Length);
                return new Header { Buffer = buffer };
            }
        }

        private readonly Stream _stream;
        private readonly BinaryReader _reader;
        private readonly long _streamSize;

        public Compression(Stream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(stream);
            _streamSize = stream.Length;
        }

        public Stream Decompress()
        {
            var output = new MemoryStream();
            var buffer = new byte[5120];

            var offset = (uint)0;

            while (true)
            {
                var header = Header.Read(_reader);

                if (header.DecompressedLength == 4096)
                {
                    Logger.Debug("Compression block header read at offset 0x{0:X}", offset);
                    Logger.Debug("Flags 0x{0:X}, Compressed {1}, Decompressed {2}, LengthSize {3}",
                        header.Flags, header.CompressedLength, header.DecompressedLength, header.LengthSize);

                    _reader.Read(buffer, 0, (int)header.CompressedLength - 9);
                    offset += header.CompressedLength;


                    if (header.IsCompressed)
                    {
                        // Here should be decompression

                    }
                    else
                        output.Write(buffer, 0, (int)header.CompressedLength);
                }
                else
                {
                    var latestBlock = _streamSize - offset;
                    Logger.Debug("Latest block read at offset 0x{0:X}, size: {1}", offset, latestBlock);

                    _reader.Read(buffer, 0, (int)(latestBlock - Header.Size));

                    output.Write(header.Buffer, 0, Header.Size);
                    output.Write(buffer, 0, (int)(latestBlock - Header.Size));
                    break;
                }
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

    }
}

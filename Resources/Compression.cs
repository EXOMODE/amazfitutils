using NLog;
using System;
using System.IO;

namespace Resources
{
    public class Compression
    {
        private const int SectionSize = 4096;
        private static readonly byte[] SomeData = new byte[16] {4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0};
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
            var sourceBuffer = new byte[SectionSize];

            var offset = (uint)0;

            while (true)
            {
                var header = Header.Read(_reader);

                if (header.DecompressedLength == SectionSize)
                {
                    Logger.Debug("Compression block header read at offset 0x{0:X}", offset);
                    Logger.Debug("Flags 0x{0:X}, Compressed {1}, Decompressed {2}, LengthSize {3}",
                        header.Flags, header.CompressedLength, header.DecompressedLength, header.LengthSize);

                    _reader.Read(sourceBuffer, 0, (int)header.CompressedLength - Header.Size);
                    offset += header.CompressedLength;

                    if (header.IsCompressed)
                    {
                        var decompressed = DecompressBuffer(sourceBuffer);
                        output.Write(decompressed, 0, (int)header.DecompressedLength);
                    }
                    else
                        output.Write(sourceBuffer, 0, (int)header.CompressedLength - Header.Size);
                }
                else
                {
                    var latestBlock = _streamSize - offset;
                    Logger.Debug("Latest block read at offset 0x{0:X}, size: {1}", offset, latestBlock);

                    _reader.Read(sourceBuffer, 0, (int)(latestBlock - Header.Size));

                    output.Write(header.Buffer, 0, Header.Size);
                    output.Write(sourceBuffer, 0, (int)(latestBlock - Header.Size));
                    break;
                }
            }

            output.Seek(0, SeekOrigin.Begin);
            return output;
        }


        private byte[] DecompressBuffer(byte[] srcBuffer)
        {
            var dstBuffer = new byte[SectionSize];
            var data = (uint)1;
            var dstOffset = 0;
            var srcOffset = 0;

            while (true)
            {
                while (true)
                {
                    if (data == 1)
                    {
                        data = BitConverter.ToUInt32(srcBuffer, srcOffset);
                        srcOffset += 4;
                    }

                    var link = BitConverter.ToUInt32(srcBuffer, srcOffset);

                    if (data << 31 == 0)
                        break;

                    data = data >> 1;

                    uint dupOffset, dupLength;

                    if (link << 30 == 0)
                    {
                        dupOffset = (uint)((byte)link >> 2);
                        dupLength = 3;
                        srcOffset++;
                    }
                    else if ((link & 2) == 0)
                    {
                        dupOffset = (uint)((ushort)link >> 2);
                        dupLength = 3;
                        srcOffset += 2;
                    }
                    else if (link << 31 == 0)
                    {
                        dupOffset = (uint)((ushort)link >> 6);
                        dupLength = ((link >> 2) & 0xF) + 3;
                        srcOffset += 2;
                    }
                    else if ((link & 0x7F) == 3)
                    {
                        dupOffset = link >> 15;
                        dupLength = ((link >> 7) & 0xFF) + 3;
                        srcOffset += 4;
                    }
                    else
                    {
                        dupOffset = (link >> 7) & 0x1FFFF;
                        dupLength = ((link >> 2) & 0x1F) + 2;
                        srcOffset += 3;
                    }

                    Array.Copy(dstBuffer, dstOffset - dupOffset, dstBuffer, dstOffset, dupLength);
                    dstOffset += (int)dupLength;
                }

                if (dstOffset >= SectionSize - 11)
                    break;

                var length = SomeData[data & 0xf];
                Array.Copy(srcBuffer, srcOffset, dstBuffer, dstOffset, 4);
                data >>= length;
                dstOffset += length;
                srcOffset += length;
            }

            while (dstOffset <= SectionSize - 1)
            {
                if (data == 1)
                {
                    data = 0x80000000;
                    srcOffset += 4;
                }
                data >>= 1;
                var tmp = srcBuffer[srcOffset];
                srcOffset++;
                dstBuffer[dstOffset] = tmp;
                dstOffset++;
            }


            return dstBuffer;
        }
    }
}

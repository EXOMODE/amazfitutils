using System;
using System.IO;

namespace Resources.Utils
{
    public class BitWriter
    {
        private readonly byte[] _masks = {128, 192, 224, 240, 248, 252, 254, 255};
        private readonly Stream _stream;

        private int _currentBit;
        private byte _currentByte;

        public BitWriter(Stream stream)
        {
            _stream = stream;
        }

        public void Write(byte value)
        {
            WriteBits(value, 8);
        }

        public void Write(bool value)
        {
            WriteBits((uint) (value ? 1 : 0), 1);
        }

        public void WriteBits(string binaryString)
        {
            var length = binaryString.Length;
            var data = Convert.ToUInt32(binaryString, 2);
            WriteBits(data, length);
        }

        public void WriteBits(ulong data, int length)
        {
            while (length > 0)
            {
                var freeBits = 8 - _currentBit;
                var dataLength = Math.Min(freeBits, length);

                ulong currentByteData;
                if (length > 8)
                    currentByteData = data >> (length - 8);
                else
                    currentByteData = data << (8 - length);

                var appendData = (currentByteData & _masks[dataLength - 1]) >> _currentBit;
                _currentByte = (byte) (_currentByte | appendData);

                _currentBit += dataLength;
                length -= dataLength;
                if (_currentBit != 8) continue;

                _stream.WriteByte(_currentByte);
                _currentBit = 0;
                _currentByte = 0;
            }
        }

        public void Flush()
        {
            if (_currentBit > 0)
                _stream.WriteByte(_currentByte);
        }
    }
}
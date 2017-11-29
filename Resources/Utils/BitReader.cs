using System;
using System.IO;

namespace Resources.Utils
{
    public class BitReader
    {
        private readonly byte[] _masks = {128, 192, 224, 240, 248, 252, 254, 255};
        private readonly Stream _stream;

        private int _bitsRemaining;
        private int _currentByte;
        private bool _isDataPresent = true;

        public BitReader(Stream stream)
        {
            _stream = stream;
        }

        public BitReader(byte[] arrayBytes)
        {
            _stream = new MemoryStream(arrayBytes);
        }

        public bool IsDataPresent
        {
            get
            {
                if (_bitsRemaining > 0) return true;

                TryReadNext();
                return _isDataPresent;
            }
        }

        public bool ReadBit()
        {
            return ReadBits(1) != 0;
        }

        public byte ReadByte()
        {
            return (byte) ReadBits(8);
        }

        public ulong ReadBits(int length)
        {
            ulong data = 0;
            while (length > 0)
            {
                if (_bitsRemaining == 0 && _isDataPresent)
                    TryReadNext();

                var dataLength = Math.Min(length, _bitsRemaining);

                var currentData = (ulong) (_currentByte & _masks[dataLength - 1]);
                if (length > 8)
                    currentData = currentData << (length - 8);
                else
                    currentData = currentData >> (8 - length);
                data = data | currentData;

                _currentByte = _currentByte << dataLength;
                length -= dataLength;
                _bitsRemaining -= dataLength;
            }
            return data;
        }

        private void TryReadNext()
        {
            _currentByte = _stream.ReadByte();
            _isDataPresent = _currentByte > -1;
            if (_isDataPresent)
                _bitsRemaining = 8;
        }
    }
}
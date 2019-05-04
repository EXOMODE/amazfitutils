using System.IO;

namespace Resources.Models
{
    public class Blob : IResource
    {
        public static string ResourceExtension = ".dat";
        private readonly byte[] _data;

        public Blob(byte[] data)
        {
            _data = data;
        }

        public string Extension => ResourceExtension;

        public void WriteTo(Stream stream)
        {
            stream.Write(_data, 0, _data.Length);
        }

        public void ExportTo(Stream stream)
        {
            stream.Write(_data, 0, _data.Length);
        }
    }
}
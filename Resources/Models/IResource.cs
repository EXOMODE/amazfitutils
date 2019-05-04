using System.IO;

namespace Resources.Models
{
    public interface IResource
    {
        string Extension { get; }
        void WriteTo(Stream stream);
        void ExportTo(Stream stream);
    }
}
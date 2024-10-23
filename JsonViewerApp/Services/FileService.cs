using System.IO;
using JsonViewerApp.Interfaces;

namespace JsonViewerApp.Services
{
    /// <summary>
    ///     Сервис для работы с файлами.
    /// </summary>
    public class FileService : IFileService
    {
        /// <inheritdoc />
        public double GetFileSizeInMegaBytes(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var fileSizeInBytes = fileInfo.Length;
            return fileSizeInBytes / (1024.0 * 1024.0);
        }

        /// <inheritdoc />
        public bool ValidateFileSize(string filePath, double maxSizeInMegaBytes)
        {
            var fileSize = GetFileSizeInMegaBytes(filePath);
            return fileSize <= maxSizeInMegaBytes;
        }
    }
}
using System;
using System.IO;

namespace HereticalSolutions.Persistence.IO
{
    public static class StreamIO
    {
        public static bool  OpenReadStream(
            FileSystemSettings settings,
            out FileStream dataStream)
        {
            string savePath = settings.FullPath;

            dataStream = default(FileStream);

            if (!FileExists(settings.FullPath))
                return false;

            dataStream = new FileStream(savePath, FileMode.Open);

            return true;
        }
        
        public static bool OpenWriteStream(
            FileSystemSettings settings,
            out FileStream dataStream)
        {
            string savePath = settings.FullPath;

            dataStream = new FileStream(savePath, FileMode.Create);

            return true;
        }

        public static void CloseStream(FileStream dataStream)
        {
            dataStream.Close();
        }

        public static void Erase(FileSystemSettings settings)
        {
            string savePath = settings.FullPath;

            if (File.Exists(savePath))
                File.Delete(savePath);
        }
        
        /// <summary>
        /// Checks whether the file at the specified path exists
        /// - Also makes sure the folder directory specified in the path actually exists anyway
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Does the file exist</returns>
        private static bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("[UnityStreamIO] INVALID PATH");
			
            string directoryPath = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directoryPath))
                throw new Exception("[UnityStreamIO] INVALID DIRECTORY PATH");
			
            if (!Directory.Exists(directoryPath))
            {
                return false;
            }

            return File.Exists(path);
        }
    }
}
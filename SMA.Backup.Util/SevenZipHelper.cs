using System;
using System.IO;
using SevenZip;
using System.Configuration;

namespace SMA.Backup.Util
{
    public class SevenZipHelper : ISevenZipHelper
    {
        public void CompressFile(string sourceFile, string destinationFile)
        {
            SevenZipBase.SetLibraryPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"7zip\64bit\7z.dll"));
            var compressor = new SevenZip.SevenZipCompressor();
            var filesToCompress = new string[]
            {
                sourceFile
            };
            compressor.CompressFiles(destinationFile, filesToCompress);
        }

        public void CompressFolder(string sourceFolder, string destinationFile)
        {
            SevenZipBase.SetLibraryPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"7zip\64bit\7z.dll"));
            var compressor = new SevenZip.SevenZipCompressor();
            var filesToCompress = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
            compressor.CompressFiles(destinationFile, filesToCompress);
        }
    }
}

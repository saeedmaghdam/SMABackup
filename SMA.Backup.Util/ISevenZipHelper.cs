namespace SMA.Backup.Util
{
    public interface ISevenZipHelper
    {
        void CompressFile(string sourceFile, string destinationFile);

        void CompressFolder(string sourceFolder, string destinationFile);
    }
}

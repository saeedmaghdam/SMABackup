using System;

namespace SMA.Backup.Util
{
    public interface ICommonUtil
    {
        string EncodeGuid(Guid guid);

        Guid DecodeGuid(string value);

        byte[] GetHashSha256(string filename);

        byte[] GetHashMD5(string filename);

        string GetStringHashSha256(string filename);

        string GetStringHashMD5(string filename);

        string BytesToString(byte[] bytes);

        string AppPath();
    }
}

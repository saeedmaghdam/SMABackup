using System;
using System.IO;
using System.Security.Cryptography;

namespace SMA.Backup.Util
{
    public class CommonUtil : ICommonUtil
    {
        // The cryptographic service provider.
        private SHA256 Sha256 = SHA256.Create();

        public string EncodeGuid(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded.Substring(0, 22);
        }

        public Guid DecodeGuid(string value)
        {
            value = value.Replace("_", "/").Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }

        // Compute the file's hash.
        public byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        public byte[] GetHashMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public string GetStringHashSha256(string filename)
        {
            return BytesToString(GetHashSha256(filename));
        }

        public string GetStringHashMD5(string filename)
        {
            return BytesToString(GetHashMD5(filename));
        }

        // Return a byte array as a sequence of hex values.
        public string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }

        public string AppPath() => AppDomain.CurrentDomain.BaseDirectory;
    }
}

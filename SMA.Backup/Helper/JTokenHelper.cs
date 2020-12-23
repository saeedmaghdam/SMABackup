using Newtonsoft.Json.Linq;
using System.Linq;

namespace SMA.Backup.Helper
{
    public static class JTokenHelper
    {
        public static string TryGetValue(this JToken token, string key)
        {
            var obj = token.SingleOrDefault(x => ((JProperty)x).Name.ToLower() == key.ToLower());

            if (obj != null)
                return ((JProperty)obj).Value.ToString();

            return string.Empty;
        }
    }
}

using SMA.Backup.Source.Framework;

namespace SMA.Backup.Source.Model.Authentication
{
    public class BasicAuthenticationModel : IAuthenticationModel
    {
        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}

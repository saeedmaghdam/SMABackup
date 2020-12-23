using SMA.Backup.BackupSource.Framework;

namespace SMA.Backup.BackupSource.Model.Authentication
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

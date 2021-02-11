namespace SMA.Backup.Destination.Configuration
{
    public class EmailConfiguration : BaseConfiguration
    {
        public string MailServer
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

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

        public string From
        {
            get;
            set;
        }

        public bool Ssl
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }

        public string Cc
        {
            get;
            set;
        }

        public string Bcc
        {
            get;
            set;
        }
    }
}

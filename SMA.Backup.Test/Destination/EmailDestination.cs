using Autofac;
using NUnit.Framework;
using SMA.Backup.Destination.Configuration;
using SMA.Backup.Destination.Framework;
using SMA.Backup.Runtime;

namespace SMA.Backup.Test.Destination
{
    public class EmailDestination
    {
        private IContainer _container;

        [SetUp]
        public void Setup()
        {
            _container = Startup.Configure();
        }

        [Test]
        public void Test1()
        {
            var email = _container.Resolve<IEmailDestination>();

            email.Upload(new EmailConfiguration()
            {
                FileName = @"D:\unittest.txt",
                From = "autobackup@avens.ir",
                MailServer = "mail.avens.ir",
                Name = "AvensMail",
                Username = "autobackup@avens.ir",
                Password = "Abcd@123456",
                Port = 25,
                Ssl = false,
                To = "smasafat@gmail.com"
            });

            Assert.Pass();
        }
    }
}

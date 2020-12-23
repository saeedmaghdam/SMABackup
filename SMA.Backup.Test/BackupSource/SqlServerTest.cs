using Autofac;
using NUnit.Framework;
using SMA.Backup.Source.Configuration;
using SMA.Backup.Source.Framework;
using SMA.Backup.Source.Model.Authentication;
using SMA.Backup.Runtime;

namespace SMA.Backup.Test.BackupSource
{
    public class SqlServerTest
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
            var sqlServer = _container.Resolve<ISqlServerSource>();

            sqlServer.Backup(new SqlServerConfiguration()
            {
                ServerName = ".",
                DatabaseName = "BackupTest",
                AuthenticationModel = new BasicAuthenticationModel()
                {
                    Username = "sa",
                    Password = "Abcd@123456"
                },
                Name = "TestName"
            });

            Assert.Pass();
        }
    }
}
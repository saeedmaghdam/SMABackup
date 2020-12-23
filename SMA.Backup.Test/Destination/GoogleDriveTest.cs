using Autofac;
using NUnit.Framework;
using SMA.Backup.Destination.Configuration;
using SMA.Backup.Destination.Framework;
using SMA.Backup.Runtime;

namespace SMA.Backup.Test.Destination
{
    public class GoogleDriveTest
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
            var googleDrive = _container.Resolve<IGoogleDriveDestination>();

            googleDrive.Upload(new GoogleDriveConfiguration()
            {
                FileName = @"C:\SMABackupService\TestName\20201220105531.zip",
                Name = "Test",
                ApplicationName = "SMABackup",
                ClientId = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx.apps.googleusercontent.com",
                ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            });

            Assert.Pass();
        }
    }
}

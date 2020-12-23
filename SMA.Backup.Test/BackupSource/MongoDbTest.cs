using Autofac;
using NUnit.Framework;
using SMA.Backup.Source.Configuration;
using SMA.Backup.Source.Framework;
using SMA.Backup.Source.Model.Authentication;
using SMA.Backup.Runtime;

namespace SMA.Backup.Test.BackupSource
{
    public class MongoDbTest
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
            var mongoDb = _container.Resolve<IMongoDbSource>();

            mongoDb.Backup(new MongoDbConfiguration()
            {
                HostName = "localhost",
                CollectionName = "xxxxxxxxx",
                AuthenticationModel = new NoAuthentication(),
                Name = "TestName"
            });

            Assert.Pass();
        }
    }
}
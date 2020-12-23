using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SMA.Backup.Source.Configuration;
using SMA.Backup.Source.Framework;
using SMA.Backup.Source.Model.Authentication;
using SMA.Backup.Destination.Configuration;
using SMA.Backup.Destination.Framework;
using SMA.Backup.Framework;
using SMA.Backup.Helper;
using SMA.Backup.Util;
using System.Linq;
using System.Threading.Tasks;
using SMA.Backup.Source.Model;

namespace SMA.Backup
{
    public class BackupHandler : IBackupHandler
    {
        private readonly ICommonUtil _commonUtil;
        private readonly ISourceHandler _sourceHandler;
        private readonly IDestinationHandler _destinationHandler;

        public BackupHandler(ICommonUtil commonUtil, ISourceHandler sourceHandler, IDestinationHandler destinationHandler)
        {
            _commonUtil = commonUtil;
            _sourceHandler = sourceHandler;
            _destinationHandler = destinationHandler;
        }

        public async Task<Model.OutputModel> Backup(string configFileFullPath)
        {
            var configFile = System.IO.File.ReadAllText(configFileFullPath);
            var json = JsonConvert.DeserializeObject(configFile);

            var sources = ((JObject)json)["Source"].ToList();
            var destinations = ((JObject)json)["Destination"].ToList();
            var backups = ((JObject)json)["Backup"].ToList();

            foreach (var backup in backups)
            {
                var sourceName = backup.TryGetValue("source");
                var destinationName = backup.TryGetValue("destination");

                var source = sources.SingleOrDefault(x => JTokenHelper.TryGetValue(x, "name") == sourceName);
                var destination = destinations.SingleOrDefault(x => JTokenHelper.TryGetValue(x, "name") == destinationName);

                ISourceConfiguration sourceConfiguration = SourceNullConfiguration.Instance();
                IDestinationConfiguration destinationConfiguration = DestinationNullConfiguration.Instance();

                var sourceType = source.TryGetValue("type");
                var destinationType = destination.TryGetValue("type");

                switch (sourceType.ToLower())
                {
                    case "sqlserver":
                        {
                            var newConfiguration = new SqlServerConfiguration();
                            newConfiguration.Name = source.TryGetValue("name");
                            newConfiguration.DatabaseName = source.TryGetValue("databasename");
                            newConfiguration.ServerName = source.TryGetValue("servername");

                            int.TryParse(source.TryGetValue("port"), out var port);
                            if (port > 0)
                                newConfiguration.Port = port;

                            var username = source.TryGetValue("username");
                            var password = source.TryGetValue("password");
                            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                                newConfiguration.AuthenticationModel = new BasicAuthenticationModel()
                                {
                                    Username = username,
                                    Password = password
                                };

                            sourceConfiguration = newConfiguration;

                            break;
                        }
                    case "mongodb":
                        {
                            var newConfiguration = new MongoDbConfiguration();
                            newConfiguration.Name = source.TryGetValue("name");
                            newConfiguration.CollectionName = source.TryGetValue("collectionname");
                            newConfiguration.HostName = source.TryGetValue("hostname");

                            int.TryParse(source.TryGetValue("port"), out var port);
                            if (port > 0)
                                newConfiguration.Port = port;

                            var username = source.TryGetValue("username");
                            var password = source.TryGetValue("password");
                            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                            {
                                newConfiguration.AuthenticationModel = new BasicAuthenticationModel()
                                {
                                    Username = username,
                                    Password = password
                                };
                            }
                            else
                            {
                                newConfiguration.AuthenticationModel = new NoAuthentication();
                            }

                            sourceConfiguration = newConfiguration;

                            break;
                        }
                }

                var sourceResult = await _sourceHandler.CreateBackup(sourceConfiguration);

                if (sourceResult != SourceNullOutputModel.Instance())
                {
                    switch (destinationType.ToLower())
                    {
                        case "googledrive":
                            {
                                var newConfiguration = new GoogleDriveConfiguration();
                                newConfiguration.ApplicationName = destination.TryGetValue("applicationname");
                                newConfiguration.ClientId = destination.TryGetValue("clientid");
                                newConfiguration.ClientSecret = destination.TryGetValue("clientsecret");
                                newConfiguration.FileName = System.IO.Path.Combine(sourceResult.Path, sourceResult.FileName + sourceResult.FileExtension);
                                newConfiguration.Name = source.TryGetValue("name");

                                destinationConfiguration = newConfiguration;

                                break;
                            }
                    }

                    var destinationResult = await _destinationHandler.CopyBackup(destinationConfiguration);
                }
            }

            return new Model.OutputModel();
        }
    }
}

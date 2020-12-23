using System;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SMA.Backup.Source.Configuration;
using SMA.Backup.Source.Framework;
using SMA.Backup.Source.Model;
using SMA.Backup.Source.Model.Authentication;
using SMA.Backup.Common;
using SMA.Backup.Util;

namespace SMA.Backup.Source
{
    public class SqlServerSource : ISqlServerSource
    {
        private readonly ISystemConfiguration _configuration;
        private readonly ICommonUtil _commonUtil;
        private readonly ISevenZipHelper _sevenZipHelper;

        public SqlServerSource(ISystemConfiguration configuration, ICommonUtil commonUtil, ISevenZipHelper sevenZipHelper)
        {
            _configuration = configuration;
            _commonUtil = commonUtil;
            _sevenZipHelper = sevenZipHelper;
        }

        public async Task<OutputModel> Backup(ISourceConfiguration backupSourceConfiguration)
        {
            var configuration = backupSourceConfiguration as SqlServerConfiguration;
            var backupDate = DateTime.UtcNow;
            var filePath = System.IO.Path.Combine(_configuration.BackupPath, configuration.Name);
            if (!System.IO.File.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
            var fileName = backupDate.ToString("yyyyMMddHHmmss");
            var fileExtension = ".bak";
            var destinationPath = System.IO.Path.Combine(filePath, fileName + fileExtension);

            Microsoft.SqlServer.Management.Smo.Backup sqlBackup = new Microsoft.SqlServer.Management.Smo.Backup();
            //Specify the type of backup, the description, the name, and the database to be backed up.

            sqlBackup.Action = BackupActionType.Database;

            sqlBackup.BackupSetDescription = "Backop of: " + configuration.DatabaseName + " on " + DateTime.UtcNow.ToShortDateString();

            sqlBackup.BackupSetName = "FullBackUp";

            sqlBackup.Database = configuration.DatabaseName;

            //Declare a BackupDeviceItem
            BackupDeviceItem deviceItem = new BackupDeviceItem(destinationPath, DeviceType.File);

            //Define Server connection
            ServerConnection connection;

            var serverName = configuration.ServerName;
            if (configuration.Port.HasValue)
                serverName += "," + configuration.Port.Value;

            if (configuration.AuthenticationModel is BasicAuthenticationModel)
            {
                var basicAuthentication = configuration.AuthenticationModel as Model.Authentication.BasicAuthenticationModel;
                connection = new ServerConnection(serverName, basicAuthentication.Username, basicAuthentication.Password);
            }
            else if (configuration.AuthenticationModel is NoAuthentication)
            {
                connection = new ServerConnection(serverName);
            }
            else
            {
                return SourceNullOutputModel.Instance();
            }

            Server sqlServer = new Server(connection);
            sqlServer.ConnectionContext.StatementTimeout = 60 * 60;
            Database db = sqlServer.Databases[configuration.DatabaseName];

            sqlBackup.Initialize = true;

            sqlBackup.Checksum = true;

            sqlBackup.ContinueAfterError = true;
            //Add the device to the Backup object.

            sqlBackup.Devices.Add(deviceItem);

            //Set the Incremental property to False to specify that this is a full database backup. 
            sqlBackup.Incremental = false;
            sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);

            //Specify that the log must be truncated after the backup is complete.        
            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;
            sqlBackup.FormatMedia = false;

            //Run SqlBackup to perform the full database backup on the instance of SQL Server. 
            sqlBackup.SqlBackup(sqlServer);

            //Remove the backup device from the Backup object.           
            sqlBackup.Devices.Remove(deviceItem);

            fileExtension = ".7z";
            var zipFileDestinationPath = System.IO.Path.Combine(filePath, fileName + fileExtension); // To compress bak file and pass to destination handler
            _sevenZipHelper.CompressFile(destinationPath, zipFileDestinationPath);

            return new OutputModel()
            {
                Path = filePath,
                FileName = fileName,
                FileExtension = fileExtension,
                FileCreationDate = backupDate,
                FileHash = _commonUtil.GetStringHashMD5(zipFileDestinationPath),
                FileSize = new System.IO.FileInfo(zipFileDestinationPath).Length
            };
        }
    }
}

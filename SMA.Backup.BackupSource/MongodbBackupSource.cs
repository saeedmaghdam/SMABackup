using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using SMA.Backup.Source.Configuration;
using SMA.Backup.Source.Framework;
using SMA.Backup.Source.Model;
using SMA.Backup.Source.Model.Authentication;
using SMA.Backup.Common;
using SMA.Backup.Util;

namespace SMA.Backup.Source
{
    public class MongodbBackupSource : IMongoDbSource
    {
        private readonly ISystemConfiguration _configuration;
        private readonly ICommonUtil _commonUtil;
        private readonly ISevenZipHelper _sevenZipHelper;

        public MongodbBackupSource(ISystemConfiguration configuration, ICommonUtil commonUtil, ISevenZipHelper sevenZipHelper)
        {
            _configuration = configuration;
            _commonUtil = commonUtil;
            _sevenZipHelper = sevenZipHelper;
        }

        public async Task<OutputModel> Backup(ISourceConfiguration backupSourceConfiguration)
        {
            var configuration = backupSourceConfiguration as MongoDbConfiguration;
            var backupDate = DateTime.UtcNow;
            var filePath = System.IO.Path.Combine(_configuration.BackupPath, configuration.Name);
            if (!System.IO.File.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
            var fileName = backupDate.ToString("yyyyMMddHHmmss");
            var fileExtension = ".7z";
            var destinationPath = System.IO.Path.Combine(filePath, fileName + fileExtension);

            var backupPath = System.IO.Path.Combine(filePath, "backup");
            if (!System.IO.File.Exists(backupPath))
                System.IO.Directory.CreateDirectory(backupPath);

            var arguments = string.Empty;
            arguments += "--db=" + configuration.CollectionName;
            if (!string.IsNullOrEmpty(configuration.HostName))
                arguments += " --host=" + configuration.HostName;
            if (configuration.Port.HasValue)
                arguments += " --port=" + configuration.Port.Value;
            arguments += $" --out=\"{backupPath}\"";

            if (configuration.AuthenticationModel is BasicAuthenticationModel)
            {
                var basicAuthentication = configuration.AuthenticationModel as Model.Authentication.BasicAuthenticationModel;
                arguments += $" --username=\"{basicAuthentication.Username}\"";
                arguments += $" --password=\"{basicAuthentication.Password}\"";
            }
            else if (configuration.AuthenticationModel is NoAuthentication)
            {
            }
            else
            {
                return SourceNullOutputModel.Instance();
            }

            var process = new Process
            {
                StartInfo =
                {
                    FileName = System.IO.Path.Combine(_commonUtil.AppPath(), "mongotools", "mongodump.exe"),
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (obj, args) => Console.WriteLine(args.Data);
            process.ErrorDataReceived += (obj, args) => Console.WriteLine(args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            _sevenZipHelper.CompressFolder(backupPath, destinationPath);

            return new OutputModel()
            {
                Path = filePath,
                FileName = fileName,
                FileExtension = fileExtension,
                FileCreationDate = backupDate,
                FileHash = _commonUtil.GetStringHashMD5(destinationPath),
                FileSize = new System.IO.FileInfo(destinationPath).Length
            };
        }
    }
}

﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SMA.Backup.BackupSource.Configuration;
using SMA.Backup.BackupSource.Framework;
using SMA.Backup.BackupSource.Model;
using SMA.Backup.BackupSource.Model.Authentication;
using SMA.Backup.Common;
using SMA.Backup.Util;

namespace SMA.Backup.BackupSource
{
    public class MongodbBackupSource : IMongoDbBackupSource
    {
        private readonly ISystemConfiguration _configuration;
        private readonly ICommonUtil _commonUtil;

        public MongodbBackupSource(ISystemConfiguration configuration, ICommonUtil commonUtil)
        {
            _configuration = configuration;
            _commonUtil = commonUtil;
        }

        public async Task<OutputModel> Backup(IBackupSourceConfiguration backupSourceConfiguration)
        {
            var configuration = backupSourceConfiguration as MongoDbConfiguration;
            var backupDate = DateTime.UtcNow;
            var filePath = System.IO.Path.Combine(_configuration.BackupPath, configuration.Name);
            if (!System.IO.File.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
            var fileName = backupDate.ToString("yyyyMMddHHmmss");
            var fileExtension = ".zip";
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
                return NullOutputModel.Create();

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

            ZipFile.CreateFromDirectory(System.IO.Path.Combine(backupPath), destinationPath);

            var result = new OutputModel()
            {
                Path = filePath,
                FileName = fileName,
                FileExtension = fileExtension,
                FileCreationDate = backupDate,
                FileHash = _commonUtil.GetStringHashMD5(destinationPath),
                FileSize = new System.IO.FileInfo(destinationPath).Length
            };

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
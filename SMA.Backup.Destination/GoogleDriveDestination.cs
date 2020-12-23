using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Logging;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using SMA.Backup.Destination.Framework;
using SMA.Backup.Destination.Model;
using SMA.Backup.Util;
using MimeTypes;
using SMA.Backup.Destination.Configuration;

namespace SMA.Backup.Destination
{
    public class GoogleDriveDestination : IGoogleDriveDestination
    {
        private readonly ICommonUtil _commonUtil;
        private readonly ILogger _logger;
        private GoogleDriveConfiguration _configuration;

        public GoogleDriveDestination(ICommonUtil commonUtil)
        {
            _commonUtil = commonUtil;
        }

        public Task<OutputModel> Upload(IDestinationConfiguration destinationConfiguration)
        {
            _configuration = destinationConfiguration as GoogleDriveConfiguration;

            var service = Authorize();
            uploadFile(service);

            return null;
        }

        private DriveService Authorize()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                DriveService.Scope.DriveFile,};
            var clientId = _configuration.ClientId;      // From https://console.developers.google.com  
            var clientSecret = _configuration.ClientSecret;                                                  // From https://console.developers.google.com  

            // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%  
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }, scopes,
                Environment.UserName, CancellationToken.None, new FileDataStore(System.IO.Path.Combine(_commonUtil.AppPath(), "token.json"), true)).Result;
            //Once consent is recieved, your token will be stored locally on the AppData directory, so that next time you wont be prompted for consent.   

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _configuration.ApplicationName

            });
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);
            //Long Operations like file uploads might timeout. 100 is just precautionary value, can be set to any reasonable value depending on what you use your service for  

            return service;
        }

        public bool uploadFile(DriveService driveService)
        {
            if (System.IO.File.Exists(_configuration.FileName))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = $"{_configuration.Name}_{System.IO.Path.GetFileName(_configuration.FileName)}";
                body.Description = "Uploaded by SMABackupService!";
                body.MimeType = MimeTypeMap.GetMimeType(_configuration.FileName);
                // body.Parents = new List<string> { parent };// UN comment if you want to upload to a folder(ID of parent folder need to be send as paramter in above method)
                byte[] byteArray = System.IO.File.ReadAllBytes(_configuration.FileName);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = driveService.Files.Create(body, stream, MimeTypeMap.GetMimeType(_configuration.FileName));
                    request.SupportsTeamDrives = true;
                    request.Upload();

                    return true;
                }
                catch (Exception e)
                {
                    _logger.Error(e.ToString());
                }
            }
            else
            {
                _logger.Error("Requested file does not exists. Filename: " + _configuration.FileName);
            }

            return false;
        }
    }
}

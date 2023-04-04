using Dropbox.Api.Files;
using Dropbox.Api;

namespace CountryInfoApi.Utilites.CloudStorage
{
    public class CLoudStorage
    {
        private readonly DropboxClient _client;

        public CLoudStorage(string accessToken)
        {
            
            _client = new DropboxClient(accessToken);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            var folder = await GetOrCreateFolderAsync(folderName);
            var path = $"{folder.PathLower}/{ChageFileName(file.FileName)}";
            using (var stream = file.OpenReadStream())
            {
                var result = await _client.Files.UploadAsync(
                    path,
                    WriteMode.Overwrite.Instance,
                    body: stream);
                return result.PathDisplay;
            }
        }

        private async Task<FolderMetadata> GetOrCreateFolderAsync(string folderName)
        {
            try
            {
                var folder = await _client.Files.GetMetadataAsync($"/{folderName}");
                if (folder.IsFolder) return (FolderMetadata)folder;
            }
            catch (ApiException<GetMetadataError> ex)
            {
                if (ex.ErrorResponse.IsPath)
                {
                    var folder = await _client.Files.CreateFolderV2Async($"/{folderName}");
                    return folder.Metadata;
                }
            }
            return null;
        }

        public string ChageFileName(string FileName)
        {
            return Guid.NewGuid().ToString() + FileName;
        }
    }
}

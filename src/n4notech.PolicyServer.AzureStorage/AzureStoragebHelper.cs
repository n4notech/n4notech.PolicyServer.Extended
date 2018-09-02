using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using n4notech.PolicyServer.Manager;
using Newtonsoft.Json;
using PolicyServer.Runtime.Client;

namespace n4notech.PolicyServer.AzureStorage
{
    public static class AzureStoragebHelper
    {
        private static CloudBlobContainer CloudBlobContainer;
        
        public static void InitCloudBlobContainer()
        {
            var connString = Environment.GetEnvironmentVariable("AzureBlobStorage");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer = cloudBlobClient.GetContainerReference(Environment.GetEnvironmentVariable("AzureBlobContainerName"));
        }

        private static async Task<string> GetBlobData(string fileId)
        {
            string policyServerConfigFileName = fileId == null ? $"policyServerConfig.json" : $"policyServerConfig_{fileId}.json";

            CloudBlockBlob cloudBlockBlob = CloudBlobContainer.GetBlockBlobReference(policyServerConfigFileName);

            var text = await cloudBlockBlob.DownloadTextAsync();

            var startIndex = 0;
            while (char.GetUnicodeCategory(text, startIndex) == UnicodeCategory.Format)
            {
                startIndex++;
            }
            text = text.Substring(startIndex, text.Length - startIndex);

            return text;
        }

        public static async Task<PolicyResult> GetConfigFileAsync(string fileId = null)
        {
            InitCloudBlobContainer();

            string text = await GetBlobData(fileId);

            var config = JsonConvert.DeserializeObject<DeserzializePolicyEditableResult>(text);

            var roles = config.Policy.Roles.Select(x => x.Name).ToArray();
            var permissions = config.Policy.Permissions.Select(x => x.Name).ToArray();

            var result = new PolicyResult()
            {
                Roles = roles.Distinct(),
                Permissions = permissions.Distinct()
            };

            return result;
        }

        public static async Task<PolicyEditableResult> GetManagerConfigFileAsync(string fileId = null)
        {
            InitCloudBlobContainer();

            string text = await GetBlobData(fileId);

            return JsonConvert.DeserializeObject<DeserzializePolicyEditableResult>(text).Policy;
        }

        public static async Task UpdateConfigFileAsync(PolicyEditableResult policy, string fileId = null)
        {
            InitCloudBlobContainer();

            string policyServerConfigFileName = fileId == null ? $"policyServerConfig.json" : $"policyServerConfig_{fileId}.json";

            CloudBlockBlob cloudBlockBlob = CloudBlobContainer.GetBlockBlobReference(policyServerConfigFileName);
            cloudBlockBlob.Properties.ContentType = "application/json";

            CloudBlockBlob cloudBlockBlobSnapshot = await cloudBlockBlob.CreateSnapshotAsync();

            try
            {
                await cloudBlockBlob.SerializeObjectToBlobAsync(new DeserzializePolicyEditableResult { Policy = policy });
            }
            catch (Exception)
            {
                await cloudBlockBlob.DeleteAsync();
                cloudBlockBlob = cloudBlockBlobSnapshot;
            }
            finally
            {
                await cloudBlockBlobSnapshot.DeleteAsync();
            }
        }

        internal static async Task SerializeObjectToBlobAsync(this CloudBlockBlob blob, object obj)
        {
            using (Stream stream = await blob.OpenWriteAsync())
            using (StreamWriter sw = new StreamWriter(stream))
            using (JsonTextWriter jtw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer() { Formatting = Formatting.Indented };
                ser.Serialize(jtw, obj);
            }
        }
    }
}

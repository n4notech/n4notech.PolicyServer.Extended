using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using n4notech.PolicyServer.Manager;
using Newtonsoft.Json;
using PolicyServer.Local;

namespace n4notech.PolicyServer.AzureStorage
{
    public static class AzureStorageHelper
    {
        private static CloudBlobContainer CloudBlobContainer;
        
        public static void InitCloudBlobContainer()
        {
            var connString = Environment.GetEnvironmentVariable("AzureBlobStorage");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer = cloudBlobClient.GetContainerReference(Environment.GetEnvironmentVariable("AzureBlobContainerName"));
        }

        public static async Task<Policy> GetConfigFileAsync(string fileId = null)
        {
            InitCloudBlobContainer();

            string policyServerConfigFileName = fileId == null ? $"policyServerConfig.json" : $"policyServerConfig_{fileId}.json";

            CloudBlockBlob cloudBlockBlob = CloudBlobContainer.GetBlockBlobReference(policyServerConfigFileName);

            var text = await cloudBlockBlob.DownloadTextAsync();

            var startIndex = 0;
            while (char.GetUnicodeCategory(text, startIndex) == UnicodeCategory.Format)
            {
                startIndex++;
            }
            text = text.Substring(startIndex, text.Length - startIndex);

            return JsonConvert.DeserializeObject<DeserializePolicyResult>(text).Policy;
        }

        public static async Task UpdateConfigFileAsync(Policy policy, string fileId = null)
        {
            InitCloudBlobContainer();

            string policyServerConfigFileName = fileId == null ? $"policyServerConfig.json" : $"policyServerConfig_{fileId}.json";

            CloudBlockBlob cloudBlockBlob = CloudBlobContainer.GetBlockBlobReference(policyServerConfigFileName);
            cloudBlockBlob.Properties.ContentType = "application/json";

            CloudBlockBlob cloudBlockBlobSnapshot = await cloudBlockBlob.CreateSnapshotAsync();

            try
            {
                await cloudBlockBlob.SerializeObjectToBlobAsync(new DeserializePolicyResult { Policy = policy });
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

using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using n4notech.PolicyServer.AzureStorage;
using n4notech.PolicyServer.Manager.Interfaces;
using PolicyServer.Local;
using Xunit;

namespace n4notech.PolicyServer.Manager.Tests
{
    public class BlobHandlingTest
    {
        private IPolicyServerRuntimeManager PolicyServerRuntimeManager;

        public BlobHandlingTest()
        {
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", "UseDevelopmentStorage=true");
        }

        [Fact]
        public async Task NotExistantBlobConfigFile()
        {
            Environment.SetEnvironmentVariable("AzureBlobContainerName", "azure-webjobs-hosts");

            try
            {
                var policy = await AzureStorageHelper.GetConfigFileAsync();
            }
            catch(StorageException sex)
            {
                // Clear variable
                Environment.SetEnvironmentVariable("AzureBlobContainerName", string.Empty);

                Assert.Contains("The specified blob does not exist.", sex.Message);
            }
        }

        [Fact]
        public async Task CustomBlobStorage()
        {
            Environment.SetEnvironmentVariable("AzureBlobStorage", "UseDevelopmentStorage=true;custom");

            try
            {
                var policy = await AzureStorageHelper.GetConfigFileAsync();
            }
            catch (FormatException sex)
            {
                // Clear variable
                Environment.SetEnvironmentVariable("AzureBlobStorage", string.Empty);

                Assert.StartsWith("Settings must be of the form", sex.Message);
            }
        }

        [Fact]
        public async Task CustomContainerName()
        {
            Environment.SetEnvironmentVariable("AzureBlobContainerName", "wrong-container-name");

            try
            {
                var policy = await AzureStorageHelper.GetConfigFileAsync();
            }
            catch (StorageException sex)
            {
                // Clear variable
                Environment.SetEnvironmentVariable("AzureBlobContainerName", string.Empty);

                Assert.Contains("The specified container does not exist.", sex.Message);
            }
        }

        [Theory]
        [InlineData("1", typeof(Policy))]
        public async Task CanGetClientConfigFile(string fileId, Type expectedResult)
        {
            var policy = await AzureStorageHelper.GetConfigFileAsync(fileId);

            Assert.True(policy.GetType() == expectedResult);
        }

        [Theory]
        [InlineData("123", "proprietari", "1", true)]
        public async Task CanAddElementToBlobConfigFile(string userId, string roleName, string fileId, bool expectedResult)
        {
            var policy = await AzureStorageHelper.GetConfigFileAsync(fileId);

            PolicyServerRuntimeManager = new PolicyServerRuntimeManager(policy);

            PolicyServerRuntimeManager.AddUserInRole(userId, roleName);

            var result = await PolicyServerRuntimeManager.SaveChangesAsync(fileId);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("123", "proprietari", "1", true)]
        public async Task CanRemoveElementFromBlobConfigFile(string userId, string roleName, string fileId, bool expectedResult)
        {
            var policy = await AzureStorageHelper.GetConfigFileAsync(fileId);

            PolicyServerRuntimeManager = new PolicyServerRuntimeManager(policy);

            PolicyServerRuntimeManager.RemoveUserFromRole(userId, roleName);

            var result = await PolicyServerRuntimeManager.SaveChangesAsync(fileId);

            Assert.Equal(expectedResult, result);
        }
    }
}

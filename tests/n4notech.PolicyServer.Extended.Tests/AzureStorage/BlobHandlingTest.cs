using System;
using System.Threading.Tasks;
using n4notech.PolicyServer.AzureStorage;
using n4notech.PolicyServer.Manager.Interfaces;
using Xunit;

namespace n4notech.PolicyServer.Manager.Tests
{
    public class BlobHandlingTest
    {
        private IPolicyServerRuntimeManager PolicyServerRuntimeManager;

        public BlobHandlingTest()
        {
            Environment.SetEnvironmentVariable("AzureBlobStorage", "UseDevelopmentStorage=true");
            Environment.SetEnvironmentVariable("AzureBlobContainerName", "azure-webjobs-hosts");
        }
        
        [Theory]
        [InlineData("123", "proprietari", "1", true)]
        public async Task CanAddElementToBlobConfigFile(string userId, string roleName, string fileId, bool expectedResult)
        {
            var policy = await BlobExtensions.GetConfigFileAsync("1");

            PolicyServerRuntimeManager = new PolicyServerRuntimeManager(policy);

            PolicyServerRuntimeManager.AddUserInRole(userId, roleName);

            var result = await PolicyServerRuntimeManager.SaveChangesAsync(fileId);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("123", "proprietari", "1", true)]
        public async Task CanRemoveElementFromBlobConfigFile(string userId, string roleName, string fileId, bool expectedResult)
        {
            var policy = await BlobExtensions.GetConfigFileAsync("1");

            PolicyServerRuntimeManager = new PolicyServerRuntimeManager(policy);

            PolicyServerRuntimeManager.RemoveUserFromRole(userId, roleName);

            var result = await PolicyServerRuntimeManager.SaveChangesAsync(fileId);

            Assert.Equal(expectedResult, result);
        }
    }
}

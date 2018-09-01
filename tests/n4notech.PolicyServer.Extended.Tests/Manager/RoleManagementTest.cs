using n4notech.PolicyServer.Extended.Tests;
using Xunit;

namespace n4notech.PolicyServer.Manager.Tests
{
    public class RoleManagementTest : IClassFixture<ConfigFixture>
    {
        ConfigFixture _configFixture;

        public RoleManagementTest(ConfigFixture configFixture)
        {
            _configFixture = configFixture;
        }
        
        [Theory]
        [InlineData("123", "amministratori", false)]
        [InlineData("123", "proprietari", true)]
        public void CanAddUserInRole(string userId, string roleName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.AddUserInRole(userId, roleName);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ABC", "amministratori", true)]
        public void CanRemoveUserFromRole(string userId, string roleName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.RemoveUserFromRole(userId, roleName);

            Assert.Equal(expectedResult, result);
        }

        [Theory()]
        [InlineData("proprietari", false)]
        [InlineData("newRoleName", true)]
        public void CanAddRole(string roleName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.AddRole(roleName);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("inquilini", true)]
        [InlineData("notExistantRole", false)]
        public void CanRemoveRole(string roleName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.RemoveRole(roleName);

            Assert.Equal(expectedResult, result);
        }
    }
}

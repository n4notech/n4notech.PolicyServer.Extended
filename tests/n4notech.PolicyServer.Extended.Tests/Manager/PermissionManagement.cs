using n4notech.PolicyServer.Extended.Tests;
using Xunit;

namespace n4notech.PolicyServer.Manager.Tests
{
    public class PermissionManagement : IClassFixture<ConfigFixture>
    {
        ConfigFixture _configFixture;

        public PermissionManagement(ConfigFixture configFixture)
        {
            _configFixture = configFixture;
        }

        [Theory]
        [InlineData("SeeInquilini", "proprietari", false)]
        [InlineData("SeeInquilini", "fornitori", true)]
        public void CanAddPermissionToRole(string permissionName, string roleName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.AddPermissionToRole(permissionName, roleName);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("SeeInquilini", "amministratori", true)]
        [InlineData("SeeInquilini", "inquilini", false)]
        public void CanRemovePermissionFromRole(string permissionName, string roleName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.RemovePermissionFromRole(permissionName, roleName);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("SendSegnalazione", false)]
        [InlineData("SeeNewPermission", true)]
        public void CanAddPermission(string permissionName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.AddPermission(permissionName);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("SeeProprietari", true)]
        public void CanRemovePermission(string permissionName, bool expectedResult)
        {
            var result = _configFixture.PolicyServerRuntimeManager.RemovePermission(permissionName);

            Assert.Equal(expectedResult, result);
        }
    }
}

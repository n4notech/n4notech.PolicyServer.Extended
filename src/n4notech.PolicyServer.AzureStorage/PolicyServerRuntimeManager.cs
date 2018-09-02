using System;
using System.Threading.Tasks;
using n4notech.PolicyServer.AzureStorage;
using n4notech.PolicyServer.Manager.Base;
using PolicyServer.Local;

namespace n4notech.PolicyServer.Manager
{
    public class PolicyServerRuntimeManager : PolicyServerRuntimeManagerBase
    {
        public PolicyServerRuntimeManager(Policy policy) : base (policy)
        {

        }

        public override async Task<bool> SaveChangesAsync(string fileId = null)
        {
            try
            {
                await AzureStorageHelper.UpdateConfigFileAsync(_policy, fileId);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

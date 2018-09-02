using System;

using Microsoft.Extensions.Configuration;

using n4notech.PolicyServer.Manager;
using n4notech.PolicyServer.Manager.Interfaces;
using PolicyServer.Local;

namespace n4notech.PolicyServer.Extended.Tests
{
    public class ConfigFixture : IDisposable
    {
        private readonly IConfiguration Configuration;
        public IPolicyServerRuntimeManager PolicyServerRuntimeManager { get; internal set; }

        protected readonly Policy _policy = new Policy();

        public ConfigFixture()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile(path: $"policyServerConfig_1.json", optional: false, reloadOnChange: true).Build();

            Configuration.GetSection("Policy").Bind(_policy);

            PolicyServerRuntimeManager = new PolicyServerRuntimeManager(_policy);
        }

        public void Dispose()
        {
            
        }
    }
}

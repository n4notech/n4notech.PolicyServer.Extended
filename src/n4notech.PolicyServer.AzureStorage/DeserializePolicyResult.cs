// Copyright (c) Emanuele Filardo, N4notecnologia srls. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using n4notech.PolicyServer.Manager;
using Newtonsoft.Json;
using PolicyServer.Local;

namespace n4notech.PolicyServer.AzureStorage
{
    class DeserializePolicyResult
    {
        [JsonProperty("Policy")]
        public Policy Policy { get; set; }
    }
}

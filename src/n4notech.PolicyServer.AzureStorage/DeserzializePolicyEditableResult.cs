// Copyright (c) Emanuele Filardo, N4notecnologia srls. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using n4notech.PolicyServer.Manager;

namespace n4notech.PolicyServer.AzureStorage
{
    class DeserzializePolicyEditableResult
    {
        [JsonProperty("Policy")]
        public PolicyEditableResult Policy { get; set; }
    }
}

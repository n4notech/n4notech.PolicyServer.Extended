// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Edited by Emanuele Filardo N4notecnologia srls. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PolicyServer.Runtime.Client;

namespace n4notech.PolicyServer.Manager
{
    public class Policy
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; set; } = new List<Role>(); // Modified

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permission> Permissions { get; set; } = new List<Permission>(); // Modified

        internal Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var roles = Roles.Where(x => x.Evaluate(user)).Select(x => x.Name).ToArray();
            var permissions = Permissions.Where(x => x.Evaluate(roles)).Select(x => x.Name).ToArray();

            var result = new PolicyResult()
            {
                Roles = roles.Distinct(),
                Permissions = permissions.Distinct()
            };

            return Task.FromResult(result);
        }
    }
}

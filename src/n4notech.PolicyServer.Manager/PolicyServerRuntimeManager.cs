// Copyright (c) Emanuele Filardo, N4notecnologia srls. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using n4notech.PolicyServer.Manager.Interfaces;
using PolicyServer.Local;

namespace n4notech.PolicyServer.Manager.Base
{
    public abstract class PolicyServerRuntimeManagerBase : IPolicyServerRuntimeManager
    {
        protected readonly Policy _policy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyServerRuntimeManagerBase"/> class.
        /// </summary>
        /// <param name="policy">The policy to manage.</param>
        protected PolicyServerRuntimeManagerBase(Policy policy)
        {
            _policy = policy;
        }

        public bool AddUserInRole(string userId, string roleName)
        {
            if (!_policy.Roles.Any(r => r.Name == roleName && r.Subjects.Contains(userId)))
            {
                _policy.Roles.SingleOrDefault(r => r.Name == roleName).Subjects.Add(userId);
                
                return true;
            }

            return false;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            if (_policy.Roles.Any(r => r.Name == roleName && r.Subjects.Contains(userId)))
            {
                _policy.Roles.SingleOrDefault(r => r.Name == roleName).Subjects.Remove(userId);

                return true;
            }

            return false;
        }

        public bool AddRole(string roleName)
        {
            if (!_policy.Roles.Any(r => r.Name == roleName))
            {
                _policy.Roles.Add(new Role { Name = roleName });

                return true;
            }

            return false;
        }

        public bool RemoveRole(string roleName)
        {            
            if (_policy.Roles.Any(r => r.Name == roleName))
            {
                _policy.Roles.Remove(_policy.Roles.SingleOrDefault(r => r.Name == roleName));
                
                return true;
            }

            return false;
        }

        public bool AddPermissionToRole(string permissionName, string roleName)
        {
            if (_policy.Permissions.Any(p => p.Name == permissionName && !p.Roles.Contains(roleName)))
            {
                _policy.Permissions.SingleOrDefault(p => p.Name == permissionName).Roles.Add(roleName);

                return true;
            }

            return false;
        }
        
        public bool RemovePermissionFromRole(string permissionName, string roleName)
        {
            if (_policy.Permissions.Any(p => p.Name == permissionName && p.Roles.Contains(roleName)))
            {
                _policy.Permissions.SingleOrDefault(p => p.Name == permissionName).Roles.Remove(roleName);
                
                return true;
            }

            return false;
        }

        public bool AddPermission(string permissionName)
        {
            if (!_policy.Permissions.Any(p => p.Name == permissionName))
            {
                _policy.Permissions.Add(new Permission { Name = permissionName });
                
                return true;
            }

            return false;
        }

        public bool RemovePermission(string permissionName)
        {
            if (_policy.Permissions.Any(p => p.Name == permissionName))
            {
                _policy.Permissions.Remove(_policy.Permissions.SingleOrDefault(p => p.Name == permissionName));
                
                return true;
            }

            return false;
        }

        public virtual Task<bool> SaveChangesAsync(string fileId = null)
        {
            throw new NotImplementedException();
        }
    }
}

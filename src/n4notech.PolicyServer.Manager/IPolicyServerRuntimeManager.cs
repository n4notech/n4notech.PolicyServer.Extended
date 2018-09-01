// Copyright (c) Emanuele Filardo, N4notecnologia srls. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;

namespace n4notech.PolicyServer.Manager.Interfaces
{
    public interface IPolicyServerRuntimeManager
    {
        bool AddPermission(string permissionName);
        bool AddPermissionToRole(string permissionName, string roleName);
        bool AddRole(string roleName);
        bool AddUserInRole(string userId, string roleName);
        bool RemovePermission(string permissionName);
        bool RemovePermissionFromRole(string permissionName, string roleName);
        bool RemoveRole(string roleName);
        bool RemoveUserFromRole(string userId, string roleName);

        Task<bool> SaveChangesAsync(string fileId = null);
    }
}
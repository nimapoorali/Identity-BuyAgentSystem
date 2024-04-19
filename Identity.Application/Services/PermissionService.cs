using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Generals;
using Identity.Application.Abstraction.Permissions;
using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Services
{
    public class PermissionService : IPermissionService
    {
        const string PermissionsCacheName = "PermissionsCache";
        const string UserPermissionsCacheName = "UserPermissionsCache";
        //private static object _lock = new object();
        private static SemaphoreSlim _semaphoreAllPermissions = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim _semaphoreUserPermissions = new SemaphoreSlim(1, 1);
        public IIdentityUnitOfWork UnitOfWork { get; }
        public ICachingService CacheService { get; }

        public PermissionService(IIdentityUnitOfWork unitOfWork, ICachingService cacheService)
        {
            UnitOfWork = unitOfWork;
            CacheService = cacheService;
        }

        public async Task<bool> UserHasPermission(Guid userId, string permissionName)
        {
            var userHasPermission = false;
            var userPermissionsCacheData = await GetUserPermissionsCacheAsync(userId);
            if (userPermissionsCacheData is not null)
                userHasPermission = userPermissionsCacheData.Contains(permissionName);

            return userHasPermission;
        }

        public void UserPermissionsChanged(Guid userId)
        {
            var userPermissionsCacheData = CacheService.GetData<Dictionary<Guid, string[]>>(UserPermissionsCacheName);

            if (userPermissionsCacheData is not null && userPermissionsCacheData.Keys.Contains(userId))
                userPermissionsCacheData.Remove(userId);
        }
        public void PermissionChanged(string permissionName)
        {
            var userPermissionsCacheData = CacheService.GetData<Dictionary<Guid, string[]>>(UserPermissionsCacheName);
            if (userPermissionsCacheData is not null)
                foreach (var item in userPermissionsCacheData.Where(d => d.Value.Contains(permissionName)).ToList())
                    userPermissionsCacheData.Remove(item.Key);
        }

        private async Task<IEnumerable<Permission>?> GetAllPermissionsCacheAsync()
        {
            var permissionsCacheData = CacheService.GetData<IEnumerable<Permission>>(PermissionsCacheName);
            if (permissionsCacheData is not null)
                return permissionsCacheData;

            //lock (_lock)
            await _semaphoreAllPermissions.WaitAsync();
            {
                try
                {
                    if (permissionsCacheData is null)
                    {
                        var expirationDate = DateTimeOffset.Now.AddMinutes(30);
                        permissionsCacheData = await UnitOfWork.Permissions.FindAll();
                        CacheService.SetData(PermissionsCacheName, permissionsCacheData, expirationDate);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    _semaphoreAllPermissions.Release();
                }
            }

            return permissionsCacheData;
        }

        private async Task<string[]?> GetUserPermissionsCacheAsync(Guid userId)
        {
            var userPermissionsCacheData = CacheService.GetData<Dictionary<Guid, string[]>>(UserPermissionsCacheName);

            if (userPermissionsCacheData is not null && userPermissionsCacheData.Keys.Contains(userId))
                return userPermissionsCacheData[userId];

            await _semaphoreUserPermissions.WaitAsync();
            {
                try
                {
                    var user = await UnitOfWork.Users.FindByIdAsync(userId);
                    if (user is null) return null;//new string[] { };

                    var userRoleIds = user.Roles.Select(r => r.RoleId).ToArray();


                    var permissionNames = (await UnitOfWork.Permissions
                        .FindAsync(p => p.Roles.Any(r => userRoleIds.Any(ur => ur == r.RoleId))))
                        .Select(p => p.Name).Select(n => n.Name).ToArray();

                    if (userPermissionsCacheData is null)
                    {
                        userPermissionsCacheData = new Dictionary<Guid, string[]>();
                        var expirationDate = DateTimeOffset.Now.AddMinutes(30);
                        CacheService.SetData(UserPermissionsCacheName, userPermissionsCacheData, expirationDate, true);
                    }

                    userPermissionsCacheData.Add(userId, permissionNames);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    _semaphoreUserPermissions.Release();
                }
            }

            return userPermissionsCacheData[userId];
        }
    }
}

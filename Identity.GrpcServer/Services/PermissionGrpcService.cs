using Grpc.Core;
using Identity.Application.Abstraction.Permissions;

namespace Identity.GrpcServer.Services
{
    public class PermissionGrpcService : Permission.PermissionBase
    {
        public IPermissionService PermissionService { get; }

        public PermissionGrpcService(IPermissionService permissionService)
        {
            PermissionService = permissionService;
        }

        public override async Task<CheckPermissionResponse> CheckPermission(CheckPermissionRequest request, ServerCallContext context)
        {
            var userHasPermission = await PermissionService.UserHasPermission(Guid.Parse(request.Userid), request.Permissionname);

            return new CheckPermissionResponse { Isallowed = userHasPermission };

            //return base.CheckPermission(request, context);
        }
    }
}

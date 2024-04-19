using Grpc.Net.Client;
using Identity.GrpcServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Shared.Api
{
    public class ExternalCheckPermissionAttribute : ActionFilterAttribute
    {
        const string IsCurrentlyAuthorized = "IsCurrentlyAuthorized";
        public string? PermissionName { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Result != null) return;   //Another filter has already returned a result so pass it on

            //Setting PermissionName
            var permissionName = PermissionName;
            if (string.IsNullOrEmpty(permissionName))
            {
                var currentlyAuthorizedItem = context.HttpContext.Items[IsCurrentlyAuthorized];
                if (currentlyAuthorizedItem is not null)
                    if ((bool)currentlyAuthorizedItem)
                        await next();

                var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor is null) return;

                var actionName = actionDescriptor.ActionName.ToLower();
                var controllerName = actionDescriptor.ControllerName.ToLower();

                permissionName = string.Join('-', new string[] { controllerName, actionName });
            }

            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration is not null)
            {
                var grpcAddress = configuration["Identity:GrpcAddress"];
                var grpcChannel = GrpcChannel.ForAddress(grpcAddress);


                string userId = "";

                var tokenGeneration = configuration["Identity:TokenGeneration"];
                if (tokenGeneration == "internal")
                {
                    var idClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
                    if (idClaim is not null)
                        userId = idClaim.Value;
                    else
                    {
                        context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                        return;
                    }

                }
                else
                {
                    var token = context.HttpContext.Request.Headers["Authorization"].ToString();
                    if (token.StartsWith("Bearer"))
                        token = token.Replace("Bearer", "").Trim();

                    var requestUser = new ValidateTokenRequest { Token = token };
                    var clientUser = new Authenticator.AuthenticatorClient(grpcChannel);
                    var responseUser = await clientUser.ValidateTokenAsync(requestUser);
                    if (responseUser is null || (responseUser is not null && !responseUser.Isvalid))
                    {
                        context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                        return;
                    }

                    userId = responseUser!.Userid;
                }



                var client = new Permission.PermissionClient(grpcChannel);
                var request = new CheckPermissionRequest { Userid = userId, Permissionname = permissionName };

                var response = await client.CheckPermissionAsync(request);
                if (response is not null && response.Isallowed)
                {
                    context.HttpContext.Items[IsCurrentlyAuthorized] = true;
                    await next();
                }
            }

            //context.Result = new ForbidResult();
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }
    }
}

using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Api
{
    public class CheckPermissionAttribute : ActionFilterAttribute
    {
        const string IsCurrentlyAuthorized = "IsCurrentlyAuthorized";
        public string? PermissionName { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //return base.OnActionExecutionAsync(context, next);

            if (context.Result != null) return;   //Another filter has already returned a result so pass it on

            var permissionName = PermissionName;//Has value when explicitly defined PermissionName 
            if (string.IsNullOrEmpty(permissionName))//Using default CheckPermissionController or not explicitly defined PermissionName
            {
                var currentlyAuthorizedItem = context.HttpContext.Items[IsCurrentlyAuthorized];
                if (currentlyAuthorizedItem is not null)
                    if ((bool)currentlyAuthorizedItem)
                        await next();

                //var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
                //var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

                var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor is null) return;

                var actionName = actionDescriptor.ActionName.ToLower();
                var controllerName = actionDescriptor.ControllerName.ToLower();

                permissionName = string.Join('-', new string[] { controllerName, actionName });
            }

            var idClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
            if (idClaim is not null)
            {
                var userId = Guid.Parse(idClaim.Value);

                var permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>();
                if (permissionService is not null)
                {
                    var userHasPermission = await permissionService.UserHasPermission(userId, permissionName);
                    if (userHasPermission)
                    {
                        context.HttpContext.Items[IsCurrentlyAuthorized] = true;
                        await next();
                    }
                }
            }

            context.Result = new ForbidResult();
        }
    }
}

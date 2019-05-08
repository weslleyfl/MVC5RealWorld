using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MVC5RealWorld.Security
{

    public class PermiteAcessoUsuarioRequirement : IAuthorizationRequirement
    {
        public string[] UserAssignedRoles { get; }

        public PermiteAcessoUsuarioRequirement(params string[] requiredPermission)
        {
            UserAssignedRoles = requiredPermission;
        }
    }

    public class PermiteAcessoUsuarioRequirementHandler : AuthorizationHandler<PermiteAcessoUsuarioRequirement>
    {
        //private bool _authorize = false;
        private AuthorizationHandlerContext _context;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermiteAcessoUsuarioRequirement requirement)
        {
            _context = context;

            var acessoPermitido = TemRequisito(requirement);
                        
            if (acessoPermitido)
                context.Succeed(requirement);
            //else
            //    context.Fail();
            
            return Task.CompletedTask;

        }

        private bool TemRequisito(PermiteAcessoUsuarioRequirement requirement)
        {
            bool authorize = false;

            foreach (var role in requirement.UserAssignedRoles)
            {
                authorize = UsuarioEstaNaFuncao(role);
                if (authorize)
                    return authorize;
            }

            return authorize;
        }

        private bool UsuarioEstaNaFuncao(string roleName)
        {
            UserManager userManager = new UserManager();
            return userManager.IsUserInRole(_context.User.Identity.Name, roleName);

        }
    }


    public class CustomPrincipal : System.Security.Principal.IPrincipal
    {
        public CustomPrincipal(CustomIdentity identity)
        {
            this.Identity = identity;
        }
        public IIdentity Identity { get; private set; }

        IIdentity IPrincipal.Identity => throw new NotImplementedException();

        public bool IsInRole(string role)
        {
            return true;
        }
    }

    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name)
        {
            this.Name = name;
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }
        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }
        public string Name { get; private set; }
    }


    #region " Codigo anteriro ao .core "

    /*
 public class CustomAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext authorizationFilterContext)
    {
        var policyProvider = authorizationFilterContext.HttpContext
            .RequestServices.GetService<IAuthorizationPolicyProvider>();
        var policy = await policyProvider.GetPolicyAsync(UserPolicy.Read);
        var requirement = (ClaimsAuthorizationRequirement)policy.Requirements
            .First(r => r.GetType() == typeof(ClaimsAuthorizationRequirement));

        if (authorizationFilterContext.HttpContext.User.Identity.IsAuthenticated)
        {
            if (!authorizationFilterContext.HttpContext
              .User.HasClaim(x => x.Value == requirement.ClaimType))
            {
                authorizationFilterContext.Result =
                   new ObjectResult(new ApiResponse(HttpStatusCode.Unauthorized));
            }
        }
    }
}
 */

    public class AuthorizeRolesAttribute : AuthorizeAttribute //IAuthorizationHandler //AuthorizeAttribute
    {
        private readonly string[] userAssignedRoles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            userAssignedRoles = roles;
        }

        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    bool authorize = false;
        //    using (DemoDBContext db = new DemoDBContext())
        //    {
        //        UserManager UM = new UserManager();

        //        foreach (var roles in userAssignedRoles)
        //        {
        //            authorize = UM.IsUserInRole(httpContext.User.Identity.Name, roles);
        //            if (authorize)
        //                return authorize;
        //        }
        //    }

        //    return authorize;
        //}

        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new RedirectResult("~/Home/UnAuthorized");
        //}

    }

    #endregion
}

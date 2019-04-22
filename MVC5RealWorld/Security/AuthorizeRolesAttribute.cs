using Microsoft.AspNetCore.Authorization;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
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
    

    #region " Codigo anteriro ao .core "

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

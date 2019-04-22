using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util
{
    public static class PlatformServicesExtensions
    {
        public static string MapPath(this IHostingEnvironment services, string path)
        {
            var result = path ?? string.Empty;
            if (services.IsPathMapped(path) == false)
            {
                var wwwroot = services.WwwRoot();
                if (result.StartsWith("~", StringComparison.Ordinal))
                {
                    result = result.Substring(1);
                }
                if (result.StartsWith("/", StringComparison.Ordinal))
                {
                    result = result.Substring(1);
                }
                result = Path.Combine(wwwroot, result.Replace('/', '\\'));
            }
            
            return result;
        }

        public static string UnmapPath(this IHostingEnvironment services, string path)
        {
            var result = path ?? string.Empty;
            if (services.IsPathMapped(path))
            {
                var wwwroot = services.WwwRoot();
                result = result.Remove(0, wwwroot.Length);
                result = result.Replace('\\', '/');

                var prefix = (result.StartsWith("/", StringComparison.Ordinal) ? "~" : "~/");
                result = prefix + result;
            }

            return result;
        }

        public static bool IsPathMapped(this IHostingEnvironment services, string path)
        {
            var result = path ?? string.Empty;
            return result.StartsWith(services.WebRootPath,
                StringComparison.Ordinal);
        }

        public static string WwwRoot(this IHostingEnvironment services)
        {
            // todo: take it from project.json!!!
            var result = Path.Combine(services.WebRootPath, "wwwroot");
            return result;
        }
    }
}

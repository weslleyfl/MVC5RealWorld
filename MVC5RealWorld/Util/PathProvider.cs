using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util
{
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;

        public PathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, path);
            return filePath;
        }
    }
}

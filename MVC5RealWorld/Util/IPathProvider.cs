using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util
{
    public interface IPathProvider
    {
        string MapPath(string path);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Models.Interface
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}

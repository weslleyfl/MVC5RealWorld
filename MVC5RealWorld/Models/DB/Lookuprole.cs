using System;
using System.Collections.Generic;

namespace MVC5RealWorld.Models.DB
{
    public partial class Lookuprole
    {
        public Lookuprole()
        {
            SysuserRole = new HashSet<SYSUserRole>();
        }

        public int LookuproleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int RowCreatedSysuserId { get; set; }
        public DateTime? RowCreatedDateTime { get; set; }
        public int RowModifiedSysuserId { get; set; }
        public DateTime? RowModifiedDateTime { get; set; }

        public virtual ICollection<SYSUserRole> SysuserRole { get; set; }
    }
}

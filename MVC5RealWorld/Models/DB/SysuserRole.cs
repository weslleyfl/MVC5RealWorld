using System;
using System.Collections.Generic;

namespace MVC5RealWorld.Models.DB
{
    public partial class SYSUserRole
    {
        public int SysuserRoleId { get; set; }
        public int SYSUserID { get; set; }
        public int LOOKUPRoleID { get; set; }
        public bool? IsActive { get; set; }
        public int RowCreatedSYSUserID { get; set; }
        public DateTime? RowCreatedDateTime { get; set; }
        public int RowModifiedSYSUserID { get; set; }
        public DateTime? RowModifiedDateTime { get; set; }

        public virtual Lookuprole Lookuprole { get; set; }
        public virtual SYSUser Sysuser { get; set; }
    }
}

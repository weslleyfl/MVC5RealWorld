using System;
using System.Collections.Generic;

namespace MVC5RealWorld.Models.DB
{
    public partial class SYSUserProfile
    {
        public int SysuserProfileId { get; set; }
        public int SYSUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int RowCreatedSYSUserID { get; set; }
        public DateTime? RowCreatedDateTime { get; set; }
        public int RowModifiedSYSUserID { get; set; }
        public DateTime? RowModifiedDateTime { get; set; }

        public virtual SYSUser Sysuser { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MVC5RealWorld.Models.DB
{
    public partial class SYSUser
    {
        public SYSUser()
        {
            SysuserProfile = new HashSet<SYSUserProfile>();
            SysuserRole = new HashSet<SYSUserRole>();
        }

        public int SYSUserID { get; set; }
        public string LoginName { get; set; }
        public string PasswordEncryptedText { get; set; }
        public int RowCreatedSYSUserID { get; set; }
        public DateTime? RowCreatedDateTime { get; set; }
        public int RowModifiedSYSUserID { get; set; }
        public DateTime? RowModifiedDateTime { get; set; }

        public virtual ICollection<SYSUserProfile> SysuserProfile { get; set; }
        public virtual ICollection<SYSUserRole> SysuserRole { get; set; }
    }
}

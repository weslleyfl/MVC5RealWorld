using System.Collections.Generic;

namespace MVC5RealWorld.Models.ViewModel
{
    public class UserRoles
    {
        public int? SelectedRoleID { get; set; }
        public IEnumerable<LOOKUPAvailableRole> UserRoleList { get; set; }
    }

}

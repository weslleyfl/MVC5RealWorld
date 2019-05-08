using System.ComponentModel.DataAnnotations;

namespace MVC5RealWorld.Models.ViewModel
{
    public class LOOKUPAvailableRole
    {
        [Key]
        public int LOOKUPRoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Roles
{
    public class Role(int id, string roleName)
    {
        [Key]
        public int RoleId { get; set; } = id;

        [Required]
        [StringLength(255)]
        public string RoleName { get; set; } = roleName;
    }
}

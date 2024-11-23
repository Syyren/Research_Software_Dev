﻿using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Roles
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(255)]
        public string RoleName { get; set; }
    }
}

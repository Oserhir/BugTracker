﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBugTracker.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get { return $"{FirstName} {LastName}"; } }

        // Avatar
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? AvatarFormFile { get; set; }

        [DisplayName( "Avatar")]
        public string? AvatarFileName { get; set; }

        public byte[]? AvatarFileData { get; set; }

        [Display(Name = "File Extension")]
        //public string? AvatarContentType { get; set; }
        public string? AvatarContentType { get; set; }

        // Foreign Key
        public int? CompanyId { get; set; }

        // ----  Navigation properties  ---- //
        public virtual Company Company { get; set; } 
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }
}

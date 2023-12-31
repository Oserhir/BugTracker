﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBugTracker.Models
{
    public class Project
    {
        // Primary Key
        public int Id { get; set; }
       
        [Required]
        [StringLength(50)]
        [DisplayName("Project Name")]
        public string? Name { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTimeOffset EndDate { get; set; }

        [DisplayName("Archived")]
        public bool Archived { get; set; }

        // Image
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFormFile { get; set; }

        [DisplayName("File Name")]
        public string? ImageFileName { get; set; }
        public byte[]? ImageFileData { get; set; }

        [DisplayName("File Extension")]
        public string? ImageContentType { get; set; }

        // Foreign Keys
        [DisplayName("Company")]
        public int? CompanyId { get; set; }

        [DisplayName("Priority")]
        public int? ProjectPriorityId { get; set; }

        // -- Navigation Prop -- //

        public virtual Company Company { get; set; }
        public virtual ProjectPriority ProjectPriority { get; set; }

        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();



    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBugTracker.Models
{
    public class TicketAttachment
    {
        // Primary Key
        public int Id { get; set; }

        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; }

        [DisplayName("File Description")]
        public string Description { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile FormFile { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }
        public Byte[] FileData { get; set; }

        [DisplayName("File Extension")]
        public string FileContentType { get; set; }

        // Foreign keys
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual BTUser User { get; set; }

    }
}

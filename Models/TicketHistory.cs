using System.ComponentModel;

namespace TheBugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Updated Item")]
        public int Property { get; set; }

        [DisplayName("Previous")]
        public int OldValue { get; set; }

        [DisplayName("Current")]
        public int NewValue { get; set; }

        [DisplayName("Date Modified")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Description of Change")]
        public int Description { get; set; }

        [DisplayName("Team Member")]
        public int UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual BTUser user { get; set; }

    }
}

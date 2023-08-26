﻿using System.ComponentModel;

namespace TheBugTracker.Models
{
    public class TicketComment
    {

        public int Id { get; set; }

        [DisplayName("Member Commend")]
        public string Commend { get; set; }

        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; }

        // public virtual Ticket Ticket { get; set; }
        public virtual BTUser user { get; set; }

    }
}

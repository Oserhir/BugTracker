using System.ComponentModel;

namespace TheBugTracker.Models
{
    public class Company
    {
        // Primary Key
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }

        [DisplayName("Company Description")]
        public string Description { get; set; }

        //----- 
        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Project> Project { get; set; } = new HashSet<Project>();
    }
}

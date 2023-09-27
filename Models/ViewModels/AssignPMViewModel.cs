using Microsoft.AspNetCore.Mvc.Rendering;

namespace TheBugTracker.Models.ViewModels
{
	public class AssignPMViewModel
	{
		public Project? Project { get; set; }
		public SelectList? PMList { get; set; }
		public string? PMId { get; set; }
	}
}

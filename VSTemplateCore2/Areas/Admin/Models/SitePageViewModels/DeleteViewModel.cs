using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VSTemplateCore2.Areas.Admin.Models.SitePageViewModels {
	public class DeleteViewModel {
		public int Id { get; set; }

		[Display(Name = "Active?")]
		public bool Active { get; set; }

		[Display(Name = "Show in Menu?")]
		public bool ShowInMenu { get; set; }

		[Display(Name = "Parent Menu")]
		public string ParentMenu { get; set; }

		[Display(Name = "Page Title")]
		public string Title { get; set; }

		[Display(Name = "Created By")]
		public string CreatedBy { get; set; }

		[Display(Name = "Created On")]
		[DataType(DataType.Date)]
		public DateTime CreatedOn { get; set; }

		[Display(Name = "Last Updated By")]
		public string LastUpdatedBy { get; set; }

		[Display(Name = "Last Updated On")]
		[DataType(DataType.Date)]
		public DateTime LastUpdatedOn { get; set; }
	}
}

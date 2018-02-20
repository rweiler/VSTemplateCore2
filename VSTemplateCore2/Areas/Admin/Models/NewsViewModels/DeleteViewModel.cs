using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTemplateCore2.Areas.Admin.Models.NewsViewModels {
	public class DeleteViewModel {
		public int Id { get; set; }
		public bool Active { get; set; }
		public bool Featured { get; set; }
		public bool Urgent { get; set; }
		public string Title { get; set; }
		public string LastUpdatedBy { get; set; }
		public DateTime LastUpdatedOn { get; set; }
	}
}

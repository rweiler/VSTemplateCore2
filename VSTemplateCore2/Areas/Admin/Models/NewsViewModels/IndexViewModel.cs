using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VSTemplateCore2.Areas.Admin.Models.NewsViewModels {
	public class IndexViewModel {
		public int Id { get; set; }

		[Display(Name = "Date")]
		[DisplayFormat(DataFormatString = "{0:MMM d, yyyy}")]
		public DateTime ArticleDate { get; set; }

		[Display(Name = "Title")]
		public string Title { get; set; }

		[Display(Name = "Friendly URL")]
		public string Alias { get; set; }

		[Display(Name = "Active?")]
		public bool Active { get; set; }

		[Display(Name = "Show in Featured?")]
		public bool Featured { get; set; }

		[Display(Name = "Show as Urgent?")]
		public bool Urgent { get; set; }
	}
}

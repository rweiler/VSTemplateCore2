using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VSTemplateCore2.Areas.Admin.Models.NewsViewModels {
	public class CreateViewModel {

		[Display(Name = "Active?")]
		public bool Active { get; set; } = true;

		[Display(Name = "Display in Featured?")]
		public bool Featured { get; set; }

		[Display(Name = "Display as Urgent?")]
		public bool Urgent { get; set; }

		[Required(ErrorMessage = "What is the Title of this News Article?")]
		[Display(Name = "News Article Title")]
		[StringLength(65, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "What is the date to associate with the news article?")]
		[DisplayFormat(DataFormatString = "{0:MMM d, yyyy}")]
		[Display(Name = "News Article Date")]
		public DateTime ArticleDate { get; set; } = DateTime.Now;

		[Required(ErrorMessage = "What is the date to display the news article?")]
		[DisplayFormat(DataFormatString = "{0:MMM d, yyyy h:mm tt}")]
		[Display(Name = "Show News Article On")]
		public DateTime DisplayOnDate { get; set; } = DateTime.Now;

		[Display(Name = "News Article Excerpt")]
		[StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
		public string Excerpt { get; set; }

		[Display(Name = "News Article Content")]
		public string Content { get; set; }

		[Display(Name = "Image for News Article")]
		[StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
		public string ImagePath { get; set; }

		[Display(Name = "Video Embed Code for News Article")]
		[StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
		public string VideoEmbedCode { get; set; }
	}
}

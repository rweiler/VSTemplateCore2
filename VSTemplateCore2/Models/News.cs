using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VSTemplateCore2.Models {
	public class News {

		public int Id { get; set; }

		[Required(ErrorMessage = "What is the Title to be used for this news article?")]
		[StringLength(65)]
		[Display(Name = "Title of News Article")]
		public string Title { get; set; }							// Always required.

		[Required]
		[StringLength(65)]
		[Display(Name = "News Article Alias")]
		public string Alias { get; set; }							// Always required. Must be unique. Used for an SEO friendly URL.

		[Required(ErrorMessage = "What is the date to be displayed with this news article?")]
		[Display(Name = "Date of News Article")]
		[DisplayFormat(DataFormatString = "{0:MMM d, yyyy}")]
		public DateTime ArticleDate { get; set; }			// Always required. Date to display in the details of the news article or in the Featured News module.

		[Required]
		[Display(Name = "Date/Time to Display News Article")]
		[DisplayFormat(DataFormatString = "{0:MMM d, yyyy h:mm tt}")]
		public DateTime DisplayOnDate { get; set; }   // Always required. Don't show the news article in the front-end (Featured News/News module)  before this date/time.

		[Display(Name = "Active?")]
		public bool Active { get; set; } = true;      // Should the News article be displayed in the list of News articles on the front-end?

		[Display(Name = "Show in Featured News?")]
		public bool Featured { get; set; } = false;   // Should the News article be displayed in the Featured News module on the front-end?

		[Display(Name = "Show as Urgent?")]
		public bool Urgent { get; set; } = false;			// Should a modal be displayed with this news article when a user first visits the site?

		[StringLength(255)]
		public string Excerpt { get; set; }

		[Display(Name = "News Article Content")]
		public string Content { get; set; }

		[Display(Name = "Image for News Article")]
		[StringLength(255)]
		public string ImagePath { get; set; }

		[Display(Name = "Video Embed Code")]
		[StringLength(255)]
		public string VideoEmbedCode { get; set; }

		[BindNever]
		[StringLength(36)]
		public string LastUpdatedBy { get; set; }

		[BindNever]
		public DateTime LastUpdatedOn { get; set; }
	}
}

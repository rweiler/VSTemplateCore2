using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VSTemplateCore2.Areas.Admin.Models {
	public class SitePage {

		public int Id { get; set; }

		[Display(Name = "Parent Menu")]
		public int? ParentMenuId { get; set; }

		public int PageOrder { get; set; }

		[BindNever]
		[StringLength(15)]
		public string MenuOrder { get; set; }							// Used to determine the order of menus, especially for menu sections.

		[Required(ErrorMessage = "What is the Title of the page?")]
		[StringLength(55)]
		[Display(Name = "Page Title")]
		public string Title { get; set; }									// Used for the browser's tab/title bar. Always required. Must be unique.

		[StringLength(65)]
		[Display(Name = "Page Name")]
		public string Name { get; set; }								// Used for the page heading in the content area of the page. Required except if entity represents a Menu Section.

		[StringLength(30)]
		[Display(Name = "Menu Title")]
		public string MenuTitle { get; set; }						// Used in the navigation menu. Required except when entiry is not shown in menu.

		[StringLength(65)]
		[Display(Name = "Page Alias")]
		public string Alias { get; set; }								// Used for the SEO friendly URL. Must be unique.

		[StringLength(155)]
		[Display(Name = "Meta Description")]
		public string MetaDescription { get; set; }			// Used for the SERP snippet

		[Display(Name = "Page Content")]
		public string Content { get; set; }

		[StringLength(50)]
		[Display(Name = "Handler: Controller")]
		public string HandlerController { get; set; }

		[StringLength(50)]
		[Display(Name = "Handler: Action")]
		public string HandlerAction { get; set; }

		[Display(Name = "Page Banner")]
		public int? PageBannerId { get; set; }         // Single image to be used as a page banner, mostly on inner pages, with the option to display a photo credit, a message on the image and a button

		[Display(Name = "Active?")]
		public bool Active { get; set; } = true;

		[Display(Name = "Show in Main Menu?")]
		public bool ShowInMenu { get; set; } = true;

		[Display(Name = "Allow page to be Edited?")]
		public bool AllowEdit { get; set; } = true;

		[Display(Name = "Allow Delete?")]
		public bool AllowDelete { get; set; } = true;

		[BindNever]
		[StringLength(36)]
		public string CreatedBy { get; set; }

		[BindNever]
		public DateTime CreatedOn { get; set; }

		[BindNever]
		[StringLength(36)]
		public string LastUpdatedBy { get; set; }

		[BindNever]
		public DateTime LastUpdatedOn { get; set; }

		// Navigation Properties
		public BannerImage PageBanner { get; set; }
	}
}

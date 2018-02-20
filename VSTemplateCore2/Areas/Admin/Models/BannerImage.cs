using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VSTemplateCore2.Areas.Admin.Models {
	
	// Defines a slide to be used within a Bootstrap carousel, or a static standalone page banner
	public class BannerImage {
		public int Id { get; set; }

		[Required(ErrorMessage = "Specify the image by clicking on the browse button")]
		[StringLength(255)]
		[Display(Name = "Path to Image")]
		public string ImagePath { get; set; }

		[StringLength(100)]
		[Display(Name = "Photo Credit")]
		public string PhotoCredit { get; set; }

		[StringLength(255)]
		public string Message { get; set; }

		[StringLength(25)]
		[Display(Name = "Button Text")]
		public string ButtonText { get; set; }

		[StringLength(255)]
		[Display(Name = "Button URL")]
		public string ButtonLink { get; set; }
	}
}

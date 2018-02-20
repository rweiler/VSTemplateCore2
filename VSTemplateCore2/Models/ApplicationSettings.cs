using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTemplateCore2.Models {
	public class ApplicationSettings {
		public NewsModule NewsModule { get; set; }
	}

	public class NewsModule {
		public bool FeaturedSupported { get; set; }
		public bool UrgentSupported { get; set; }
		public bool ImageSuported { get; set; }
		public bool VideoSupported { get; set; }
	}
}

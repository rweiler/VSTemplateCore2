using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSTemplateCore2.Models {
	public class ModelConfiguration {
		// This is the name of the root configuration section in appsettings.json
		public DefaultModelConfiguration DefaultModelConfiguration { get; set; }
	}


	// Defines the sections within DefaultModelConfiguration in appsettings.json
	public class DefaultModelConfiguration {
		public DefaultPages[] DefaultPages { get; set; }
	}


	public class DefaultPages {
		public string PageName { get; set; }
		public string PageTitle { get; set; }
		public string MenuTitle { get; set; }
		public string PageAlias { get; set; }
		public int PageOrder { get; set; }
		public string MenuOrder { get; set; }
		public bool ShowInMenu { get; set; } = true;
		public bool AllowDelete { get; set; } = false;
	}
}

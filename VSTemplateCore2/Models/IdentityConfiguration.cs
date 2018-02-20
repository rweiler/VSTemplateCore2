
namespace VSTemplateCore2.Models {
	public class IdentityConfiguration {
		// This is the name of the root configuration section in appsettings.json
		public DefaultIdentityConfiguration DefaultIdentityConfiguration { get; set; }
	}

	// Defines the sections within DefaultIdentityConfiguration in appsettings.json
	public class DefaultIdentityConfiguration {
		public DefaultUsers[] DefaultUsers { get; set; }
	}


	public class DefaultUsers {
		public string Name { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
	}
}

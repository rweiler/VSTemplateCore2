using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VSTemplateCore2.Areas.Admin.Data;
using VSTemplateCore2.Areas.Admin.Models;
using VSTemplateCore2.Models;

namespace VSTemplateCore2.Data {
	public class SeedData {

		public static async Task Initialize(IServiceProvider serviceProvider) {
			using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())) {
				// Make sure the database has been created
				context.Database.EnsureCreated();

				// Create the default application users as defined in appsettings.json in the DefaultIdentityConfiguration section, if required
				await SeedIdentity(serviceProvider);

				// Create the default application models as defined in appsettings.json in the DefaultModelConfiguration section, if required
				await SeedModel(serviceProvider);
			}
		}


		public static async Task SeedIdentity(IServiceProvider serviceProvider) {
			// Get the Identity Configuration from appsettings in preparation for seeding the default ApplicationUsers as defined in appsettings.json
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.AddUserSecrets<Startup>()
				.Build();

			// The DefaultAdminPassword is set from Manage User Secrets found in the project's context menu
			//   "DefaultAdminPassword": <pw>   -- Note: the password must meet whatever password requirements have been configured in the Startup class.
			var defaultPassword = configuration["DefaultAdminPassword"];

			// Retrieve the DefaultIdentityConfiguration section in appsettings.json into an enumerable object
			// Note: Role must be set to one of the role Constants in Authorization.Operations
			var IdentityConfiguration = new IdentityConfiguration();
			configuration.Bind(IdentityConfiguration);

			// Iterate through the users to be created and ensure that each has been created and their respective role assigned (if the role doesn't exist, create it)
			foreach (var user in IdentityConfiguration.DefaultIdentityConfiguration.DefaultUsers) {
				var userId = await EnsureUserCreated(serviceProvider, user, defaultPassword);
			}
		}

		public static async Task<string> EnsureUserCreated(IServiceProvider serviceProvider, DefaultUsers userToCreate, string defaultPassword) {
			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			var user = await userManager.FindByNameAsync(userToCreate.Name);
			if (user == null) {
				user = new ApplicationUser { UserName = userToCreate.Name, Email = userToCreate.Email };
				var newUser = await userManager.CreateAsync(user, defaultPassword);
				if (newUser.Succeeded) {
					// Add the user to the specified role
					var userAddedToRole = await AddUserToRole(serviceProvider, user.Id, userToCreate.Role);
					if (!userAddedToRole.Succeeded) {
						throw new Exception(userAddedToRole.Errors.ToString());
					}
				} else {
					throw new Exception(newUser.Errors.ToString());
				}
			}
			return user.Id;
		}

		public static async Task<IdentityResult> AddUserToRole(IServiceProvider serviceProvider, string userId, string role) {
			IdentityResult result = null;

			// If the specified role doesn't already exist, create it
			var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
			if (!await roleManager.RoleExistsAsync(role)) {
				result = await roleManager.CreateAsync(new IdentityRole(role));
				if (result.Succeeded) {
					// Assign the role to the user if the user doesn't already belong to the role
					var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
					var user = await userManager.FindByIdAsync(userId);
					if (!await userManager.IsInRoleAsync(user, role)) {
						result = await (userManager.AddToRoleAsync(user, role));
					}
				}
			}

			return result;
		}


		public static async Task SeedModel(IServiceProvider serviceProvider) {
			var repository = serviceProvider.GetService<ISitePageRepository>();
			// Get the Model Configuration from appsettings in preparation for seeding the default model entities as defined in appsettings.json
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			// Retrieve the DefaultModelConfiguration section in appsettings.json into an enumberable oject
			var ModelConfiguration = new ModelConfiguration();
			configuration.Bind(ModelConfiguration);

			// Get the User.Id for the vsgroup Constants.MasterAdminRole user
			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			var masterAdminUser = await userManager.FindByNameAsync("vsgroup");

			// Iterate through the default SitePages as defined in DefaultModelConfiguration:DefaultPages in appsettings.json and ensure that each page has been created
			foreach (var pageToCreate in ModelConfiguration.DefaultModelConfiguration.DefaultPages) {
				var page = await EnsurePageCreated(repository, pageToCreate, masterAdminUser.Id);
				if (page == null) {
					// There was an error while creating the page so throw an error
					throw new Exception($"There was an error while creating page {pageToCreate.PageTitle}");
				}
			}
		}

		public static async Task<SitePage> EnsurePageCreated(ISitePageRepository repository, DefaultPages pageToCreate, string masterAdminUserId) {
			// Check to see if the page to created already exists
			//var page = context.SitePages.FirstOrDefault(m => m.Name == pageToCreate.PageName);
			var page = await repository.FindByTitleAsync(pageToCreate.PageTitle);
			if (page == null) {
				// The page doesn't exist so create it.
				// Capture the DateTime the page is being created
				var pageCreatedOn = DateTime.Now;
				page = new SitePage {
					Title = pageToCreate.PageTitle,
					Name = pageToCreate.PageName,
					MenuTitle = pageToCreate.MenuTitle,
					Alias = pageToCreate.PageAlias,
					PageOrder = pageToCreate.PageOrder,
					MenuOrder = pageToCreate.MenuOrder,
					ShowInMenu = pageToCreate.ShowInMenu,
					AllowDelete = false,
					CreatedBy = masterAdminUserId,
					CreatedOn = pageCreatedOn,
					LastUpdatedBy = masterAdminUserId,
					LastUpdatedOn = pageCreatedOn
				};
				var createdPage = await repository.SaveAsync(page);
				return createdPage;
			} else {
				return page;
			}
		}
	}
}

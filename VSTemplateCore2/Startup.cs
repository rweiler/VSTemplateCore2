using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VSTemplateCore2.Areas.Admin.Data;
using VSTemplateCore2.Authorization;
using VSTemplateCore2.Data;
using VSTemplateCore2.Models;
using VSTemplateCore2.Services;

namespace VSTemplateCore2 {
	public class Startup {

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options => {
				// Password settings
				options.Password.RequiredLength = 8;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				// Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;
				//User settings
				options.User.RequireUniqueEmail = true;
			});

			services.ConfigureApplicationCookie(options => {
				// Cookie settings
				options.Cookie.HttpOnly = true;
				options.Cookie.Expiration = TimeSpan.FromDays(150);
				options.SlidingExpiration = true;
			});

			// Configure External Login Authorities (configuration credentials stored in user secrets)
			services.AddAuthentication()
				.AddFacebook(facebookOptions => {
					facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
					facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
				})
				.AddGoogle(googleOptions => {
					googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
					googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
				})
				.AddMicrosoftAccount(microsoftOptions => {
					microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ApplicationId"];
					microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:Password"];
				});

			// Add application services.
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<ISitePageRepository, EFSitePageRepository>();
			services.AddTransient<INewsRepository, EFNewsRepository>();

			// Services: Authorization Handlers
			services.AddSingleton<IAuthorizationHandler, MasterAdminAuthorizationHandler>();
			services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();

			// Require SSL for this web application
			services.Configure<MvcOptions>(options => {
				options.Filters.Add(new RequireHttpsAttribute());
			});

			// Get the ApplicationSettings from appsettings.json
			services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
				app.UseDatabaseErrorPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseAuthentication();

			var options = new RewriteOptions()
				.AddRedirectToHttps();
			app.UseRewriter(options);

			app.UseMvc(routes => {
				routes.MapRoute(
					name: "content",
					template: "en/{menuSection:maxlength(65)?}/{pageAlias:maxlength(65)}",
					defaults: new { controller = "Home", action = "RenderPageContent" });

				routes.MapRoute(
					name: "news",
					template: "en/news/{newsAlias:maxlength(65)}",
					defaults: new { controller = "Home", action = "RenderNewsArticle" });

				routes.MapRoute(
					name: "areas",
					template: "{area:exists}/{controller=Home}/{Action=Index}/{id?}");

				routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}

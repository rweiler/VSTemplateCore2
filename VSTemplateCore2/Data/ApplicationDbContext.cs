using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VSTemplateCore2.Areas.Admin.Models;
using VSTemplateCore2.Models;

namespace VSTemplateCore2.Data {
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options) {
		}

		public DbSet<SitePage> SitePages { get; set; }
		public DbSet<News> News { get; set; }

		protected override void OnModelCreating(ModelBuilder builder) {
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more. Add your customizations after calling base.OnModelCreating(builder);
			builder.Entity<SitePage>().ToTable("SitePage");
			builder.Entity<News>().ToTable("News");
		}
	}
}

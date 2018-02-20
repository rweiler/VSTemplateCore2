using System.Collections.Generic;
using System.Threading.Tasks;
using VSTemplateCore2.Areas.Admin.Infrastructure;
using VSTemplateCore2.Areas.Admin.Models;

namespace VSTemplateCore2.Areas.Admin.Data {
	public interface ISitePageRepository {
		IEnumerable<SitePage> SitePages { get; }

		Task<SitePage> FindByIdAsync(int id);
		Task<SitePage> FindByTitleAsync(string pageTitle);
		Task<SitePage> FindByAliasAsync(string pageAlias);

		Task<int> FindMaxPageOrderAsync();

		Task<int> CountChildPagesInSectionAsync(int? parentMenuId);

		Task<SitePage> SaveAsync(SitePage sitePage);
		Task<SitePage> DeleteAsync(SitePage sitePage);
		Task<SitePage> DeleteAsync(int id);

		Task ReorderSitePageAsync(int pageIdToReorder, ReorderDirections direction);
	}
}

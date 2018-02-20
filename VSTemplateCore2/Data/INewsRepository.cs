using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSTemplateCore2.Models;

namespace VSTemplateCore2.Data {
	public interface INewsRepository {
		IEnumerable<News> News { get; }

		Task<News> FindByIdAsync(int id);
		Task<News> FindByTitleAsync(string title);
		Task<News> FindByAliasAsync(string alias);

		Task<News> SaveAsync(News newsArticle);
		Task<News> DeleteAsync(News newsArticle);
		Task<News> DeleteAsync(int id);

		void ResetUrgent();
	}
}

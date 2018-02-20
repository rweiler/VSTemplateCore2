using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSTemplateCore2.Models;

namespace VSTemplateCore2.Data {
	public class EFNewsRepository : INewsRepository {

		private readonly ApplicationDbContext _db;

		public EFNewsRepository(ApplicationDbContext context) {
			_db = context;
		}


		public IEnumerable<News> News =>
			_db.News;


		public async Task<News> FindByIdAsync(int id) =>
			await _db.News.SingleOrDefaultAsync(m => m.Id == id);

		public async Task<News> FindByTitleAsync(string title) =>
			await _db.News.SingleOrDefaultAsync(m => m.Title == title);

		public async Task<News> FindByAliasAsync(string alias) =>
			await _db.News.SingleOrDefaultAsync(m => m.Alias == alias);


		public async Task<News> SaveAsync(News newsArticle) {
			// Create a holding object to return the created/updated News entity in
			News dbEntity;

			if (newsArticle.Id == 0) {
				// This is a new News article. Add it to the database
				dbEntity = (await _db.News.AddAsync(newsArticle)).Entity;
			} else {
				// This is an existing News article. Update the instance in the database
				dbEntity = await _db.News.SingleOrDefaultAsync(m => m.Id == newsArticle.Id);
				if (dbEntity != null) {
					dbEntity.Title = newsArticle.Title;
					dbEntity.Alias = newsArticle.Alias;
					dbEntity.ArticleDate = newsArticle.ArticleDate;
					dbEntity.DisplayOnDate = newsArticle.DisplayOnDate;
					dbEntity.Active = newsArticle.Active;
					dbEntity.Featured = newsArticle.Featured;
					dbEntity.Urgent = newsArticle.Urgent;
					dbEntity.Excerpt = newsArticle.Excerpt;
					dbEntity.Content = newsArticle.Content;
					dbEntity.ImagePath = newsArticle.ImagePath;
					dbEntity.VideoEmbedCode = newsArticle.VideoEmbedCode;
					dbEntity.LastUpdatedBy = newsArticle.LastUpdatedBy;
					dbEntity.LastUpdatedOn = newsArticle.LastUpdatedOn;
				}
			}
			await _db.SaveChangesAsync();
			return dbEntity;
		}

		public async Task<News> DeleteAsync(News newsArticle) {
			var dbEntity = await _db.News.SingleOrDefaultAsync(m => m.Id == newsArticle.Id);
			if (dbEntity != null) {
				_db.News.Remove(dbEntity);
				await _db.SaveChangesAsync();
			}
			return dbEntity;
		}

		public async Task<News> DeleteAsync(int id) {
			var dbEntity = await _db.News.SingleOrDefaultAsync(m => m.Id == id);
			if (dbEntity != null) {
				_db.News.Remove(dbEntity);
				await _db.SaveChangesAsync();
			}
			return dbEntity;
		}

		public void ResetUrgent() {
		}
	}
}

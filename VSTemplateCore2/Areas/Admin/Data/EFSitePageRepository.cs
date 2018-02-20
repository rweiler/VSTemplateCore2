using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSTemplateCore2.Areas.Admin.Infrastructure;
using VSTemplateCore2.Areas.Admin.Models;
using VSTemplateCore2.Data;

namespace VSTemplateCore2.Areas.Admin.Data {
	public class EFSitePageRepository : ISitePageRepository {

		private readonly ApplicationDbContext _db;

		public EFSitePageRepository(ApplicationDbContext context) {
			_db = context;
		}


		public IEnumerable<SitePage> SitePages =>
			_db.SitePages;


		public async Task<SitePage> FindByIdAsync(int id) =>
			await _db.SitePages.SingleOrDefaultAsync(p => p.Id == id);

		public async Task<SitePage> FindByTitleAsync(string pageTitle) =>
			await _db.SitePages.SingleOrDefaultAsync(p => p.Title == pageTitle);

		public async Task<SitePage> FindByAliasAsync(string pageAlias) =>
			await _db.SitePages.SingleOrDefaultAsync(p => p.Alias == pageAlias);


		public async Task<int> FindMaxPageOrderAsync() =>
			await _db.SitePages.MaxAsync(m => m.PageOrder);


		public async Task<int> CountChildPagesInSectionAsync(int? parentMenuId) =>
			await _db.SitePages.CountAsync(m => m.ParentMenuId == parentMenuId);


		public async Task<SitePage> SaveAsync(SitePage sitePage) {
			// Create a holding object to return the created/updated SitePage in
			SitePage dbEntity;

			if (sitePage.Id == 0) {
				// This is a new SitePage. Add it to the database
				dbEntity = (await _db.SitePages.AddAsync(sitePage)).Entity;
			} else {
				// This is an existing SitePage. Update the instance in the database
				dbEntity = await _db.SitePages.SingleOrDefaultAsync(p => p.Id == sitePage.Id);
				if (dbEntity != null) {
					dbEntity.ParentMenuId = sitePage.ParentMenuId;
					dbEntity.Title = sitePage.Title;
					dbEntity.Name = sitePage.Name;
					dbEntity.MenuTitle = sitePage.MenuTitle;
					dbEntity.Alias = sitePage.Alias;
					dbEntity.MetaDescription = sitePage.MetaDescription;
					dbEntity.Content = sitePage.Content;
					dbEntity.MenuOrder = sitePage.MenuOrder;
					dbEntity.HandlerController = sitePage.HandlerController;
					dbEntity.HandlerAction = sitePage.HandlerAction;
					dbEntity.PageBannerId = sitePage.PageBannerId;
					dbEntity.Active = sitePage.Active;
					dbEntity.ShowInMenu = sitePage.ShowInMenu;
					dbEntity.AllowEdit = sitePage.AllowEdit;
					dbEntity.AllowDelete = sitePage.AllowDelete;
					dbEntity.LastUpdatedBy = sitePage.LastUpdatedBy;
					dbEntity.LastUpdatedOn = sitePage.LastUpdatedOn;
				}
			}
			await _db.SaveChangesAsync();
			return dbEntity;
		}

		public async Task<SitePage> DeleteAsync(SitePage sitePage) {
			var dbEntity = await _db.SitePages.SingleOrDefaultAsync(p => p.Id == sitePage.Id);
			if (dbEntity != null) {
				_db.SitePages.Remove(dbEntity);
				await _db.SaveChangesAsync();
			}
			return dbEntity;
		}

		public async Task<SitePage> DeleteAsync(int id) {
			var dbEntity = await _db.SitePages.SingleOrDefaultAsync(p => p.Id == id);
			if (dbEntity != null) {
				_db.SitePages.Remove(dbEntity);
				await _db.SaveChangesAsync();
			}
			return dbEntity;
		}


		public async Task ReorderSitePageAsync(int pageIdToReorder, ReorderDirections direction) {
			// Retrieve the page to reorder
			var pageToReOrder = await _db.SitePages.SingleOrDefaultAsync(p => p.Id == pageIdToReorder);
			if (pageToReOrder == null) {
				throw new ArgumentOutOfRangeException("pageIdToReorder", "The SitePage with the Id of pageIdToReorder does not exist in the database.");
			}
			var parentMenuId = pageToReOrder.ParentMenuId;
			var pageOrderToReorder = pageToReOrder.PageOrder;

			SitePage pageToReorderWith;
			// Retrieve the page to be reordered with based on the reorder direction
			switch (direction) {
				case ReorderDirections.Up:
					pageToReorderWith = await _db.SitePages.SingleOrDefaultAsync(sp => sp.PageOrder ==
						(_db.SitePages.Where(p => p.ParentMenuId == parentMenuId).Where(p => p.PageOrder < pageOrderToReorder).Max(p => p.PageOrder)));
					break;

				case ReorderDirections.Down:
					pageToReorderWith = await _db.SitePages.SingleOrDefaultAsync(sp => sp.PageOrder ==
					(_db.SitePages.Where(p => p.ParentMenuId == parentMenuId).Where(p => p.PageOrder > pageOrderToReorder).Min(p => p.PageOrder)));
					break;

				default:
					throw new ArgumentOutOfRangeException("direction", "The supplied value for direction is not supported.");
			}
			var pageOrderToReorderWith = pageToReorderWith.PageOrder;

			// Update the values for PageOrder and MenuOrder for the two pages
			pageToReOrder.PageOrder = pageOrderToReorderWith;
			pageToReOrder.MenuOrder = $"{parentMenuId ?? 0:00000}.{pageOrderToReorderWith:00000}";

			pageToReorderWith.PageOrder = pageOrderToReorder;
			pageToReorderWith.MenuOrder = $"{parentMenuId ?? 0:00000}.{pageOrderToReorder:00000}";

			await _db.SaveChangesAsync();
		}
	}
}

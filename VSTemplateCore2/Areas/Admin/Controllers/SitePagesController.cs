using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VSTemplateCore2.Areas.Admin.Data;
using VSTemplateCore2.Areas.Admin.Infrastructure;
using VSTemplateCore2.Areas.Admin.Models;
using VSTemplateCore2.Areas.Admin.Models.SitePageViewModels;
using VSTemplateCore2.Authorization;
using VSTemplateCore2.Extensions;
using VSTemplateCore2.Models;

namespace VSTemplateCore2.Areas.Admin.Controllers {

	[Area("Admin")]
	[Authorize]
	public class SitePagesController : Controller {

		private readonly ISitePageRepository _repository;
		private readonly UserManager<ApplicationUser> _userManager;

		public SitePagesController(ISitePageRepository repository, UserManager<ApplicationUser> userManager) {
			_repository = repository;
			_userManager = userManager;
		}


		// GET: SitePages
		public IActionResult Index() =>
			View(_repository.SitePages.OrderBy(p => p.MenuOrder));
		


		// GET: SitePages/Create
		public IActionResult Create() {
			var ParentPageList = _repository.SitePages.Where(p => p.ParentMenuId == 0).ToList();
			// Only MasterAdmins can create root level pages or create a menu section
			if (User.IsInRole(Constants.MasterAdminRole)) {
				ParentPageList.Insert(0, new SitePage { Id = 0, Title = "This is a Menu Section" });
				ParentPageList.Insert(0, new SitePage { Id = -1, Title = "This is a Root Page" });
			}
			ViewBag.ParentPageList = new SelectList(ParentPageList, "Id", "Title");
			return View(new SitePage());
		}

		// POST: SitePages/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ParentMenuId,Title,Name,MenuTitle,Alias,MetaDescription,Content,Active,ShowInMenu,AllowEdit,AllowDelete,HandlerController,HandlerAction")] SitePage sitePage) {
			if (ModelState.IsValid) {
				// If the ParentMenuId is -1 (can't set the Id to null for the SelectList this comes from) then set to null
				if (sitePage.ParentMenuId == -1) {
					sitePage.ParentMenuId = null;
				}

				// Set the values for unbound field(s) before saving to repository: CreatedBy, CreatedOn, LastUpdatedBy, LastUpdatedOn, PageOrder
				var adminUser = await _userManager.FindByNameAsync(User.Identity.Name);
				var datePageCreatedOn = DateTime.Now;
				var pageOrder = await _repository.FindMaxPageOrderAsync();
				sitePage.CreatedBy = adminUser.Id;
				sitePage.CreatedOn = datePageCreatedOn;
				sitePage.LastUpdatedBy = adminUser.Id;
				sitePage.LastUpdatedOn = datePageCreatedOn;

				sitePage.PageOrder = ++pageOrder;

				// If the PageAlias field is blank, calculate the page alias for the page
				if (sitePage.Alias == null || string.IsNullOrEmpty(sitePage.Alias)) {
					sitePage.Alias = sitePage.Title.CreateAlias(65);
				}

				// Calcuate the MenuOrder for the page just saved to the database
				var menuOrderPrime = sitePage.ParentMenuId == 0 ? $"{sitePage.PageOrder:00000}" : "00000";
				var menuOrderSub = sitePage.ParentMenuId == 0 ? "00000" : $"{sitePage.PageOrder:00000}";
				// If the page is a child page of a Menu Section, then menuOrderPrime=ParentMenuId and menuOrderSub=PageOrder
				if (sitePage.ParentMenuId > 0) {
					var parentMenuPage = await _repository.FindByIdAsync((int)sitePage.ParentMenuId);
					menuOrderPrime = $"{parentMenuPage.PageOrder:00000}";
					menuOrderSub = $"{sitePage.PageOrder:00000}";
				}
				sitePage.MenuOrder = $"{menuOrderPrime}.{menuOrderSub}";

				// Create the SitePage in the database
				await _repository.SaveAsync(sitePage);

				return RedirectToAction(nameof(Index));
			}
			return View(sitePage);
		}



		// GET: SitePages/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null) {
				return NotFound();
			}

			var sitePage = await _repository.FindByIdAsync((int)id);
			if (sitePage == null) {
				return NotFound();
			}

			var ParentPageList = _repository.SitePages.Where(p => p.ParentMenuId == 0).ToList();
			ParentPageList.Insert(0, new SitePage { Id = 0, Title = "This is a Menu Section" });
			ParentPageList.Insert(0, new SitePage { Id = -1, Title = "This is a Root Page" });
			ViewBag.ParentPageList = new SelectList(ParentPageList, "Id", "Title");

			return View(sitePage);
		}

		// POST: SitePages/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, SitePage sitePage) {
			if (id != sitePage.Id) {
				return NotFound();
			}

			if (ModelState.IsValid) {
				// If the ParentMenuId is -1 (can't set the Id to null for the SelectList this comes from) then set to null
				if (sitePage.ParentMenuId == -1) {
					sitePage.ParentMenuId = null;
				}

				// Calculate the value for MenuOrder in case the Parent Menu has been changed
				var menuOrderPrime = sitePage.ParentMenuId == 0 ? sitePage.PageOrder : 0;
				var menuOrderSub = sitePage.ParentMenuId == 0 ? 0 : sitePage.PageOrder;
				// If the page is a child page of a Menu Section, then menuOrderPrime=ParentMenuId and menuOrderSub=PageOrder
				if (sitePage.ParentMenuId > 0) {
					var parentMenuPage = await _repository.FindByIdAsync((int)sitePage.ParentMenuId);
					menuOrderPrime = parentMenuPage.PageOrder;
					menuOrderSub = sitePage.PageOrder;
				}
				sitePage.MenuOrder = $"{menuOrderPrime:00000}.{menuOrderSub:00000}";

				// Set the values of unbound field(s) before saving changes to the database: LastUpdatedBy, LastUpdatedOn
				var adminUser = await _userManager.FindByNameAsync(User.Identity.Name);
				sitePage.LastUpdatedBy = adminUser.Id;
				sitePage.LastUpdatedOn = DateTime.Now;

				await _repository.SaveAsync(sitePage);
				return RedirectToAction(nameof(Index));
			}
			return View(sitePage);
		}



		// GET: SitePages/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null) {
				return NotFound();
			}

			var sitePage = await _repository.FindByIdAsync((int)id);
			if (sitePage == null) {
				return NotFound();
			}

			var pageToDelete = new DeleteViewModel() {
				Id = sitePage.Id,
				Active = sitePage.Active,
				ShowInMenu = sitePage.ShowInMenu,
				Title = sitePage.Title,
				CreatedBy = sitePage.CreatedBy,
				CreatedOn = sitePage.CreatedOn,
				LastUpdatedBy = sitePage.LastUpdatedBy,
				LastUpdatedOn = sitePage.LastUpdatedOn
			};
			// Set the ParentMenu to the page title of the menu section. If a root level page, then set to "This is a Root Page"
			if (sitePage.ParentMenuId == null) {
				pageToDelete.ParentMenu = "This is a Root Page";
			} else {
				pageToDelete.ParentMenu = (await _repository.FindByIdAsync((int)sitePage.ParentMenuId)).Title;
			}

			return View(pageToDelete);
		}

		// POST: SitePages/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			var sitePage = await _repository.FindByIdAsync(id);
			await _repository.DeleteAsync(sitePage);
			return RedirectToAction(nameof(Index));
		}


		// POST: SitePages/Reorder/1?direction=Up|Down
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Reorder(int id, string direction) {
			await _repository.ReorderSitePageAsync(id, (ReorderDirections)Enum.Parse(typeof(ReorderDirections), direction));
			return RedirectToAction(nameof(Index));
		}
	}
}

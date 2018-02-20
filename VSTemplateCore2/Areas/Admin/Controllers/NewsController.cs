using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VSTemplateCore2.Data;
using VSTemplateCore2.Extensions;
using VSTemplateCore2.Models;
using VSTemplateCore2.Areas.Admin.Models.NewsViewModels;

namespace VSTemplateCore2.Areas.Admin.Controllers {

	[Area("Admin")]
	[Authorize]
	public class NewsController : Controller {

		private readonly INewsRepository _repository;
		private readonly UserManager<ApplicationUser> _userManager;

		public NewsController(INewsRepository repository, UserManager<ApplicationUser> userManager) {
			_repository = repository;
			_userManager = userManager;
		}


		// GET: News
		public IActionResult Index() {
			var newsList = new List<IndexViewModel>();
			foreach (var item in _repository.News.OrderByDescending(m => m.ArticleDate)) {
				newsList.Add(new IndexViewModel() {
					Id = item.Id,
					ArticleDate = item.ArticleDate,
					Title = item.Title,
					Alias = $"/en/news/{item.Alias}",
					Active = item.Active,
					Featured = item.Featured,
					Urgent = item.Urgent
				});
			}
			return View(newsList);
		}


		// GET: News/Create
		public IActionResult Create() =>
			View(new CreateViewModel());


		// POST: News/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateViewModel newsArticle) {
			if (ModelState.IsValid) {
				var adminUser = await _userManager.FindByNameAsync(User.Identity.Name);
				// Create a News object to send to SaveAsync
				var news = new News {
					Title = newsArticle.Title,
					ArticleDate = newsArticle.ArticleDate,
					DisplayOnDate = newsArticle.DisplayOnDate,
					Active = newsArticle.Active,
					Featured = newsArticle.Featured,
					Urgent = newsArticle.Urgent,
					Excerpt = newsArticle.Excerpt,
					Content = newsArticle.Content,
					ImagePath = newsArticle.ImagePath,
					VideoEmbedCode = newsArticle.VideoEmbedCode,

					// Calculate the value of the alias for this news article
					Alias = newsArticle.Title.CreateAlias(65),

					LastUpdatedBy = adminUser.Id,
					LastUpdatedOn = DateTime.Now
				};
				await _repository.SaveAsync(news);
				return RedirectToAction(nameof(Index));
			}
			return View(newsArticle);
		}



		// GET: News/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null) {
				return NotFound();
			}

			var newsArticle = await _repository.FindByIdAsync((int)id);
			if (newsArticle == null) {
				return NotFound();
			}

			return View(new EditViewModel {
				Id = newsArticle.Id,
				Active = newsArticle.Active,
				Featured = newsArticle.Featured,
				Urgent = newsArticle.Urgent,
				Title = newsArticle.Title,
				Alias = newsArticle.Alias,
				ArticleDate = newsArticle.ArticleDate,
				DisplayOnDate = newsArticle.DisplayOnDate,
				Excerpt = newsArticle.Excerpt,
				Content = newsArticle.Content,
				ImagePath = newsArticle.ImagePath,
				VideoEmbedCode = newsArticle.VideoEmbedCode
			});
		}

		// POST: News/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditViewModel newsArticle) {
			if (id != newsArticle.Id) {
				return NotFound();
			}

			if (ModelState.IsValid) {
				var adminUser = await _userManager.FindByNameAsync(User.Identity.Name);
				// Create a News object to send to SaveAsync
				var news = new News {
					Id = newsArticle.Id,
					Title = newsArticle.Title,
					Alias = newsArticle.Alias,
					ArticleDate = newsArticle.ArticleDate,
					DisplayOnDate = newsArticle.DisplayOnDate,
					Active = newsArticle.Active,
					Featured = newsArticle.Featured,
					Urgent = newsArticle.Urgent,
					Excerpt = newsArticle.Excerpt,
					Content = newsArticle.Content,
					ImagePath = newsArticle.ImagePath,
					VideoEmbedCode = newsArticle.VideoEmbedCode,

					LastUpdatedBy = adminUser.Id,
					LastUpdatedOn = DateTime.Now
				};
				await _repository.SaveAsync(news);
				return RedirectToAction(nameof(Index));
			}
			return View(newsArticle);
		}


		// GET: News/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null) {
				return NotFound();
			}

			var newsArticle = await _repository.FindByIdAsync((int)id);
			if (newsArticle == null) {
				return NotFound();
			}

			return View(new DeleteViewModel {
				Id = newsArticle.Id,
				Active = newsArticle.Active,
				Featured = newsArticle.Featured,
				Urgent = newsArticle.Urgent,
				Title = newsArticle.Title,
				LastUpdatedBy = newsArticle.LastUpdatedBy,
				LastUpdatedOn = newsArticle.LastUpdatedOn
			});
		}

		// POST: News/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			var newsArticle = await _repository.FindByIdAsync(id);
			await _repository.DeleteAsync(newsArticle);
			return RedirectToAction(nameof(Index));
		}
	}
}

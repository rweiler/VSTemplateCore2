using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VSTemplateCore2.Areas.Admin.Data;
using VSTemplateCore2.Models;

namespace VSTemplateCore2.Controllers {
	public class HomeController : Controller {

		private readonly ISitePageRepository _repository;
		private readonly IConfiguration _config;

		public HomeController(ISitePageRepository repository, IConfiguration configuration) {
			_repository = repository;
			_config = configuration;
		}


		public IActionResult Index() =>
			View();

		public IActionResult About() {
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact() {
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<IActionResult> RenderPageContent(string menuSection, string pageAlias) {
			// Get the SitePage specified by the unqiue pageAlias
			var sitePage = await _repository.FindByAliasAsync(pageAlias);

			// Populate ViewData with information needed to render the Meta Data
			if (sitePage.ParentMenuId != null) {
				ViewData["MenuSection"] = (await _repository.FindByIdAsync((int)sitePage.ParentMenuId)).Title;
			}
			ViewData["Company"] = _config["ApplicationSettings:CompanyName"];

			return View(sitePage);
		}
	}
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using VSTemplateCore2.Areas.Admin.Models;

namespace VSTemplateCore2.Authorization {
	public class MasterAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, SitePage> {
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, SitePage resource) {
			// Make sure there is a user and a resource
			if (context.User == null || resource == null) {
				return Task.FromResult(0);
			}

			// Verify the User is a MasterAdmin as MasterAdmins can edit and/or delete all pages
			if (context.User.IsInRole(Constants.MasterAdminRole)) {
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}
}

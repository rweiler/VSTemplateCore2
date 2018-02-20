using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using VSTemplateCore2.Areas.Admin.Models;


namespace VSTemplateCore2.Authorization {
	public class AdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, SitePage> {
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, SitePage resource) {
			// Make sure there is a user and a resource
			if (context.User != null && resource != null) {
				// Admins can only edit pages marked as AllowPageEdit or delete pages marked as AllowPageDelete
				if ((requirement.Name == Constants.UpdateOperationName && resource.AllowEdit) || (requirement.Name == Constants.DeleteOperationName && resource.AllowDelete)) {
					// Verify the user is an Admin before allowing the edit or delete priviledge
					if (context.User.IsInRole(Constants.AdminRole)) {
						context.Succeed(requirement);
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}

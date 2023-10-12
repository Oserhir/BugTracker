using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheBugTracker.Extensions;
using TheBugTracker.Models;
using TheBugTracker.Models.Enums;
using TheBugTracker.Models.ViewModels;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly IBTRolesService _rolesService;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly UserManager<BTUser> _userManager;

        public UserRolesController(IBTRolesService rolesService, 
                        IBTCompanyInfoService companyInfoService, 
                        UserManager<BTUser> userManager)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            // Add instance of the ViewModel as a list
            List<ManageUserRolesViewModel> model = new();

            // Get CompanyId
            int companyId = User.Identity.GetCompanyId().Value;

            // Get all company users
            List<BTUser> users = await _companyInfoService.GetAllMembersAsync(companyId);

            foreach (BTUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();

                viewModel.BTUser = user;

                // Get Roles for the user
                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);
               
                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", selected);

                model.Add(viewModel);
            }


           // Return the model to the View
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {

            // Check for Demo User
            if (await IsDemoUser())
            {
                return new RedirectResult("~/Identity/Account/AccessDenied");
            }

            // Get the company Id
            int companyId = User.Identity.GetCompanyId().Value;

            // Instantiate the BTUser
            BTUser user = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.BTUser.Id);

            // Get Roles for the user
            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(user);

            if (member.SelectedRoles != null)
            {
                // Remove user from their roles
                if (await _rolesService.RemoveUserFromRolesAsync(user, roles))
                {
                    // Add user to the new selected role(s)
                    foreach (string role in member.SelectedRoles)
                    {
                        await _rolesService.AddUserToRoleAsync(user, role);
                    }
                }
            }

            // Navigate back to the view
            return RedirectToAction(nameof(ManageUserRoles));
        }

        #region Is Demo User
        private async Task<bool> IsDemoUser()
        {
            // Check if Demo User
            BTUser btUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(btUser, nameof(Roles.DemoUser)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

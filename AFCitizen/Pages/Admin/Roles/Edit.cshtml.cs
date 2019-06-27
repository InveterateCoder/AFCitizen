using System.Collections.Generic;
using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin.Roles
{
    public class EditModel : PageModel
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<CitizenUser> userManager;
        public EditModel(RoleManager<IdentityRole> roleMgr, UserManager<CitizenUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }
        public Models.Admin.RoleEditMod RoleEditModel { get; set; }
        public async Task OnGetAsync(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<CitizenUser> members = new List<CitizenUser>();
            List<CitizenUser> nonMembers = new List<CitizenUser>();
            foreach (CitizenUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            RoleEditModel = new Models.Admin.RoleEditMod
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
        }
        public async Task<IActionResult> OnPostAsync(Models.Admin.RoleModificationMod model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    CitizenUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            AddErrorsFromResult(result);
                    }
                }
            }
            foreach (string userId in model.IdsToDelete ?? new string[] { })
            {
                CitizenUser user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        AddErrorsFromResult(result);
                }
            }
            if (ModelState.IsValid)
                return RedirectToPage("Index");
            else
                return Redirect(Url.Page("Edit", new { id = model.RoleId }));
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
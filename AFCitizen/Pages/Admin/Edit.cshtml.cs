using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin
{
    public class EditModel : PageModel
    {
        private UserManager<CitizenUser> userManager;
        private IUserValidator<CitizenUser> userValidator;
        private IPasswordValidator<CitizenUser> passwordValidator;
        private IPasswordHasher<CitizenUser> passwordHasher;
        public EditModel(UserManager<CitizenUser> usrMgr, IUserValidator<CitizenUser> userValid,
            IPasswordValidator<CitizenUser> passValid, IPasswordHasher<CitizenUser> passwordHash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }
        [BindProperty]
        public Models.Admin.EditMod Form { get; set; } = new Models.Admin.EditMod();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            CitizenUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                Form.Id = user.Id;
                Form.User = user.UserName;
                Form.Email = user.Email;
                Form.Dispatcher = user.Dispatcher;
                Form.Position = user.Position;
                return Page();
            }
            else
                return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostAsync(string password)
        {
            CitizenUser _user = await userManager.FindByIdAsync(Form.Id);
            bool isDispPosModified = false;
            if (_user != null)
            {
                if (!string.IsNullOrEmpty(Form.User))
                    _user.UserName = Form.User;
                if(Form.Dispatcher != _user.Dispatcher)
                {
                    _user.Dispatcher = Form.Dispatcher;
                    isDispPosModified = true;
                }
                if(Form.Position != _user.Position)
                {
                    _user.Position = Form.Position;
                    isDispPosModified = true;
                }
                IdentityResult validEmail = null;
                if (!string.IsNullOrEmpty(Form.Email))
                {
                    _user.Email = Form.Email;
                    validEmail = await userValidator.ValidateAsync(userManager, _user);
                    if (!validEmail.Succeeded)
                        AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, _user, password);
                    if (validPass.Succeeded)
                        _user.PasswordHash = passwordHasher.HashPassword(_user, password);
                    else
                        AddErrorsFromResult(validPass);
                }
                if (!(!isDispPosModified || (string.IsNullOrEmpty(Form.User) && (validPass == null && validEmail == null)) || (validEmail != null && !validEmail.Succeeded || validPass != null && !validPass.Succeeded)))
                {
                    IdentityResult result = await userManager.UpdateAsync(_user);
                    if (result.Succeeded)
                        return RedirectToPage("Index");
                    else
                        AddErrorsFromResult(result);
                }
            }
            else
                ModelState.AddModelError("", "Пользователь не найден");
            return Page();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
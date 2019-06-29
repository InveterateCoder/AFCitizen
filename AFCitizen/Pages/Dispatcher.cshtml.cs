using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    [Authorize(Policy = "ƒиспетчер")]
    public class DispatcherModel : PageModel
    {
        public string Authority { get; set; }
        public IEnumerable<Block> OpenBlocks;
        public IEnumerable<CitizenUser> Employees;
        public async Task<IActionResult> OnGetAsync([FromServices] UserManager<CitizenUser> userMgr)
        {
            Authority = User.Identity.Name;
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as Models.ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as Models.ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as Models.ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            OpenBlocks = await Task.Run(() => levelDbContext.Blocks.Where(block => block.To == Authority && block.Type == BlockType.Open
                  && !levelDbContext.Blocks.Any(rep => (rep.DocId == block.DocId && block.Type != BlockType.Open))).ToArray());
            Employees = await Task.Run(() => userMgr.Users.Where(user => user.Dispatcher == User.Identity.Name).ToArray());
            return Page();
        }
    }
}
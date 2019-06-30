using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    [Authorize(Policy = "�����")]
    public class AgentModel : PageModel
    {
        public string Header { get; set; }
        public IEnumerable<Block> OpenBlocks { get; set; }
        public IActionResult OnGet()
        {
            Header = User.Identity.Name;
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("�����������"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("�����������"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("����������"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            OpenBlocks = levelDbContext.Blocks.Where(block => block.To == User.Identity.Name && block.Type == BlockType.Accept && !levelDbContext.Blocks.Any(ab => ab.DocId == block.DocId && ab.Type == BlockType.Close));
            return Page();
        }
    }
}
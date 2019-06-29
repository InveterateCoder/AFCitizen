using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AFCitizen.Models;

namespace AFCitizen.Pages
{
    [Authorize(Roles = "Пользователь")]
    public class ActiveModel : PageModel
    {
        public string Header { get; set; }
        public IEnumerable<Block> OpenBlocks;
        public void OnGet([FromServices]UserLevelDbContext userDb)
        {
            Header = User.Identity.Name;
            OpenBlocks = userDb.Blocks.Where(block => !userDb.Blocks.Any(b => b.DocId == block.DocId && b.From == User.Identity.Name && b.Type == BlockType.Close));
        }
    }
}
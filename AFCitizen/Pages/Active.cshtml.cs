using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AFCitizen.Models;
using Microsoft.EntityFrameworkCore;

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
            OpenBlocks = userDb.Blocks.Where(b => (b.From == User.Identity.Name || b.To == User.Identity.Name) && !userDb.Blocks.Any(ba => ba.DocId == b.DocId && ba.isClosed));
            int i = OpenBlocks.Count();
        }
        public async Task<IActionResult> OnPostCloseBlockAsync([FromServices]UserLevelDbContext userDbContex, string blockId)
        {
            Block block = await userDbContex.Blocks.FindAsync(blockId);
            Block newBlock = new Block();
            newBlock.DocId = block.DocId;
            newBlock.isClosed = true;
            newBlock.From = User.Identity.Name;
            newBlock.To = User.Identity.Name;
            newBlock.Type = BlockType.Close;
            newBlock.City = block.City;
            newBlock.PreviousHash = block.Hash;
            newBlock.AuthorityType = block.AuthorityType;
            newBlock.Document = block.Document;
            newBlock.Lock();
            await userDbContex.Blocks.AddAsync(newBlock);
            await userDbContex.SaveChangesAsync();
            return RedirectToPage("Active");
        }
        public async Task<IActionResult> OnPostRedirectBlockAsync([FromServices]UserLevelDbContext userDbContex, string blockId, ushort authorityLevel, string to, string authorityType)
        {
            Block block = await userDbContex.Blocks.FindAsync(blockId);
            Block newBlock = new Block();
            newBlock.AuthorityType = authorityType;
            newBlock.DocId = Guid.NewGuid().ToString();
            newBlock.From = User.Identity.Name;
            newBlock.To = to;
            newBlock.Type = BlockType.Open;
            newBlock.City = block.City;
            newBlock.PreviousHash = block.Hash;
            newBlock.Document = block.Document;
            newBlock.Lock();
            ILevelDbContext levelDbContext = null;
            switch (authorityLevel)
            {
                case 1:
                    levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
                    break;
                case 2:
                    levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
                    break;
                case 3:
                    levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
                    break;
            }
            await userDbContex.Blocks.AddAsync(newBlock);
            await userDbContex.SaveChangesAsync();
            await levelDbContext.Blocks.AddAsync(newBlock);
            await ((DbContext)levelDbContext).SaveChangesAsync();
            newBlock = new Block();
            newBlock.DocId = block.DocId;
            newBlock.isClosed = true;
            newBlock.From = User.Identity.Name;
            newBlock.To = User.Identity.Name;
            newBlock.Type = BlockType.Close;
            newBlock.PreviousHash = block.Hash;
            newBlock.City = block.City;
            newBlock.AuthorityType = block.AuthorityType;
            newBlock.Document = block.Document;
            newBlock.Lock();
            await userDbContex.Blocks.AddAsync(newBlock);
            await userDbContex.SaveChangesAsync();
            return RedirectToPage("Active");
        }
        public async Task<IActionResult> OnPostAppealAsync([FromServices]UserLevelDbContext userDbContex, string blockId)
        {
            Block block = await userDbContex.Blocks.FindAsync(blockId);
            if (string.IsNullOrEmpty(block.Replies))
                return RedirectToPage("Error");
            Reply[] replies = (Reply[])Newtonsoft.Json.JsonConvert.DeserializeObject(block.Replies, typeof(Reply[]));
            Authority authority = null;
            try
            {
                authority = Authority.Cities[block.City][block.AuthorityType][replies.Length];
            }
            catch { }
            Block newBlock = new Block();
            newBlock.DocId = block.DocId;
            newBlock.isClosed = true;
            newBlock.From = User.Identity.Name;
            newBlock.To = User.Identity.Name;
            newBlock.Type = BlockType.Close;
            newBlock.City = block.City;
            newBlock.PreviousHash = block.Hash;
            newBlock.AuthorityType = block.AuthorityType;
            newBlock.Document = block.Document;
            newBlock.Lock();
            await userDbContex.Blocks.AddAsync(newBlock);
            await userDbContex.SaveChangesAsync();
            if (authority != null)
            {
                var newBlock2 = new Block();
                newBlock2.AuthorityType = block.AuthorityType;
                newBlock2.City = block.City;
                newBlock2.DocId = Guid.NewGuid().ToString();
                newBlock2.Document = block.Document;
                newBlock2.From = User.Identity.Name;
                newBlock2.PreviousHash = newBlock.Hash;
                newBlock2.Replies = block.Replies;
                newBlock2.To = authority.Name;
                newBlock2.Type = BlockType.Open;
                newBlock2.Lock();
                ILevelDbContext levelDbContext = null;
                switch (authority.Level)
                {
                    case 1:
                        levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
                        break;
                    case 2:
                        levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
                        break;
                    case 3:
                        levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
                        break;
                }
                if (levelDbContext == null)
                    return RedirectToPage("Error");
                await userDbContex.Blocks.AddAsync(newBlock2);
                await userDbContex.SaveChangesAsync();
                await levelDbContext.Blocks.AddAsync(newBlock2);
                await ((DbContext)levelDbContext).SaveChangesAsync();
            }
            return RedirectToPage("Active");
        }
    }
}
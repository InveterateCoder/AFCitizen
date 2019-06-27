using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    public class ComposeModel : PageModel
    {
        [BindProperty]
        public Models.Body Body { get; set; } = new Models.Body();
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string Authority { get; set; }
        public void OnGet()
        {
        }
    }
}
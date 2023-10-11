using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IdentityUsers.Areas.Identity.Data;

namespace IdentityUsers.Areas.Identity.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly IdentityUsers.Areas.Identity.Data.ClaimContext _context;

        public CreateModel(IdentityUsers.Areas.Identity.Data.ClaimContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ClaimEntity ClaimEntity { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.ClaimsEntity == null || ClaimEntity == null)
            {
                return Page();
            }

            _context.ClaimsEntity.Add(ClaimEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

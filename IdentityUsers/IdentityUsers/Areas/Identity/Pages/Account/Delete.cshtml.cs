using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityUsers.Areas.Identity.Data;

namespace IdentityUsers.Areas.Identity.Pages.Account
{
    public class DeleteModel : PageModel
    {
        private readonly IdentityUsers.Areas.Identity.Data.ClaimContext _context;

        public DeleteModel(IdentityUsers.Areas.Identity.Data.ClaimContext context)
        {
            _context = context;
        }

        [BindProperty]
      public ClaimEntity ClaimEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ClaimsEntity == null)
            {
                return NotFound();
            }

            var claimentity = await _context.ClaimsEntity.FirstOrDefaultAsync(m => m.Id == id);

            if (claimentity == null)
            {
                return NotFound();
            }
            else 
            {
                ClaimEntity = claimentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ClaimsEntity == null)
            {
                return NotFound();
            }
            var claimentity = await _context.ClaimsEntity.FindAsync(id);

            if (claimentity != null)
            {
                ClaimEntity = claimentity;
                _context.ClaimsEntity.Remove(ClaimEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

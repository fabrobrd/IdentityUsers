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
    public class DetailsModel : PageModel
    {
        private readonly IdentityUsers.Areas.Identity.Data.ClaimContext _context;

        public DetailsModel(IdentityUsers.Areas.Identity.Data.ClaimContext context)
        {
            _context = context;
        }

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
    }
}

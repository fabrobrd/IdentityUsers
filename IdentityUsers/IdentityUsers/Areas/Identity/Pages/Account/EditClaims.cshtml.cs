using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityUsers.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IdentityUsers.Areas.Identity.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly IdentityUsers.Areas.Identity.Data.ClaimContext _context;

        public EditModel(IdentityUsers.Areas.Identity.Data.ClaimContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public ClaimEntity ClaimEntity { get; set; } = default!;
        public class InputModel
        {

            public int Id { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [Display(Name = "Nombre")]
            public string ClaimName { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [Display(Name = "Valor")]
            public string ClaimValue { get; set; }

            [Display(Name = "Activo?")]
            public bool Active { get; set; }

        }

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

            InputModel changeValues = AssingValuesToModel(claimentity);

            Input = changeValues;
            ClaimEntity = claimentity;

            return Page();
        }

        private static InputModel AssingValuesToModel(ClaimEntity? claimentity)
        {
            InputModel changeValues = new InputModel();

            changeValues.ClaimName = claimentity.Name;
            changeValues.ClaimValue = claimentity.Value;
            changeValues.Id = claimentity.Id;
            changeValues.Active = claimentity.Active;
            return changeValues;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Account/Claims");                
            }

            _context.Attach(ClaimEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimEntityExists(ClaimEntity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Account/Claims");
        }
        private bool ClaimEntityExists(int id)
        {
          return (_context.ClaimsEntity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using IdentityUsers.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Collections;
using System.Security.Claims;
using IdentityUsers.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

namespace IdentityUsers.Areas.Identity.Pages.Account
{
    public class ClaimsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClaimContext _context;

        public ClaimsModel(UserManager<ApplicationUser> userManager
                            , ClaimContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<ViewModel> ClaimsPropierties { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [Display(Name = "Nombre")]
            public string ClaimName { get; set; }

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [Display(Name = "Valor")]
            public string ClaimValue { get; set; }
        }
        public class ViewModel:InputModel
        {
            public int Id { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            IList<ClaimEntity> claims = await GetAllClaimsActive();
            ClaimsPropierties = new List<ViewModel>();
            foreach (var items in claims.OrderBy(x => x.Name))
            {
                var list = new ViewModel();
                list.Id = items.Id;
                list.ClaimName = items.Name;
                list.ClaimValue = items.Value;
                ClaimsPropierties.Add(list);
            }
        }

        protected async Task<IList<ClaimEntity>> GetAllClaimsActive()
        {
            IList<ClaimEntity> claims = new List<ClaimEntity>();
            claims = await (from cl in _context.ClaimsEntity
                            where cl.Active == true
                            select cl).ToListAsync();
            return claims;
        }
//        public async Task<IActionResult> OnPostAsync(InputModel model, string returnUrl = null)
        public async Task<IActionResult> OnPostAsync(InputModel model)
        {
            string returnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateClaim();
                if (await FindByClaimNameAsync(model.ClaimName) == false)
                {
                    await SaveClaimEntity(model);
                }

            }
            return Page();
        }
        private ApplicationUser CreateClaim()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        protected async Task<bool> FindByClaimNameAsync(string claimName)
        {
            bool result = await (from cl in _context.ClaimsEntity
                                 where cl.Name == claimName
                                 select cl).AnyAsync();
            return result;
        }

        protected async Task SaveClaimEntity(InputModel model)
        {
            ClaimEntity claimsEntity = new ClaimEntity();
            claimsEntity.Name = model.ClaimName;
            claimsEntity.Value = model.ClaimValue;
            claimsEntity.Active = true;
            await _context.SaveChangesAsync();
        }
    }
}


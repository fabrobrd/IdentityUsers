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
using Abakon.SharedKernel.Domain;
using MessagePack.Formatters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            ClaimsPropierties = await ListOfClaims();
            ReturnUrl = returnUrl;

        }

        protected async Task<IList<ClaimEntity>> GetAllClaimsActive()
        {
            IList<ClaimEntity> claims = new List<ClaimEntity>();
            claims = await (from cl in _context.ClaimsEntity
                            where cl.Active == true
                            select cl).ToListAsync();
            return claims;
        }

        protected async Task<List<ViewModel>> ListOfClaims()
        {
            IList<ClaimEntity> claims = await GetAllClaimsActive();
            List<ViewModel> claimsPropierties = new List<ViewModel>();
            if (claims.Count != 0)
            {
                foreach (var items in claims.OrderBy(x => x.Name))
                {
                    var list = new ViewModel();
                    list.Id = items.Id;
                    list.ClaimName = items.Name;
                    list.ClaimValue = items.Value;
                    claimsPropierties.Add(list);
                }
            }
            return claimsPropierties;


        }

        //        public async Task<IActionResult> OnPostAsync(InputModel model, string returnUrl = null)
        public async Task<IActionResult> OnPostAsync(InputModel Input)
        {
            string returnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateClaim();
                if (await FindByClaimNameAsync(Input.ClaimName) == false)
                {
                    await SaveClaimEntity(Input);
                }

            }
            ClaimsPropierties = await ListOfClaims();
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
            await _context.ClaimsEntity.AddAsync(claimsEntity);
            try
            {
                var result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            string returnUrl = Url.Content("~/");

            ClaimEntity claimRemoved = await _context.ClaimsEntity.FindAsync(id);
            bool searchClaim = await checkIfTheClaimIsUsed(claimRemoved.Name);
            if (searchClaim)
            {
                var deleted = _context.ClaimsEntity.Remove(claimRemoved);
                int saved = await _context.SaveChangesAsync();
            }

            ClaimsPropierties = await ListOfClaims();
            return Page();
        }

        protected async Task<bool> checkIfTheClaimIsUsed(string claimName)
        {

            Claim claim = new Claim(claimName, "");
            var result = await _userManager.GetUsersForClaimAsync(claim);
            if (result.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el CLAIM, hay usuarios utilizándolo");
            }
            return true;
        }

        public async Task<IActionResult> OnPutAsync(int id)
        {
            string returnUrl = Url.Content("~/");
            ClaimEntity claimRemoved = await _context.ClaimsEntity.FindAsync(id);
            bool searchClaim = await checkIfTheClaimIsUsed(claimRemoved.Name);

            

            if (searchClaim)
            {
            }

                ClaimsPropierties = await ListOfClaims();
            return Page();
        }

    }
}


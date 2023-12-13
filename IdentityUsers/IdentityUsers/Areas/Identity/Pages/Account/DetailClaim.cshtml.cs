using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityUsers.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Abakon.SharedKernel.Domain;
using static IdentityUsers.Areas.Identity.Pages.Account.DetailsModel;

namespace IdentityUsers.Areas.Identity.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private readonly IdentityUsers.Areas.Identity.Data.ClaimContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(IdentityUsers.Areas.Identity.Data.ClaimContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ClaimEntity ClaimEntity { get; set; } = default!;
        public ClaimModel modelClaim { get; set; }
        public ClaimToUsers toUsers { get; set; }
        public List<ClaimToUsers> toUsersList { get; set; }
        public IEnumerable<ClaimToUsers> listOfUsers { get; set; }
        public ModelUsers modelUsers { get; set; }

        public class ClaimModel
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

        public class ClaimToUsers
        {
            [Display(Name = "Nombre de usuario")]
            public string UserName { get; set; }
            public string UserId { get; set; }
        }

        public class ModelUsers
        {
            public List<ClaimToUsers> toUsersList { get; set; }
            public IEnumerable<ClaimToUsers> listOfUsers { get; set; }
            public int? idClaim { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ModelUsers modelU = new ModelUsers();

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
                modelU = await GetUsersClaimAsync(id, claimentity);

                ClaimEntity = claimentity;
            }
            return Page();
        }

        private async Task<ModelUsers> GetUsersClaimAsync(int? id, ClaimEntity? claimentity)
        {
            List<ClaimToUsers> allUsers;
            ClaimModel changeValues = AssingValuesToModel(claimentity);
            ModelUsers modelU = new ModelUsers();

            modelClaim = changeValues;

            toUsersList = await ListUsersAssingClaim(modelClaim.ClaimName, modelClaim.ClaimValue);
            allUsers = ListAllUsers();

            listOfUsers = CompareListOfUsers(toUsersList, allUsers);

            modelU.toUsersList = toUsersList;
            modelU.listOfUsers = listOfUsers;
            modelU.idClaim = id;
            return modelU;
        }

        [NonAction]
        public virtual PartialViewResult PartialView(string viewName, object model) 
        {
            ViewData.Model = model;

            return new PartialViewResult()
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };

        }

        public async Task<IActionResult> OnPostShowUsersClaim(int idclaim, string uservalue)
        {
            List<ClaimToUsers> allUsers;
            DetailsModel model = new DetailsModel(_context, _userManager);
            ModelUsers modelU = new ModelUsers();

            var claimentity = await _context.ClaimsEntity.FirstOrDefaultAsync(m => m.Id == idclaim);
            var userAdd = await _userManager.FindByIdAsync(uservalue);

            if (claimentity != null)
            {
                var addClaimToUser = await _userManager.AddClaimAsync(userAdd, new Claim(claimentity.Name, claimentity.Value));
            }

            modelU = await GetUsersClaimAsync(idclaim, claimentity);

            return PartialView("_tableUserClaims", modelU);
        }

        private static ClaimModel AssingValuesToModel(ClaimEntity? claimentity)
        {
            ClaimModel changeValues = new ClaimModel();

            changeValues.ClaimName = claimentity.Name;
            changeValues.ClaimValue = claimentity.Value;
            changeValues.Id = claimentity.Id;
            changeValues.Active = claimentity.Active;
            return changeValues;
        }

        protected async Task<List<ClaimToUsers>> ListUsersAssingClaim(string claimName, string claimValue)
        {
            Claim claim = new Claim(claimName, claimValue);
            List<ClaimToUsers> claimList = new List<ClaimToUsers>();
            var usersClaim = await _userManager.GetUsersForClaimAsync(claim);
            var result = _userManager.Users.Count();
            foreach (var user in usersClaim)
            {
                ClaimToUsers claimToUsers = new ClaimToUsers();
                claimToUsers.UserId = user.Id;
                claimToUsers.UserName = user.UserName;
                claimList.Add(claimToUsers);
            }
            return claimList;
        }

        protected List<ClaimToUsers> ListAllUsers()
        {
            List<ClaimToUsers> claimList = new List<ClaimToUsers>();
            foreach( var user in _userManager.Users.OrderBy(x => x.UserName))
            {
                ClaimToUsers claims = new ClaimToUsers();
                claims.UserId = user.Id;
                claims.UserName = user.UserName;
                claimList.Add(claims);
            }
            return claimList;
        }

        protected IEnumerable<ClaimToUsers> CompareListOfUsers(List<ClaimToUsers> usersWhithClaim, List<ClaimToUsers> allUsers)
        {
            IEnumerable<ClaimToUsers> result = allUsers.Except(usersWhithClaim);
            return result;
        }

        protected async Task<ClaimEntity> FindClaimEntityByIdAsync(int climId)
        {
            ClaimEntity claimFind = await _context.ClaimsEntity.FindAsync(climId);
            return claimFind;
        }

    }
}

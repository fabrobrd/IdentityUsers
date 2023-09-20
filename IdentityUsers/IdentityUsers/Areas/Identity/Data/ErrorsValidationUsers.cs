using Microsoft.AspNetCore.Identity;

namespace IdentityUsers.Areas.Identity.Data
{
    public class ErrorsValidationUser : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "El pass requiere al menos una letra minúscula" };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "El pass requiere al menos una letra mayúscula" };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "El pass requiere al menos número" };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "El pass requiere al menos un caracter no alfanumérico (Ej. _-/?!*)" };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError { Code = nameof(InvalidUserName), Description = "El nombre de usuario no es válido" };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = nameof(DuplicateUserName), Description = "El usuario ya existe. Verifique..." };
        }
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = "El email no es válido" };
        }
    }
}

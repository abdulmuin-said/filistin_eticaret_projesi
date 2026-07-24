using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using FilistinProje.Web.Resources;

namespace FilistinProje.Core.Helpers
{
    /// <summary>Localizes ASP.NET Identity errors using the active storefront culture.</summary>
    public class TurkceIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public TurkceIdentityErrorDescriber(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError DuplicateEmail(string email)
            => Error(nameof(DuplicateEmail), "Identity_DuplicateEmail", email);

        public override IdentityError DuplicateUserName(string userName)
            => Error(nameof(DuplicateUserName), "Identity_DuplicateUserName", userName);

        public override IdentityError InvalidEmail(string? email)
            => Error(nameof(InvalidEmail), "Identity_InvalidEmail", email ?? string.Empty);

        public override IdentityError InvalidUserName(string? userName)
            => Error(nameof(InvalidUserName), "Identity_InvalidUserName", userName ?? string.Empty);

        public override IdentityError PasswordMismatch()
            => Error(nameof(PasswordMismatch), "Identity_PasswordMismatch");

        public override IdentityError PasswordRequiresDigit()
            => Error(nameof(PasswordRequiresDigit), "Identity_PasswordRequiresDigit");

        public override IdentityError PasswordRequiresLower()
            => Error(nameof(PasswordRequiresLower), "Identity_PasswordRequiresLower");

        public override IdentityError PasswordRequiresUpper()
            => Error(nameof(PasswordRequiresUpper), "Identity_PasswordRequiresUpper");

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => Error(nameof(PasswordRequiresNonAlphanumeric), "Identity_PasswordRequiresNonAlphanumeric");

        public override IdentityError PasswordTooShort(int length)
            => Error(nameof(PasswordTooShort), "Identity_PasswordTooShort", length);

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => Error(nameof(PasswordRequiresUniqueChars), "Identity_PasswordRequiresUniqueChars", uniqueChars);

        public override IdentityError UserAlreadyHasPassword()
            => Error(nameof(UserAlreadyHasPassword), "Identity_UserAlreadyHasPassword");

        public override IdentityError UserAlreadyInRole(string role)
            => Error(nameof(UserAlreadyInRole), "Identity_UserAlreadyInRole", role);

        public override IdentityError UserNotInRole(string role)
            => Error(nameof(UserNotInRole), "Identity_UserNotInRole", role);

        public override IdentityError UserLockoutNotEnabled()
            => Error(nameof(UserLockoutNotEnabled), "Identity_UserLockoutNotEnabled");

        public override IdentityError DefaultError()
            => Error(nameof(DefaultError), "Identity_DefaultError");

        public override IdentityError ConcurrencyFailure()
            => Error(nameof(ConcurrencyFailure), "Identity_ConcurrencyFailure");

        public override IdentityError RecoveryCodeRedemptionFailed()
            => Error(nameof(RecoveryCodeRedemptionFailed), "Identity_RecoveryCodeRedemptionFailed");

        public override IdentityError LoginAlreadyAssociated()
            => Error(nameof(LoginAlreadyAssociated), "Identity_LoginAlreadyAssociated");

        public override IdentityError InvalidToken()
            => Error(nameof(InvalidToken), "Identity_InvalidToken");

        private IdentityError Error(string code, string resourceKey, params object[] arguments)
            => new() { Code = code, Description = _localizer[resourceKey, arguments].Value };
    }
}

using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(SignInManager<IdentityUser> signInManager,
            IMessageBrokerService messageBrokerService,
            UserManager<IdentityUser> userManager,
            ILogger<AccountService> logger)
        {
            _signInManager = signInManager;
            _messageBrokerService = messageBrokerService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityUser> GetUser(string email)
        {
            IdentityUser user = await _userManager.FindByNameAsync(email);

            if (user is null)
            {
                _logger.LogError($"User with email: {email} not found");
            }

            return user;
        }

        public async Task<SignInResult> SignIn(HttpContext httpContext, SignInRequest loginRequest)
        {
            return await _signInManager.PasswordSignInAsync(loginRequest.Email,
                loginRequest.Password, false, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<DatabaseActionResult<IdentityUser>> SignUp(SignUpRequest signUpRequestData)
        {
            var user = new IdentityUser(signUpRequestData.Username) 
            { 
                Email = signUpRequestData.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, signUpRequestData.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError(string.Join(".", createUserResult.Errors));
                return new DatabaseActionResult<IdentityUser>(false, "error during user creation");
            }

            if (!_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else
            {
                var accountConfirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
                 _messageBrokerService.BroadcastSignUpEmail(user.Email, accountConfirmationToken);
            }

            return new DatabaseActionResult<IdentityUser>(true, obj: user);
        }
    }
}

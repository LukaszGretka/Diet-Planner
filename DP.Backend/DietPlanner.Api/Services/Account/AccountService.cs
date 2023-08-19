using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DietPlanner.Api.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IConfiguration _configuration;

        public AccountService(SignInManager<IdentityUser> signInManager,
            IMessageBrokerService messageBrokerService,
            UserManager<IdentityUser> userManager,
            ILogger<AccountService> logger,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _messageBrokerService = messageBrokerService;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
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

        public async Task<DatabaseActionResult<SignUpResponse>> SignUp(SignUpRequest signUpRequestData)
        {
            var user = new IdentityUser(signUpRequestData.Username)
            {
                Email = signUpRequestData.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, signUpRequestData.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError(string.Join(".", createUserResult.Errors));
                return new DatabaseActionResult<SignUpResponse>(false, "error during user creation");
            }

            if (!_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else
            {
                var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedConfirmationToken = HttpUtility.UrlEncode(token);
                string spaHostAddress = _configuration.GetSection("SpaConfig:HostAddress").Value;
                string emailConfirmationLink = $"{spaHostAddress}/confirm-email?" +
                    $"email={user.Email}&confirmationToken={encodedConfirmationToken}";
                _messageBrokerService.BroadcastSignUpEmail(user.Email, emailConfirmationLink);
            }

            return new DatabaseActionResult<SignUpResponse>(true,
                obj: new SignUpResponse
                {
                    UserName = user.UserName,
                    RequireEmailConfirmation = _userManager.Options.SignIn.RequireConfirmedAccount
                });
        }

        public async Task<IdentityResult> ConfirmUserEmail(EmailConfirmationRequest emailConfirmationRequest)
        {
            IdentityUser user = await _userManager.FindByNameAsync(emailConfirmationRequest.Email);

            if(user is null)
            {
                _logger.LogError($"Error during email confirmation for: {emailConfirmationRequest.Email}. User not found");
                return new IdentityResult();
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, emailConfirmationRequest.ConfirmationToken);

            if (!confirmationResult.Succeeded)
            {
                confirmationResult.Errors.ToList().ForEach(error => _logger.LogError(error.Description));
            }

            return confirmationResult;
        }
    }
}

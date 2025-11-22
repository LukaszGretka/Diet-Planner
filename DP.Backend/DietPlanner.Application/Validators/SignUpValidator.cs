using FluentValidation;

namespace DietPlanner.Application.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpValidator()
        {
            RuleFor(login => login.Username).NotEmpty()
                                            .Length(6, 20);


            RuleFor(login => login.Email).NotEmpty()
                                         .Must(email => email.Any(char.IsLetter))
                                         .Matches(@"[a-z0-9]+@[a-z]+\.[a-z]{2,3}");
        }
    }
}

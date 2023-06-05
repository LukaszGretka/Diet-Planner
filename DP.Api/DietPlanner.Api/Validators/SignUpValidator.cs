﻿using DietPlanner.Api.Models.Account;
using FluentValidation;
using System.Linq;

namespace DietPlanner.Api.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpValidator()
        {
            RuleFor(login => login.Username).NotEmpty()
                                            .Length(6, 20);


            RuleFor(login => login.Email).NotEmpty()
                                         .Must(x => x.Any(char.IsLetter))
                                         .Matches(@"[a-z0-9]+@[a-z]+\.[a-z]{2,3}");
        }
    }
}

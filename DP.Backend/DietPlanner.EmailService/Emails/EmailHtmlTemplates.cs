﻿namespace DietPlanner.EmailService.Emails
{
    internal class EmailHtmlTemplates
    {
        internal static string GetSignUpVerificationEmailBody(string confirmationLink)
        {
            return @"Thank you for signing-up to Diet Planner!" +
                "<p>Please confirm your email address by clicking on following link:</p>" +
                $"<a href=\"{confirmationLink}\">Confirm</a>";
        }
    }
}

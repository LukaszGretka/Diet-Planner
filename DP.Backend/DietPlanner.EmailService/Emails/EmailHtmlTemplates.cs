namespace DietPlanner.EmailService.Emails
{
    internal class EmailHtmlTemplates
    {
        internal static string GetSignUpVerificationEmailBody(string activationLink)
        {
            return @"Thank you for signing-up to Diet Planner!" +
                "<p>Please activate your account by clicking on following link:</p>" +
                $"<a href=\"{activationLink}\">Activate my account!</a>";
        }
    }
}

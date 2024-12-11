namespace BankSystemProject.Repositories.Interface
{
    public interface IEmail
    {
         Task SendRegistrationEmailAsync(string recipientEmail, string userName,string AccountNum="");

        //Task<bool> SendEmailAsync(string recipientEmail, string participantName, string subject);
        Task<bool> SendEmailAsync(string recipientEmail, string participantName, string subject, string htmlPart);
    }
}

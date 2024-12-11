using BankSystemProject.Repositories.Interface;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace BankSystemProject.Repositories.Service
{
    public class EmailService:IEmail
    {
        private readonly string _apiKey = "20e0b14a2473ad105ddd312d9140f74e";
        private readonly string _apiSecret = "6d0e299336d383bc201a3e1782f39708";
        public readonly MailjetClient mailjet;
        private readonly IConfiguration _configuration;
        public EmailService(MailjetClient mailjet, IConfiguration configuration)
        {
            this.mailjet = mailjet;
            _configuration = configuration;

        }
        public async Task SendRegistrationEmailAsync(string recipientEmail, string userName, string AccountNum)
        {
          //  MailjetClient client = new MailjetClient(_apiKey, _apiSecret);

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("muhammadibraheem99m@gmail.com", "Muhammad Bank"))
                .WithSubject("Welcome to Our Bank!")
                .WithHtmlPart($"<h3>Hello {userName},</h3><p>Thank you for registering with us. We're excited to have you on board!<br>" +
                $"Now you are have an account bank with account number ==> {AccountNum}</p>")
                .WithTo(new SendContact(recipientEmail))
                .Build();

            var response = await mailjet.SendTransactionalEmailAsync(email);

            if (response.Messages[0].Status != "success")
            {
                throw new Exception("Failed to send registration email.");
            }
        }


        //public async Task SendRegistrationEmailAsync(string recipientEmail, string userName, string accountNum, string confirmationLink)
        //{
        //    MailjetClient client = new MailjetClient(_apiKey, _apiSecret);

        //    var email = new TransactionalEmailBuilder()
        //        .WithFrom(new SendContact("muhammadibraheem99m@gmail.com", "Muhammad Bank"))
        //        .WithSubject("Confirm Your Email Address")
        //        .WithHtmlPart($@"
        //    <h3>Hello {userName},</h3>
        //    <p>Thank you for registering with us. We're excited to have you on board!</p>
        //    {(!string.IsNullOrEmpty(accountNum) ? $"<p>Your account number is: <strong>{accountNum}</strong></p>" : string.Empty)}
        //    <p>Please confirm your email address by clicking the link below:</p>
        //    <a href='{confirmationLink}'>Confirm Email</a>")
        //        .WithTo(new SendContact(recipientEmail))
        //        .Build();

        //    var response = await client.SendTransactionalEmailAsync(email);

        //    if (response.Messages[0].Status != "success")
        //    {
        //        throw new Exception("Failed to send registration email.");
        //    }
        //}

        //public async Task<bool> SendEmailAsync(string recipientEmail, string participantName, string subject, string htmlPart)
        //{
        //    try
        //    {
        //        var request = new MailjetRequest
        //        {
        //            Resource = Mailjet.Client.Resources.Send.Resource,
        //        }
        //        .Property(Mailjet.Client.Resources.Send.FromEmail, "muhammadibraheem99m@gmail.com")
        //        .Property(Mailjet.Client.Resources.Send.FromName, "Events")
        //        .Property(Mailjet.Client.Resources.Send.HtmlPart, htmlPart)
        //        .Property(Mailjet.Client.Resources.Send.To, recipientEmail)
        //        .Property(Mailjet.Client.Resources.Send.Subject, subject);

        //        // Send the email
        //        var response = await mailjet.PostAsync(request);

        //        // Check if the response indicates success
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return true; // Email sent successfully
        //        }
        //        else
        //        {
        //            // Log the error response for debugging
        //            Console.WriteLine($"Failed to send email. Status: {response.StatusCode}, Error: {response.GetErrorInfo}");
        //            return false; // Email sending failed
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging
        //        Console.WriteLine($"Exception occurred while sending email: {ex.Message}");
        //        return false; // Email sending failed due to an exception
        //    }
        //}


public async Task<bool> SendEmailAsync(string recipientEmail, string participantName, string subject, string htmlPart)
    {
        try
        {
            var request = new MailjetRequest
            {
                Resource = Mailjet.Client.Resources.Send.Resource,
            }
            .Property(Mailjet.Client.Resources.Send.FromEmail, "muhammadibraheem99m@gmail.com")
            .Property(Mailjet.Client.Resources.Send.FromName, "Events")
            .Property(Mailjet.Client.Resources.Send.Subject, subject)
            .Property(Mailjet.Client.Resources.Send.HtmlPart, htmlPart)
            .Property(Mailjet.Client.Resources.Send.Recipients, new JArray
            {
            new JObject { {"Email", recipientEmail} }
            });

            // Initialize the MailjetClient
            var mailjet = new MailjetClient("20e0b14a2473ad105ddd312d9140f74e", "6d0e299336d383bc201a3e1782f39708");

            // Send the email
            var response = await mailjet.PostAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true; // Email sent successfully
            }
            else
            {
                Console.WriteLine($"Failed to send email. Status: {response.StatusCode}, Error: {response.GetErrorInfo()}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while sending email: {ex.Message}");
            return false;
        }
    }


}
}

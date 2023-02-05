using SendGrid;
using SendGrid.Helpers.Mail;

namespace FreshFarmMarket.Models
{
    public class EmailSender
    {
        public async Task<bool> SendEmail(string recipientEmail, string subject, string emailContent)
        {
            string apiKey = "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";
            var client = new SendGridClient(apiKey);
            var senderEmail = new EmailAddress("vionaerikakwok@gmail.com", "Viona");
            var recieverEmail = new EmailAddress(recipientEmail, "Dear Fresh Farm Market customer,");

            var msg = MailHelper.CreateSingleEmail(senderEmail, recieverEmail, subject, "", emailContent);

            var res = await client.SendEmailAsync(msg);

            return res.IsSuccessStatusCode;
        }
    }

}

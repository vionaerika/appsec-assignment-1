using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FreshFarmMarket.Models
{
    public class SMSSender
    {
        public async Task<bool> SendSMS(string recipientNumber, string message)
        {
            string accountSid = "ACde35fe248819989e2655243a7263b97b";
            string authToken = "67d984c927d84c537124129f08a2c7d4";
            string senderNumber = "+18142610013";

            TwilioClient.Init(accountSid, authToken);

            var res = MessageResource.CreateAsync(
                to: new PhoneNumber(recipientNumber),
                from: new PhoneNumber(senderNumber),
                body: message);

            return res.IsCompleted;
        }
    }
}

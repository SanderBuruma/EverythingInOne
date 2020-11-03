using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
namespace PersonalWebsite.Controllers
{
   public class DevController : BaseController
   {
      private readonly IHttpContextAccessor _httpContextAccessor;
      private static string emailPassword = ""; // i need to be filled in by SetDevEmailPassword

      public DevController(IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
      {
         _httpContextAccessor = httpContextAccessor;
      }

      public static void Initialize()
      {

      }

      [HttpPost]
      public object EmailTheDev([FromBody] EmailtoSend email) {

         var smtpClient = new SmtpClient("smtp.gmail.com")
         {
            Port = 587,
            Credentials = new NetworkCredential("sanderburuma@gmail.com", emailPassword),
            EnableSsl = true,
         };
            
         smtpClient.Send(email.Sender, "sanderburuma+personalwebsite@gmail.com", email.Subject, email.Body);
         
         var msg = "ok";
         return new { msg }; // makes the post method on the frontend work correctly because it expects JSON
      }

      [HttpGet]
      public bool SetDevEmailPassword(string secretPhrase, string newEmailPassword) {

         if (!CheckSecretPhrase(secretPhrase)) return false;

         emailPassword = newEmailPassword;

         return true;

      }

      
   }

   public class EmailtoSend {
      public string Sender { get; set; }
      public string Receiver { get; set; }
      public string Subject { get; set; }
      public string Body { get; set; }

      public EmailtoSend()
      {
          
      }

      public EmailtoSend(string sender, string receiver, string subject, string body)
      {
         Sender = sender;
         Receiver = receiver;
         Subject = subject;
         Body = body;
      }
   }
}

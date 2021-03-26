using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class EmailVerificationLink
    {
        public static void SendVerificationLinkEmail(Users objUser, string activationlink, string emailid, string password)
        {
            var fromEmail = new MailAddress(emailid, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(objUser.EmailID);
            var fromEmailPassword =password; // Replace with actual password
            string subject = "Note Marketplace - Email Verification";
            string body = "Hello " + objUser.FirstName + " " + objUser.LastName + "<br/>";
            body += "<br/>Thank you for signing up with us.Please click on below link to verify your email address and to do login.<br/>";
            body += "<a href='" + activationlink + "'> Click To VerifyEmail</a>";
            body += "<br/><br/>Best Regards,<br/>";
            body += "Notes Marketplace";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}
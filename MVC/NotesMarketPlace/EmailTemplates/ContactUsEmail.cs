using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class ContactUsEmail
    {
        public static void ContactEmail(ContactUs contactUS, string emailid, string password,string emails)
        {
            foreach (string email in emails.Split(','))
            {
                var fromEmail = new MailAddress(emailid, contactUS.EmailID); //need system email
                var toEmail = new MailAddress(email);
                var fromEmailPassword = password; // Replace with actual password
                string subject = " " + contactUS.FullName + " - " + contactUS.Subject + " ";
                string body = "Hello Admin, <br/></br>" + contactUS.Comments + "<br/>";
                body += "<br/><br/>Best Regards,<br/>";
                body += contactUS.FullName;

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
}
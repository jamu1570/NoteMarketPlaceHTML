using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class PublishedNoteEmailcs
    {

        public static void PublishedNoteNotify(Users sellerDetail,string notetitle, string emailid, string password,string emails)
        {
            foreach (string email in emails.Split(','))
            {
                var fromEmail = new MailAddress(emailid, "NotesMarketPlace"); //need system email
                var toEmail = new MailAddress(email);
                var fromEmailPassword = password; // Replace with actual password
                string subject = " " + sellerDetail.FirstName + " " + sellerDetail.LastName + " - sent his note for review";
                string body = "Hello Admin,<br/> We want to inform you that,<b>" + sellerDetail.FirstName + " " + sellerDetail.LastName + "</b>" +
                    " sent his note <b>" + notetitle + "</b>" + " for review. Please look at the notes and take required actions. <br/>";
                body += "<br/><br/>Regards,<br/>";
                body += "NotesMarketPlace";

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

/*Email from: < Support Email >
 Email sent to: < Admin Email Address(es) mentioned over system configuration tab>
Subject: < Seller name > sent his note for review
Body:
Hello Admins,
We want to inform you that, < Seller name> sent his note
<Note Title> for review. Please look at the notes and take required actions.
Regards,
Notes Marketplace*/
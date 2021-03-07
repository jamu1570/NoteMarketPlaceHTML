using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class ReportedIssuesEmail
    {
        public static void ReportedIssuesNoteNotify(Users sellerDetail, Users buyerdetails, string notetile)
        {
            var fromEmail = new MailAddress("makwanajaymin2033@gmail.com", "NotesMarketPlace"); //need system email
            var toEmail = new MailAddress("makwanajaymin2033@gmail.com");
            var fromEmailPassword = "JAmU#1570@"; // Replace with actual password
            string subject = " "+ buyerdetails.FirstName +" "+ buyerdetails.LastName +" - Reported an issue for : "+notetile+" ";
            string body = "Hello  Admin, <br/><br/> We want to inform you that, <b>" + buyerdetails.FirstName + " " + buyerdetails.LastName +"</b>"+" Reported an issue for <b>"+ sellerDetail.FirstName + " " + sellerDetail.LastName + "'s </b> Note with title <b>" + notetile + "</b>" + ".<br/> Please look at the notes and take required actions.";
            body += "<br/><br/>Best Regards,<br/>";
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

        internal static void ReportedIssuesNoteNotify()
        {
            throw new NotImplementedException();
        }
    }
}
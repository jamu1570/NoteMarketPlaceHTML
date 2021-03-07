using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class AllowDownloadEmail
    {
        public static void AlloWDownloadNotifyEmail(Users sellerDetail, Users BuyerDetail)
        {
            var fromEmail = new MailAddress("makwanajaymin2033@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(BuyerDetail.EmailID);
            var fromEmailPassword = "JAmU#1570@"; // Replace with actual password
            string subject = " " + sellerDetail.FirstName +" "+sellerDetail.LastName + " - Allows you to download a note";
            string body = "Hello <b>" + BuyerDetail.FirstName + " " + BuyerDetail.LastName + "</b>"+",<br/>";
            body += "<br/>We would like to inform you that, <b> " + sellerDetail.FirstName + " " + sellerDetail.LastName + "</b>" +
                " Allows you to download a note Please login and see My Download tabs to download particular note..<br/>";
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
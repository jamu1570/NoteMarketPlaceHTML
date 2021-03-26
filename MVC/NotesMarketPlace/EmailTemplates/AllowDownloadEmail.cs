using NotesMarketPlace.Healpers;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class AllowDownloadEmail
    {
        public static void AlloWDownloadNotifyEmail(Users sellerDetail, Users BuyerDetail,string emailid,string password)
        {
            var fromEmail = new MailAddress(emailid, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(BuyerDetail.EmailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = " " + sellerDetail.FirstName + " " + sellerDetail.LastName + " - Allows you to download a note";
            string body = "Hello <b>" + BuyerDetail.FirstName + " " + BuyerDetail.LastName + "</b>" + ",<br/>";
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

            /* System.Net.NetworkCredential credencials = new System.Net.NetworkCredential();
             credencials.UserName = "SuperEmail.SuperAdminEmail()";
             MailMessage mail = new MailMessage();
             SmtpClient SmtpServer = new SmtpClient();
             mail.To.Add(BuyerDetail.EmailID);
             mail.From = new MailAddress("makwanajaymin2033@gmail.com");
             mail.Subject = " " + sellerDetail.FirstName + " " + sellerDetail.LastName + " - Allows you to download a note";
             mail.IsBodyHtml = true;
             mail.Body += "Hello <b>" + BuyerDetail.FirstName + " " + BuyerDetail.LastName + "</b>" + ",<br/>";
             mail.Body += "<br/>We would like to inform you that, <b> " + sellerDetail.FirstName + " " + sellerDetail.LastName + "</b>" +
              " Allows you to download a note Please login and see My Download tabs to download particular note..<br/>";
             mail.Body += "<br/><br/>Best Regards,<br/>";
             mail.Body += "Notes Marketplace";
             SmtpServer.Host = "smtpserver";
             SmtpServer.Port = 25;
             SmtpServer.Credentials = credencials;
             SmtpServer.EnableSsl = true;
             SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
             SmtpServer.Send(mail);*/

            /*MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient();
            mail.To.Add(BuyerDetail.EmailID);
            mail.From = new MailAddress("makwanajaymin2033@gmail.com");
            mail.Subject = " " + sellerDetail.FirstName + " " + sellerDetail.LastName + " - Allows you to download a note";
            mail.IsBodyHtml = true;
            mail.Body = "Hello <b>" + BuyerDetail.FirstName + " " + BuyerDetail.LastName + "</b>" + ",<br/>";
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.Port = 25;
            SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                if (ex.InnerException != null)
                    Debug.WriteLine("Exception Inner:   " + ex.InnerException);
            }*/
        }   
    }
}
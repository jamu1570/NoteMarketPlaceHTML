using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class UnPublishednote
    {

        public static void UnPublishedNoteNotify(Users sellerDetail, string notetitle,string remaks, string emailid, string password)
        {
            var fromEmail = new MailAddress(emailid, "NotesMarketPlace"); //need system email
            var toEmail = new MailAddress(sellerDetail.EmailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = "Sorry! We need to remove your notes from our portal ";
            string body = "Hello "+ sellerDetail.FirstName +" "+ sellerDetail.LastName + ",<br/> We want to inform you that, your note  <b>" + notetitle + "</b>" + " has been removed from the portal.<br/><br/>Please find our remarks as below - <br/><b>"+remaks +"<b>"+" ";
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
    }
}
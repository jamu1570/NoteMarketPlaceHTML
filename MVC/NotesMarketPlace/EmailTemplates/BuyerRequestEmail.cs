﻿using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class BuyerRequestEmail
    {

        public static void BuyerNotifyEmail(Users objUser, Users sellerRecord, string emailid, string password)
        {
            var fromEmail = new MailAddress(emailid, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(sellerRecord.EmailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = " "+objUser.FirstName + " " + objUser.LastName+" - wants to purchase your notes";
            string body = "Hello " + sellerRecord.FirstName + " " + sellerRecord.LastName + "<br/>";
            body += "<br/>We would like to inform you that, <b> " + objUser.FirstName + " " + objUser.LastName + "</b>" +
                " wants to purchase your notes. Please see Buyer Requests tab and allow download access to Buyer if you have received the payment from him.<br/>"; 
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
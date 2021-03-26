using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Healpers
{
    public class SuperEmail
    {
       private NotesMarketPlaceEntities objMember = new NotesMarketPlaceEntities();

        public static string SuperAdminEmail()
        {
            /*return SystemConfigurations.Where(x => x.keys == "SupportEmailID").Select(y => y.Value).FirstOrDefault();*/
            return "makwanajaymin2033@gmail.com";
        }
    }
}
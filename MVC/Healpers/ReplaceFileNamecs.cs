using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Healpers
{
    public class ReplaceFileNamecs
    {
        public static string AppendTimeStamp(string fileName)
        {
            return string.Concat(
                /*Path.GetFileNameWithoutExtension(fileName),*/
       
                
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

        internal static string AppendTimeStamp(object p)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroAPI
{
    public class Utils
    {
        public static byte[] GetFileAsBytes(string link)
        {
            try
            {
                return (new System.Net.Http.HttpClient()).GetByteArrayAsync(link).Result;
            }
            catch { }

            return null;
        }
    }
}

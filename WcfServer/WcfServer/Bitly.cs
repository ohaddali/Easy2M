using BitlyDotNET.Implementations;
using BitlyDotNET.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServer
{
    public class Bitly
    {
        private const string LOGIN = "o_1oj1m78p6q";
        private const string API_KEY = "R_e126c979728b46589448f63742b6c688";
        public static string getShortenedURL(string url)
        {

            IBitlyService service = new BitlyService(LOGIN, API_KEY);
            if (service.Shorten(url, out string shortened) == StatusCode.OK)
                return shortened;

            return null;

        }
    }
}
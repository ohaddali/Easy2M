using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServer.ClientModels
{
    public class CompanyClient
    {
        public long id { get; set; }
        public string name { get; set; }
        public long ownerID { get; set; }
        public string logoUrl { get; set; }
        public string description { get; set; }
    }
}
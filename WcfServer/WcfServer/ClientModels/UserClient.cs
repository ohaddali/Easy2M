using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServer.ClientModels
{
    public class UserClient
    {
        public bool loggedIn { get; set; }
        public string username { get; set; }
        public bool admin { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string birthdate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfServer.ClientModels;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class AuthService : IAuthService
    {
        DBHandler handler = new linqDBHandler();
     
        public UserClient login(string userName, string password)
        {
            return handler.auth(userName, password);
        }

        public bool register(string username, string password,string fullName , string birthdate, string phone, bool admin)
        {
            return handler.register(username, password, fullName, birthdate, phone, admin);
        }
    }
}

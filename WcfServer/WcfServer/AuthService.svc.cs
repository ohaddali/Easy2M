using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class AuthService : IAuthService
    {
        public bool login(string userName, string password)
        {
            Easy2MEntities ent = new Easy2MEntities();
            
            var L2EQuery = from u in ent.Users
            where u.userName == userName && u.password == password
            select u;

            var user = L2EQuery.FirstOrDefault<User>();

            return user != null;
        }

        public bool register(string userName, string password, string admin)
        {
            bool ad;
            bool.TryParse(admin, out ad);
            User user = new User()
            {
                userName = userName,
                password = password,
                admin = ad
            };

            Easy2MEntities ent = new Easy2MEntities();
            ent.Users.Add(user);
            ent.SaveChanges();
            return true;
        }
    }
}

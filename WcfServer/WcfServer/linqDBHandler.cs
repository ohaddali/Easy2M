using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServer
{
    public class linqDBHandler : DBHandler
    {
        private Easy2MEntities ent = new Easy2MEntities();

      

        public bool auth(string username, string password)
        {
            var res = from user in ent.Users where user.username == username && user.password == password select user;
            return res.FirstOrDefault<User>() != null;
        }

        public bool register(string username, string password, bool admin)
        {
            User user = new User()
            {
                username = username,
                password = password,
                admin = admin
            };
            ent.Users.Add(user);

            try
            {
                ent.SaveChanges();
            }
            catch
            {
                return false;
            }
            
            return true;
        }


        public bool addCompany(string companyName, long ownerID, string picUrl, string description)
        {
            Company company = new Company()
            {
                name = companyName,
                ownerID = ownerID,
                logoUrl = picUrl,
                description = description
            };
            ent.Companies.Add(company);
            return Save();
           
        }




        private bool Save()
        {
            try
            {
                ent.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
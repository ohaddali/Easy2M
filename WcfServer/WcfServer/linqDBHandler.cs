using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServer
{
    public class linqDBHandler : DBHandler
    {
        private Easy2MEntities ent = new Easy2MEntities();

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


        public bool insertCompany(Company company)
        {
            ent.Companies.Add(company);
            return Save(); 
        }


        public bool updateCompany(Company updatedCompany)
        {
            var res = from c in ent.Companies where c.name == "das" select c;
            Company s = res.FirstOrDefault<Company>();
            var company = ent.Companies.Find(updatedCompany.id);
            if(company==null)
                return false;
            ent.Entry(company).CurrentValues.SetValues(updatedCompany);
            return Save();
        }

        public bool deleteCompany(long id)
        {
            Company compToRemove = ent.Companies.Find(id);
            if (compToRemove == null)
                return false;
            ent.Companies.Remove(compToRemove);
            return Save();
        }
    }
}
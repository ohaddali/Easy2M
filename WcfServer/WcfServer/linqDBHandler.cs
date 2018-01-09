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

        public bool addShift(Shift newShift)
        {
            ent.Shifts.Add(newShift);
            return Save();
        }

        public bool deleteShift(int shiftId)
        {
            Shift shiftToRemove = ent.Shifts.Find(shiftId);
            if (shiftToRemove == null)
                return false;
            ent.Shifts.Remove(shiftToRemove);
            return Save();
        }

        public bool updateShift(Shift updatedShift)
        {
            var shift = ent.Shifts.Find(updatedShift.id);
            if (shift == null)
                return false;
            ent.Entry(shift).CurrentValues.SetValues(updatedShift);
            return Save();
        }

        public bool addRole(long compnayId, string roleName)
        {
            Role role = new Role()
            {
                companyId = compnayId,
                roleName = roleName
            };

            ent.Roles.Add(role);
            return Save();
        }

        public bool deleteRole(long roleId)
        {
            Role roleToRemove = ent.Roles.Find(roleId);
            if (roleToRemove == null)
                return false;
            ent.Roles.Remove(roleToRemove);
            return Save();
        }

        public bool addWorkerToCompany(long workerId, long companyId , long roleId)
        {
            workerCompany wc = new workerCompany()
            {
                workerId = workerId,
                companyId = companyId,
                roleId = roleId
            };

            ent.workerCompanies.Add(wc);
            return Save();
        }

        public long clockEnter(Clock clock)
        {
            Clock added = ent.Clocks.Add(clock);
            if(Save())
            {
                return added.id;
            }
            return -1;
        }

        public bool clockExit(long entityId , DateTime endTime)
        {
            Clock clock = ent.Clocks.Find(entityId);
            if (clock == null)
                return false;
            clock.endTime = endTime;
            return Save();
        }

    }
}
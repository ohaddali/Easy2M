using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfServer.ClientModels;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "companiesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select companiesService.svc or companiesService.svc.cs at the Solution Explorer and start debugging.
    public class companiesService : IcompaniesService
    {
        DBHandler handler = new linqDBHandler();
        public Company addCompany(Company newCompany)
        {
            return handler.insertCompany(newCompany);
        }

        public bool addWorkerToCompany(long workerId, long comapnyId, long roleId)
        {
            return handler.addWorkerToCompany(workerId, comapnyId , roleId);
        }

        public bool deleteCompany(long id)
        {
            return handler.deleteCompany(id);
        }

        public CompanyClient[] getWorkerCompanies(long workerId)
        {
            return handler.getAllworkerCompanies(workerId);
        }

        public string NotifyWorkerToJoinCompany(string userPhone, long companyId, long roleId)
        {
            User user = handler.getUserByPhoneNumber(userPhone);
            if (user != null)
            {
                addUserToRole(user, companyId, roleId);
                return null;
            }

            string token = handler.generateToken(companyId, roleId);
            string url = "http://easy2m.com/?token=" + token;
            url = Bitly.getShortenedURL(url);

            return url;
        }

        private async void addUserToRole(User user, long companyId, long roleId)
        {
          if(!user.Companies.Select(company => company.id).Contains(companyId))
           {
                List<long> usersId = new List<long>();
                usersId.Add(user.id);
                if (handler.addWorkerToCompany(user.id, companyId, roleId))
                {
                    Company c = handler.getCompanyById(companyId);
                    if (c == null)
                        return;
                    await Notifications.Instance.Notify("Hey , You join to " +  c.name , usersId);
                }
           }
        }

        public bool updateCompany(Company updatedCompany)
        {
            return handler.updateCompany(updatedCompany);
        }
    }
}

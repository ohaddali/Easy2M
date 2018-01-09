using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "companiesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select companiesService.svc or companiesService.svc.cs at the Solution Explorer and start debugging.
    public class companiesService : IcompaniesService
    {
        DBHandler handler = new linqDBHandler();
        public bool addCompany(Company newCompany)
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

        public bool updateCompany(Company updatedCompany)
        {
            return handler.updateCompany(updatedCompany);
        }
    }
}

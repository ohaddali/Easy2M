﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer
{
    interface DBHandler
    {
        bool register(string username, string password, bool admin);
        bool auth(string username, string password);

        bool insertCompany(Company company);
        bool updateCompany(Company company);
        bool addWorkerToCompany(long workerId, long companyId , long roleId);


        bool addShift(Shift newShift);

        bool deleteShift(int shiftId);

        bool updateShift(Shift updatedShift);

        bool addRole(long compnayId, string roleName);

        bool deleteRole(long roleId);


        long clockEnter(Clock clock);
        bool clockExit(long entityId,DateTime endTime);
        bool deleteCompany(long id);

    }
}

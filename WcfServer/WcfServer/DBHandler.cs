using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer
{
    interface DBHandler
    {
        //Users Table
        bool register(string username, string password, bool admin);
        bool auth(string username, string password);

        //Companies Table
        bool insertCompany(Company company);
        bool updateCompany(Company company);
        bool deleteCompany(long id);

        //WorkerCompany Table
        bool addWorkerToCompany(long workerId, long companyId, long roleId);

        //Shifts Table
        bool addShift(Shift newShift);
        bool deleteShift(int shiftId);
        bool updateShift(Shift updatedShift);

        //Roles Table
        bool addRole(long compnayId, string roleName);
        bool deleteRole(long roleId);

        //Clock Table
        long clockEnter(Clock clock);
        bool clockExit(long entityId,DateTime endTime);

        //ShiftsBoard Table
        bool setShift(ShiftsBoard shiftBoardEnt); //To man a shift with a new workre
        bool updateShift(ShiftsBoard shiftBoardEnt); //Update shift worker.
    }
}

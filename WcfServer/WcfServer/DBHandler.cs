using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfServer.ClientModels;

namespace WcfServer
{
    public interface DBHandler
    {
        //Users Table
        bool register(string userName, string password, string fullName, string birthdate, string phone, bool admin , string token);
        UserClient auth(string username, string password);

        //Companies Table -- Admin Methods
        Company insertCompany(Company company);
        bool updateCompany(Company company);
        bool deleteCompany(long id);
        

        //WorkerCompany Table - Admin Method
        bool addWorkerToCompany(long workerId, long companyId, long roleId);

        //Shifts Table - Admin Methods
        bool addShift(Shift newShift);
        bool deleteShift(int shiftId);
        bool updateShift(Shift updatedShift);
        List<Clock> getClocks(long workerId, DateTime date);
        List<Clock> getClocksByMonth(long workerId, DateTime date);
        Report[] getReportsOfAdmin(long companyId);
        long getDefaultShiftId(long roleId, int dayInTheWeek);

        //Roles Table -- Admin Methods
        long addRole(long compnayId, string roleName);
        bool deleteRole(long roleId);
        CompanyClient[] getAllworkerCompanies(long workerId);
        long getRoleOfWorker(long workerId, long companyId);

        //Clock Table
        long clockEnter(Clock clock);
        WorkerReport[] getReportsOfWorker(long workerId);
        bool clockExit(long entityId,DateTime endTime);
        bool updateClock(Clock updatedClock);
        Clock getClock(long id); //Admin Method
        User getUserByPhoneNumber(string userPhone);

        //ShiftsBoard Table -- Admin Methods
        bool setShift(ShiftsBoard shiftBoardEnt); //To man a shift with a new workre
        string generateToken(long companyId, long roleId);
        bool updateShift(ShiftsBoard shiftBoardEnt); //Update shift worker.

        //ShiftsRequests Table
        bool requestShift(ShiftRequest request);
        bool cancelShiftRequest(ShiftRequest request);

        //Reports Table

        Report getReportByDate(long companyId, DateTime date);
        WorkerReport getWorkerReportByDate(long workerId, DateTime date);
        Company getCompanyById(long companyId);
        bool addWorkerReport(WorkerReport workerReport);
        bool addReport(Report report);

        IEnumerable<Company> getAllCompanies();
        IEnumerable<User> getAllCompanyWorkers(long companyId);


        bool addWorkerToCompanyByToken(long workerId , string token);
    }
}

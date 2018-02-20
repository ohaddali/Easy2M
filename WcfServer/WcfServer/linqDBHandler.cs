using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WcfServer.ClientModels;

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

        public UserClient auth(string username, string password)
        {
            var res = from user in ent.Users where user.username == username && user.password == password select user;
            User serverUser = res.FirstOrDefault<User>();
            if (serverUser == null)
            {
                return new UserClient()
                {
                    loggedIn = false
                };
                
            }
            return new UserClient()
            {
                loggedIn = true,
                username = serverUser.username,
                name = serverUser.name,
                phoneNumber = serverUser.phoneNumber,
                admin = serverUser.admin
            };
            
        }

        public bool register(string username, string password, string fullName, string birthdate, string phone, bool admin)
        {
            User user = new User()
            {
                username = username,
                password = password,
                admin = admin,
                name = fullName,
                birthdate =birthdate,
                phoneNumber= phone
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

        public bool updateClock(Clock updatedClock)
        {
            var clock = ent.Clocks.Find(updatedClock.id);
            if (clock == null)
                return false;
            ent.Entry(clock).CurrentValues.SetValues(updatedClock);
            return Save();
        }

        public bool setShift(ShiftsBoard shiftBoardEnt)
        {
            ent.ShiftsBoards.Add(shiftBoardEnt);
            return Save();
        }

        public bool updateShift(ShiftsBoard shiftBoardEnt)
        {
            var shift = ent.Shifts.Find(shiftBoardEnt.shiftId,shiftBoardEnt.week,shiftBoardEnt.year);
            if (shift == null)
                return false;
            ent.Entry(shift).CurrentValues.SetValues(shiftBoardEnt);
            return Save();
        }

        public bool requestShift(ShiftRequest request)
        {
            ent.ShiftRequests.Add(request);
            return Save();
        }

        public bool cancelShiftRequest(ShiftRequest request)
        {
            // ShiftRequest shiftRequest = ent.ShiftRequests.Find(request.shiftId,request.week,request.year);
            var shiftRequest = (from req in ent.ShiftRequests
                                where req.shiftId == request.shiftId && req.week == request.week
                                && req.year == request.year && req.workerId == request.workerId
                                select req).FirstOrDefault();
            if (shiftRequest == null)
                return false;
            ent.ShiftRequests.Remove(shiftRequest);
            return Save();
        }

        public Clock getClock(long id)
        {
            return ent.Clocks.Find(id);
        }

        public Report getReportByDate(long companyId, DateTime date)
        {
            var reports = from rep in ent.Reports
                          where rep.companyId == companyId &&
                          rep.date.Equals(date)
                          select rep;

            return reports.FirstOrDefault();
        }

        public WorkerReport getWorkerReportByDate(long workerId, DateTime date)
        {
            var reports = from rep in ent.WorkerReports
                          where rep.workerId == workerId &&
                          getWeekOfDate(rep.date) == getWeekOfDate(date) && rep.date.Year == date.Year
                          select rep;

            return reports.FirstOrDefault();
        }

        public List<Clock> getClocks(long workerId , DateTime date)
        {
            var clocks = (from c in ent.Clocks
                        where c.workerId == workerId 
                        && getWeekOfDate(c.startTime) == getWeekOfDate(date)
                        && c.startTime.Year == date.Year
                       select c);
            if (clocks == null)
                return null;

            return clocks.ToList();
        }

        private int getWeekOfDate(DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            return week;
        }

        public bool addWorkerReport(WorkerReport workerReport)
        {
            ent.WorkerReports.Add(workerReport);
            return Save();
        }

        public bool addReport(Report report)
        {
            ent.Reports.Add(report);
            return Save();
        }

        public IEnumerable<Company> getAllCompanies()
        {
            return ent.Companies.AsEnumerable();
        }

        public IEnumerable<User> getAllCompanyWorkers(long companyId)
        {
            return (from worker in ent.workerCompanies where worker.companyId == companyId select worker.Worker);
        }

        public List<Clock> getClocksByMonth(long workerId, DateTime date)
        {
            var clocks = (from c in ent.Clocks
                          where c.workerId == workerId
                          && c.startTime.Month == date.Month
                          && c.startTime.Year == date.Year
                          select c);
            if (clocks == null)
                return null;

            return clocks.ToList();
        }

        public List<Company> getAllworkerCompanies(long workerId)
        {
            var companies = from wc in ent.workerCompanies
                            where wc.workerId == workerId
                            select wc.Company;
            return companies.ToList();
        }
    }
}
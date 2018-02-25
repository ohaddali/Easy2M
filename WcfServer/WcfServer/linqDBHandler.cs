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
            catch(Exception e)
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
                id = serverUser.id,
                username = serverUser.username,
                name = serverUser.name,
                phoneNumber = serverUser.phoneNumber,
                admin = serverUser.admin,
                birthdate = serverUser.birthdate
            };
            
        }

        public bool register(string username, string password, string fullName, string birthdate, string phone, bool admin , string token)
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
            User newUser = ent.Users.Add(user);

            try
            {
                ent.SaveChanges();
            }
            catch
            {
                return false;
            }
            
            if(token != null)
                return addWorkerToCompanyByToken(newUser.id, token);

            return true;
        }


        public Company insertCompany(Company company)
        {
            Company newCompany = ent.Companies.Add(company);
            if (Save())
                return newCompany;

            return new Company { id = -1 };
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

        public long addRole(long compnayId, string roleName)
        {
            Role role = new Role()
            {
                companyId = compnayId,
                roleName = roleName
            };

            role = ent.Roles.Add(role);
            if (Save())
            {
                Shift shift = new Shift()
                {
                    companyId = compnayId,
                    roleId = role.roleId ,
                    startTime = "00:00:00",
                    endTime ="23:59:59",
                    dayInTheWeek = 1
                };

                ent.Shifts.Add(shift);
                if (Save())
                    return role.roleId;
                else
                    return -1;
            }

            return -1;
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
            /*var reports = from rep in ent.WorkerReports
                          where rep.workerId == workerId &&
                          getWeekOfDate(rep.date) == getWeekOfDate(date) && rep.date.Year == date.Year
                          select rep;*/

            var preReports = ent.WorkerReports.Where(rep => rep.date.Year == date.Year);
            WorkerReport workerReport = null;
            foreach(WorkerReport report in preReports)
            {
                if(getWeekOfDate(report.date) == getWeekOfDate(date))
                {
                    workerReport = report;
                    break;
                }
            }

            return workerReport;
        }

        public List<Clock> getClocks(long workerId , DateTime date)
        {
            /*var clocks = (from c in ent.Clocks
                        where c.workerId == workerId 
                        && getWeekOfDate(c.startTime) == getWeekOfDate(date)
                        && c.startTime.Year == date.Year
                       select c);*/

            var preClocks = ent.Clocks.Where(c => c.workerId == workerId && c.startTime.Year == date.Year);
            List<Clock> clocks = new List<Clock>();
            foreach(Clock clock in preClocks)
            {
                if (getWeekOfDate(clock.startTime) == getWeekOfDate(date))
                    clocks.Add(clock);
            }

            if (clocks == null)
                return null;

            return clocks;
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

        public CompanyClient[] getAllworkerCompanies(long workerId)
        {
            var user = (from u in ent.Users where u.id == workerId select u).FirstOrDefault();
            Company[] companies;
            if (user.admin)
            {
                companies = (from company in ent.Companies
                             where company.ownerID == workerId
                             select company).ToArray();
            }
            else
            {
                companies = (from wc in ent.workerCompanies
                            where wc.workerId == workerId
                            select wc.Company).ToArray();
            }
            CompanyClient[] comapinesClient = new CompanyClient[companies.Length];
            for(int i = 0 ; i < companies.Length ; i++)
            {
                Company company = companies[i];
                CompanyClient companyClient = new CompanyClient()
                {
                    id = company.id,
                    name = company.name,
                    ownerID = company.ownerID,
                    logoUrl = company.logoUrl,
                    description = company.description
                };

                comapinesClient[i] = companyClient;
            }

            return comapinesClient;
        }

        public User getUserByPhoneNumber(string userPhone)
        {
            return (from user in ent.Users where user.phoneNumber == userPhone select user).FirstOrDefault();
        }

        public string generateToken(long companyId, long roleId)
        {
            Token token = new Token()
            {
                companyId = companyId,
                roleId = roleId,
                valid = true
            };

            Token newToken = ent.Tokens.Add(token);
            Save();

            return newToken.tokenId + "";
        }

        public bool addWorkerToCompanyByToken(long workerId , string token)
        {
            long.TryParse(token, out long tokenId);
            Token newToken = (from t in ent.Tokens
                              where t.tokenId == tokenId
                              select t).FirstOrDefault();
            if (newToken == null)
                return false;

            if (!newToken.valid)
                return false;

            Token updateToken = new Token()
            {
                tokenId = newToken.tokenId,
                companyId = newToken.companyId,
                roleId = newToken.roleId,
                valid = false
            };

            ent.Entry(newToken).CurrentValues.SetValues(updateToken);
            if (!Save())
                return false;

            return addWorkerToCompany(workerId, newToken.companyId, newToken.roleId);
        }

        public Company getCompanyById(long companyId)
        {
            return (from c in ent.Companies where c.id == companyId select c).FirstOrDefault();
        }

        public WorkerReport[] getReportsOfWorker(long workerId)
        {
            return (from report in ent.WorkerReports where report.workerId == workerId select report).ToArray();
        }

        public Report[] getReportsOfAdmin(long companyId)
        {
            return (from report in ent.Reports where report.companyId == companyId select report).ToArray();
        }

        public long getDefaultShiftId(long roleId, int dayInTheWeek)
        {
            var shifts = from shift in ent.Shifts
                         where shift.roleId == roleId && shift.dayInTheWeek == dayInTheWeek
                         select shift;
            return shifts.FirstOrDefault().id;

        }

        public long getRoleOfWorker(long workerId, long companyId)
        {
            return (from workerCompany in ent.workerCompanies
                    where workerCompany.workerId == workerId && workerCompany.companyId == companyId
                    select workerCompany).FirstOrDefault().roleId;
        }
    }
}
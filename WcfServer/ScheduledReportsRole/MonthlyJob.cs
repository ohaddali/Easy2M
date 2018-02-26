using Microsoft.Office.Interop.Excel;
using Quartz;
using ReportsRole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WcfServer;

namespace ScheduledReportsRole
{
    public class MonthlyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return monthlyReport();
        }

        private async Task monthlyReport()
        {
            DBHandler db = new linqDBHandler();
            AzureBlob blob = new AzureBlob();

            foreach (Company c in db.getAllCompanies())
            {
                ExcelBuilder builder = new ExcelBuilder();
                List<string> columns = new List<string>();
                columns.Add("userName");
                columns.Add("Start Time");
                columns.Add("End Time");
                List<List<string>> rows = new List<List<string>>();
                DateTime monthDate = DateTime.Now;
                monthDate = monthDate.AddMonths(-1);
                TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                monthDate = TimeZoneInfo.ConvertTime(monthDate, israelTimeZone);

                foreach (User u in db.getAllCompanyWorkers(c.id))
                {
                    long userId = u.id;
                    List<Clock> clocks = db.getClocksOfComapny(userId, monthDate , c.id);

                    foreach (Clock shift in clocks)
                    {
                        List<string> row = new List<string>();
                        row.Add(u.name);
                        row.Add(shift.startTime.ToString());
                        row.Add(shift.endTime.ToString());

                        rows.Add(row);
                    }
                }

                FileInfo workBook = builder.write(columns, rows);

                string fileName = c.id + "_" + monthDate.Ticks + ".xlsx";
                string url = await blob.uploadFileAsync(workBook.Name, fileName);
                workBook.Delete();

                Report report = new Report();
                report.companyId = c.id;
                report.date = monthDate;
                report.url = fileName;

                db.addReport(report);

                Trace.TraceInformation("new report : " + url);
            }
        }
    }
}

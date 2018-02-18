using Microsoft.Office.Interop.Excel;
using Quartz;
using ReportsRole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            ExcelBuilder builder = new ExcelBuilder();
            AzureBlob blob = new AzureBlob();

            foreach (Company c in db.getAllCompanies())
            {
                List<string> columns = new List<string>();
                columns.Add("userName");
                columns.Add("Start Time");
                columns.Add("End Time");
                List<List<string>> rows = new List<List<string>>();
                DateTime monthDate = new DateTime();
                TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                monthDate = TimeZoneInfo.ConvertTime(monthDate, israelTimeZone);

                int year = monthDate.Year;
                int month = monthDate.Month - 1;
                if (month == 0)
                {
                    month = 12;
                    year -= 1;
                }

                monthDate = new DateTime(year, month, 1);

                foreach (User u in db.getAllCompanyWorkers(c.id))
                {
                    long userId = u.id;
                    List<Clock> clocks = db.getClocks(userId, monthDate);

                    foreach (Clock shift in clocks)
                    {
                        List<string> row = new List<string>();
                        row.Add(u.name);
                        row.Add(shift.startTime.ToString());
                        row.Add(shift.endTime.ToString());

                        rows.Add(row);
                    }
                }


                Workbook workBook = builder.write(columns, rows);
                workBook.SaveAs("temp.xlsx");
                string url = await blob.uploadFileAsync("temp.xlsx", c.id + "_" + monthDate + ".xlsx");
                File.Delete("temp.xlsx");

                Report report = new Report();
                report.companyId = c.id;
                report.date = monthDate;
                report.url = url;

                db.addReport(report);

                Trace.TraceInformation("new report : " + url);
            }
        }
    }
}

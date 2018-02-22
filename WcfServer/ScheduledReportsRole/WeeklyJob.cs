﻿using Microsoft.Office.Interop.Excel;
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
    public class WeeklyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return weeklyReport();
        }

        private async Task weeklyReport()
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

                DateTime weekDate = new DateTime();
                TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                weekDate = TimeZoneInfo.ConvertTime(weekDate, israelTimeZone);

                foreach (User u in db.getAllCompanyWorkers(c.id))
                {
                    long userId = u.id;
                    List<Clock> clocks = db.getClocks(userId, weekDate);

                    foreach (Clock shift in clocks)
                    {
                        List<string> row = new List<string>();
                        row.Add(u.name);
                        row.Add(shift.startTime.ToString());
                        row.Add(shift.endTime.ToString());

                        rows.Add(row);
                    }
                }

                object misValue = System.Reflection.Missing.Value;
                Workbook workBook = builder.write(misValue,columns, rows);
                workBook.SaveAs("temp.xlsx");

                workBook.Close(true, misValue, misValue);
                builder.quit();

                Marshal.ReleaseComObject(workBook);
                Marshal.ReleaseComObject(builder.getApp());

                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fileName = c.id + "_" + weekDate.Ticks + ".xlsx";
                string url = await blob.uploadFileAsync(path+"/temp.xlsx", fileName);
                File.Delete(path+"/temp.xlsx");

                Report report = new Report();
                report.companyId = c.id;
                report.date = weekDate;
                report.url = fileName;

                db.addReport(report);

                Trace.TraceInformation("new report : " + url);
            }
        }
    }
}

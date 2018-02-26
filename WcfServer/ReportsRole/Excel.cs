using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using OfficeOpenXml;
using System.IO;

namespace ReportsRole
{
    public class ExcelBuilder
    {

        public FileInfo write(List<String> columns , List<List<String>> rows)
        {
            FileInfo file = new FileInfo("temp.xlsx");
            
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("First Sheet");
                int rowIndex = 1;
                for (int column = 1; column <= columns.Count; column++)
                {
                    worksheet.Cells[rowIndex, column].Value = columns.ElementAt(column - 1);
                }

                rowIndex++;

                foreach (List<String> row in rows)
                {
                    for (int column = 1; column <= row.Count; column++)
                        worksheet.Cells[rowIndex, column].Value = row.ElementAt(column - 1);

                    rowIndex++;
                }

                package.Save();

            }
            
            return file;
        }
    }
}

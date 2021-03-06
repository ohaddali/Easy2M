﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ReportsRole
{
    public class ExcelBuilder
    {
        private Application xlApp;

        public ExcelBuilder()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();

        }

        public Workbook write(List<String> columns , List<List<String>> rows)
        {
            Workbook workbook = xlApp.Workbooks.Add(Missing.Value);
            Worksheet workSheet = (Worksheet)workbook.Worksheets.get_Item(1);
            int rowIndex = 0;
            for(int column = 0 ; column < columns.Count; column++)
            {
                workSheet.Cells[rowIndex, column] = columns.ElementAt(column);
            }

            rowIndex++;

            foreach(List<String> row in rows)
            {
                for (int column = 0; column < row.Count; column++)
                    workSheet.Cells[rowIndex, column] = row.ElementAt(column);

                rowIndex++;
            }
            
            return workbook;
        }
    }
}

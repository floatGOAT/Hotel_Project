using System;
using System.CodeDom;
using System.Security.Permissions;
using Excel = Microsoft.Office.Interop.Excel;

namespace Hotel_Project
{
    public class ExcelSerializer
    {
        private Excel.Application excelApp;
        private Excel.Worksheet worksheet;
        private static string _bookName = "Книга учёта.xlsx";

        public ExcelSerializer()
        {
            excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.UserControl = true;
            excelApp.SheetsInNewWorkbook = 1;
        }

        public void isExisted()
        {
            try
            {
                excelApp.Workbooks.Open(_bookName);
                excelApp.Workbooks[0].Sheets.Add();
                worksheet = excelApp.Workbooks[1].Worksheets.get_Item(excelApp.Workbooks[1].Worksheets.Count);
                return;
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                excelApp.Workbooks.Add();
                worksheet = excelApp.Workbooks[1].Worksheets.get_Item(1);
            }
        }

        public void Write(Visitor visitor)
        {
            int counter = 1;

            var FIO = visitor.FIO.Split(' ');
            foreach (var s in FIO)
            {
                switch (counter)
                {
                    case 1:
                        worksheet.Cells[counter, 1] = "Фамилия:";
                        break;
                    case 2:
                        worksheet.Cells[counter, 1] = "Имя:";
                        break;
                    case 3:
                        worksheet.Cells[counter, 1] = "Отчество:";
                        break;
                    default:
                        break;
                }
                worksheet.Cells[counter, 2] = s;
                counter++;
            }
            worksheet.Cells[counter, 1] = "Номер телефона:";
            worksheet.Cells[counter, 2] = visitor.Phone_Number;
            counter++;
            worksheet.Cells[counter, 1] = "Время проживания:";
            worksheet.Cells[counter, 2] = visitor.Duration.ToString();
            counter++;
            worksheet.Cells[counter, 1] = "Число спальных мест:";
            worksheet.Cells[counter, 2] = visitor.Room_Capacity.ToString();
            counter++;
            worksheet.Cells[counter, 1] = "Номер комнеты";
            worksheet.Cells[counter, 2] = visitor.Room_Number;
            counter++;
            worksheet.Cells[counter, 1] = "Тип апартаментов:";
            worksheet.Cells[counter, 2] = visitor.Room_Rang;
            counter++;
            worksheet.Cells[counter, 1] = "Расчётная стоимость проживания:";
            worksheet.Cells[counter, 2] = visitor.Counting_Value();

            try
            {
                excelApp.Workbooks[1].SaveAs(_bookName);
            }
            catch (Exception e)
            {
                excelApp.Workbooks[1].Save();
            }
        }

        public void Close()
        {
            excelApp.Workbooks[1].Close(true);
        }

    }
}
using System;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace Hotel_Project
{
    public class WordSerializer
    {
        private Word.Application wordApp;
        private Word.Document document;
        private static string _docname = "Книга ведомости.docx";

        public WordSerializer()
        {
            wordApp = new Word.Application();
            wordApp.Visible = false;
        }

        public void isExisted()
        {
            try
            {
                wordApp.Documents.Open(_docname, FileMode.Append);
                document = wordApp.Documents.get_Item(wordApp.Documents.Count - 1);
            }
            catch (Exception e)
            {
                wordApp.Documents.Add();
                document = wordApp.Documents.get_Item(1);
            }
        }

        public void Write(Visitor visitor)
        {
            string text = DateTime.Today.Day + "." +
                          DateTime.Today.Month + "." +
                          DateTime.Today.Year +
                          " в " +
                          DateTime.Now.Hour +
                          ":" + DateTime.Now.Minute +
                          ":" + DateTime.Now.Second +
                          " посетитель " +
                          visitor.FIO +
                          " забронировал комнату №" +
                          visitor.Room_Number +
                          " класса " +
                          visitor.Room_Rang +
                          " на " +
                          visitor.Duration +
                          " дней. " +
                          "Расчётная стоимость проживания: " +
                          visitor.Counting_Value() +
                          " денежных единиц.\n";

            wordApp.Selection.Text = text;
            try
            {
                wordApp.ActiveDocument.SaveAs(_docname);
            }
            catch (Exception e)
            {
                wordApp.Documents.Save();
            }
        }

        public void Close()
        {
            wordApp.Documents.Close(true);
        }
    }
}
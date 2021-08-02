using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
//using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Office.Interop.Models;
using Point = Office.Interop.Models.Point;
using System.Globalization;

namespace Office.Interop
{
    
    class Program
    {
        static void Main(string[] args)
        {
            
            
            Console.WriteLine("Геерация отчета ...");
            ExportToWord();
            Console.ReadLine();
        }

        DateTime date = new DateTime();


       public static void ExportToWord()
        {
            var templateFileName = Path.Combine(Environment.CurrentDirectory, "templateTaxationCard2.docx");
    
            if (!File.Exists(templateFileName)) throw new FileNotFoundException("Файл не существует", templateFileName);

            var catalog = new Catalog
            {
                Points = new[]
                {
                    new Point {Yarus = 1, Koef = 5, Poroda = "C", Let = 50, Nmetr = 10, Diametr = 10, Prois = 1, Polnota = 0.8},
            


                }
            };

            using (var application = new NetOffice.WordApi.Application { Visible = true })
            {
                using (var document = application.Documents.Add(templateFileName))
                {
                    var coordinatesTable = document.Bookmarks["Cooredinates"].Range.Tables[1];
                  //  var pointNum = 0;
                    foreach (var point in catalog.Points){
                        var coordinatesRow = coordinatesTable.Rows.Add();
                        coordinatesRow.Cells[1].Range.Text = point.N.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[2].Range.Text = point.Yarus.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[3].Range.Text = point.Koef.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[4].Range.Text = point.Poroda.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[5].Range.Text = point.Let.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[6].Range.Text = point.Nmetr.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[7].Range.Text = point.Diametr.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[8].Range.Text = point.Prois.ToString(CultureInfo.InvariantCulture);
                        coordinatesRow.Cells[10].Range.Text = point.Polnota.ToString(CultureInfo.InvariantCulture);

                        //coordinatesRow.Cells[2].Range.Text = point.X.ToString(CultureInfo.InvariantCulture);
                        //coordinatesRow.Cells[3].Range.Text = point.Y.ToString(CultureInfo.InvariantCulture);

                    }
                    coordinatesTable.Rows[2].Delete();


                }
                application.Activate();
            }

        }


       public static void ExportToExcel()
        {

        }
































        //SaveFileDialog saveFileDialog = new SaveFileDialog();

        //public string TemplateFile;

        //public void WordExport()
        //{
        //    string curDir = Directory.GetCurrentDirectory();
        //    TemplateFile = String.Format("file:///{0}/template/TKP45Template.docx", curDir);
        //    var wordApp = new Microsoft.Office.Interop.Word.Words.Application();
        //    try
        //    {

        //        wordApp.Visible = false;
        //        var wordDoc = wordApp.Documents.Open(TemplateFile);

        //        //saveFileDialog.Filter = "Документ Word  (*.docx*)|*.docx | All files (*.*)|*.*";
        //        //if (saveFileDialog.ShowDialog() == true)
        //        //{
        //        //    curDir = saveFileDialog.FileName;
        //        //}
        //        //else
        //        //{
        //        //    wordDoc.Close();
        //        //}

        //        ReplaceWordStub("{Address}", SelectedHouse.Address, wordDoc);
        //        ReplaceWordStub("{City}", SelectedHouse.City, wordDoc);
        //        ReplaceWordStub("{Owner}", SelectedHouse.Owner, wordDoc);
        //        ReplaceWordStub("{ExpOrg}", SelectedHouse.ExpOrg, wordDoc);
        //        ReplaceWordStub("{Year}", SelectedHouse.Year, wordDoc);
        //        ReplaceWordStub("{CountFloor}", SelectedHouse.CountFloor, wordDoc);
        //        ReplaceWordStub("{PublishData}", SelectedHouse.PublishData.ToString("d"), wordDoc);
        //        ReplaceWordStub("{ReadyWinter}", SelectedHouse.ReadyWinter, wordDoc);
        //        ReplaceWordStub("{Description}", SelectedHouse.Description, wordDoc);
        //        ReplaceWordStub("{Num}", SelectedHouse.Num, wordDoc);
        //        ReplaceWordStub("{Korp}", SelectedHouse.Korp, wordDoc);
        //        ReplaceWordStub("{MatWalls}", SelectedHouse.MatWalls, wordDoc);
        //        ReplaceWordStub("{Basement}", SelectedHouse.Basement, wordDoc);
        //        ReplaceWordStub("{Predsedateli}", SelectedHouse.Predsedateli, wordDoc);
        //        ReplaceWordStub("{Predstoviteli}", SelectedHouse.Predstoviteli, wordDoc);
        //        wordDoc.SaveAs(curDir);
        //        wordDoc.Close();

        //    }
        //    catch { }
        //}
        //private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        //{
        //    try
        //    {
        //        var range = wordDocument.Content;
        //        range.Find.ClearFormatting();
        //        range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        //    }
        //    catch { }

        //}
    }
}

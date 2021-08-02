using System;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Excel = Microsoft.Office.Interop.Excel;
namespace ExcelInterop
{
    class Program
    {
        static AppServiceConnection connection = null;
        static AutoResetEvent appServiceExit;

        static void Main(string[] args)
        {
            // connect to app service and wait until the connection gets closed
            appServiceExit = new AutoResetEvent(false);
            InitializeAppServiceConnection();
            appServiceExit.WaitOne();
        }

        static async void InitializeAppServiceConnection()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "ExcelInteropService";
            connection.PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            AppServiceConnectionStatus status = await connection.OpenAsync();
            if (status != AppServiceConnectionStatus.Success)
            {
                // TODO: error handling
            }
        }

        private static void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            // signal the event so the process can shut down
            appServiceExit.Set();
        }

        private async static void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // Get a deferral because we use an awaitable API below to respond to the message
            // and we don't want this call to get cancelled while we are waiting.
            var messageDeferral = args.GetDeferral();

            string value = args.Request.Message["REQUEST"] as string;
            string result = "";
            switch (value)
            {
                case "CreateSpreadsheet":
                    try
                    {
                        // call Office Interop APIs to create the Excel spreadsheet
                        Excel.Application excel = new Excel.Application();
                        excel.Visible = true;
                        Excel.Workbook wb = excel.Workbooks.Add();
                        Excel.Worksheet sh = wb.Sheets.Add();
                        sh.Name = "DataGrid";
                        sh.Cells[1, "A"].Value2 = "Id";
                        sh.Cells[1, "B"].Value2 = "Номер квартала";
                        sh.Cells[1, "C"].Value2 = "Лесничество";
                        sh.Cells[1, "D"].Value2 = "Номер выдела";
                        sh.Cells[1, "E"].Value2 = "Площадь";
                        sh.Cells[1, "F"].Value2 = "Вид земель";
                        sh.Cells[1, "G"].Value2 = "ОРЛ";
                        sh.Cells[1, "H"].Value2 = "Крутизна";
                        sh.Cells[1, "I"].Value2 = "Экспозиция";

                        for (int i = 0; i < args.Request.Message.Values.Count / 9; i++)
                        {
                            sh.Cells[i + 2, "A"].Value2 = args.Request.Message["Id" + i.ToString()];
                            sh.Cells[i + 2, "B"].Value2 = args.Request.Message["Номер квартала" + i.ToString()];
                            sh.Cells[i + 2, "C"].Value2 = args.Request.Message["Лесничество" + i.ToString()];
                            sh.Cells[i + 2, "D"].Value2 = args.Request.Message["Номер выдела" + i.ToString()];
                            sh.Cells[i + 2, "E"].Value2 = args.Request.Message["Площадь" + i.ToString()];
                            sh.Cells[i + 2, "F"].Value2 = args.Request.Message["Вид земель" + i.ToString()];
                            sh.Cells[i + 2, "G"].Value2 = args.Request.Message["ОРЛ" + i.ToString()];
                            sh.Cells[i + 2, "H"].Value2 = args.Request.Message["Крутизна" + i.ToString()];
                            sh.Cells[i + 2, "I"].Value2 = args.Request.Message["Экспозиция" + i.ToString()];

                        }
                        result = "SUCCESS";
                    }
                    catch (Exception exc)
                    {
                        result = exc.Message;
                    }
                    break;
                default:
                    result = "unknown request";
                    break;
            }

            ValueSet response = new ValueSet();
            response.Add("RESPONSE", result);

            try
            {
                await args.Request.SendResponseAsync(response);
            }
            finally
            {
                // Complete the deferral so that the platform knows that we're done responding to the app service call.
                // Note for error handling: this must be called even if SendResponseAsync() throws an exception.
                messageDeferral.Complete();
            }
        }
    }
}

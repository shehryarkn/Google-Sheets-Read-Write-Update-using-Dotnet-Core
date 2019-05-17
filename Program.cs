using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Google_Sheets_Read_Write_Update_Dotnet_Core_Example
{
    class Program
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Dot Tutorials";
        static readonly string sheet = "Sheet1";
        static readonly string SpreadsheetId = "1KLGHGy-dUlREUjKQozMPhumtFXG2uFiTr7cWMr-RMK4";
        static SheetsService service;

        static void Main(string[] args)
        {
            Init();

            ReadSheet();
            AddRow();
            UpdateCell();
        }

        static void Init(){

            GoogleCredential credential;
            //Reading Credentials File...
            using (var stream = new FileStream("app_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Creating Google Sheets API service...
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        static void AddRow() { 

            // Specifying Column Range for reading...
            var range = $"{sheet}!A:E";
            var valueRange = new ValueRange();

            // Data for another Student...
            var oblist = new List<object>() { "Harry", "80", "77", "62", "98" };
            valueRange.Values = new List<IList<object>> { oblist };

            // Append the above record...
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }

        static void ReadSheet()
        {
            // Specifying Column Range for reading...
            var range = $"{sheet}!A:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);

            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    // Writing Data on Console...
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4} ", row[0], row[1], row[2], row[3], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }

        static void UpdateCell()
        {
            // Setting Cell Name...
            var range = $"{sheet}!C5";
            var valueRange = new ValueRange();

            // Setting Cell Value...
            var oblist = new List<object>() { "32" };
            valueRange.Values = new List<IList<object>> { oblist };

            // Performing Update Operation...
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = updateRequest.Execute();
        }
    }
}

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ProgramGooleSheet
    {
        static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static readonly string ApplicationName = "ApartmentRentalApp";
        static readonly string SpreadsheetId = "18Eh4SJYH6CNlhLaoI-6roLTnMIlHrxpYYJ5YyWXkKU0";
        static readonly string Range = "Detail location!A1:E"; // For example, A1:E reads columns A through E.

        public static void Main()
        {
            GoogleCredential credential;

            using (var stream = new FileStream("credentials/apartmentrentalapp-2d6827136a8c.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, Range);

            // Read the data.
            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {

                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine($"{row[0]}, {row[4]}");
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }
    }
}

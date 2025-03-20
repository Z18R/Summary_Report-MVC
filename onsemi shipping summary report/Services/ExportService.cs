using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using onsemi_shipping_summary_report.Models;
using System.Data;

namespace onsemi_shipping_summary_report.Services
{
    public class ExportService
    {
        private readonly ILogger<ExportService> _logger;
        private readonly IConfiguration _configuration;

        public ExportService(ILogger<ExportService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ExportResult> ExportDataAsync(DateFilterViewModel model, string storedProcedureName, string reportType, bool isReport1, bool isReport2)
        {
            DataTable dataTable = new DataTable();
            string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");

            try
            {
                dataTable = await GetDataFromStoredProcedureAsync(model, storedProcedureName, connectionString);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ExportedData");
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    FormatExcelSheet(worksheet, dataTable, isReport1, isReport2);

                    var stream = new MemoryStream();
                    await package.SaveAsAsync(stream);  
                    stream.Position = 0;

                    return new ExportResult
                    {
                        Stream = stream,
                        FileName = $"{reportType}-{model.FromDate:yyyyMMdd}_{model.ToDate:yyyyMMdd}.xlsx",
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        IsSuccess = true
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Exception occurred while exporting data to Excel.");
                return new ExportResult
                {
                    IsSuccess = false,
                    ErrorMessage = "SQL Exception occurred while exporting data. Please try again later."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while exporting data to Excel.");
                return new ExportResult
                {
                    IsSuccess = false,
                    ErrorMessage = "An error occurred while exporting data. Please try again later."
                };
            }
        }

        private async Task<DataTable> GetDataFromStoredProcedureAsync(DateFilterViewModel model, string storedProcedureName, string connectionString)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(); 

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", model.FromDate);
                    command.Parameters.AddWithValue("@ToDate", model.ToDate);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable)); 
                    }
                }
            }

            return dataTable;
        }

        private void FormatExcelSheet(ExcelWorksheet worksheet, DataTable dataTable, bool isReport1, bool isReport2)
        {
            if (isReport1)
            {
                int[] dateColumns = { 6, 7, 8, 9, 10 }; 
                foreach (int colIndex in dateColumns)
                {
                    for (int rowIndex = 2; rowIndex <= dataTable.Rows.Count + 1; rowIndex++)
                    {
                        if (double.TryParse(worksheet.Cells[rowIndex, colIndex].Text, out double dateNumber))
                        {
                            DateTime date = DateTime.FromOADate(dateNumber);
                            worksheet.Cells[rowIndex, colIndex].Value = date;
                            worksheet.Cells[rowIndex, colIndex].Style.Numberformat.Format = "yyyy-MM-dd";
                        }
                    }
                }

                int assyCtColumnIndex = 11; // Adjust to your AssyCT column index
                int testCtColumnIndex = 12; // Adjust to your TestCT column index
                for (int rowIndex = 2; rowIndex <= dataTable.Rows.Count + 1; rowIndex++)
                {
                    worksheet.Cells[rowIndex, assyCtColumnIndex].Formula = $"G{rowIndex}-F{rowIndex}"; // G - F
                    worksheet.Cells[rowIndex, testCtColumnIndex].Formula = $"J{rowIndex}-H{rowIndex}"; // J - H
                }
            }

            if (isReport2)
            {
                int[] dateColumns = { 5 }; 
                foreach (int colIndex in dateColumns)
                {
                    for (int rowIndex = 2; rowIndex <= dataTable.Rows.Count + 1; rowIndex++)
                    {
                        if (double.TryParse(worksheet.Cells[rowIndex, colIndex].Text, out double dateNumber))
                        {
                            DateTime date = DateTime.FromOADate(dateNumber);
                            worksheet.Cells[rowIndex, colIndex].Value = date;
                            worksheet.Cells[rowIndex, colIndex].Style.Numberformat.Format = "yyyy-MM-dd";
                        }
                    }
                }
            }
        }
    }
}

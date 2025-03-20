using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using onsemi_shipping_summary_report.Models;

namespace onsemi_shipping_summary_report.Services
{
    public class ProductionOrderService
    {
        private readonly IConfiguration _configuration;

        public ProductionOrderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to get paginated production orders
        public List<ProductionOrder> GetProductionOrders(int pageNumber, int pageSize)
        {
            List<ProductionOrder> orders = new List<ProductionOrder>();
            string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int offset = (pageNumber - 1) * pageSize;

                using (SqlCommand command = new SqlCommand(
                    "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY id DESC) AS RowNum, * FROM CST_Gem_Lot_Dieprep) AS MyDerivedTable WHERE RowNum BETWEEN @StartRow AND @EndRow",
                    connection))
                {
                    command.Parameters.AddWithValue("@StartRow", offset + 1);
                    command.Parameters.AddWithValue("@EndRow", offset + pageSize);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new ProductionOrder
                        {
                            ID = !reader.IsDBNull(1) ? reader.GetInt64(1) : 0,
                            Lotnumber = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty,
                            OrderDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : System.DateTime.MinValue,
                            workorder = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty,
                            ATEC_WO = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty,
                            SO = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty
                        });
                    }
                }
            }
            return orders;
        }

        // Method to get the total record count
        public int GetTotalRecordCount()
        {
            int totalRecords = 0;
            string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM CST_Gem_Lot_Dieprep", connection))
                {
                    totalRecords = (int)command.ExecuteScalar();
                }
            }
            return totalRecords;
        }
    }
}

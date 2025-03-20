using Microsoft.Extensions.Configuration;
using onsemi_shipping_summary_report.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace onsemi_shipping_summary_report.Services
{
    public class WaferService
    {
        private readonly IConfiguration _configuration;

        public WaferService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<onsemi_wafermap> SearchWafermap(string device)
        {
            List<onsemi_wafermap> wafermapList = new List<onsemi_wafermap>();
            string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, FileName, FolderName, Wafermap, DateTransfered FROM tbl_WaferMaps WHERE FileName LIKE @device";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@device", "%" + device + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            onsemi_wafermap wafermap = new onsemi_wafermap
                            {
                                id = reader["id"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                FolderName = reader["FolderName"].ToString(),
                                Wafermap = reader["Wafermap"].ToString()
                            };

                            wafermapList.Add(wafermap);
                        }
                    }
                }
            }

            return wafermapList; // Return the list of wafer maps
        }
    }
}

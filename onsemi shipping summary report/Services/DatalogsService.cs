using Microsoft.Extensions.Configuration;
using onsemi_shipping_summary_report.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace onsemi_shipping_summary_report.Services
{
    public class DatalogsService
    {
        private readonly IConfiguration _configuration;

        public DatalogsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<datalogs> SearchDatalogs(string device)
        {
            List<datalogs> datalogsList = new List<datalogs>();
            //string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");
            string connectionString = "Server=MSDynamics-DB\\AXDB;Database=EMMS1;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, FileName, File1, File2, hasTransferred FROM ONSEMI_Datalogs_FTP_Transfer WHERE FileName LIKE @device";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@device", "%" + device + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            datalogs datalogs = new datalogs
                            {
                                id = reader["id"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                File1 = reader["File1"].ToString(),
                                File2 = reader["File2"].ToString(),
                                hasTransferred = reader["hasTransferred"].ToString()
                            };

                            datalogsList.Add(datalogs);
                        }
                    }
                }
            }

            return datalogsList; // Return the list of wafer maps
        }
    }
}

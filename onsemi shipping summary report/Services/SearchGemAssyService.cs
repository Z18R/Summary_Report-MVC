using Microsoft.Extensions.Configuration;
using onsemi_shipping_summary_report.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace onsemi_shipping_summary_report.Services
{
    public class SearchGemAssyService
    {
        private readonly IConfiguration _configuration;
        public SearchGemAssyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<GemLotAssy> SearchGemLots(string lotnumber)
        {
            List<GemLotAssy> search_GemLotsList = new List<GemLotAssy>();
            string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM CST_Gem_Lot WHERE lotnumber LIKE @lotnumber  ORDER BY ID DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lotnumber", "%" + lotnumber + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GemLotAssy search_GemLots = new GemLotAssy
                            {
                                Lotnumber = reader["Lotnumber"].ToString(),
                                Batchnumbering = reader["Batchnumbering"].ToString(),
                                Workordernumbering = reader["Workordernumbering"].ToString(),
                                workorder = reader["workorder"].ToString(),
                                ATEC_WO = reader["ATEC_WO"].ToString(),
                                SO_number = reader["SO#"].ToString(),
                                SoLine = reader["SoLine"].ToString()
                            };
                            search_GemLotsList.Add(search_GemLots);
                        }
                    }
                }
            }
            return search_GemLotsList;
        }

    }
}

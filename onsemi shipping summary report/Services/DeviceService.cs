using Microsoft.Extensions.Configuration;
using onsemi_shipping_summary_report.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace onsemi_shipping_summary_report.Services
{
    public class DeviceService
    {
        private readonly IConfiguration _configuration;

        public DeviceService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void InsertDevice(ViewModel model)
        {
            string connectionString = _configuration.GetConnectionString("MES_ATEC_Connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("usp_ONSEMI_SkipLot_InsertDevice", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Device", model.Device);
                    command.Parameters.AddWithValue("@Date_created", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
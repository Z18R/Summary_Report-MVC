using System;
using System.Data;
using System.Data.SqlClient;
using onsemi_shipping_summary_report.Models;
using Microsoft.Extensions.Configuration;
using onsemi_shipping_summary_report.Models;

namespace LotTrackingApp.Services
{
    public class LotTrackingService
    {
        private readonly string MES_ATEC_Connection;

        public LotTrackingService(IConfiguration configuration)
        {
            MES_ATEC_Connection = configuration.GetConnectionString("MES_ATEC_Connection");
        }

        public LotTracking GetLotDetails(string lotAlias)
        {
            LotTracking lotTracking = new LotTracking();

            using (SqlConnection conn = new SqlConnection(MES_ATEC_Connection))
            {
                using (SqlCommand cmd = new SqlCommand("usp_TRN_GetLotDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LotAlias", lotAlias);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            lotTracking.LotCode = reader["LotCode"].ToString();
                            lotTracking.LotAlias = reader["LotAlias"].ToString();
                            lotTracking.CustomerID = reader["CustomerID"].ToString();
                            lotTracking.ProductID = reader["ProductID"].ToString();
                            lotTracking.MaterialID = reader["MaterialID"].ToString();
                            lotTracking.FlowID = reader["FlowID"].ToString();
                            lotTracking.PackageID = reader["PackageID"].ToString();
                            lotTracking.LeadTypeID = reader["LeadTypeID"].ToString();
                            lotTracking.StatusID = reader["StatusID"].ToString();
                            lotTracking.StoreID = reader["StoreID"].ToString();
                            lotTracking.StageID = reader["StageID"].ToString();
                            lotTracking.RecipeID = reader["RecipeID"].ToString();
                            lotTracking.CurrentQty = reader["CurrentQty"] != DBNull.Value ? Convert.ToInt32(reader["CurrentQty"]) : (int?)null;
                            lotTracking.CurrentSequence = reader["CurrentSequence"] != DBNull.Value ? Convert.ToInt32(reader["CurrentSequence"]) : (int?)null;
                            lotTracking.StatusCode = reader["StatusCode"].ToString();
                            lotTracking.StageCode = reader["StageCode"].ToString();
                            lotTracking.CustomerCode = reader["CustomerCode"].ToString();
                            int lotCode = Convert.ToInt32(lotTracking.LotCode);
                            int currentQty = lotTracking.CurrentQty.GetValueOrDefault(0);
                            lotTracking.TotalSavedRejects = GetTotalSavedRejects(lotCode, currentQty);
                            lotTracking.TotalSavedAccounts = GetTotalSavedAccounts(lotCode, currentQty);
                            lotTracking.RecipeCode = reader["RecipeCode"].ToString();
                            lotTracking.DateCodeVerifiedBy = reader["DateCodeVerifiedBy"].ToString();
                            lotTracking.DateCode = reader["DateCode"].ToString();
                            lotTracking.POCode = reader["POCode"].ToString();
                        }
                    }
                }
            }

            return lotTracking;
        }

        private int GetTotalSavedRejects(int lotCode, int currentQty)
        {
            int totalRejects = 0;
            return totalRejects;
        }

        private int GetTotalSavedAccounts(int lotCode, int currentQty)
        {
            int totalAccounts = 0;
            return totalAccounts;
        }
    }
}

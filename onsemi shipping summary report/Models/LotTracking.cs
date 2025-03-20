namespace onsemi_shipping_summary_report.Models
{
    public class LotTracking
    {
        public string LotCode { get; set; }
        public string LotAlias { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public string MaterialID { get; set; }
        public string FlowID { get; set; }
        public string PackageID { get; set; }
        public string LeadTypeID { get; set; }
        public string StatusID { get; set; }
        public string StoreID { get; set; }
        public string StageID { get; set; }
        public string RecipeID { get; set; }
        public int? CurrentQty { get; set; } // Nullable int
        public int? CurrentSequence { get; set; } // Nullable int
        public string StatusCode { get; set; }
        public string StageCode { get; set; }
        public string CustomerCode { get; set; }
        public int? TotalSavedRejects { get; set; } // Nullable int
        public int? TotalSavedAccounts { get; set; } // Nullable int
        public string RecipeCode { get; set; }
        public string DateCodeVerifiedBy { get; set; }
        public string DateCode { get; set; }
        public string POCode { get; set; }

    }
}

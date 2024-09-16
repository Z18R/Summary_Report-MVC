namespace onsemi_shipping_summary_report.Models
{
    public class ProductionOrder
    {
        public long ID { get; set; }
        public string Lotnumber { get; set; }
        public DateTime OrderDate { get; set; }
            
        public string workorder { get; set; }

        public string ATEC_WO { get; set; }

        public string SO { get; set; }
    }
}

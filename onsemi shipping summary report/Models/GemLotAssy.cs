namespace onsemi_shipping_summary_report.Models
{
    public class GemLotAssy
    {

        public string ID { get; set; }

        public string Lotnumber { get; set; }

        public string Batchnumbering { get; set; }

        public string Workordernumbering { get; set; }

        public DateTime date { get; set; }

        public string workorder { get; set; }

        public string ATEC_WO { get; set; }

        public string SO_number { get; set; }

        public string SoLine { get; set; }
    }
}

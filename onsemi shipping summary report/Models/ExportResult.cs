namespace onsemi_shipping_summary_report.Models
{
    public class ExportResult
    {
        public MemoryStream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}

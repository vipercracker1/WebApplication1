namespace WebApplication1.Models
{
    public class PMAPIClient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? productBatchName { get; set; }
        public string? orderName { get; set; }
        public string? productionUnitName { get; set; }
        public string? partName { get; set; }
        public int? State { get; set; }
        public Dictionary<string, string> Descriptions { get; set; } = new Dictionary<string, string>();
    }
}

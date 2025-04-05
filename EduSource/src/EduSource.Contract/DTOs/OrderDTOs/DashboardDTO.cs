namespace EduSource.Contract.DTOs.OrderDTOs;

public static class DashboardDTO
{
    public class DataDTO
    {
        public string Name { get; set; }
        public int Sales {  get; set; }
        public int Revenue { get; set; }
    }

    public class MonthlyTargetDTO
    {
        public double Progress { get; set; }
        public double Target { get; set; }
        public int Revenue { get; set; }
        public int TodayRevenue { get; set; }
        public double GrowthPercentage { get; set; }
        public string Currency { get; set; }
        public int Comparison { get; set; }
    }
}

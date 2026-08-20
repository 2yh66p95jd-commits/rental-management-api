namespace RentalManagement.Api.DTOs;

public class DashboardSummaryDto
{
    public int TotalProperties { get; set; }
    public int OccupiedProperties { get; set; }
    public int AvailableProperties { get; set; }
    public decimal OccupancyRate { get; set; }
    public int ActiveLeases { get; set; }
    public decimal MonthlyRentalIncome { get; set; }
    public decimal PaymentsThisMonth { get; set; }
    public decimal OutstandingPayments { get; set; }
}

namespace RentalManagement.Api.DTOs;

public class LeaseCreateDto
{
    public int PropertyId { get; set; }
    public int TenantId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
}

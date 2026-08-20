namespace RentalManagement.Api.DTOs;

public class LeaseUpdateDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public string Status { get; set; } = "Active";
}

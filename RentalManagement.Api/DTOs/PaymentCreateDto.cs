namespace RentalManagement.Api.DTOs;

public class PaymentCreateDto
{
    public int LeaseId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}

namespace RentalManagement.Api.DTOs;

public class PaymentResponseDto
{
    public int Id { get; set; }
    public int LeaseId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

namespace RentalManagement.Api.DTOs;

public class PaymentUpdateDto
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = "Paid";
}

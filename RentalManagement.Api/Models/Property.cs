namespace RentalManagement.Api.Models;

public class Property
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public int Bedrooms { get; set; }
    public decimal MonthlyRent { get; set; }
    public bool IsAvailable { get; set; } = true;

    public ICollection<Lease> Leases { get; set; } = new List<Lease>();
}

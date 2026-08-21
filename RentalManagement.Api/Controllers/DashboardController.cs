using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Api.Data;

namespace RentalManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly RentalDbContext _context;

    public DashboardController(RentalDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalProperties = await _context.Properties.CountAsync();

        var occupiedProperties = await _context.Leases
            .Where(l => l.Status == "Active")
            .Select(l => l.PropertyId)
            .Distinct()
            .CountAsync();

        var availableProperties = totalProperties - occupiedProperties;

        var occupancyRate = totalProperties == 0
            ? 0
            : (decimal)occupiedProperties / totalProperties * 100;

        var activeLeases = await _context.Leases
            .Where(l => l.Status == "Active")
            .CountAsync();

        var monthlyRentalIncome = await _context.Leases
            .Where(l => l.Status == "Active")
            .SumAsync(l => l.MonthlyRent);

        var startOfMonth = new DateTime(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            1
        );

        var paymentsThisMonth = await _context.Payments
            .Where(p =>
                p.Status == "Paid" &&
                p.PaymentDate >= startOfMonth)
            .SumAsync(p => p.Amount);

        var outstandingPayments = await _context.Payments
            .Where(p => p.Status != "Paid")
            .SumAsync(p => p.Amount);

        return Ok(new
        {
            totalProperties,
            occupiedProperties,
            availableProperties,
            occupancyRate,
            activeLeases,
            monthlyRentalIncome,
            paymentsThisMonth,
            outstandingPayments
        });
    }
}

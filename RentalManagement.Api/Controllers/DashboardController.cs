using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Api.Data;
using RentalManagement.Api.DTOs;

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
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var totalProperties = await _context.Properties.CountAsync();

        var occupiedProperties = await _context.Properties
            .CountAsync(p => !p.IsAvailable);

        var availableProperties = totalProperties - occupiedProperties;

        var occupancyRate = totalProperties == 0
            ? 0
            : Math.Round((decimal)occupiedProperties / totalProperties * 100, 2);

        var activeLeases = await _context.Leases
            .CountAsync(l => l.Status == "Active");

        var monthlyRentalIncome = await _context.Leases
            .Where(l => l.Status == "Active")
            .SumAsync(l => (decimal?)l.MonthlyRent) ?? 0;

        var now = DateTime.UtcNow;

        var paymentsThisMonth = await _context.Payments
            .Where(p =>
                p.Status == "Paid" &&
                p.PaymentDate.Year == now.Year &&
                p.PaymentDate.Month == now.Month)
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        var outstandingPayments = Math.Max(
            monthlyRentalIncome - paymentsThisMonth,
            0);

        return Ok(new DashboardSummaryDto
        {
            TotalProperties = totalProperties,
            OccupiedProperties = occupiedProperties,
            AvailableProperties = availableProperties,
            OccupancyRate = occupancyRate,
            ActiveLeases = activeLeases,
            MonthlyRentalIncome = monthlyRentalIncome,
            PaymentsThisMonth = paymentsThisMonth,
            OutstandingPayments = outstandingPayments
        });
    }
}

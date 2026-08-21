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
public async Task<IActionResult> GetSummary()
{
    var totalProperties = await _context.Properties.CountAsync();

    var occupiedProperties = await _context.Properties
                .CountAsync(p => _context.Leases.Any(l =>
                    l.PropertyId == p.Id &&
                    l.Status == "Active"));

    var activeLeases = await _context.Leases
        .CountAsync(l => l.Status == "Active");

    var paymentsThisMonth = await _context.Payments
        .Where(p => p.Status == "Paid")
        .SumAsync(p => p.Amount);

    return Ok(new
    {
        totalProperties,
        occupiedProperties,
        activeLeases,
        paymentsThisMonth
    });
}
}

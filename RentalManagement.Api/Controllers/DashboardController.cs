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
        .CountAsync(p => !p.IsAvailable);

    var activeLeases = await _context.Leases
        .CountAsync(l => l.Status == "Active");

    return Ok(new
    {
        totalProperties,
        occupiedProperties,
        activeLeases
    });
}
}

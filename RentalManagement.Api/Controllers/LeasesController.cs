using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Api.Data;
using RentalManagement.Api.DTOs;
using RentalManagement.Api.Models;

namespace RentalManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeasesController : ControllerBase
{
    private readonly RentalDbContext _context;

    public LeasesController(RentalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaseResponseDto>>> GetLeases()
    {
        var leases = await _context.Leases
            .AsNoTracking()
            .Select(l => new LeaseResponseDto
            {
                Id = l.Id,
                PropertyId = l.PropertyId,
                TenantId = l.TenantId,
                PropertyAddress = l.Property.Address,
                TenantName = l.Tenant.FullName,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                MonthlyRent = l.MonthlyRent,
                Status = l.Status
            })
            .ToListAsync();

        return Ok(leases);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaseResponseDto>> GetLease(int id)
    {
        var lease = await _context.Leases
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new LeaseResponseDto
            {
                Id = l.Id,
                PropertyId = l.PropertyId,
                TenantId = l.TenantId,
                PropertyAddress = l.Property.Address,
                TenantName = l.Tenant.FullName,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                MonthlyRent = l.MonthlyRent,
                Status = l.Status
            })
            .FirstOrDefaultAsync();

        if (lease == null)
        {
            return NotFound();
        }

        return Ok(lease);
    }

    [HttpPost]
    public async Task<ActionResult<LeaseResponseDto>> CreateLease(LeaseCreateDto dto)
    {
        var property = await _context.Properties.FindAsync(dto.PropertyId);

        if (property == null)
            return BadRequest("Property does not exist.");

        var tenant = await _context.Tenants.FindAsync(dto.TenantId);

        if (tenant == null)
            return BadRequest("Tenant does not exist.");

        var existingActiveLease = await _context.Leases
            .AnyAsync(l =>
                l.PropertyId == dto.PropertyId &&
                l.Status == "Active");

        if (existingActiveLease)
            return BadRequest("Property already has an active lease.");

        if (!property.IsAvailable)
            return BadRequest("Property is not available.");

        if (dto.EndDate <= dto.StartDate)
            return BadRequest("End date must be after start date.");

        var lease = new Lease
        {
            PropertyId = dto.PropertyId,
            TenantId = dto.TenantId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            MonthlyRent = dto.MonthlyRent,
            Status = "Active"
        };

        property.IsAvailable = false;

        _context.Leases.Add(lease);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetLease),
            new { id = lease.Id },
            new LeaseResponseDto
            {
                Id = lease.Id,
                PropertyId = lease.PropertyId,
                TenantId = lease.TenantId,
                PropertyAddress = property.Address,
                TenantName = tenant.FullName,
                StartDate = lease.StartDate,
                EndDate = lease.EndDate,
                MonthlyRent = lease.MonthlyRent,
                Status = lease.Status
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLease(
        int id,
        LeaseUpdateDto dto)
    {
        var lease = await _context.Leases.FindAsync(id);

        if (lease == null)
            return NotFound();

        if (dto.EndDate <= dto.StartDate)
            return BadRequest("End date must be after start date.");

        lease.StartDate = dto.StartDate;
        lease.EndDate = dto.EndDate;
        lease.MonthlyRent = dto.MonthlyRent;
        lease.Status = dto.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLease(int id)
    {
        var lease = await _context.Leases.FindAsync(id);

        if (lease == null)
            return NotFound();

        var property = await _context.Properties.FindAsync(lease.PropertyId);

        if (property != null)
            property.IsAvailable = true;

        _context.Leases.Remove(lease);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

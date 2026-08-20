using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Api.Data;
using RentalManagement.Api.DTOs;
using RentalManagement.Api.Models;

namespace RentalManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly RentalDbContext _context;

    public TenantsController(RentalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantResponseDto>>> GetTenants()
    {
        var tenants = await _context.Tenants
            .AsNoTracking()
            .Select(t => new TenantResponseDto
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            })
            .ToListAsync();

        return Ok(tenants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TenantResponseDto>> GetTenant(int id)
    {
        var tenant = await _context.Tenants
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TenantResponseDto
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            })
            .FirstOrDefaultAsync();

        if (tenant == null)
        {
            return NotFound();
        }

        return Ok(tenant);
    }

    [HttpPost]
    public async Task<ActionResult<TenantResponseDto>> CreateTenant(TenantCreateDto dto)
    {
        var tenant = new Tenant
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        var response = new TenantResponseDto
        {
            Id = tenant.Id,
            FullName = tenant.FullName,
            Email = tenant.Email,
            PhoneNumber = tenant.PhoneNumber
        };

        return CreatedAtAction(
            nameof(GetTenant),
            new { id = tenant.Id },
            response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenant(
        int id,
        TenantUpdateDto dto)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        tenant.FullName = dto.FullName;
        tenant.Email = dto.Email;
        tenant.PhoneNumber = dto.PhoneNumber;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenant(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

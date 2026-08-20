using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Api.Data;
using RentalManagement.Api.DTOs;
using RentalManagement.Api.Models;

namespace RentalManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly RentalDbContext _context;

    public PropertiesController(RentalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetProperties()
    {
        var properties = await _context.Properties
            .AsNoTracking()
            .Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Address = p.Address,
                City = p.City,
                PropertyType = p.PropertyType,
                Bedrooms = p.Bedrooms,
                MonthlyRent = p.MonthlyRent,
                IsAvailable = p.IsAvailable
            })
            .ToListAsync();

        return Ok(properties);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyResponseDto>> GetProperty(int id)
    {
        var property = await _context.Properties
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Address = p.Address,
                City = p.City,
                PropertyType = p.PropertyType,
                Bedrooms = p.Bedrooms,
                MonthlyRent = p.MonthlyRent,
                IsAvailable = p.IsAvailable
            })
            .FirstOrDefaultAsync();

        if (property == null)
        {
            return NotFound();
        }

        return Ok(property);
    }

    [HttpPost]
    public async Task<ActionResult<PropertyResponseDto>> CreateProperty(
        PropertyCreateDto dto)
    {
        var property = new Property
        {
            Address = dto.Address,
            City = dto.City,
            PropertyType = dto.PropertyType,
            Bedrooms = dto.Bedrooms,
            MonthlyRent = dto.MonthlyRent,
            IsAvailable = true
        };

        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        var response = new PropertyResponseDto
        {
            Id = property.Id,
            Address = property.Address,
            City = property.City,
            PropertyType = property.PropertyType,
            Bedrooms = property.Bedrooms,
            MonthlyRent = property.MonthlyRent,
            IsAvailable = property.IsAvailable
        };

        return CreatedAtAction(
            nameof(GetProperty),
            new { id = property.Id },
            response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(
        int id,
        PropertyUpdateDto dto)
    {
        var property = await _context.Properties.FindAsync(id);

        if (property == null)
        {
            return NotFound();
        }

        property.Address = dto.Address;
        property.City = dto.City;
        property.PropertyType = dto.PropertyType;
        property.Bedrooms = dto.Bedrooms;
        property.MonthlyRent = dto.MonthlyRent;
        property.IsAvailable = dto.IsAvailable;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        var property = await _context.Properties.FindAsync(id);

        if (property == null)
        {
            return NotFound();
        }

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

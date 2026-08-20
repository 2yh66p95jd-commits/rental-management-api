using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Api.Data;
using RentalManagement.Api.DTOs;
using RentalManagement.Api.Models;

namespace RentalManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly RentalDbContext _context;

    public PaymentsController(RentalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetPayments()
    {
        var payments = await _context.Payments
            .AsNoTracking()
            .Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                LeaseId = p.LeaseId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Status = p.Status
            })
            .ToListAsync();

        return Ok(payments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentResponseDto>> GetPayment(int id)
    {
        var payment = await _context.Payments
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PaymentResponseDto
            {
                Id = p.Id,
                LeaseId = p.LeaseId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Status = p.Status
            })
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponseDto>> CreatePayment(
        PaymentCreateDto dto)
    {
        var lease = await _context.Leases.FindAsync(dto.LeaseId);

        if (lease == null)
        {
            return BadRequest("Lease does not exist.");
        }

        if (dto.Amount <= 0)
        {
            return BadRequest("Payment amount must be greater than zero.");
        }

        var payment = new Payment
        {
            LeaseId = dto.LeaseId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            Status = "Paid"
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var response = new PaymentResponseDto
        {
            Id = payment.Id,
            LeaseId = payment.LeaseId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            Status = payment.Status
        };

        return CreatedAtAction(
            nameof(GetPayment),
            new { id = payment.Id },
            response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(
        int id,
        PaymentUpdateDto dto)
    {
        var payment = await _context.Payments.FindAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        payment.Amount = dto.Amount;
        payment.PaymentDate = dto.PaymentDate;
        payment.Status = dto.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _context.Payments.FindAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

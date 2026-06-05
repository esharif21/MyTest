using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.Model.Entities;
using System;

[ApiController]
[Route("api/[controller]")]

public class CustomerController : ControllerBase
{
    private readonly ModelContext _context;

    public CustomerController(ModelContext context)
    {
        _context = context;
    }

    // =========================
    // GET: api/customer
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.Customers.ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET: api/customer/5
    // =========================
    [Authorize] // FOR TESTING AUTHORIZATION, ADDED ON GETBYID API
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(decimal id)
    {
        var data = await _context.Customers.FindAsync(id);

        if (data == null)
            return NotFound(new { message = "Customer not found" });

        return Ok(data);
    }

    // =========================
    // POST: api/customer
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        customer.Createddate = DateTime.Now;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Customer created successfully",
            data = customer
        });
    }

    // =========================
    // PUT: api/customer/5
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(decimal id, Customer customer)
    {
        var data = await _context.Customers.FindAsync(id);

        if (data == null)
            return NotFound(new { message = "Customer not found" });

        data.Name = customer.Name;
        data.Email = customer.Email;
        data.Mobile = customer.Mobile;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Customer updated successfully",
            data
        });
    }

    // =========================
    // DELETE: api/customer/5
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(decimal id)
    {
        var data = await _context.Customers.FindAsync(id);

        if (data == null)
            return NotFound(new { message = "Customer not found" });

        _context.Customers.Remove(data);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Customer deleted successfully"
        });
    }
}
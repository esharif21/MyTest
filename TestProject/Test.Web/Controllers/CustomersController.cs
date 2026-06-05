using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Test.Model;
using Test.Model.Entities;

public class CustomersController : Controller
{
    private readonly TestProjectContext _context;

    public CustomersController(TestProjectContext context)
    {
        _context = context;
    }

    // GET: Page
    public IActionResult Index()
    {
        return View();
    }

    // GET: All Customers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.Customers.ToListAsync();
        return Json(data);
    }

    // GET: Single
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var data = await _context.Customers.FindAsync(id);
        return Json(data);
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        customer.CreatedDate = DateTime.Now;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    // UPDATE
    [HttpPost]
    public async Task<IActionResult> Update(Customer customer)
    {
        var data = await _context.Customers.FindAsync(customer.Id);

        if (data == null)
            return Json(new { success = false });

        data.Name = customer.Name;
        data.Email = customer.Email;
        data.Mobile = customer.Mobile;

        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var data = await _context.Customers.FindAsync(id);

        if (data == null)
            return Json(new { success = false });

        _context.Customers.Remove(data);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }
}
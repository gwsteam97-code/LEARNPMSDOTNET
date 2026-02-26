using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // ✅ GET ALL SUPPLIERS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(suppliers);
    }

    // ✅ CREATE SUPPLIER
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Supplier supplier)
    {
        var result = await _supplierService.AddAsync(supplier);

        if (!result.success)
            return BadRequest(new
            {
                success = false,
                message = result.message
            });

        return Ok(new
        {
            success = true,
            message = result.message,
            data = result.data
        });
    }
}
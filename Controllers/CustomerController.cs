using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Common;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;
    private readonly ResponseService _response;

    public CustomerController(ICustomerService service, ResponseService response)
    {
        _service = service;
        _response = response;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(_response.SuccessResponse(data, "Customer list fetched"));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        var result = await _service.AddAsync(customer);
        return Ok(_response.SuccessResponse(result, "Customer added successfully"));
    }
}
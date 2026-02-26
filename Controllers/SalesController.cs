using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Common;
using PharmacyManagementSystem.DTOs;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _service;
    private readonly ResponseService _response;

    public SalesController(ISalesService service, ResponseService response)
    {
        _service = service;
        _response = response;
    }

    // ✅ CREATE SALE
    [HttpPost("create")]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto request)
    {
        if (request == null)
            return BadRequest(_response.FailResponse<string>("Invalid data"));

        var result = await _service.CreateSaleAsync(
            request.MedicineId,
            request.Quantity
        );

        if (!result.success)
            return BadRequest(_response.FailResponse<string>(result.message));

        return Ok(_response.SuccessResponse(result.data, result.message));
    }

    // ✅ GET SALES
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(_response.SuccessResponse(data, "Sales list fetched"));
    }
}
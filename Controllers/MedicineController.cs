using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Common;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services.Interfaces;

namespace PharmacyManagementSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicineController : ControllerBase
{
    private readonly IMedicineService _service;
    private readonly ResponseService _response;

    public MedicineController(IMedicineService service, ResponseService response)
    {
        _service = service;
        _response = response;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medicines = await _service.GetAllAsync();
        return Ok(_response.SuccessResponse(medicines, "Medicine list fetched"));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddMedicine([FromBody] Medicine medicine)
    {
        if (medicine == null)
            return BadRequest(_response.FailResponse<string>("Invalid data"));

        var result = await _service.AddAsync(medicine);

        // ✅ IF MEDICINE EXISTS
        if (!result.success)
        {
            return Ok(_response.FailResponse<string>(result.message));
        }

        return Ok(_response.SuccessResponse(result.data, result.message));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var medicine = await _service.GetByIdAsync(id);
        if (medicine == null)
            return NotFound(_response.FailResponse<string>("Medicine not found"));

        return Ok(_response.SuccessResponse(medicine, "Medicine fetched"));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Medicine medicine)
    {
        var result = await _service.AddAsync(medicine);
        return Ok(_response.SuccessResponse(result, "Medicine added successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Medicine medicine)
    {
        var updated = await _service.UpdateAsync(id, medicine);
        if (updated == null)
            return NotFound(_response.FailResponse<string>("Medicine not found"));

        return Ok(_response.SuccessResponse(updated, "Medicine updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(_response.FailResponse<string>("Medicine not found"));

        return Ok(_response.SuccessResponse<string>("Deleted", "Medicine deleted successfully"));
    }
}
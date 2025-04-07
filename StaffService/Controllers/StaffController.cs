using Microsoft.AspNetCore.Mvc;
using StaffService.Context;
using StaffService.HelperClass;
using StaffService.Models;

namespace StaffService.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly StaffDbContext _context;
    private readonly StaffServiceHandler _staffService;

    public StaffController(StaffDbContext context, StaffServiceHandler staffService)
    {
        _context = context;
        _staffService = staffService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterStaff([FromBody] StaffService.DTOs.StaffRegistrationDto dto)
    {
        await _staffService.RegisterStaffAsync(dto);
        return Ok("Staff registered and message sent.");

    }
}

using AppointmentService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly OpenTokService _openTokService;

    public AppointmentController(OpenTokService openTokService)
    {
        _openTokService = openTokService;
    }

    /// <summary>
    /// Creates a new OpenTok session and returns the sessionId and token.
    /// </summary>
    [HttpPost("create-room")]
    public IActionResult CreateRoom()
    {
        var (sessionId, token) = _openTokService.CreateVideoRoom();
        return Ok(new { sessionId, token });
    }

    /// <summary>
    /// Generates a new token for an existing sessionId.
    /// </summary>
    [HttpGet("token/{sessionId}")]
    public IActionResult GetToken(string sessionId)
    {  
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest("Session ID is required.");
        }

        var token = _openTokService.GenerateToken(sessionId);
        return Ok(new { token });
    }
}

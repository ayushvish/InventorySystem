using Inventory.Application.Common.Interfaces;
using Inventory.Shared;
using Inventory.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<ApiResponse<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        // For basic authentication, we validate hardcoded/seeded admin credentials
        if (request.Username == "admin" && request.Password == "Admin123")
        {
            var permissions = new[] { Permissions.Read, Permissions.Write, Permissions.Delete };
            var token = _jwtTokenGenerator.GenerateToken(
                userId: "1",
                userName: "admin",
                email: "admin@pharmacy.com",
                role: Roles.Admin,
                permissions: permissions);

            var response = new LoginResponse(token, "admin", "admin@pharmacy.com", Roles.Admin);
            return Ok(new ApiResponse<LoginResponse>(response, "Login successful."));
        }

        return Unauthorized(new ApiResponse<object>(StatusCodes.Status401Unauthorized, "Invalid credentials."));
    }
}

public record LoginRequest(string Username, string Password);
public record LoginResponse(string Token, string Username, string Email, string Role);

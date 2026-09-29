using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace demo_hello.Controllers;

[ApiController]
[Route("")]
public class AuthController : ControllerBase
{
    private const string UserName = "admin";
    private const string Password = "123456";

    private const string SecretKey =
        "MySecretKeyForJwtAuthentication123456789";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Kiểm tra username và password
        if (request.UserName != UserName ||
            request.Password != Password)
        {
            return Unauthorized(new
            {
                message = "Username hoặc password không đúng"
            });
        }

        // Thông tin lưu trong JWT
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName)
        };

        // Tạo khóa bí mật
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(SecretKey));

        // Cấu hình thuật toán ký JWT
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        // Tạo JWT
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        // Chuyển JWT thành chuỗi
        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Ok(new
        {
            userName = request.UserName,
            token = tokenString
        });
    }

    [HttpGet("auth")]
    public IActionResult Auth()
    {
        return Ok(new
        {
            message = "Token hợp lệ",
            userName = User.Identity?.Name
        });
    }
}

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
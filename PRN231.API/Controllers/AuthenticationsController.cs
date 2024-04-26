using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231.Repo.Interfaces;
using PRN231.Repo.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;


namespace PRN231.API.Controllers;

public class AuthenticationsController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthenticationsController(IUnitOfWork unitOfWork, IMapper mapper,IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(UserRoleViewModel userViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Error when input data");
        }

        var user = await _unitOfWork.UserRoleRepository.FindUserAsync(u => u.Username == userViewModel.Username && u.Passphrase == userViewModel.Passphrase);

        if (user == null)
        {
            return Unauthorized();
        }

        var role = "";

        switch (user.UserRole1)
        {
            case 1:
                role = "admin";
                break;
            case 2:
                role = "staff";
                break;
            case 3:
                role = "member";
                break;
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtToken = GenerateJwtToken(claims);
        return Ok(new { token = jwtToken });
    }
    
    private string GenerateJwtToken(List<Claim> claims)
    {
        string secretKey = _configuration.GetSection("JWTSection:SecretKey").Value;
        byte[] secretInBytes = Encoding.UTF8.GetBytes(secretKey);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretInBytes);
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration.GetSection("JWTSection:Issuer").Value,
            audience: _configuration.GetSection("JWTSection:Audience").Value,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("JWTSection:ExpiresInMinutes").Value)),
            claims: claims,
            signingCredentials: credentials
        );

        JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
        string jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }
}
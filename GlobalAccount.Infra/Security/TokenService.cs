using GlobalAccount.Infra.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public string? GenerateToken(string username, string password)
    {
        _logger.LogInformation("Tentando gerar token para usuário: {Username}", username);

        try
        {
            if (username != _jwtSettings.Username || password != _jwtSettings.Password)
            {
                _logger.LogWarning("Credenciais inválidas para usuário: {Username}", username);
                return null;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Token gerado com sucesso para usuário: {Username}", username);
            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar token para usuário: {Username}", username);
            return null;
        }
    }
}

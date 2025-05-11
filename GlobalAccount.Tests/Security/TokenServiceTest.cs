using GlobalAccount.Infra.Security;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Tests.Security
{
    public class TokenServiceTest
    {
        private readonly TokenService _tokenService;
        private const string ValidUsername = "admin";
        private const string ValidPassword = "123456";

        public TokenServiceTest()
        {
            var settings = Options.Create(new JwtSettings
            {
                Key = "yqyLSntmZenzsnzGr551qJxhIMPxI8fj",
                Issuer = "GlobalAccount",
                Audience = "GlobalAccountUsers",
                ExpireMinutes = 60,
                Username = ValidUsername,
                Password = ValidPassword
            });

            _tokenService = new TokenService(settings);
        }

        [Fact]
        public void Deve_Gerar_Token_Com_Credenciais_Validas()
        {
            var token = _tokenService.GenerateToken(ValidUsername, ValidPassword);
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void Nao_Deve_Gerar_Token_Com_Username_Invalido()
        {
            var token = _tokenService.GenerateToken("usuarioInvalido", ValidPassword);
            Assert.Null(token);
        }

        [Fact]
        public void Nao_Deve_Gerar_Token_Com_Senha_Invalida()
        {
            var token = _tokenService.GenerateToken(ValidUsername, "senhaIncorreta");
            Assert.Null(token);
        }

        [Fact]
        public void Token_Deve_Conter_Claim_Name_Com_Username()
        {
            var tokenString = _tokenService.GenerateToken(ValidUsername, ValidPassword);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var nameClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);
            Assert.NotNull(nameClaim);
            Assert.Equal(ValidUsername, nameClaim.Value);
        }

        [Fact]
        public void Token_Deve_Conter_Claim_Jti()
        {
            var tokenString = _tokenService.GenerateToken(ValidUsername, ValidPassword);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var jti = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            Assert.NotNull(jti);
            Assert.False(string.IsNullOrWhiteSpace(jti.Value));
        }
    }
}
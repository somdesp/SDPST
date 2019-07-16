using Domain.Entity;
using Infra.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        #region Declaraçoes
        private readonly AppSettings _appSettings;
        private TokenConfigurations _tokenConfigurations;
        public IConfiguration _configuration { get; }
        private IAuthRepository _authRepository;
        #endregion

        #region Construtor 
        public AuthService(IOptions<AppSettings> appSettings,
            IConfiguration configuration,
            TokenConfigurations tokenConfigurations,
            IAuthRepository authRepository)
        {
            _tokenConfigurations = tokenConfigurations;
            _configuration = configuration;
            _appSettings = appSettings.Value;
            _authRepository = authRepository;
        }
        #endregion

        #region Metodo valida e gera token
        public async Task<Token> Authenticate(User user)
        {

            return await Task.Run(() =>
            {
                try
                {
                  var authUsu = _authRepository.AuthUser(user);

                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Name, "Name"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    });


                    var keyByteArray = Encoding.ASCII.GetBytes("F3FF60788F2A1A49B292255CE98455C58F8E7CC3770DFF68B97D7F78252D27BB2B74DCC6E63A99E06A39EA99D7C982E0D2D2366776236ED6134A802EC133A8AF");
                    var signingKey = new SymmetricSecurityKey(keyByteArray);

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao + TimeSpan.FromDays(_tokenConfigurations.Days);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = _tokenConfigurations.Issuer,
                        Audience = _tokenConfigurations.Audience,
                        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });
                    var token = handler.WriteToken(securityToken);
                    Token valToken = new Token
                    {
                        Authenticated = true,
                        Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        AccessToken = token,
                        Message = "OK"
                    };
                    return valToken;


                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            });
        }
        #endregion

    }
}

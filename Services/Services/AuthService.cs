using Domain.Entity;
using Domain.ViewModel;
using Infra.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Auth.Service.Interface;
using Services.Auth.Service.Service;
using Services.Helpers;
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
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly AppSettings _appSettings;
        private TokenConfigurations _tokenConfigurations;
        public IConfiguration _configuration { get; }
        private IAuthRepository _authRepository;
        private Encryption encryption = new Encryption();

        #endregion

        #region Construtor 
        public AuthService(AppSettings appSettings,
            IConfiguration configuration,
            TokenConfigurations tokenConfigurations,
            IAuthRepository authRepository, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _tokenConfigurations = tokenConfigurations;
            _configuration = configuration;
            _appSettings = appSettings;
            _authRepository = authRepository;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }
        #endregion

        #region Metodo valida e gera token
        public async Task<Token> Authenticate(LoginViewModel user)
        {

            return await Task.Run(() =>
            {
                try
                {
                    //Criptografia da senha com a chave
                    user.Password = encryption.HashHmac(user.Email + "OPTIMUS@@ECM", user.Password);

                    //verifica no banco
                    var authUsu = _authRepository.AuthUserAsync(user);


                    //Valida se existe usuarios no banco
                    if (authUsu == null) return null;

                    //Cria as regras do usuario conforme seus acessos
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(authUsu.Name, "Login"),
                    new[] {
                        new Claim(ClaimTypes.Hash, Guid.NewGuid().ToString("N")),
                        new Claim(ClaimTypes.Name, authUsu.Name),
                        new Claim(ClaimTypes.Email, authUsu.Email),
                        new Claim(ClaimTypes.NameIdentifier, authUsu.Id.ToString()),
                        new Claim(ClaimTypes.Role, "CRIAR_USUARIO"),
                        new Claim ("Tipo_Tecnico", "Serralheiro")
                    }); ;

                    //Gera a chave conforme o token de retorno das configuraçoes
                    var keyByteArray = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var signingKey = new SymmetricSecurityKey(keyByteArray);

                    //Configura o token 
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
                catch (Exception ex)
                {
                    return null;
                    throw;
                }
            });
        }
        #endregion

        public async Task<ClaimsIdentity> GetClaimsIdentity(LoginViewModel user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                    return await Task.FromResult<ClaimsIdentity>(null);

                user.Password = encryption.HashHmac(user.Email + "OPTIMUS@@ECM", user.Password);

                // get the user to verifty
                var userToVerify = _authRepository.AuthUserAsync(user);

                if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);


                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(user.Email, userToVerify.Id.ToString()));

            }
            catch (Exception ex)
            {

                throw;
            }



        }      

    }
}

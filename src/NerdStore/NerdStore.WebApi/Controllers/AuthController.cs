using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Core.WebApi.Controllers;
using NerdStore.WebApi.Extencoes;

namespace NerdStore.WebApi.Controllers
{
    [Route("Auth")]
    public class AuthController : BaseController
    {
        private readonly AppSettings _appSettings;

        public AuthController(IOptions<AppSettings> appSettings,
            INotificationHandler<DomainNotificaton> domainNotificationHandler,
            IMediatrHandler mediatrHandler) : base(domainNotificationHandler, mediatrHandler)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost("nova-conta")]
        public IActionResult Registrar(string email, string senha)
        {
            return RespostaPersonalizada(GerarToken(email));
        }


        [Authorize]
        [HttpPost("login")]
        public IActionResult EfetuarLogin(string email)
        {
            var tesate = User;

            return RespostaPersonalizada(
                User.Identity.Name
            );
        }


        private string GerarToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoEmHoras),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, email),
                    new(ClaimTypes.Email, email),
                    new(ClaimTypes.Sid, email)
                })
            });

            var encoded = tokenHandler.WriteToken(token);
            return encoded;
        }
    }
}
using ApiCubosSegundoPractico.Helpers;
using ApiCubosSegundoPractico.Models;
using ApiCubosSegundoPractico.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiCubosSegundoPractico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryCubo repo;
        private HelperActionServicesOauth helper;
        public AuthController(RepositoryCubo repo, HelperActionServicesOauth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(Login login)
        {
            Usuario user = await this.repo.Login(login.Email,login.Pass);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken
                    (
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                    );
                return Ok
                    (
                        new
                        {
                            response = new JwtSecurityTokenHandler().WriteToken(token)
                        }
                    );
            }
        }
    }
}

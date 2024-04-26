using ApiCubosSegundoPractico.Models;
using ApiCubosSegundoPractico.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCubosSegundoPractico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuboController : ControllerBase
    {
        private RepositoryCubo repo;
        public CuboController(RepositoryCubo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubos();
        }

        [HttpGet]
        [Route("[action]/{marca}")]
        public async Task<ActionResult<List<Cubo>>> GetCubosMarca(string marca)
        {
            return await this.repo.GetCubosMarca(marca);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateUsuario(Usuario user)
        {
            await this.repo.CreateUsuario(user.IdUsuario,user.Nombre,user.Email,user.Pass,user.Imagen);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<Usuario>> PerfilUsuario(int idusuario)
        {
            return await this.repo.PerfilUsuario(idusuario);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<List<Compra>>> GetComprasUsuario(int idusuario)
        {
            return await this.repo.GetComprasUsuario(idusuario);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateCompraUsuario(Compra compra)
        {
            await this.repo.CreateCompraUsuario(compra.IdPedido,compra.IdCubo,compra.IdUsuario,compra.Fecha);
            return Ok();
        }
    }
}

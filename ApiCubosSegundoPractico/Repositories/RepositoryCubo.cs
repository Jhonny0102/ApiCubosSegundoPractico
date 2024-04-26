using ApiCubosSegundoPractico.Data;
using ApiCubosSegundoPractico.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosSegundoPractico.Repositories
{
    public class RepositoryCubo
    {
        private CuboContext context;
        public RepositoryCubo(CuboContext context)
        {
            this.context = context;
        }

        //Metodo para mostrar todos los cubos
        public async Task<List<Cubo>> GetCubos()
        {
            return await this.context.Cubos.ToListAsync();
        }

        //Metodo para buscar cubos por marca
        public async Task<List<Cubo>> GetCubosMarca(string marca)
        {
            return await this.context.Cubos.Where(z => z.Marca == marca).ToListAsync();
        }

        //Metodo para crear nuevo usuario
        public async Task CreateUsuario(int idusuario, string nombre , string email , string pass , string imagen)
        {
            Usuario user = new Usuario();
            user.IdUsuario = idusuario;
            user.Nombre = nombre;
            user.Email = email;
            user.Pass = pass;
            user.Imagen = imagen;
            await this.context.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        //*** TOKEN ***//
        //**  ||||| ***//
        //*** VVVVV ***//

        //Metodo para ver el perfilusuario(FIND)
        public async Task<Usuario> PerfilUsuario(int idusuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(z => z.IdUsuario == idusuario);
        }

        //Metodo para ver los pedidos de un usuario
        public async Task<List<Compra>> GetComprasUsuario(int idusuario)
        {
            return await this.context.Compras.Where(z => z.IdUsuario == idusuario).ToListAsync();
        } 

        //Metodo para relizar pedido de un usuario
        public async Task CreateCompraUsuario(int idpedido , int idcubo , int idusuario , DateTime fechapedido)
        {
            Compra compra = new Compra();
            compra.IdPedido = idpedido;
            compra.IdCubo = idcubo;
            compra.IdUsuario = idusuario;
            compra.Fecha = fechapedido;
            await this.context.Compras.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }

        //Metodo para buscar usuario por su email y pass
        public async Task<Usuario> Login(string email , string pass)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(z => z.Email == email && z.Pass == pass);
        }
    }
}

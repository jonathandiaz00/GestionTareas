using Dapper;
using GestionTareas;
using Microsoft.AspNetCore.Mvc;
using Tarea.Api.Data;
using System.Threading.Tasks;

namespace Tarea.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = "SELECT * FROM Usuario";
            using var connection = _context.CreateConnection();
            var usuarios = await connection.QueryAsync<Usuario>(query);
            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = "SELECT * FROM Usuario WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var usuario = await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Id = id });

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            var query = @"
                INSERT INTO Usuario (Nombre, Correo, Contrasenia, Rol)
                VALUES (@Nombre, @Correo, @Contrasenia, @Rol)";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, usuario);
            return Ok(new { mensaje = "Usuario creado", filas = result });
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            var query = @"
                UPDATE Usuario 
                SET Nombre = @Nombre, Correo = @Correo, Contrasenia = @Contrasenia, Rol = @Rol 
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new
            {
                usuario.Nombre,
                usuario.Correo,
                usuario.Contrasenia,
                usuario.Rol,
                Id = id
            });

            return Ok(new { mensaje = "Usuario actualizado", filas = result });
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = "DELETE FROM Usuario WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = id });
            return Ok(new { mensaje = "Usuario eliminado", filas = result });
        }
    }
}

using Dapper;
using GestionTareas;
using Microsoft.AspNetCore.Mvc;
using Tarea.Api.Data;

namespace Tarea.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProyectosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Proyectos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = "SELECT * FROM Proyecto";
            using var connection = _context.CreateConnection();
            var proyectos = await connection.QueryAsync<Proyecto>(query);
            return Ok(proyectos);
        }

        // GET: api/Proyectos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = "SELECT * FROM Proyecto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var proyecto = await connection.QuerySingleOrDefaultAsync<Proyecto>(query, new { Id = id });

            if (proyecto == null)
                return NotFound();

            return Ok(proyecto);
        }

        // POST: api/Proyectos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Proyecto proyecto)
        {
            var query = @"
                INSERT INTO Proyecto (Nombre, Descripcion, FechaInicio, FechaFin)
                VALUES (@Nombre, @Descripcion, @FechaInicio, @FechaFin)";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, proyecto);
            return Ok(new { mensaje = "Proyecto creado", filas = result });
        }

        // PUT: api/Proyectos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Proyecto proyecto)
        {
            var query = @"
                UPDATE Proyecto 
                SET Nombre = @Nombre, Descripcion = @Descripcion, FechaInicio = @FechaInicio, FechaFin = @FechaFin
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new
            {
                proyecto.Nombre,
                proyecto.Descripcion,
                proyecto.FechaInicio,
                proyecto.FechaFin,
                Id = id
            });

            return Ok(new { mensaje = "Proyecto actualizado", filas = result });
        }

        // DELETE: api/Proyectos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = "DELETE FROM Proyecto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = id });
            return Ok(new { mensaje = "Proyecto eliminado", filas = result });
        }
    }
}

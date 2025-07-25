using Dapper;
using GestionTareas;
using Microsoft.AspNetCore.Mvc;
using Tarea.Api.Data;

namespace Tarea.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reporte
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = "SELECT * FROM Reporte";
            using var connection = _context.CreateConnection();
            var reporte = await connection.QueryAsync<Reporte>(query);
            return Ok(reporte);
        }

        // GET: api/Reporte/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = "SELECT * FROM Reporte WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var reporte = await connection.QuerySingleOrDefaultAsync<Reporte>(query, new { Id = id });

            if (reporte == null)
                return NotFound();

            return Ok(reporte);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reporte reporte)
        {
            var query = @"
                INSERT INTO Reporte (FechaGeneracion)";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, reporte);
            return Ok(new { mensaje = "Reporte creado", filas = result });
        }

        // PUT: api/Reporte/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Reporte reporte)
        {
            var query = @"
                UPDATE Reporte 
                SET Nombre = @FechaGeneracion
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new
            {
                reporte.FechaGeneracion,
                Id = id
            });

            return Ok(new { mensaje = "Reporte actualizado", filas = result });
        }

        // DELETE: api/Reporte/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = "DELETE FROM Reporte WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = id });
            return Ok(new { mensaje = "Reporte eliminado", filas = result });
        }
    }
}

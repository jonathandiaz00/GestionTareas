using Dapper;
using GestionTareas;
using Microsoft.AspNetCore.Mvc;
using Tarea.Api.Data;

namespace Tarea.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaProyectosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TareaProyectosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TareaProyectos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = "SELECT * FROM TareaProyecto";
            using var connection = _context.CreateConnection();
            var tareas = await connection.QueryAsync<TareaProyecto>(query);
            return Ok(tareas);
        }

        // GET: api/TareaProyectos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = "SELECT * FROM TareaProyecto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var tarea = await connection.QuerySingleOrDefaultAsync<TareaProyecto>(query, new { Id = id });

            if (tarea == null)
                return NotFound();

            return Ok(tarea);
        }

        // GET: api/TareaProyectos/detallado
        [HttpGet("detallado")]
        public async Task<IActionResult> GetAllConProyectoYUsuario()
        {
            var query = @"
                SELECT tp.Id, tp.Titulo, tp.Descripcion, tp.Estado, tp.Prioridad, tp.FechaVencimiento,
                       tp.ProyectoId, tp.UsuarioAsignadoId,
                       p.Id, p.Nombre, 
                       u.Id, u.Nombre
                FROM TareaProyecto tp
                INNER JOIN Proyecto p ON tp.ProyectoId = p.Id
                INNER JOIN Usuario u ON tp.UsuarioAsignadoId = u.Id";

            using var connection = _context.CreateConnection();

            var tareaDict = new Dictionary<int, TareaProyecto>();

            var result = await connection.QueryAsync<TareaProyecto, Proyecto, Usuario, TareaProyecto>(
                query,
                (tarea, proyecto, usuario) =>
                {
                    if (!tareaDict.TryGetValue(tarea.Id, out var current))
                    {
                        current = tarea;
                        tareaDict.Add(current.Id, current);
                    }

                    current.Proyecto = proyecto;
                    current.UsuarioAsignado = usuario;

                    return current;
                },
                splitOn: "Id,Id");

            return Ok(result.Distinct());
        }

        // POST: api/TareaProyectos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TareaProyecto tarea)
        {
            var query = @"
                INSERT INTO TareaProyecto 
                (Titulo, Descripcion, Estado, Prioridad, FechaVencimiento, ProyectoId, UsuarioAsignadoId)
                VALUES 
                (@Titulo, @Descripcion, @Estado, @Prioridad, @FechaVencimiento, @ProyectoId, @UsuarioAsignadoId)";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, tarea);
            return Ok(new { mensaje = "Tarea creada", filas = result });
        }

        // PUT: api/TareaProyectos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TareaProyecto tarea)
        {
            var query = @"
                UPDATE TareaProyecto 
                SET Titulo = @Titulo,
                    Descripcion = @Descripcion,
                    Estado = @Estado,
                    Prioridad = @Prioridad,
                    FechaVencimiento = @FechaVencimiento,
                    ProyectoId = @ProyectoId,
                    UsuarioAsignadoId = @UsuarioAsignadoId
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new
            {
                tarea.Titulo,
                tarea.Descripcion,
                tarea.Estado,
                tarea.Prioridad,
                tarea.FechaVencimiento,
                tarea.ProyectoId,
                tarea.UsuarioAsignadoId,
                Id = id
            });

            return Ok(new { mensaje = "Tarea actualizada", filas = result });
        }

        // DELETE: api/TareaProyectos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = "DELETE FROM TareaProyecto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = id });
            return Ok(new { mensaje = "Tarea eliminada", filas = result });
        }
    }
}

using Dapper;
using GestionTareas;
using Microsoft.Data.SqlClient;


namespace Tareas.Mvc.Services
{
    public class UsuarioService
    {
        private readonly IConfiguration _config;

        public UsuarioService(IConfiguration config)
        {
            _config = config;
        }

        public async Task RegistrarUsuarioAsync(Usuario usuario)
        {
            var sql = @"INSERT INTO Usuario (Nombre, Correo, Contrasenia, Rol)
                        VALUES (@Nombre, @Correo, @Contrasenia, @Rol)";

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync(sql, usuario);
        }
    }
}

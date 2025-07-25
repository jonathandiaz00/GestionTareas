using GestionTareas;
using Microsoft.AspNetCore.Mvc;

using Tareas.Mvc.Services;

namespace Tareas.Mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario usuario)
        {
            usuario.Rol ??= "User";
            await _usuarioService.RegistrarUsuarioAsync(usuario);
            return RedirectToAction("Index", "Home");
        }
    }
}


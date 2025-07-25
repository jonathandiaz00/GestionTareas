using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas
{
    public class TareaProyecto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } 
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "Pendiente";  
        public string Prioridad { get; set; } = "Media";  
        public DateTime FechaVencimiento { get; set; }
        public int ProyectoId { get; set; }
        public int UsuarioAsignadoId { get; set; }

        

        public Proyecto? Proyecto { get; set; }
        public Usuario? UsuarioAsignado { get; set; }

    }
}

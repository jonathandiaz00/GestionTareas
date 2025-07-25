using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionTareas
{
    public class Reporte
    {
        public int Id { get; set; } 
        public DateTime FechaGeneracion { get; set; }

        public List<TareaProyecto>? Tareas { get; set; } 
    }
}

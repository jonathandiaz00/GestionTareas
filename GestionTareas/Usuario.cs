﻿namespace GestionTareas
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } 
        public string Correo { get; set; } 
        public string Contrasenia { get; set; } 
        public string Rol { get; set; } = "Usuario"; 
    }
}

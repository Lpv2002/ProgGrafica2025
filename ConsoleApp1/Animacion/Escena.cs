using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Escena
    {
        public string Nombre { get; set; }
        public List<Accion> Acciones { get; set; }
        public long TiempoDeDuracion { get; set; }

        public Escena()
        {
            Acciones = new List<Accion>();
            TiempoDeDuracion = 0;
            Nombre = "";
        }

        public Escena(string nombre, List<Accion> lista)
        {
            this.Nombre = nombre;
            Acciones = lista;
            TiempoDeDuracion = 0;
            foreach (var accion in lista)
                TiempoDeDuracion += (accion.tiempoF - accion.tiempoI + 1);
        }

        public Accion GetAccion(int i)
        {
            return Acciones.ElementAt(i);
        }

        public int GetCantidad()
        {
            return Acciones.Count;
        }
    }
}
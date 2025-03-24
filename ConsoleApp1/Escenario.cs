using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Escenario
    {
        public Dictionary<string, Objeto> objetos;
        private float originX, originY, originZ;

        public Escenario(float originX, float originY, float originZ)
        {
            this.originX = originX;
            this.originY = originY;
            this.originZ = originZ;
            objetos = new Dictionary<string, Objeto>();
        }

        public void AgregarObjeto(string nombre, Objeto objeto)
        {
            objetos.Add(nombre, objeto);
        }

        public void DeleteObjeto(string nombre)
        {
            objetos.Remove(nombre);
        }

        public void Dibujar()
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Dibujar(originX, originY, originZ);
            }
        }
    }
}
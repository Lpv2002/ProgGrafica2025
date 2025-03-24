using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Objeto
    {
        public Dictionary<string, Parte> partes;
        private float originX, originY, originZ;

        public Objeto(float originX, float originY, float originZ)
        {
            this.originX = originX;
            this.originY = originY;
            this.originZ = originZ;
            partes = new Dictionary<string, Parte>();
        }

        public void AgregarParte(string nombre, Parte parte)
        {
            partes.Add(nombre, parte);
        }

        public void Dibujar(float offsetX, float offsetY, float offsetZ)
        {
            foreach (Parte parte in partes.Values)
            {
                parte.Dibujar(originX + offsetX, originY + offsetY, originZ + offsetZ);
            }
        }
    }
}
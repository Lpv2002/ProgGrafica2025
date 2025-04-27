
using OpenTK.Graphics.OpenGL;
using System;
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

        public Objeto get(string nombreobjeto)
        {
            if (objetos.ContainsKey(nombreobjeto))
            {
                return objetos[nombreobjeto];
            }
            else
            {
                throw new Exception($"La parte {nombreobjeto} no existe en este objeto.");
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Trasladar(x, y, z);
            }
        }
        public void Escalar(float n)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Escalar(n);
            }
        }
        public void Rotar(string eje, float angulo)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Rotar(eje, angulo);
            }
        }
        public void Dibujar()
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Trasladar(originX, originY, originZ);
                objeto.Dibujar();
            }
        }
    }
}
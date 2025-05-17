using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Escenario
    {
        public Dictionary<string, Objeto> objetos;
        private float originX, originY, originZ;

        // Variables para almacenar rotaciones del escenario
        private Vertice RotacionGlobal;

        public Escenario(float originX, float originY, float originZ)
        {
            this.originX = originX;
            this.originY = originY;
            this.originZ = originZ;
            objetos = new Dictionary<string, Objeto>();
            RotacionGlobal = new Vertice(0, 0, 0);
        }

        public void AgregarObjeto(string nombre, Objeto objeto)
        {
            objetos.Add(nombre, objeto);
        }

        public Objeto ObtenerObjeto(string nombreobjeto)
        {
            if (objetos.ContainsKey(nombreobjeto))
            {
                return objetos[nombreobjeto];
            }
            else
            {
                throw new Exception($"El objeto {nombreobjeto} no existe en este escenario.");
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
            if (eje == "x")
                RotacionGlobal.X += angulo;
            else if (eje == "y")
                RotacionGlobal.Y += angulo;
            else if (eje == "z")
                RotacionGlobal.Z += angulo;

            foreach (Objeto objeto in objetos.Values)
            {
                objeto.RotarE(eje, angulo);
            }
        }

        private void AplicarTransformacionesGlobales()
        {
            // Aplicar rotación global
            GL.Rotate(RotacionGlobal.X, 1, 0, 0);
            GL.Rotate(RotacionGlobal.Y, 0, 1, 0);
            GL.Rotate(RotacionGlobal.Z, 0, 0, 1);

            // Aplicar traslación al origen del escenario
            GL.Translate(originX, originY, originZ);
        }

        public void Dibujar()
        {
            GL.PushMatrix();

            // Aplicar transformaciones globales del escenario
            AplicarTransformacionesGlobales();

            // Dibujar todos los objetos
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Dibujar();
            }

            GL.PopMatrix();
        }
    }
}
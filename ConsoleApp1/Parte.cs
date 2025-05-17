using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    public class Parte
    {
        public Vertice CentroDeMasa { get; set; }
        public Dictionary<string, Cara> caras;

        // Variables para almacenar rotaciones
        private Vertice RotacionCM;
        private Vertice RotacionO;
        private Vertice RotacionE;

        public Parte(Vertice centroDeMasa)
        {
            caras = new Dictionary<string, Cara>();
            CentroDeMasa = centroDeMasa;
            RotacionCM = new Vertice(0, 0, 0);
            RotacionO = new Vertice(0, 0, 0);
            RotacionE = new Vertice(0, 0, 0);
        }

        public void AgregarCara(string nombre, Cara cara)
        {
            caras.Add(nombre, cara);
        }

        public Cara ObtenerCara(string nombre)
        {
            if (caras.ContainsKey(nombre))
            {
                return caras[nombre];
            }
            else
            {
                throw new Exception($"La cara {nombre} no existe en esta parte.");
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            foreach (Cara cara in caras.Values)
            {
                cara.Trasladar(x, y, z);
            }
        }

        public void Escalar(float n)
        {
            foreach (Cara cara in caras.Values)
            {
                cara.Escalar(n);
            }
        }

        // Rotación respecto al centro de masa
        public void Rotar(string eje, float angulo)
        {
            if (eje == "x")
                RotacionCM.X += angulo;
            else if (eje == "y")
                RotacionCM.Y += angulo;
            else if (eje == "z")
                RotacionCM.Z += angulo;

            foreach (Cara cara in caras.Values)
            {
                cara.Rotar(eje, angulo);
            }
        }

        // Rotación respecto al origen
        public void RotarO(string eje, float angulo)
        {
            if (eje == "x")
                RotacionO.X += angulo;
            else if (eje == "y")
                RotacionO.Y += angulo;
            else if (eje == "z")
                RotacionO.Z += angulo;

            foreach (Cara cara in caras.Values)
            {
                cara.RotarO(eje, angulo);
            }
        }

        // Rotación respecto al escenario
        public void RotarE(string eje, float angulo)
        {
            if (eje == "x")
                RotacionE.X += angulo;
            else if (eje == "y")
                RotacionE.Y += angulo;
            else if (eje == "z")
                RotacionE.Z += angulo;

            foreach (Cara cara in caras.Values)
            {
                cara.RotarE(eje, angulo);
            }
        }

        private void AplicarTransformaciones()
        {
            // Aplicar rotación de escenario
            Rotar(new Vertice(0, 0, 0), RotacionE);

            // Aplicar rotación de origen
            Rotar(CentroDeMasa, RotacionO);

            // Aplicar rotación de centro de masa
            Rotar(CentroDeMasa, RotacionCM);
        }

        private void Rotar(Vertice origen, Vertice rotacion)
        {
            GL.Translate(origen.X, origen.Y, origen.Z);
            GL.Rotate(rotacion.X, 1, 0, 0);
            GL.Rotate(rotacion.Y, 0, 1, 0);
            GL.Rotate(rotacion.Z, 0, 0, 1);
            GL.Translate(-origen.X, -origen.Y, -origen.Z);
        }

        public void Dibujar()
        {
            GL.PushMatrix();

            // Aplicar transformaciones a nivel de parte
            AplicarTransformaciones();

            // Trasladar al centro de masa
            GL.Translate(CentroDeMasa.X, CentroDeMasa.Y, CentroDeMasa.Z);

            // Dibujar todas las caras
            foreach (Cara cara in caras.Values)
            {
                cara.Draw();
            }

            GL.PopMatrix();
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Objeto
    {
        public Vertice CentroDeMasa { get; set; }
        public Dictionary<string, Parte> partes;

        // Variables para almacenar rotaciones
        private Vertice RotacionCM;
        private Vertice RotacionO;
        private Vertice RotacionE;

        public Objeto(Vertice centro, Dictionary<string, Parte> partes)
        {
            CentroDeMasa = centro;
            this.partes = partes ?? new Dictionary<string, Parte>();
            RotacionCM = new Vertice(0, 0, 0);
            RotacionO = new Vertice(0, 0, 0);
            RotacionE = new Vertice(0, 0, 0);
        }

        public void AgregarParte(string nombre, Parte parte)
        {
            partes.Add(nombre, parte);
        }

        public Parte ObtenerParte(string nombreParte)
        {
            if (partes.ContainsKey(nombreParte))
            {
                return partes[nombreParte];
            }
            else
            {
                throw new Exception($"La parte {nombreParte} no existe en este objeto.");
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            // Actualizar el centro de masa del objeto
            CentroDeMasa.X += x;
            CentroDeMasa.Y += y;
            CentroDeMasa.Z += z;

            // Trasladar todas las partes
            foreach (Parte parte in partes.Values)
            {
                parte.Trasladar(x, y, z);
            }
        }

        public void Escalar(float n)
        {
            foreach (Parte parte in partes.Values)
            {
                parte.Escalar(n);
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

            foreach (Parte parte in partes.Values)
            {
                parte.Rotar(eje, angulo);
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

            foreach (Parte parte in partes.Values)
            {
                parte.RotarO(eje, angulo);
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

            foreach (Parte parte in partes.Values)
            {
                parte.RotarE(eje, angulo);
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
            OpenTK.Graphics.OpenGL.GL.Translate(origen.X, origen.Y, origen.Z);
            OpenTK.Graphics.OpenGL.GL.Rotate(rotacion.X, 1, 0, 0);
            OpenTK.Graphics.OpenGL.GL.Rotate(rotacion.Y, 0, 1, 0);
            OpenTK.Graphics.OpenGL.GL.Rotate(rotacion.Z, 0, 0, 1);
            OpenTK.Graphics.OpenGL.GL.Translate(-origen.X, -origen.Y, -origen.Z);
        }

        public void Dibujar()
        {
            OpenTK.Graphics.OpenGL.GL.PushMatrix();

            // Aplicar transformaciones a nivel de objeto
            AplicarTransformaciones();

            // Trasladar al centro de masa
            OpenTK.Graphics.OpenGL.GL.Translate(CentroDeMasa.X, CentroDeMasa.Y, CentroDeMasa.Z);

            // Dibujar todas las partes
            foreach (Parte parte in partes.Values)
            {
                parte.Dibujar();
            }

            OpenTK.Graphics.OpenGL.GL.PopMatrix();
        }
    }
}
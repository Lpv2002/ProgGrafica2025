using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ConsoleApp1
{
    // La clase Cara representa una cara de un objeto tridimensional.
    public class Cara
    {
        // Lista de vértices que forman la cara.
        public List<Vertice> Vertices { get; set; }

        // Variables para transformaciones
        private Vertice CentroDeMasa { get; set; }
        private Vertice RotacionCM { get; set; }
        private Vertice RotacionO { get; set; }
        private Vertice RotacionE { get; set; }
        private Color Color { get; set; }

        // Constructor de la clase Cara.
        public Cara()
        {
            // Inicializa la lista de vértices.
            Vertices = new List<Vertice>();
            CentroDeMasa = new Vertice(0, 0, 0);
            RotacionCM = new Vertice(0, 0, 0);
            RotacionO = new Vertice(0, 0, 0);
            RotacionE = new Vertice(0, 0, 0);
            Color = Color.Red;
        }

        // Método para trasladar la cara
        public void Trasladar(float x, float y, float z)
        {
            foreach (var vertice in Vertices)
            {
                vertice.X += x;
                vertice.Y += y;
                vertice.Z += z;
            }
        }

        // Método para escalar la cara
        public void Escalar(float n)
        {
            if (n <= 0) n = 1;

            foreach (var vertice in Vertices)
            {
                vertice.X *= n;
                vertice.Y *= n;
                vertice.Z *= n;
            }

            CalcularCentroDeMasa(); // Recalcular centro después de escalar
        }

        // Rotación respecto al centro de masa
        public void Rotar(string eje, float angulo)
        {
            float radianes = MathHelper.DegreesToRadians(angulo);
            if (eje == "x")
                RotacionCM.X += angulo;
            else if (eje == "y")
                RotacionCM.Y += angulo;
            else if (eje == "z")
                RotacionCM.Z += angulo;
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
        }

        // Método para calcular el centro de masa
        private void CalcularCentroDeMasa()
        {
            if (Vertices.Count == 0) return;

            double sumX = 0, sumY = 0, sumZ = 0;
            foreach (var vertice in Vertices)
            {
                sumX += vertice.X;
                sumY += vertice.Y;
                sumZ += vertice.Z;
            }

            CentroDeMasa = new Vertice(
                sumX / Vertices.Count,
                sumY / Vertices.Count,
                sumZ / Vertices.Count
            );
        }

        // Método para aplicar transformaciones
        private void AplicarTransformaciones()
        {
            // Aplicar rotación de escenario
            Rotar(new Vertice(0, 0, 0), RotacionE);

            // Aplicar rotación de origen
            Rotar(CentroDeMasa, RotacionO);

            // Aplicar rotación de centro de masa
            Rotar(CentroDeMasa, RotacionCM);
        }

        // Método auxiliar para rotación
        private void Rotar(Vertice origen, Vertice rotacion)
        {
            GL.Translate(origen.X, origen.Y, origen.Z);
            GL.Rotate(rotacion.X, 1, 0, 0);
            GL.Rotate(rotacion.Y, 0, 1, 0);
            GL.Rotate(rotacion.Z, 0, 0, 1);
            GL.Translate(-origen.X, -origen.Y, -origen.Z);
        }

        // Método para dibujar la cara.
        public void Draw()
        {
            GL.PushMatrix();

            AplicarTransformaciones();

            // Comienza el dibujo de la cara como un polígono.
            GL.Begin(PrimitiveType.Polygon);

            // Establece el color de la cara.
            GL.Color3(Color);

            // Itera sobre los vértices y los dibuja.
            foreach (Vertice vertex in Vertices)
            {
                GL.Vertex3(vertex.X, vertex.Y, vertex.Z);
            }

            // Finaliza el dibujo del polígono.
            GL.End();

            GL.PopMatrix();
        }

        // Método para establecer el color de la cara.
        public void SetColor(Color color)
        {
            this.Color = color;
        }

        // Método para cargar los vértices en la cara.
        public void LoadVertices(Vertice[] vertices)
        {
            // Agrega cada vértice a la lista de vértices de la cara.
            foreach (Vertice vertex in vertices)
            {
                Vertices.Add(vertex);
            }

            CalcularCentroDeMasa();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    // La clase Cara representa una cara de un objeto tridimensional.
    public class Poligono
    {
        // Lista de vértices que forman la cara.
        public List<Vertice> Vertices { get; set; }

        // Constructor de la clase Cara.
        public Poligono()
        {
            // Inicializa la lista de vértices.
            Vertices = new List<Vertice>();
        }

        // Método para dibujar la cara.
        public void Draw(float X, float Y, float Z)
        {
            // Comienza el dibujo de la cara como un polígono.
            GL.Begin(PrimitiveType.Polygon);

            // Establece el color de la cara.
            SetColor();

            // Itera sobre los vértices y los dibuja.
            foreach (Vertice vertex in Vertices)
            {
                GL.Vertex3(X + vertex.X, Y + vertex.Y, Z + vertex.Z);
            }

            // Finaliza el dibujo del polígono.
            GL.End();
        }

        // Método para establecer el color de la cara.
        public void SetColor()
        {
            // Aquí se establecería el color utilizando OpenGL.
            // En este ejemplo, asumimos que el color ya está definido.
            GL.Color3(0.65f, 0.0f, 0.0f);
        }

        // Método para cargar los vértices en la cara.
        public void LoadVertices(Vertice[] vertices)
        {
            // Agrega cada vértice a la lista de vértices de la cara.
            foreach (Vertice vertex in vertices)
            {
                Vertices.Add(vertex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    // La clase Cara representa una cara de un objeto tridimensional.
    public class Cara
    {
        // Lista de vértices que forman la cara.
        public List<Vertice> Vertices { get; set; }

        private Matrix4 MatrizTransformacion = Matrix4.Identity;

        // Constructor de la clase Cara.
        public Cara()
        {
            // Inicializa la lista de vértices.
            Vertices = new List<Vertice>();
        }

        public void Trasladar(float x, float y, float z)
        {
            MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateTranslation(x, y, z));
        }

        public void Escalar(float n)
        {
            MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateScale(n));
        }
        public void Rotar(string eje, float angulo)
        {
            float radians = MathHelper.DegreesToRadians(angulo);
            if (eje == "x")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateRotationX(radians));
            else if (eje == "y")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateRotationY(radians));
            else if (eje == "z")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateRotationZ(radians));
        }
        // Método para dibujar la cara.
        public void Draw()
        {
            // Comienza el dibujo de la cara como un polígono.
            GL.Begin(PrimitiveType.Polygon);

            // Establece el color de la cara.
            SetColor();

            // Itera sobre los vértices y los dibuja.
            foreach (Vertice vertex in Vertices)
            {
                Vector4 Trasformado = Vector4.Transform(new Vector4((float)vertex.X, (float)vertex.Y, (float)vertex.Z, 1), MatrizTransformacion);
                GL.Vertex4(Trasformado);
            }

            // Finaliza el dibujo del polígono.
            GL.End();

            MatrizTransformacion = Matrix4.Identity;
        }

        // Método para establecer el color de la cara.
        private void SetColor()
        {
            // Aquí se establecería el color utilizando OpenGL.
            // En este ejemplo, asumimos que el color ya está definido.
            GL.Color3(0.65f, 0.0f, 0.0f);  // Color rojo
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

        // Otros métodos relacionados con la manipulación de la cara, como calcular la normal, etc.
    }
}
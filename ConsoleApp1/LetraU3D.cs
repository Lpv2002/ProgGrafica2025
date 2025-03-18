using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class LetraU3D
    {
        public void Draw()
        {
            GL.Color3(System.Drawing.Color.Red); // Color U

            // Dibujar las 3 partes principales
            DibujarBrazoVertical(-2.0f); // Brazo izquierdo
            DibujarBrazoVertical(1.0f);  // Brazo derecho
            DibujarBaseHorizontal();
        }

        private void DibujarBrazoVertical(float posX)
        {
            // Parámetros comunes
            float altura = 5.0f;
            float grosor = 1.0f;
            float profundidad = 1.0f;

            // Cara frontal (orden horario)
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(posX, 0, 0);
            GL.Vertex3(posX + grosor, 0, 0);
            GL.Vertex3(posX + grosor, altura, 0);
            GL.Vertex3(posX, altura, 0);
            GL.End();

            // Cara trasera (orden horario)
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(posX, 0, profundidad);
            GL.Vertex3(posX, altura, profundidad);
            GL.Vertex3(posX + grosor, altura, profundidad);
            GL.Vertex3(posX + grosor, 0, profundidad);
            GL.End();

            // Cara lateral izquierda/derecha
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(posX, 0, 0);
            GL.Vertex3(posX, 0, profundidad);
            GL.Vertex3(posX, altura, profundidad);
            GL.Vertex3(posX, altura, 0);
            GL.End();

            // Cara lateral opuesta
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(posX + grosor, 0, 0);
            GL.Vertex3(posX + grosor, altura, 0);
            GL.Vertex3(posX + grosor, altura, profundidad);
            GL.Vertex3(posX + grosor, 0, profundidad);
            GL.End();
        }

        private void DibujarBaseHorizontal()
        {
            float longitud = 4.0f;
            float altura = 1.0f;
            float profundidad = 1.0f;

            // Cara superior
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(-2.0f, 0, 0);
            GL.Vertex3(2.0f, 0, 0);
            GL.Vertex3(2.0f, 0, profundidad);
            GL.Vertex3(-2.0f, 0, profundidad);
            GL.End();

            // Cara inferior
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(-2.0f, altura, 0);
            GL.Vertex3(-2.0f, altura, profundidad);
            GL.Vertex3(2.0f, altura, profundidad);
            GL.Vertex3(2.0f, altura, 0);
            GL.End();

            // Cara frontal
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(-2.0f, 0, 0);
            GL.Vertex3(2.0f, 0, 0);
            GL.Vertex3(2.0f, altura, 0);
            GL.Vertex3(-2.0f, altura, 0);
            GL.End();

            // Cara trasera
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(-2.0f, 0, profundidad);
            GL.Vertex3(-2.0f, altura, profundidad);
            GL.Vertex3(2.0f, altura, profundidad);
            GL.Vertex3(2.0f, 0, profundidad);
            GL.End();
        }
    }
}
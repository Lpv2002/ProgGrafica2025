using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ConsoleApp1
{
    public class Game : GameWindow
    {
        private Escenario Escenario1;
        private float rotationAngle = 0.0f;

        public Game() : base(800, 600)
        {
            Escenario1 = new Escenario(0.0f, 0.0f, 0.0f);
            Objeto objetoU = new Objeto(0.0f, 0.0f, 0.0f);

            // Partes de la U
            Parte brazoIzquierdo = new Parte();
            Parte brazoDerecho = new Parte();
            Parte baseHorizontal = new Parte();

            // Polígonos 
            Poligono poligonoIzquierdo = new Poligono();
            Poligono poligonoDerecho = new Poligono();
            Poligono poligonoBase = new Poligono();

            // Configurar escenario
            Escenario1.AgregarObjeto("ObjetoU", objetoU);
            
            // Ensamblar U
            objetoU.AgregarParte("BrazoIzquierdo", brazoIzquierdo);
            objetoU.AgregarParte("BrazoDerecho", brazoDerecho);
            objetoU.AgregarParte("Base", baseHorizontal);

            // Asignar polígonos
            brazoIzquierdo.AgregarPoligono("PoligonoIzquierdo", poligonoIzquierdo);
            brazoDerecho.AgregarPoligono("PoligonoDerecho", poligonoDerecho);
            baseHorizontal.AgregarPoligono("PoligonoBase", poligonoBase);

            // Vértices brazo izquierdo
            Vertice[] vIzquierda = {
                // Cara frontal
                new Vertice(-7, 5, 2.5),
                new Vertice(-6, 5, 2.5),
                new Vertice(-6, -5, 2.5),
                new Vertice(-7, -5, 2.5),

                // Cara trasera
                new Vertice(-7, 5, -2.5),
                new Vertice(-6, 5, -2.5),
                new Vertice(-6, -5, -2.5),
                new Vertice(-7, -5, -2.5),

                // Caras laterales
                new Vertice(-7, 5, 2.5), new Vertice(-7, 5, -2.5), new Vertice(-7, -5, -2.5), new Vertice(-7, -5, 2.5), // Izquierda
                new Vertice(-6, 5, 2.5), new Vertice(-6, 5, -2.5), new Vertice(-6, -5, -2.5), new Vertice(-6, -5, 2.5), // Derecha

                // Superior e inferior
                new Vertice(-7, 5, 2.5), new Vertice(-6, 5, 2.5), new Vertice(-6, 5, -2.5), new Vertice(-7, 5, -2.5), // Superior
                new Vertice(-7, -5, 2.5), new Vertice(-6, -5, 2.5), new Vertice(-6, -5, -2.5), new Vertice(-7, -5, -2.5) // Inferior
            };
            poligonoIzquierdo.LoadVertices(vIzquierda);

            // Vértices brazo derecho
            Vertice[] vDerecha = {
                // Cara frontal
                new Vertice(6, 5, 2.5),
                new Vertice(7, 5, 2.5),
                new Vertice(7, -5, 2.5),
                new Vertice(6, -5, 2.5),

                // Cara trasera
                new Vertice(6, 5, -2.5),
                new Vertice(7, 5, -2.5),
                new Vertice(7, -5, -2.5),
                new Vertice(6, -5, -2.5),

                // Caras laterales
                new Vertice(6, 5, 2.5), new Vertice(6, 5, -2.5), new Vertice(6, -5, -2.5), new Vertice(6, -5, 2.5), // Izquierda
                new Vertice(7, 5, 2.5), new Vertice(7, 5, -2.5), new Vertice(7, -5, -2.5), new Vertice(7, -5, 2.5), // Derecha

                // Superior e inferior
                new Vertice(6, 5, 2.5), new Vertice(7, 5, 2.5), new Vertice(7, 5, -2.5), new Vertice(6, 5, -2.5), // Superior
                new Vertice(6, -5, 2.5), new Vertice(7, -5, 2.5), new Vertice(7, -5, -2.5), new Vertice(6, -5, -2.5) // Inferior
            };
            poligonoDerecho.LoadVertices(vDerecha);

            // Vértices base horizontal
            Vertice[] vBase = {
                // Cara frontal
                new Vertice(-7, -5, 2.5),
                new Vertice(7, -5, 2.5),
                new Vertice(7, -6, 2.5),
                new Vertice(-7, -6, 2.5),

                // Cara trasera
                new Vertice(-7, -5, -2.5),
                new Vertice(7, -5, -2.5),
                new Vertice(7, -6, -2.5),
                new Vertice(-7, -6, -2.5),

                // Caras laterales
                new Vertice(-7, -5, 2.5), new Vertice(-7, -5, -2.5), new Vertice(-7, -6, -2.5), new Vertice(-7, -6, 2.5), // Izquierda
                new Vertice(7, -5, 2.5), new Vertice(7, -5, -2.5), new Vertice(7, -6, -2.5), new Vertice(7, -6, 2.5), // Derecha

                // Superior e inferior
                new Vertice(-7, -5, 2.5), new Vertice(7, -5, 2.5), new Vertice(7, -5, -2.5), new Vertice(-7, -5, -2.5), // Superior
                new Vertice(-7, -6, 2.5), new Vertice(7, -6, 2.5), new Vertice(7, -6, -2.5), new Vertice(-7, -6, -2.5) // Inferior
            };
            poligonoBase.LoadVertices(vBase);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.FromArgb(0, 0, 0));
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            rotationAngle += 30.0f * (float)e.Time;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.5, 0.5, -0.5, 0.5, 1.0, 100.0); // Reducir el frustum

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Ajustes para vista más cercana:
            GL.Translate(0.0f, 0.0f, -25.0f);  // Reducir distancia (original: -40)
            GL.Rotate(15.0f, 1.0f, 0.0f, 0.0f); // Reducir inclinación (original: 25)
            GL.Rotate(rotationAngle, 0.0f, 1.0f, 0.0f);

            Escenario1.Dibujar();
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }
    }
}
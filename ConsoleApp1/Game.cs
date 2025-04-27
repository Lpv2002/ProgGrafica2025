using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using OpenTK.Input;

namespace ConsoleApp1
{
    public class Game : GameWindow
    {
        private Escenario Escenario1;
        private Objeto U1, U2;
        private float angulo = 0.0f;

        // Variables para la cámara
        private float cameraX = 0.0f;
        private float cameraY = 0.0f;
        private float cameraZ = -15.0f;
        private float cameraRotationY = 0.0f;
        private float cameraRotationX = 0.0f;

        // Variables para control de rotación global
        private string ejeRotacion = null;
        private float velocidadRotacion = 1.0f;

        public Game() : base(800, 600) // Constructor que define el tamaño de la ventana
        {
            Escenario1 = new Escenario(0.0f, 0.0f, 0.0f);

            // Leer el JSON desde el archivo
            string json = File.ReadAllText(@"C:\Users\hp\Downloads\GraficaClases-Tarea5\GraficaClases-Tarea5\ConsoleApp1\Objeto.txt");
            U1 = JsonConvert.DeserializeObject<Objeto>(json);
            U2 = JsonConvert.DeserializeObject<Objeto>(json);

            Escenario1.AgregarObjeto("U1", U1);
            Escenario1.AgregarObjeto("U2", U2);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.FromArgb(0, 0, 0));
            GL.Enable(EnableCap.DepthTest); // Habilitar prueba de profundidad
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            ProcesarEntrada(); // Consolidamos la lógica de entrada en un método
        }

        private void ProcesarEntrada()
        {
            var state = Keyboard.GetState(); // Obtenemos el estado del teclado una vez para evitar redundancia

            // Rotación global de todos los objetos
            if (state.IsKeyDown(OpenTK.Input.Key.X)) ejeRotacion = "x";
            else if (state.IsKeyDown(OpenTK.Input.Key.Y)) ejeRotacion = "y";
            else if (state.IsKeyDown(OpenTK.Input.Key.Z)) ejeRotacion = "z";

            if (!string.IsNullOrEmpty(ejeRotacion))
            {
                Escenario1.Rotar(ejeRotacion, velocidadRotacion);
            }

            // Control de la cámara
            if (state.IsKeyDown(OpenTK.Input.Key.Left)) cameraRotationY -= 1.0f;
            if (state.IsKeyDown(OpenTK.Input.Key.Right)) cameraRotationY += 1.0f;
            if (state.IsKeyDown(OpenTK.Input.Key.Up)) cameraRotationX -= 1.0f;
            if (state.IsKeyDown(OpenTK.Input.Key.Down)) cameraRotationX += 1.0f;
            if (state.IsKeyDown(OpenTK.Input.Key.W)) cameraZ += 0.2f;
            if (state.IsKeyDown(OpenTK.Input.Key.S)) cameraZ -= 0.2f;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ConfigurarProyeccion();
            ConfigurarVista();

            // Dibuja el escenario y aplica transformaciones a los objetos
            DibujarEscenario();

            SwapBuffers();
        }

        private void ConfigurarProyeccion()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-1.0, 1.0, -1.0, 1.0, 1.0, 100.0); // Proyección perspectiva
        }

        private void ConfigurarVista()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Aplica la traslación y rotación de la cámara
            GL.Translate(cameraX, cameraY, cameraZ);
            GL.Rotate(cameraRotationX, 1.0f, 0.0f, 0.0f); // Rotación en X
            GL.Rotate(cameraRotationY, 0.0f, 1.0f, 0.0f); // Rotación en Y
        }

        private void DibujarEscenario()
        {
            // Rotación y transformación de los objetos
            U1.get("BrazoIzquierdo").Rotar("x",angulo);
            U1.Trasladar(5, 0, 0);
            U1.Escalar(0.9f);

            U2.Trasladar(-5, 0, 0);

            Escenario1.Dibujar();

            angulo += 1f;
            if (angulo > 360f) angulo = 0.0f; // Resetear el ángulo si sobrepasa 360
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height); // Ajustar el viewport al tamaño de la ventana
        }
    }
}

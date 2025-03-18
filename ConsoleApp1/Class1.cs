using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ConsoleApp1
{
    class Class1
    {
        public GameWindow windows;
        private LetraU3D U;
        private float rotationAngle = 0.0f;
        private float rotationSpeed = 45.0f; // Velocidad de rotación en grados por segundo

        public Class1(GameWindow windowsInput)
        {
            this.windows = windowsInput;
            U = new LetraU3D(); // Instancia de la letra U 3D

            // Configuración de eventos
            windows.Load += windows_Load;
            windows.RenderFrame += Windows_RenderFrame;
            windows.UpdateFrame += Windows_UpdateFrame;
            windows.Closing += Windows_Closing;
        }

        private void Windows_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cierra la aplicación correctamente
            windows.Exit();
        }

        private void Windows_UpdateFrame(object sender, FrameEventArgs e)
        {
            // Actualiza el ángulo de rotación según el tiempo transcurrido
            rotationAngle += rotationSpeed * (float)e.Time;
        }

        private void Windows_RenderFrame(object sender, FrameEventArgs e)
        {
            // Limpia los buffers de color y profundidad
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configuración de matriz de proyección
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-1.0, 1.0, -1.0, 1.0, 1.0, 100.0); // Perspectiva 3D

            // Configuración de matriz de vista del modelo
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Posicionamiento de cámara
            GL.Translate(0.0f, -2.0f, -5.0f); // Posición de la cámara (X, Y, Z)
            GL.Rotate(-5.0f, 1.0f, 0.0f, 0.0f); // Inclinación de la cámara
            GL.Rotate(rotationAngle, 0.0f, 1.0f, 0.0f); // Rotación automática continua en eje Y

            // Dibuja la letra U
            U.Draw();

            // Intercambia los buffers para mostrar el frame
            windows.SwapBuffers();
        }

        void windows_Load(object sender, EventArgs e)
        {
            // Color de fondo
            GL.ClearColor(Color.FromArgb(1, 1, 1));

            // Habilita el test de profundidad para correcto renderizado 3D
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
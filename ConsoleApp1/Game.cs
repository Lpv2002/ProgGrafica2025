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
        private Objeto U1, auto, arbol;
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

        // Controlador de animación
        private Ejecutor ejecutor;
        private bool animacionCargada = false;

        public Game() : base(800, 600) // Constructor que define el tamaño de la ventana
        {
            Escenario1 = new Escenario(0.0f, 0.0f, 0.0f);

            // Leer el JSON desde el archivo
            string json = File.ReadAllText(@"C:\Users\hp\Downloads\GraficaClases-Tarea5\GraficaClases-Tarea5\ConsoleApp1\Objeto.txt");
            string json2 = File.ReadAllText(@"C:\Users\hp\Downloads\GraficaClases-Tarea5\GraficaClases-Tarea5\ConsoleApp1\auto.txt");
            string json3 = File.ReadAllText(@"C:\Users\hp\Downloads\GraficaClases-Tarea5\GraficaClases-Tarea5\ConsoleApp1\arbol.txt");

            //U1 = JsonConvert.DeserializeObject<Objeto>(json);
            auto = JsonConvert.DeserializeObject<Objeto>(json2);
            arbol = JsonConvert.DeserializeObject<Objeto>(json3);

            //Escenario1.AgregarObjeto("U1", U1);
            Escenario1.AgregarObjeto("AUTO", auto);
            Escenario1.AgregarObjeto("ARBOL", arbol);
            Escenario1.ObtenerObjeto("ARBOL").Trasladar(-7,0,0);

            // Inicializar el controlador de animación
            ejecutor = new Ejecutor(Escenario1);

            // Crear un guion de animación
            CrearAnimacion();
        }

        private void CrearAnimacion()
        {
            // Crear un nuevo guion
            Guion guion = new Guion();

            // Crear la primera escena
            Escena escena1 = new Escena();
            escena1.Nombre = "Escena 1: Movimiento inicial";

            // Lista de acciones para la escena 1
            List<Accion> accionesEscena1 = new List<Accion>();

            // Acción 1: Trasladar el auto durante 3 segundos
            Accion trasladarAuto = new Accion(
                "AUTO",                                   // Objeto
                new List<byte> { 0, 0, 1 },              // [escalar, rotar, trasladar] => trasladar
                new List<object> {5.0f, 0.0f, 0.0f },  // Parámetros (x, y, z)
                0,                                       // Tiempo inicial (ms)
                1000,                                    // Tiempo final (ms)
                true                                     // Estado activo
            );

            Accion trasladarAuto2 = new Accion(
                "AUTO",                                   // Objeto
                new List<byte> { 0, 0, 1 },              // [escalar, rotar, trasladar] => trasladar
                new List<object> { -5.0f, 0.0f, 0.0f },  // Parámetros (x, y, z)
                1001,                                       // Tiempo inicial (ms)
                2000,                                    // Tiempo final (ms)
                true                                     // Estado activow
            );

            // Agregar las acciones a la escena
            accionesEscena1.Add(trasladarAuto);
            accionesEscena1.Add(trasladarAuto2);

            // Establecer las acciones en la escena
            escena1.Acciones = accionesEscena1;

            // Agregar la primera escena al guion
            guion.AddEscena(escena1);

            // --- SEGUNDA ESCENA ---

            // Crear la segunda escena
            Escena escena2 = new Escena();
            escena2.Nombre = "Escena 2: escalamiento";

            // Lista de acciones para la escena 2
            List<Accion> accionesEscena2 = new List<Accion>();

            // Acción 2: Escalar el auto (durante 2 segundos, después de la rotación)
            Accion escalarAuto = new Accion(
                "AUTO",                                   // Objeto
                new List<byte> { 1, 0, 0 },              // [escalar, rotar, trasladar] => escalar
                new List<object> { 2f },               // Parámetro (factor de escala)
                2001,                                    // Tiempo inicial (ms)
                4000,                                    // Tiempo final (ms)
                true                                     // Estado activo
            );

            Accion rotarArbol = new Accion(
                "ARBOL",                                   // Objeto
                new List<byte> { 1, 0, 0 },              // [escalar, rotar, trasladar] => escalar
                new List<object> { -0.5f },               // Parámetro (factor de escala)
                2001,                                    // Tiempo inicial (ms)
                4000,                                    // Tiempo final (ms)
                true                                    // Estado activo
            );

            // Agregar las acciones a la escena 2
            accionesEscena2.Add(escalarAuto);
            accionesEscena2.Add(rotarArbol);

            // Establecer las acciones en la escena 2
            escena2.Acciones = accionesEscena2;

            // Agregar la segunda escena al guion
            guion.AddEscena(escena2);

            // Cargar el guion en el controlador de animación
            ejecutor.CargarGuion(guion);
            animacionCargada = true;
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

            // Controles de animación
            if (state.IsKeyDown(Key.Space) && animacionCargada)
            {
                ejecutor.IniciarAnimacion();
            }

            if (state.IsKeyDown(Key.P))
            {
                ejecutor.PausarAnimacion();
            }

            if (state.IsKeyDown(Key.Escape))
            {
                ejecutor.DetenerAnimacion();
            }

            // Rotación global de todos los objetos
            if (state.IsKeyDown(Key.X)) ejeRotacion = "x";
            else if (state.IsKeyDown(Key.Y)) ejeRotacion = "y";
            else if (state.IsKeyDown(Key.Z)) ejeRotacion = "z";
            else ejeRotacion = null; // Resetear eje si no se presiona ninguna tecla

            if (!string.IsNullOrEmpty(ejeRotacion))
            {
                Escenario1.Rotar(ejeRotacion, velocidadRotacion);
            }

            // Control de la cámara
            if (state.IsKeyDown(Key.Left)) cameraRotationY -= 1.0f;
            if (state.IsKeyDown(Key.Right)) cameraRotationY += 1.0f;
            if (state.IsKeyDown(Key.Up)) cameraRotationX -= 1.0f;
            if (state.IsKeyDown(Key.Down)) cameraRotationX += 1.0f;
            if (state.IsKeyDown(Key.W)) cameraZ += 0.2f;
            if (state.IsKeyDown(Key.S)) cameraZ -= 0.2f;

            // Resetear posición de la cámara
            if (state.IsKeyDown(Key.R))
            {
                cameraX = 0.0f;
                cameraY = 0.0f;
                cameraZ = -15.0f;
                cameraRotationX = 0.0f;
                cameraRotationY = 0.0f;
            }
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
            // Las transformaciones ahora se manejan a través del controlador de animación
            // Ya no necesitamos aplicar transformaciones manuales aquí

            // Mantener la traslación inicial del auto para posicionarlo antes de la animación
            if (!animacionCargada ||
                (ejecutor != null && !ejecutor.GetEnEjecucion()))
            {
                auto.Trasladar(0, 0, 0);
            }

            Escenario1.Dibujar();

            // Incrementamos el ángulo para animaciones manuales si se necesitan
            angulo += 1f;
            if (angulo > 360f) angulo = 0.0f; // Resetear el ángulo si sobrepasa 360
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height); // Ajustar el viewport al tamaño de la ventana
        }

        protected override void OnUnload(EventArgs e)
        {
            // Detener la animación cuando se cierra la ventana
            if (ejecutor != null)
            {
                ejecutor.DetenerAnimacion();
            }

            base.OnUnload(e);
        }
    }
}
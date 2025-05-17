using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    public class Ejecutor
    {
        private Guion guion;
        private Thread hilo;
        private bool enEjecucion;
        private Escenario escenario;
        private bool pausado;

        // Referencias a transformaciones de objetos actuales para animaciones progresivas
        private Dictionary<string, Vector3> posicionesActuales;
        private Dictionary<string, Vector3> rotacionesActuales;
        private Dictionary<string, float> escalasActuales;

        public Ejecutor(Escenario escenario)
        {
            this.escenario = escenario;
            this.guion = new Guion();
            this.enEjecucion = false;
            this.pausado = false;

            // Inicializar diccionarios de estado
            posicionesActuales = new Dictionary<string, Vector3>();
            rotacionesActuales = new Dictionary<string, Vector3>();
            escalasActuales = new Dictionary<string, float>();

            // Inicializar estados para todos los objetos del escenario
            foreach (var obj in escenario.objetos)
            {
                posicionesActuales[obj.Key] = new Vector3(0, 0, 0);
                rotacionesActuales[obj.Key] = new Vector3(0, 0, 0);
                escalasActuales[obj.Key] = 1.0f;
            }
        }

        public void CargarGuion(Guion guion)
        {
            this.guion = guion;
        }

        public void AgregarEscena(Escena escena)
        {
            this.guion.AddEscena(escena);
        }

        public void IniciarAnimacion()
        {
            if (enEjecucion) return;

            // Reiniciamos el estado de los objetos
            enEjecucion = true;
            pausado = false;

            hilo = new Thread(new ThreadStart(Play));
            hilo.Start();
        }

        public void PausarAnimacion()
        {
            pausado = !pausado;
        }

        public void DetenerAnimacion()
        {
            enEjecucion = false;

            // Esperamos a que termine el hilo
            if (hilo != null && hilo.IsAlive)
            {
                hilo.Join(1000); // Esperamos máximo 1 segundo
                if (hilo.IsAlive)
                    hilo.Abort(); // Forzamos la detención si sigue vivo
            }
        }

        private void Play()
        {
            if (guion.GetCantidadDeEscenas() == 0) return;

            int escenaActual = 0;

            while (escenaActual < guion.GetCantidadDeEscenas() && enEjecucion)
            {
                ReproducirEscena(guion.GetEscena(escenaActual));
                escenaActual++;
            }

            enEjecucion = false;
        }

        private void ReproducirEscena(Escena escena)
        {
            int tiempoInicial = Environment.TickCount & Int32.MaxValue;
            int tiempoActual = 0;

            // Calculamos el tiempo máximo de la escena
            long tiempoMaximo = 0;
            foreach (var accion in escena.Acciones)
            {
                if (accion.tiempoF > tiempoMaximo)
                    tiempoMaximo = accion.tiempoF;
            }

            // Mientras no haya terminado la escena y la animación esté en ejecución
            while (tiempoActual <= tiempoMaximo && enEjecucion)
            {
                // Si está pausado, esperamos
                if (pausado)
                {
                    Thread.Sleep(100);
                    continue;
                }

                // Actualizamos el tiempo actual
                tiempoActual = (Environment.TickCount & Int32.MaxValue) - tiempoInicial;

                // Procesamos todas las acciones que deban ejecutarse en este momento
                foreach (var accion in escena.Acciones)
                {
                    if (tiempoActual >= accion.tiempoI && tiempoActual <= accion.tiempoF)
                    {
                        ReproducirAccion(accion, tiempoActual);
                    }
                }

                // Damos tiempo al rendering y evitamos consumo excesivo de CPU
                Thread.Sleep(16); // Aproximadamente 60 FPS
            }
        }

        private void ReproducirAccion(Accion accion, int tiempoActual)
        {
            // Verificamos si el objeto existe en el escenario
            if (!escenario.objetos.ContainsKey(accion.nombreObjeto))
                return;

            // Calculamos cuánto tiempo ha pasado desde el inicio de la acción y lo normalizamos (0-1)
            float tiempoTranscurrido = (float)(tiempoActual - accion.tiempoI);
            float duracionTotal = (float)(accion.tiempoF - accion.tiempoI);
            float factorProgreso = tiempoTranscurrido / duracionTotal;

            // Limitamos el factor de progreso entre 0 y 1
            factorProgreso = Math.Max(0, Math.Min(1, factorProgreso));

            // Implementamos la acción según el tipo
            if (accion.accion[0] == 1) // Escalar
            {
                // Asumimos que el parámetro es un valor de escala (float)
                float escalaObjetivo = (float)accion.parametros[0];
                float escalaInicial = escalasActuales[accion.nombreObjeto];
                float escalaActual = escalaInicial + (escalaObjetivo - escalaInicial) * factorProgreso;

                // Aplicamos la escala
                escenario.objetos[accion.nombreObjeto].Escalar(escalaActual);

                // Actualizamos el estado
                escalasActuales[accion.nombreObjeto] = escalaActual;
            }

            if (accion.accion[1] == 1) // Rotar
            {
                // Asumimos que los parámetros son [eje, ángulo]
                string eje = (string)accion.parametros[0];
                float angulo = (float)accion.parametros[1];

                // Aplicamos la rotación
                escenario.objetos[accion.nombreObjeto].Rotar(eje, angulo * factorProgreso);

                // Actualizamos el estado de rotación dependiendo del eje
                Vector3 rotacion = rotacionesActuales[accion.nombreObjeto];
                if (eje == "x") rotacion.X += angulo * factorProgreso;
                else if (eje == "y") rotacion.Y += angulo * factorProgreso;
                else if (eje == "z") rotacion.Z += angulo * factorProgreso;

                rotacionesActuales[accion.nombreObjeto] = rotacion;
            }

            if (accion.accion[2] == 1) // Trasladar
            {
                // Asumimos que los parámetros son [x, y, z]
                float x = (float)accion.parametros[0];
                float y = (float)accion.parametros[1];
                float z = (float)accion.parametros[2];

                // Calculamos el desplazamiento progresivo
                float deltaX = x * factorProgreso;
                float deltaY = y * factorProgreso;
                float deltaZ = z * factorProgreso;

                // Aplicamos la traslación
                escenario.objetos[accion.nombreObjeto].Trasladar(deltaX, deltaY, deltaZ);

                // Actualizamos el estado
                Vector3 posicion = posicionesActuales[accion.nombreObjeto];
                posicion.X += deltaX;
                posicion.Y += deltaY;
                posicion.Z += deltaZ;
                posicionesActuales[accion.nombreObjeto] = posicion;
            }
        }

        // Método para obtener el estado de ejecución de la animación
        public bool GetEnEjecucion()
        {
            return enEjecucion;
        }
    }
}
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Accion
    {
        public string nombreObjeto;
        public long tiempoI;
        public long tiempoF;
        public long tiempoSiguiente;
        public bool estado;
        public List<byte> accion;  // 0 = escalar, 1 = rotar, 2 = trasladar
        public List<object> parametros; // String para eje de rotación, float para otros valores

        public Accion()
        {
            this.nombreObjeto = "";
            this.accion = new List<byte> { 0, 0, 0 }; // [escalar, rotar, trasladar]
            this.parametros = new List<object>();
            this.tiempoI = 0;
            this.tiempoF = 0;
            this.estado = true;
            this.tiempoSiguiente = 0;
        }

        public Accion(string objeto, List<byte> accion, List<object> parametros, long tiempoI, long tiempoF, bool estado)
        {
            this.nombreObjeto = objeto;
            this.accion = accion;
            this.parametros = parametros;
            this.tiempoI = this.tiempoSiguiente = tiempoI;
            this.tiempoF = tiempoF;
            this.estado = estado;
        }
    }
}
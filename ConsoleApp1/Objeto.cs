
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    internal class Objeto
    {
        [JsonProperty(Order = 1)]
        public Vertice CentroDeMasa { get;  set; }
        [JsonProperty(Order = 2)]
        public Dictionary<string, Parte> partes;
        public Objeto(Vertice centro, Dictionary<string, Parte> partes)
        {
            CentroDeMasa = centro;
            this.partes = partes ?? new Dictionary<string, Parte>();
        }

        public void AgregarParte(string nombre, Parte parte)
        {
            partes.Add(nombre, parte);
        }

        public Parte get(string nombreParte)
        {
            if (partes.ContainsKey(nombreParte))
            {
                return partes[nombreParte];
            }
            else
            {
                throw new Exception($"La parte {nombreParte} no existe en este objeto.");
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            foreach (Parte parte in partes.Values)
            {
                parte.Trasladar(x, y, z);
            }
        }
        public void Escalar(float n)
        {
            foreach (Parte parte in partes.Values)
            {
                parte.Escalar(n);
            }
        }
        public void Rotar(string eje, float angulo)
        {
            foreach (Parte parte in partes.Values)
            {
                parte.Rotar(eje, angulo);
            }
        }
        public void Dibujar()
        {
            foreach (Parte parte in partes.Values)
            {
                parte.Trasladar((float)CentroDeMasa.X, (float)CentroDeMasa.Y, (float)CentroDeMasa.Z);
                parte.Dibujar();
            }
        }
    }
}
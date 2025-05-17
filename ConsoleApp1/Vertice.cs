using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Vertice
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vertice(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Constructor para crear un vértice en el origen (0,0,0)
        public Vertice() : this(0, 0, 0) { }

        // Constructor de copia
        public Vertice(Vertice other)
        {
            if (other != null)
            {
                X = other.X;
                Y = other.Y;
                Z = other.Z;
            }
            else
            {
                X = Y = Z = 0;
            }
        }

        // Método para acumular valores
        public void Acumular(double x, double y, double z)
        {
            X += x;
            Y += y;
            Z += z;
        }

        // Método para multiplicar las coordenadas por un escalar
        public void Multiplicar(double x, double y, double z)
        {
            X *= x;
            Y *= y;
            Z *= z;
        }

        // Método para multiplicar las coordenadas por un valor uniforme
        public void Multiplicar(double n)
        {
            X *= n;
            Y *= n;
            Z *= n;
        }

        // Método para comparar vértices
        public bool CompareTo(Vertice other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        // Sobrecarga del método ToString para facilitar la depuración
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
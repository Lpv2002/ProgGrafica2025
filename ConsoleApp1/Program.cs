using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameWindow window = new GameWindow(800,600);
            Class1 game = new Class1(window);

            window.Run();
        }
    }
}

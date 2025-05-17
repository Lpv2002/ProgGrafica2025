using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Guion
    {
        [JsonProperty] public List<Escena> escenas { get; set; }

        public Guion()
        {
            escenas = new List<Escena>();
        }

        public Guion(List<Escena> escenas)
        {
            this.escenas = escenas ?? new List<Escena>();
        }

        public Escena GetEscena(int i)
        {
            if (i >= 0 && i < escenas.Count)
            {
                return escenas.ElementAt(i);
            }
            return null;
        }

        public void AddEscena(Escena e)
        {
            if (e != null)
            {
                escenas.Add(e);
            }
        }

        public int GetCantidadDeEscenas()
        {
            return escenas.Count;
        }
    }
}
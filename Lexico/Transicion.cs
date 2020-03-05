using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico
{
    class Transicion
    {
        public int estado { get; set; }
        public char leyendo { get; set; }
        public int newestado { get; set; }

        public Transicion(int estado, char leyendo, int newestado)
        {
            this.estado = estado;
            this.leyendo = leyendo;
            this.newestado = newestado;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico
{
    class TablaValor
    {
        public int num { get; set; }
        public int valor { get; set; }
        public char columna { get; set; }

        public TablaValor(int num, int valor, char col)
        {
            this.num = num;
            this.valor = valor;
            this.columna = col;
        }
    }
}

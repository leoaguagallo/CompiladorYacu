using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico
{
    class TablaSimbolos
    {
        public string nombreToken { get; set; }
        public int tipoDato { get; set; }
        public int tamanio { get; set; }
        public Object valor { get; set; }

        public TablaSimbolos(string nombreToken, int tipoDato, int tamanio, object valor)
        {
            this.nombreToken = nombreToken;
            this.tipoDato = tipoDato;
            this.tamanio = tamanio;
            this.valor = valor;
        }
    }
}

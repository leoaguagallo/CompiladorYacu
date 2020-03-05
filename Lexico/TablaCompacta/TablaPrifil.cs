using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico
{
    class TablaPrifil
    {
        public int num { get; set; }
        public int prifil { get; set; }
        public int cantidadFila { get; set; }

        public TablaPrifil(int num, int prifil, int fil)
        {
            this.num = num;
            this.prifil = prifil;
            this.cantidadFila = fil;
        }
    }
}

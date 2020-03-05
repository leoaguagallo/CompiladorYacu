using CompiladorWCL.Models.Lexico.TablaCompacta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico
{
    class Lexico_p
    {
        // Cargar Ficheros
        public static CargarArchivos claseCargarArchivos;
        public static TablaCompacta_p claseTablaCompacta;
        public static TokenReconocido claseTokenReconocidos;

        public static void inicializarModuloLexico(string rutaAFD, string rutaAlfabeto)
        {
            // cargar todos los ficheros
            claseCargarArchivos = new CargarArchivos(rutaAFD, rutaAlfabeto);
            // inicializar la clase de la tabla Compacta
            claseTablaCompacta = new TablaCompacta_p(claseCargarArchivos.matriz_tabla_transicion, claseCargarArchivos.listX);
            // inicializar clase de token reconocidos
            claseTokenReconocidos = new TokenReconocido(claseTablaCompacta, claseCargarArchivos.listAlfabeto);
        }
    }
}

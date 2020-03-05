using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico.TablaCompacta
{
    class TablaCompacta_p
    {
        // Atributos declarados
        private int error = -999;
        public List<char> l_columnas { get; set; } // guarda los simbolos de la matriz de transicion
        public List<TablaValor> list_tablaValor { get; set; } // Esta lista contiene la tabla de valores
        public List<TablaPrifil> list_tablaPrifil { get; set; } // Esta lista cintienen la tabla prifil
        public TablaCompacta_p(int[,] matriz, List<char> columnas)
        {
            this.l_columnas = columnas;
            this.list_tablaValor = llenar_table_valor(matriz);
            this.list_tablaPrifil = llenar_table_prifil(matriz, this.list_tablaValor.Count);
        }

        /**
         *  Con el algoritmo generamos la labla valor
         *
         * @param matriz: debe ingresar la matriz de transicion
         * @return : retorno un objeto vector donde { valor, simbolo}
         * **/
        public List<TablaValor> llenar_table_valor(int[,] matriz)
        {
            List<TablaValor> lista_valor = new List<TablaValor>();
            // recorro toda la matriz
            int id = 0;
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    if (matriz[i, j] != error) // != -999 : porque los espacion en blancon fueron llenados con -999
                    {
                        lista_valor.Add(new TablaValor(id, matriz[i, j], this.l_columnas[j])); // ingreso los datos a la lista
                        id++;
                    }
                }
            }
            return lista_valor;
        }

        /**
         *  Con el algoritmo generamos la labla prifil
         *
         * @param matriz: debe ingresar la matriz de transicion
         * @param filas: la filas que creo al momento de hacer la tabla valor
         * @return : retorno un objeto vector donde { prifil, fil}
         * **/
        public List<TablaPrifil> llenar_table_prifil(int[,] m, int filas)
        {
            List<TablaPrifil> lista = new List<TablaPrifil>();
            int cont_suma = 0; // cuenta el salto
            int cant_valores = 0; // cuenta los elemetos encontrados de cada fila
            int id = 0;
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    if (m[i, j] != error) // > -1: porque los estados son positivos y el -1 es un vacio
                    {
                        cant_valores++; // cuento los elementos que se encuentra en esa fila
                    }
                }

                if (cont_suma <= filas) // debe ser meno igual con la filas obtenidas de la lista tabla_valor
                {
                    lista.Add(new TablaPrifil (id,  cont_suma, cant_valores));
                    id++;
                    // sumo el prifil y el nro de elementos encontrado en la fila para llevar el nuevo prifil
                    cont_suma += cant_valores;
                    cant_valores = 0; // reinicio el valor en 0
                }
                else
                {
                    break; // cuando cont_suma sobrepasa de las filas obtenidas con la lista tabla_valor
                }

            }
            return lista;
        }

        /**
         * Encargado de seguir los movimientos del archivo ejecutado
         * 
         * @param fila: esta valor iniciara en 0 porque es el nodo inicial y dea ira cambiando dependiendo lo que se vaya leyendo
         * @param simbolo: cada simbolo o letra que vaya leyendo
         * return : reterno el proximo nodo a leer
         */
        public int movimiento(int fila, char simbolo)
        {
            if (fila != error)
            {
                int fila_tvalor = this.list_tablaPrifil[fila].prifil;// obtego el prifil
                int cantidad = this.list_tablaPrifil[fila].cantidadFila; // obtengo la cantidad de elementos encontrados
                for (int i = fila_tvalor; i < fila_tvalor + cantidad; i++) // fila_tvalor + cantidad: con esto se cuantos nodos debo recorrer
                {
                    //if ((simbolo.ToString()).Equals(this.list_tablaValor[i][1])) // si el simbolo a leer es controntrado
                    if (simbolo == this.list_tablaValor[i].columna) // si el simbolo a leer es controntrado
                    {
                        return (int)this.list_tablaValor[i].valor; // retorno el nodo que sigue
                    }
                }
            }
            return error; // el caso de que no se encuentre
        }
    }
}

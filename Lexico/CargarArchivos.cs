using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompiladorWCL.Models.Lexico
{
    class CargarArchivos
    {
        private int error = -999;
        public string[] columnas_simbolos { get; set; }
        public int[,] matriz_tabla_transicion { get; set; }
        private XDocument documentoAFD;
        private XDocument documentoAlafabeto;
        public List<int> listQ { get; set; } // estados
        public List<char> listX { get; set; } // alfabeto
        public int qo { get; set; } // estado inicial
        public List<int> listF { get; set; } // estados finales
        public List<Transicion> listTransicion { get; set; } // transicion

        public List<Token> listAlfabeto { get; set; }
        public CargarArchivos(string rutaAFD, string rutaAlfabeto)
        {
            this.documentoAFD = XDocument.Load(rutaAFD);

            this.listQ = list_Q();
            this.listX = list_X();
            this.qo = q0();
            this.listF = list_F();
            this.listTransicion = list_Transicion();
            this.matriz_tabla_transicion = generarMatrizTransitiva(this.listQ, this.listX, this.listTransicion);

            this.documentoAlafabeto = XDocument.Load(rutaAlfabeto);
            this.listAlfabeto = generarListaAlfabeto(documentoAlafabeto);
        }

        /**
         * Estraigo los estados de un archivo xml
         * 
         * return: retorno una lista de estados
         */
        private List<int> list_Q()
        {
            // Extraer Q -> conjunto de estados
            var Q = from cje in this.documentoAFD.Descendants("Q") select cje;
            List<int> lQ = new List<int>();

            foreach (XElement u in Q.Elements("estado"))
            {
                lQ.Add(int.Parse(u.Value));
            }
            return lQ;
        }

        /**
         * Estraigo el alfabeto de un archivo xml
         * 
         * return: retorno una lista de alfabeto
         */
        private List<char> list_X()
        {
            // Extraer X -> alfabeto
            var X = from al in this.documentoAFD.Descendants("X") select al;
            var listX = new List<char>();
            foreach (XElement u in X.Elements("simbolo"))
            {
                listX.Add(u.Value.ElementAt(0));
            }
            return listX;
        }

        /**
         * Estraigo el estado inicial de un archivo xml
         * 
         * return: retorno un entro que es el estado inicial
         */
        private int q0()
        {
            // Extraer qo -> estado inicial
            var qo = from ei in this.documentoAFD.Descendants("qo") select ei;
            int qo_ei = 0;
            foreach (XElement u in qo.Elements("estadoInicial"))
            {
                qo_ei = int.Parse(u.Value);
            }
            return qo_ei;
        }

        /**
         * Estraigo el conjunto de estados finales de un archivo xml
         * 
         * return: retorno una lista de estados finales
         */
        public List<int> list_F()
        {
            // Extraer F -> Conjunto de estados finales
            var F = from al in this.documentoAFD.Descendants("F") select al;
            List<int> listF = new List<int>();
            foreach (XElement u in F.Elements("estadoFinal"))
            {
                listF.Add(int.Parse(u.Value));
            }
            return listF;
        }

        /**
         * Estraigo las transiciones del automata de un archivo xml
         * 
         * return: retorno una lista de transiciones
         */
        private List<Transicion> list_Transicion()
        {
            // transicion
            var transicion = from tran in this.documentoAFD.Descendants("T") select tran;
            var listTransicion = new List<Transicion>();
            int estado_ini = -1;
            char lee = ' ';
            int estado_fin = -1;
            foreach (XElement u in transicion.Elements("transicion"))
            {
                estado_ini = int.Parse(u.Element("estado_ini").Value);
                lee = char.Parse(u.Element("lee").Value);
                estado_fin = int.Parse(u.Element("estado_fin").Value);
                listTransicion.Add(new Transicion(estado_ini, lee, estado_fin));
            }
            return listTransicion;
        }

        /**
         * Generar la matriz con los estados, el alfabeto y la transicion del archivo xml
         * 
         * return : retorno la matriz
         */
        public int[,] generarMatrizTransitiva(List<int> listQ, List<char> listX, List<Transicion> listTransicion)
        {
            int[,] m = crearMatriz(listQ.Count, listX.Count);
            int fila = 0;
            int columna = 0;
            foreach (Transicion dt in listTransicion)
            {
                fila = listQ.FindIndex(x => x == dt.estado);
                columna = listX.FindIndex(x => x.Equals(dt.leyendo));
                //Console.WriteLine("estado : " + dt.estado + "  lee: " + dt.leyendo);
                m[fila, columna] = dt.newestado;
            }
            return m;
        }

        private int[,] crearMatriz(int fila, int columnas)
        {
            int[,] m = new int[fila, columnas];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    m[i, j] = error;
                }
            }
            return m;
        }

        public static List<Token> generarListaAlfabeto(XDocument d)
        {
            // Extraer alfabeto -> conjunto de estados
            var tokens = from cje in d.Descendants("alfabeto") select cje;
            List<Token> l_tokens = new List<Token>();

            // inicializo variables
            int numtoken = 0;
            char simbolo = ' ';
            string nombretoken = "";
            string lexema = "";
            foreach (XElement u in tokens.Elements("token")) // leo la estructura
            {
                numtoken = int.Parse(u.Element("numtoken").Value);
                simbolo = char.Parse(u.Element("sinonimo").Value);
                nombretoken = u.Element("nombretoken").Value;
                lexema = u.Element("lexema").Value;
                //Console.WriteLine("<"+numtoken+","+simbolo+","+nombretoken+","+lexema+">");
                l_tokens.Add(new Token(numtoken, simbolo, nombretoken, lexema));
            }
            return l_tokens;
        }
    }
}

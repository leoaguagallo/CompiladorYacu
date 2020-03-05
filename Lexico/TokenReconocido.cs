using CompiladorWCL.Models.Lexico.TablaCompacta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorWCL.Models.Lexico
{
    class TokenReconocido
    {
        private int error = -999;
        bool banderaDeclare = false;
        bool banderaMain = false;
        public List<TablaSimbolos> listTablaSimbolos;
        public List<Token> listaDeLosTokenReconocidos { get; set; }
        char aux_sinonimo = ' ';
        int aux_verificar = 0;
        int verificar = 0;
        private TablaCompacta_p tablaComp;


        public List<Token> listAlfabeto { get; set; }

        public TokenReconocido(TablaCompacta_p tablaComp, List<Token> listAlfabeto)
        {
            this.listaDeLosTokenReconocidos = new List<Token>();
            this.tablaComp = tablaComp;
            this.listAlfabeto = listAlfabeto;
            this.listTablaSimbolos = new List<TablaSimbolos>();
        }


        public void recorridoCodeTokenReconocidos(string[] code, MainWindow mainWind)
        {
            // inicializo las variables
            this.listaDeLosTokenReconocidos = new List<Token>();
            this.listTablaSimbolos = new List<TablaSimbolos>();
            int estado = 0;
            int auxestado = 0;
            int pos = -1;
            char sim = ' ';
            string cadena = "";
            string line = "";

            for (int k=0; k < code.Length; k++)
            {
                line = code[k];
                estado = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    sim = line.ElementAt(i);
                  
                    if (' ' == sim || '\t' == sim)
                    {
                        sim = '$';
                    }
                    auxestado = estado;
                    estado = this.tablaComp.movimiento(estado, sim);

                    if (sim != '$')
                    {
                        cadena += sim + "";
                    }
                    
                    if (estado < 0 && estado > error)
                    {
                        pos = -estado - 1;
                        // verificar que un identificador no tenga mas de 30 caracteres
                        if (cadena.Length < 29)
                        {
                            this.listaDeLosTokenReconocidos.Add(new Token(this.listAlfabeto[pos].numtoken, this.listAlfabeto[pos].sinonimo,
                                this.listAlfabeto[pos].nombretoken, cadena, k+1));
                        }

                        if (this.listAlfabeto[pos].sinonimo == '¡')
                        {
                            banderaDeclare = true;
                            banderaMain = false;
                        }

                        if (this.listAlfabeto[pos].sinonimo == 'p')
                        {
                            banderaDeclare = false;
                            banderaMain = true;
                        }

                        if (banderaDeclare)
                        {
                            if (this.listAlfabeto[pos].sinonimo == 'i')
                            {
                                if (cadena.Length > 29)
                                {
                                    mainWind.txt_mensaje.Text += "Error 104: " + cadena + " limete maximo de caracteres es 30 (Line: "+(k+1)+")\n";                                }
                                else if (buscarIdentificador(cadena))
                                {
                                    mainWind.txt_mensaje.Text += "Error 103: " + cadena + " ya declarado (Line: " + (k + 1) + ")\n";
                                    this.listTablaSimbolos.RemoveAt(this.listTablaSimbolos.Count - 1);
                                }
                                else // agrego a la tabla de simbolos
                                {
                                    listTablaDeSimbolos(this.listAlfabeto[pos].sinonimo, cadena);
                                }
                            }
                            else // agrego a la tabla de simbolos
                            {
                                listTablaDeSimbolos(this.listAlfabeto[pos].sinonimo, cadena);
                            }
                        }

                        if (banderaMain)
                        {
                            if (this.listAlfabeto[pos].sinonimo == 'i')
                            {
                                if (!buscarIdentificador(cadena))
                                {
                                    mainWind.txt_mensaje.Text  += "Error 102: " + cadena + " no esta declarado (Line: " + (k + 1) + ")\n";
                                }
                            }
                        }

                        estado = 0;
                        cadena = "";
                    }
                    if (estado == error)
                    {
                        estado = 0;
                        if (!"".Equals(cadena))
                        {
                            mainWind.txt_mensaje.Text += "Error 105: " + cadena + " lexema incorrecto (Line: " + (k + 1) + ")\n";
                        }
                        cadena = "";
                    }
                }
            }            
        }

        public void listTablaDeSimbolos(char sinonimo, Object entrada)
        {
            if (sinonimo == 'i' && verificar != aux_verificar)
            {
                this.listTablaSimbolos.Add(new TablaSimbolos("", obtener_tipoDato(aux_sinonimo), obtener_tamanio(aux_sinonimo), null));
            }

            if (sinonimo == 'e' || sinonimo == 'r' || sinonimo == 'l' || sinonimo == 'c' || sinonimo == 'd')
            {
                this.listTablaSimbolos.Add(new TablaSimbolos("", obtener_tipoDato(sinonimo), obtener_tamanio(sinonimo), null));
                aux_verificar = verificar;
                aux_sinonimo = sinonimo;

            }
            else if (sinonimo == 'i')
            {
                verificar++;

                if (this.listTablaSimbolos.Count > 0)
                {
                    this.listTablaSimbolos[this.listTablaSimbolos.Count - 1].nombreToken = entrada.ToString().Substring(1, entrada.ToString().Length - 1);
                }
            }
            else if (sinonimo == 'a')
            {
                if (this.listTablaSimbolos.Count > 0)
                {
                    this.listTablaSimbolos[this.listTablaSimbolos.Count - 1].valor = entrada;
                }
            }
        }

        public bool buscarIdentificador(string lexema)
        {
            int pos = this.listTablaSimbolos.FindIndex(x => x.nombreToken.Equals(lexema.Substring(1)));
            if (pos > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int obtener_tipoDato(char sinonimo)
        {
            if (sinonimo == 'e')
            {
                return 1;
            }
            else if (sinonimo == 'r')
            {
                return 2;
            }
            else if (sinonimo == 'c')
            {
                return 3;
            }
            else if (sinonimo == 'd')
            {
                return 4;
            }
            else if (sinonimo == 'l')
            {
                return 5;
            }
            else
            {
                return -1;
            }
        }

        public int obtener_tamanio(char sinonimo)
        {
            if (sinonimo == 'e')
            {
                return 4;
            }
            else if (sinonimo == 'r')
            {
                return 8;
            }
            else if (sinonimo == 'c')
            {
                return 1;
            }
            else if (sinonimo == 'd')
            {
                return 80;
            }
            else if (sinonimo == 'l')
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}

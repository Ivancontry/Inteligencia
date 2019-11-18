using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Red;


namespace Presentacion
{
    public partial class Entrenamiento : Form
    {
        public Entrenamiento()
        {
            InitializeComponent();
        }
        string linea="",lineaSimulacion="";
        string vectorPesos = "",vectorUmbral="";
        double[] patron;
        ArrayList listaPesos = new ArrayList();
        ArrayList listaUmbrales = new ArrayList();
        ArrayList errorEntrenamiento = new ArrayList();
        ArrayList posicion = new ArrayList();
        ArrayList vector = new ArrayList();
        ArrayList umbral = new ArrayList();
        ArrayList pesos = new ArrayList();
        double[,] patrones, salidas;
        string ruta = "D:/8 Semestre/IA/PatronesRedNeuronal.txt";
        string rutaSimulacion = "D:/8 Semestre/IA/redlista.txt";
        String[] cadena,numeroSalidas,numeroEntradas;
        double[,] salidasRedFinal;
        RedNeuronal redNeuronal = new RedNeuronal();
        List<Neurona> listaNeurona = new List<Neurona>();
        double[,] salidasRedFinalSimulacion;


        double sumatoriaErrorLineal,sumatoriaErrorPatron,x0 = 1;
        Random random = new Random();
        int cont;
        bool aprendio=true;

       

        private void btnEntrenar_Click(object sender, EventArgs e)
        {
            redNeuronal.Patrones = new double[posicion.Count-1,numeroEntradas.Length];
            redNeuronal.Patrones = patrones;
            redNeuronal.Salidas =  new double[posicion.Count - 1, numeroSalidas.Length];
            redNeuronal.Salidas = salidas;
            redNeuronal.NumeroIteraciones = int.Parse(txtNumeroIteraciones.Text);
            redNeuronal.RataAprendizaje = float.Parse(txtRataAprendizaje.Text);
            redNeuronal.ErrorMaximoPermitido = float.Parse(txtErrorMaximo.Text);
            redNeuronal.ErrorPatrones = new double[posicion.Count-1];
           
            entrenarRed();
        }

        public void entrenarRed() {
            cont = 1;
            Console.WriteLine("" + redNeuronal.NumeroIteraciones);
            double errorIteracion = 1;
            while (cont<=redNeuronal.NumeroIteraciones && redNeuronal.ErrorMaximoPermitido<= errorIteracion)
            {
                Console.WriteLine( "Iteracion -------" + cont + "\n");
                errorIteracion = entrenamiento()/(posicion.Count-1);
                sumatoriaErrorPatron = 0;
                Console.WriteLine("Error de la iteracion: " + errorIteracion);
                
                errorEntrenamiento.Add(errorIteracion);
                if (errorIteracion <= redNeuronal.ErrorMaximoPermitido)
                {
                    guardarArchivo();
                    labelSalida.Text = "Exitoso";
                }
                else if (cont >= redNeuronal.NumeroIteraciones)
                {
                    aprendio = false;
                    guardarArchivo();
                    labelSalida.Text = "Fracaso";
                }
                cont++;
            }
            LoadData();
            
           
        }
      

        private void btnCargar_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog buscar = new OpenFileDialog();
            if (buscar.ShowDialog() == DialogResult.OK) {
                txtRuta.Text = buscar.FileName;
                Console.WriteLine(txtRuta.Text);
            }
            cargarArchivo(txtRuta.Text);
        }
        public void cargarArchivoSimular() {
            //Cargamos el archivo
            StreamReader archivo = new StreamReader(rutaSimulacion);
            
            //Leemos el archivo
            while (lineaSimulacion != null)
            {
                lineaSimulacion = archivo.ReadLine();

                if (lineaSimulacion != null) vector.Add(lineaSimulacion);

            }

            for (int i = 0; i < 2; i++)
            {
                string[] cadena = vector[i].ToString().Split(';');
                umbral.Add(cadena[0]);
                string[] p = cadena[1].Split(' ');
                pesos.Add(p[0]);
                pesos.Add(p[1]);
                Console.WriteLine("Archivo simulacion" + umbral[i] + " " + p[0] + " " + p[1]);
            }
            Console.Write(" Cantidad de valores del vector pesos " +pesos.Count);
        }
        public void simularRed() {
            List<Neurona> listaNeuronaSimular = new List<Neurona>();
            cargarArchivoSimular();
            salidasRedFinalSimulacion = new double[posicion.Count - 1, numeroSalidas.Length];
            txtSalida.Text = "x1 x2 y1 y2 y1r y2r"+"\n";
            for (int i = 0; i < posicion.Count - 1; i++)
            {
                
                Console.WriteLine("Patrones de entrada " + "\n");
                string cadena="",salidaReal="",salidaRed="";
                //Recorres cada entrada de un patron
                for (int j = 0; j < numeroEntradas.Length; j++)
                {
                    Console.WriteLine(redNeuronal.Patrones[i, j] + "\n");

                    //Recorro cada neurona
                    for (int p = 0; p < numeroSalidas.Length; p++)
                    {

                        if (listaNeuronaSimular.Count < numeroSalidas.Length)
                        {
                            Neurona neurona = new Neurona();
                            //Inicializo los pesos de cada neurona
                            neurona.Pesos = inicializarPesosSimulacion(p);
                            //Inicializo el umbral de cada neurona
                            neurona.Umbral = double.Parse(umbral[p].ToString());
                            Console.WriteLine("Umbral de activacion" + neurona.Umbral);
                            // Calculo la funciom soma de cada neurona
                            neurona.Soma += redNeuronal.Patrones[i, j] * neurona.Pesos[j];
                            listaNeuronaSimular.Add(neurona);
                        }
                        else
                        {
                            // Calculo la funciom soma sumando ambas entrada de un patron, para cada neurona
                            listaNeuronaSimular[p].Soma += redNeuronal.Patrones[i, j] * listaNeuronaSimular[p].Pesos[j];
                           
                        }

                    }
                }
                //para calcular el error lineal y ajuste de pesos y de umbrar por  neurona
                for (int k = 0; k < numeroSalidas.Length; k++)
                {
                   
                    // le sumo el umbral
                    listaNeuronaSimular[k].Soma += listaNeuronaSimular[k].Umbral;
                    Console.WriteLine("Soma de la neurona" + k + " " + listaNeuronaSimular[k].Soma);
                    //Calculo la funcion de activacion 
                    if (listaNeuronaSimular[k].Soma > 0)
                    {
                        listaNeuronaSimular[k].Salida = 1;
                    }
                    else
                    {
                        listaNeuronaSimular[k].Salida = 0;
                    }
                     cadena+=" "+redNeuronal.Patrones[i, k];
                    salidaReal += " " + redNeuronal.Salidas[i, k]+" ";
                    salidaRed += " " + listaNeuronaSimular[k].Salida+"   ";
                    Console.WriteLine("Salida de la neurona "+k+" "+listaNeuronaSimular[k].Salida);
                    listaNeuronaSimular[k].Soma = 0;
                    salidasRedFinalSimulacion[i, k] = listaNeuronaSimular[k].Salida;
                }
                
                txtSalida.Text += " "+cadena+" "+salidaReal+" "+ salidaRed+"\n";              

            }

        }

        public double[] inicializarPesosSimulacion(int neurona) {
            double[] p = new double[numeroEntradas.Length];
            if (neurona == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    p[i] = double.Parse(pesos[i].ToString());
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    p[i] = double.Parse(pesos[i+2].ToString());
                }

            }
            
            return p;
        }
        public void LoadData()
        {
            DataTable dtPatrones = new DataTable();
            DataTable dtPesos = new DataTable();
            string[,] fila = new string[posicion.Count - 1, numeroEntradas.Length + (numeroSalidas.Length)*2];
            for (int i = 0; i < posicion.Count-1; i++)
            {
                for (int j = 0; j < numeroEntradas.Length; j++)
                {
                    fila[i, j] = redNeuronal.Patrones[i, j].ToString();
                }
            }
            for (int i = 0; i < posicion.Count - 1; i++)
            {
                for (int j = 0; j < numeroSalidas.Length; j++)
                {
                    fila[i, j + numeroEntradas.Length] = redNeuronal.Salidas[i, j].ToString();
                }
            }
            for (int i = 0; i < posicion.Count - 1; i++)
            {
                for (int j = 0; j < numeroSalidas.Length; j++)
                {
                    fila[i, j + (numeroSalidas.Length*2)] = salidasRedFinal[i,j].ToString();
                }
            }

            for (int i = 0; i < posicion.Count - 1; i++)
            {
                string[] addFila = new string[numeroEntradas.Length + numeroSalidas.Length];
                for (int j = 0; j < numeroEntradas.Length; j++)
                {
                    addFila[j] = redNeuronal.Patrones[i, j].ToString();
                }
                for (int j = 0; j < numeroSalidas.Length; j++)
                {
                    addFila[j + numeroEntradas.Length] = redNeuronal.Salidas[i, j].ToString();
                }
                tabla.Rows.Add(addFila);
            }
                    
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        public void limpiar() {
            txtSalida.Text = "";
            txtNumeroIteraciones.Text = "";
            txtRataAprendizaje.Text = "";
            txtErrorMaximo.Text = "";
        }

       

        private void btnSimular_Click(object sender, EventArgs e)
        {
            simularRed();
        }
     


        public void guardarArchivo() {
            
            string ruta = "D:/8 Semestre/IA/redlista.txt";
            ArrayList pe = new ArrayList();
            ArrayList um = new ArrayList();
            File.Delete(ruta);
            if (!File.Exists(ruta))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(ruta))
                {
                    for (int i = 0; i < listaNeurona.Count; i++)
                    {
                        string pesos = "", umbral = "";
                        umbral += "" + listaNeurona[i].Umbral + ";";
                         

                        for (int j = 0; j < listaNeurona.Count; j++)
                        {
                            if (j == 0)
                            {
                                pesos += "" + listaNeurona[i].PesosAnterior[j] + " ";
                            }
                            else
                            {
                                pesos += listaNeurona[i].PesosAnterior[j];
                            };

                        }
                        pe.Add(pesos);
                        um.Add(umbral);
                        
                        Console.WriteLine("Umbral y pesos ajustados "+umbral + " " + pesos);
                        sw.WriteLine(umbral + "" + pesos);
                    }
                    
                    txtSalida_1_Umbral.Text = um[0].ToString();
                    txtSalida_2_Umbral.Text = um[1].ToString();

                    string[] cadena = pe[0].ToString().Split(' ');
                    txtSalida_1_Peso_1.Text = cadena[0];
                    txtSalida_1_Peso_2.Text = cadena[1];
                    string[] cadena2 = pe[1].ToString().Split(' ');
                    txtSalida_2_Peso_1.Text = cadena2[0];
                    txtSalida_2_Peso_2.Text = cadena2[1];

                }
            }
            
            
        }
        public double entrenamiento() {
            
            patron = new double[numeroSalidas.Length];
            //Recorrer los patrones
            salidasRedFinal = new double[posicion.Count - 1, numeroSalidas.Length];
            for (int i = 0; i < posicion.Count - 1; i++)
            {
               sumatoriaErrorLineal = 0;
               
                //Recorres cada entrada de un patron
                for (int j = 0; j < numeroEntradas.Length; j++)
                {
                    Console.WriteLine("Patron"+ redNeuronal.Patrones[i,j]);
                    patron[j] = redNeuronal.Patrones[i, j];
                    //Recorro cada neurona
                    for (int p = 0; p < numeroSalidas.Length; p++)
                    {
                        
                        if (listaNeurona.Count < numeroSalidas.Length)
                        {
                            Neurona neurona = new Neurona();
                            //Inicializo los pesos de cada neurona
                            if (aprendio == false) {
                                neurona.Pesos = inicializarPesosSimulacion(p);
                            }
                            else
                            {
                                neurona.Pesos = inicializarPesos(p);
                            }
                            
                            //Inicializo el umbral de cada neurona
                            neurona.Umbral = x0;
                            // Calculo la funciom soma de cada neurona
                            neurona.Soma += redNeuronal.Patrones[i, j] * neurona.Pesos[j];
                            listaNeurona.Add(neurona);
                        }
                        else
                        {
                            // Calculo la funciom soma sumando ambas entrada de un patron, para cada neurona
                           listaNeurona[p].Soma += redNeuronal.Patrones[i, j] * listaNeurona[p].Pesos[j];
                          
                        }

                    }
                }
                //para calcular el error lineal y ajuste de pesos y de umbrar por  neurona
                for (int k = 0; k < numeroSalidas.Length; k++)
                {
                   
                    // le sumo el umbral
                    listaNeurona[k].Soma += listaNeurona[k].Umbral;
                    //Calculo la funcion de activacion 
                    if (listaNeurona[k].Soma > 0)
                    {
                        listaNeurona[k].Salida = 1;
                    }
                    else
                    {
                        listaNeurona[k].Salida = 0;
                    }
                 
                    //calculo el error lineal de cada neurona
                    listaNeurona[k].ErrorLineal = redNeuronal.Salidas[i, k] - listaNeurona[k].Salida;
                    //Reajusto los pesos para cada neurona
                    
                    listaNeurona[k].Pesos = ajustarPesos(listaNeurona[k].Pesos, redNeuronal.RataAprendizaje, listaNeurona[k].ErrorLineal, patron, k);
                    if (k == 0)
                    {
                        vectorUmbral += listaNeurona[k].Umbral + " ";
                    }
                    else
                    {
                        vectorUmbral += listaNeurona[k].Umbral;                         
                    }
                    vectorPesos += ";";
                    salidasRedFinal[i, k] = listaNeurona[k].Salida;
                    listaNeurona[k].Umbral = listaNeurona[k].Umbral + redNeuronal.RataAprendizaje * listaNeurona[k].ErrorLineal * x0;
                    sumatoriaErrorLineal += Math.Abs(listaNeurona[k].ErrorLineal);
                    listaNeurona[k].Soma = 0;

                }

                Console.WriteLine("Salida de la neurona 1 " +listaNeurona.First().Salida);
                Console.WriteLine("Salida de la neurona 2 " + listaNeurona.Last().Salida);
                //Calculo el error por patrones
                listaUmbrales.Add(vectorUmbral);
              //  Console.WriteLine("Umbrar de cada patron " + i + "--" + vectorUmbral);
                vectorUmbral = "";                
                redNeuronal.ErrorPatrones[i] = sumatoriaErrorLineal/numeroSalidas.Length;
                sumatoriaErrorPatron += redNeuronal.ErrorPatrones[i];
                //Console.WriteLine("pesos de cada patron "+i+"--" + vectorPesos);
                listaPesos.Add(vectorPesos);
                vectorPesos = "";
            }

            return sumatoriaErrorPatron;
        }

        public double[] ajustarPesos(double[] pesoActual, double rataAprendizaje, double errorLineal, double[] entrada, int neurona) {
            

            double[] pesos = new double[numeroSalidas.Length];
            listaNeurona[neurona].PesosAnterior = pesoActual;
            for (int i = 0; i < numeroSalidas.Length; i++)
            {
                if (i == 0) { vectorPesos += pesoActual[i] + " ";} else { vectorPesos += pesoActual[i]; }
                listaNeurona[neurona].Pesos[i] += (errorLineal * entrada[i]*rataAprendizaje);
                
            }
          // Console.WriteLine("Vector de pesos reajustados"+neurona+"   " +vectorPesos);
            return listaNeurona[neurona].Pesos;
                  
        }
        
        public double[] inicializarPesos(int neurona) {
            
            string pesos1 = "";
            double[] pesos = new double[numeroEntradas.Length];
            for (int i = 0; i < numeroEntradas.Length; i++)
            {
                pesos[i] = random.NextDouble();
                pesos1 += "" + pesos[i];
            }
            //Console.WriteLine("Inicializacion del vector pesos "+neurona+"---"+ pesos[0]+" "+pesos[1]);
            return pesos;
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            

        }

        public void cargarArchivo(string rutaArchivo)
        {
            //Cargamos el archivo
            StreamReader archivo = new StreamReader(rutaArchivo);
            //Leemos el archivo
            while (linea != null)
            {
                linea = archivo.ReadLine();

                if (linea != null) posicion.Add(linea);

            }

            cadena = posicion[0].ToString().Split(' ');
            numeroEntradas = cadena[0].Split(';');
            numeroSalidas = cadena[1].Split(';');
            //Inicializamos la matriz de patrones y salidad
            patrones = new double[posicion.Count - 1, numeroEntradas.Length];
            salidas = new double[posicion.Count - 1, numeroSalidas.Length];

            //Llenamos la matriz de patrones y salidad
            for (int i = 1; i < posicion.Count; i++)
            {
                String[] linea = posicion[i].ToString().Split(';');
                for (int j = 0; j < linea.Length; j++)
                {
                    if (j < numeroEntradas.Length)
                    {
                        patrones[i - 1, j] = double.Parse(linea[j].ToString());
                    }
                    else
                    {
                        salidas[i - 1, j - numeroEntradas.Length] = double.Parse(linea[j].ToString());
                    }
                }
            }

            labelNumeroEntradas.Text = "" + numeroEntradas.Length;
            labelNumeroSalidas.Text = "" + numeroSalidas.Length;
            labelNumeroPatrones.Text = "" + (posicion.Count - 1);
            
        }
    }
}

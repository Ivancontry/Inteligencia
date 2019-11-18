using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red
{
    public class Neurona
    {
        private double[] pesos;
        private double[] pesosAnterior;
        private double umbral;
        private int salida;
        private double errorLineal;
        private double soma;

        public Neurona()
        {
        }

        public Neurona(double[] pesos, double[] pesosAnterior, double umbral, int salida, double errorLineal, double soma)
        {
            this.pesos = pesos;
            this.umbral = umbral;
            this.salida = salida;
            this.errorLineal = errorLineal;
            this.soma = soma;
        }

        public double Soma{ get => soma; set => soma = value; }
        public double[] Pesos { get => pesos; set => pesos = value; }
        public double[] PesosAnterior { get => pesosAnterior; set => pesosAnterior = value; }
        public double Umbral { get => umbral; set => umbral = value; }
        public int Salida { get => salida; set => salida = value; }
        public double ErrorLineal { get => errorLineal; set => errorLineal = value; }
    }
}

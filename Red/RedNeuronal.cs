using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red
{
    public class RedNeuronal
    {
        private double[,] patrones;
        private double[,] salidas;
        private int numeroIteraciones;
        private float errorMaximoPermitido;
        private float rataAprendizaje;
        private double[] errorPatrones;
        private double errorEntranamiento;

        public RedNeuronal()
        {
        }

        public RedNeuronal(double[,] patrones, double[,] salidas, int numeroIteraciones, float errorMaximoPermitido, float rataAprendizaje, double[] errorPatrones, double errorEntranamiento)
        {
            this.patrones = patrones;
            this.salidas = salidas;
            this.numeroIteraciones = numeroIteraciones;
            this.errorMaximoPermitido = errorMaximoPermitido;
            this.rataAprendizaje = rataAprendizaje;
            this.errorEntranamiento = errorEntranamiento;
            this.errorPatrones = errorPatrones;
        }

        public double[,] Patrones { get => patrones; set => patrones = value; }
        public double[,] Salidas { get => salidas; set => salidas = value; }
        public int NumeroIteraciones { get => numeroIteraciones; set => numeroIteraciones = value; }
        public float ErrorMaximoPermitido { get => errorMaximoPermitido; set => errorMaximoPermitido = value; }
        public float RataAprendizaje { get => rataAprendizaje; set => rataAprendizaje = value; }
        public double[] ErrorPatrones { get => errorPatrones; set => errorPatrones = value; }
        public double ErrorEntrenamiento { get => errorEntranamiento; set => errorEntranamiento = value; }
    }
}

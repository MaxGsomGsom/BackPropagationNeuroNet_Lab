using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNets6
{
    [Serializable]
    public class Neuron
    {

        double[] w; //связи
        double err=0; //внутренняя и обратная ошибка
        double nOut = 0;

        string name = ""; //название класса

        double outDiff = 0;


        public string Name
        {
            get
            {
                return name;
            }

        }

        public double Err
        {
            get
            {
                return err;
            }
            set
            {
                err = value;
            }

        }


        public double[] W
        {
            get
            {
                return w;
            }

            set
            {
                w = value;
            }
        }

        public double Out
        {
            get
            {
                return nOut;
            }

            set
            {
                nOut = value;
            }
        }

        public double OutDiff
        {
            get
            {
                return outDiff;
            }

            set
            {
                outDiff = value;
            }
        }

        public Neuron(string value = "")
        {
            this.name = value;

        }

        public void InitW(int size, Random r)
        {
            w = new double[size];
            //инициализация весов
            for (int i = 0; i < size; i++)
            {
                w[i] = r.Next(-50, 50) * 0.01;
            }
        }

    }


    [Serializable]
    public struct Image
    {
        public double[] x;
        public int neuronNum;

        public Image(double[] x, int neuronNum)
        {
            this.x = x;
            this.neuronNum = neuronNum;
        }
    }
}

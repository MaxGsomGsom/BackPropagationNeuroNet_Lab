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

        double wXsum = 0;


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

        public double WXsum
        {
            get
            {
                return wXsum;
            }

            set
            {
                wXsum = value;
            }
        }

        public Neuron(string value = "")
        {
            this.name = value;

        }

        public void InitW(int size)
        {
            w = new double[size];
            Random r = new Random((int)DateTime.Now.Ticks);
            //инициализация весов
            for (int i = 0; i < size; i++)
            {
                w[i] = r.Next(-50, 50) * 0.01;
            }
        }

    }

    //при обучении определяет верный или неверный образ
    public enum TrueImage
    {
        False=-1,
        True=1
    }

    [Serializable]
    public struct Image
    {
        public int[] x;
        public int neuronNum;

        public Image(int[] x, int neuronNum)
        {
            this.x = x;
            this.neuronNum = neuronNum;
        }
    }
}

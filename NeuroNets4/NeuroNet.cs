using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace NeuroNets6
{
    public class NeuroNet
    {

        List<Neuron> midNeurons, endNeurons;
        int sizeIn, sizeOut, sizeMid; //размер входного сигнала

        List<Neuron>[] container;

        public NeuroNet(int sizeIn)
        {
            midNeurons = new List<Neuron>();
            endNeurons = new List<Neuron>();
            this.sizeIn = sizeIn;
            this.sizeOut = 0;

            sizeMid = (sizeIn + sizeOut) / 2;

            container = new List<Neuron>[] { midNeurons, endNeurons };
        }


        //сохранить
        public void SaveNeurons()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName == "") return;

            FileStream file = File.Create(dialog.FileName);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, container);
            file.Close();
        }

        //загрузить
        public void LoadNeurons()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName == "") return;

            FileStream file = File.OpenRead(dialog.FileName);
            BinaryFormatter formatter = new BinaryFormatter();
            container = (List<Neuron>[])formatter.Deserialize(file);
            file.Close();

            midNeurons = container[0];
            endNeurons = container[1];
            
        }

        //добавить нейрон (обнуляет сеть)
        public void AddNeuron(string value)
        {
            sizeOut++;
            sizeMid = (sizeIn + sizeOut) / 2;

            midNeurons.Clear();
            for (int i = 0; i < sizeMid; i++)
            {
                Neuron neuron = new Neuron();
                neuron.InitW(sizeIn);
                midNeurons.Add(neuron);
            }

            for (int i = 0; i < (sizeOut-1); i++)
            {
                endNeurons[i].InitW(sizeMid);
            }

            Neuron neuron1 = new Neuron();
            neuron1.InitW(sizeMid);
            endNeurons.Add(neuron1);
        }
 
        public string GetNeuronName(int pos)
        {
            return endNeurons[pos].Name;
        }

        public int Count
        {
            get { return endNeurons.Count; }
        }



        //распознать
        public string Recognize(int[] x)
        {
            //выходы и входящие суммы средних нейронов
            for (int j = 0; j < midNeurons.Count; j++)
            {
                double WXsum = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    WXsum += midNeurons[j].W[i] * x[i];
                }
                midNeurons[j].WXsum = WXsum;
                midNeurons[j].Out = F(WXsum);
            }

            //выходы и входящие суммы конечных нейронов
            for (int k = 0; k < endNeurons.Count; k++)
            {
                double WXsum = 0;
                for (int j = 0; j < midNeurons.Count; j++)
                {
                    WXsum += endNeurons[k].W[j] * midNeurons[j].Out;
                }
                endNeurons[k].WXsum = WXsum;
                endNeurons[k].Out = F(WXsum);
            }

            //получаем нейрон с положительным выходом
            for (int k = 0; k < endNeurons.Count; k++)
            {
                if (endNeurons[k].Out > 0) return endNeurons[k].Name;
            }

            return "Not recognized";
        }

        //функция активации
        double F(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        //производная функции активации
        double F_(double x)
        {
            return F(x) * (1 - F(x));
        }

        //обучение
        public void Learn(int neuronNum, int[] x)
        {
            //вычисляем входные суммы и выходы нейронов
            Recognize(x);

            //для конечных нейронов
            for (int k = 0; k < endNeurons.Count; k++)
            {
                //находим внутреннюю ошибку конечного нейрона
                double err = 0;
                if (k == neuronNum) err = ((double)TrueImage.True - endNeurons[k].Out) * F_(endNeurons[k].WXsum);
                else err = ((double)TrueImage.False - endNeurons[k].Out) * F_(endNeurons[k].WXsum);
                endNeurons[k].Err = err;

                //меняем связи конечного нейрона
                for (int j = 0; j < midNeurons.Count; j++)
                {
                    endNeurons[k].W[j] = endNeurons[k].W[j] + err * midNeurons[j].Out;
                }

            }



            //для внутренних нейронов
            for (int j = 0; j < midNeurons.Count; j++)
            {
                //находим сумму ошибок последующих нейронов
                double errBack = 0;
                for (int k = 0; k < endNeurons.Count; k++)
                {
                    errBack += endNeurons[k].W[j] * endNeurons[k].Err;
                }


                //находим внутреннюю ошибку нейрона
                double err = errBack * F_(midNeurons[j].WXsum);
                midNeurons[j].Err = err;

                //меняем связи нейрона
                for (int i = 0; i < midNeurons.Count; i++)
                {
                    midNeurons[j].W[i] = midNeurons[j].W[i] + err * x[i];
                }

            }



        }

    }
}

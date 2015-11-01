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

        object[] container;

        List<Image> images;

        public NeuroNet(int sizeIn)
        {
            midNeurons = new List<Neuron>();
            images = new List<Image>();
            endNeurons = new List<Neuron>();
            this.sizeIn = sizeIn;
            this.sizeOut = 0;

            //sizeMid = (sizeIn + sizeOut) / 2;
            sizeMid = sizeOut * 100;
            container = new object[] { midNeurons, endNeurons, images };

            
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
            container = (object[])formatter.Deserialize(file);
            file.Close();

            midNeurons = (List<Neuron>)container[0];
            endNeurons = (List<Neuron>)container[1];
            images = (List<Image>)container[2];
        }

        //добавить нейрон (обнуляет сеть)
        public void AddNeuron(string value)
        {
            sizeOut++;
            //sizeMid = (sizeIn + sizeOut) / 2;
            sizeMid = sizeOut * 100;
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

            Neuron neuron1 = new Neuron(value);
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
            double max = double.MinValue;
            double min = double.MaxValue;
            int num = -1;
            for (int i = 0; i < endNeurons.Count; i++)
            {
                double result = endNeurons[i].Out;
                if (result >= max)
                {
                    max = result;
                    num = i;
                }
                if (result <= min) min = result;
            }

            if (num == -1) return "Not recognized";

            return endNeurons[num].Name;






        }

        //функция активации
        double F(double x)
        {
            double h = 1;

            return 1 / (1 + Math.Pow(Math.E, -x/h));
        }

        //производная функции активации
        double F_(double x)
        {
            return F(x) * (1 - F(x));
        }

        //обучение
        public void Learn(int neuronNum, int[] x)
        {

            bool needLearn = true;
            double h = 1; //скорость обучения

            //добавляем в список образов
            images.Add(new Image(x, neuronNum));
            images.Reverse();

            while (needLearn)
            {
                needLearn = false;

                foreach (Image item in images)
                {

                    //вычисляем входные суммы и выходы нейронов
                    Recognize(item.x);


                    double sumErr = 0;
                    //для конечных нейронов
                    for (int k = 0; k < endNeurons.Count; k++)
                    {
                        //находим внутреннюю ошибку конечного нейрона
                        double err = 0;
                        if (k == item.neuronNum) err = ((double)TrueImage.True - endNeurons[k].Out) * F_(endNeurons[k].WXsum); 
                        else err = ((double)TrueImage.False - endNeurons[k].Out) * F_(endNeurons[k].WXsum);
                        endNeurons[k].Err = err;

                        sumErr += Math.Abs(err);
                    }


                    //если ошибка мала, ничего не делаем, иначе помечаем, что необходим ещё 1 круг обучения
                    if (Math.Abs(sumErr) < 0.01 * endNeurons.Count) continue;
                    else needLearn = true;



                    //для конечных нейронов
                    for (int k = 0; k < endNeurons.Count; k++)
                    {
                        //меняем связи конечного нейрона
                        for (int j = 0; j < midNeurons.Count; j++)
                        {
                            endNeurons[k].W[j] = endNeurons[k].W[j] + endNeurons[k].Err * midNeurons[j].Out * h;
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
                            midNeurons[j].W[i] = midNeurons[j].W[i] + midNeurons[j].Err * item.x[i] * h;
                        }

                    }


                }

            }


        }

    }
}

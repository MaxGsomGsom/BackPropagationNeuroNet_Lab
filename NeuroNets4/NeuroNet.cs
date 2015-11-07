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

        Random r;

        static int midNeuronsPerOutput = 10;
        static double maxErrTrue = 0.1;
        static double maxErrFalse = 0.5;
        static double learningSpeed = 1;

        List<Image> images;

        public NeuroNet(int sizeIn)
        {
            midNeurons = new List<Neuron>();
            images = new List<Image>();
            endNeurons = new List<Neuron>();
            this.sizeIn = sizeIn;
            this.sizeOut = 0;

            r = new Random((int)DateTime.Now.Ticks);

            //sizeMid = (sizeIn + sizeOut) / 2;
            sizeMid = sizeOut * midNeuronsPerOutput;
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
            sizeMid = sizeOut * midNeuronsPerOutput;
            midNeurons.Clear();
            for (int i = 0; i < sizeMid; i++)
            {
                Neuron neuron = new Neuron();
                neuron.InitW(sizeIn, r);
                midNeurons.Add(neuron);
            }

            for (int i = 0; i < (sizeOut-1); i++)
            {
                endNeurons[i].InitW(sizeMid, r);
            }

            Neuron neuron1 = new Neuron(value);
            neuron1.InitW(sizeMid, r);
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
        public string Recognize(double[] x, bool needReturn = true)
        {
            //выходы и входящие суммы средних нейронов
            for (int j = 0; j < midNeurons.Count; j++)
            {
                double WXsum = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    WXsum += midNeurons[j].W[i] * x[i];
                }
                midNeurons[j].OutDiff = F_(WXsum);
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
                endNeurons[k].OutDiff = F_(WXsum);
                endNeurons[k].Out = F(WXsum);
            }

            if (needReturn)
            {
                //получаем нейрон с положительным  наибольшим выходом
                double max = double.MinValue;
                //double min = double.MaxValue;
                int num = -1;
                for (int i = 0; i < endNeurons.Count; i++)
                {
                    double result = endNeurons[i].Out;
                    if (result >= max)
                    {
                        max = result;
                        num = i;
                    }
                    //if (result <= min) min = result;
                }

                if (max < (1-maxErrFalse)) return "Not recognized";

                return endNeurons[num].Name; 
            }

            return null;




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
        public void Learn(int neuronNum, double[] x)
        {

            bool needLearn = true;

            //добавляем в список образов
            images.Add(new Image(x, neuronNum));
            images.Reverse();

            while (needLearn)
            {
                needLearn = false;

                foreach (Image item in images)
                {

                    //вычисляем входные суммы и выходы нейронов
                    Recognize(item.x, false);


                    //для конечных нейронов
                    for (int k = 0; k < endNeurons.Count; k++)
                    {
                        //находим внешнюю ошибку конечного нейрона
                        double partErr = 0;
                        if (k == item.neuronNum) partErr = (1 - endNeurons[k].Out); 
                        else partErr = (0 - endNeurons[k].Out);

                        //если ошибка велика, потребуется ещё круг обучения
                        if (k == item.neuronNum && Math.Abs(partErr) > maxErrTrue) needLearn = true;
                        if (k != item.neuronNum && Math.Abs(partErr) > maxErrFalse) needLearn = true;

                        //находим внутреннюю ошибку конечного нейрона
                        endNeurons[k].Err = partErr * endNeurons[k].OutDiff;

                        //меняем связи конечного нейрона
                        for (int j = 0; j < midNeurons.Count; j++)
                        {
                            endNeurons[k].W[j] = endNeurons[k].W[j] + endNeurons[k].Err * midNeurons[j].Out * learningSpeed;
                        }
                    }



                    //для внутренних нейронов
                    for (int j = 0; j < midNeurons.Count; j++)
                    {
                        //находим сумму ошибок конечных нейронов
                        double errBack = 0;
                        for (int k = 0; k < endNeurons.Count; k++)
                        {
                            errBack += endNeurons[k].W[j] * endNeurons[k].Err;
                        }


                        //находим внутреннюю ошибку нейрона
                        midNeurons[j].Err = errBack * midNeurons[j].OutDiff;

                        //меняем связи нейрона
                        for (int i = 0; i < midNeurons.Count; i++)
                        {
                            midNeurons[j].W[i] = midNeurons[j].W[i] + midNeurons[j].Err * item.x[i] * learningSpeed;
                        }

                    }

                   


                }

            }


        }

    }
}

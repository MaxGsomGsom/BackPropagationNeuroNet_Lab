using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace NeuroNets6
{
    public partial class MainForm : Form
    {
        NeuroNet net; //сеть
        int size = 200; //размер квадрата
        Graphics picBoxG; //картинка для вывода на экран
        Bitmap img; 
        Graphics workG; //картинка для считывания

        public MainForm()
        {
            InitializeComponent();

            net = new NeuroNet(size*size);

            img = new Bitmap(size, size);
            workG = Graphics.FromImage(img);
            workG.Clear(Color.White);

            picBoxG = pictureBox1.CreateGraphics();

        }

        //распознать
        private void button1_Click(object sender, EventArgs e)
        {
            string result = net.Recognize(ReadFromField());

            MessageBox.Show(result);
        }

        //рисование
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                picBoxG.FillEllipse(Brushes.Black, e.X, e.Y, 15, 15);
                picBoxG.DrawEllipse(Pens.Black, e.X, e.Y, 15, 15);

                workG.FillEllipse(Brushes.Black, e.X, e.Y, 15, 15);
                workG.DrawEllipse(Pens.Black, e.X, e.Y, 15, 15);
            }
        }


        //чтение массива из картинки
        private int[] ReadFromField()
        {
            int[] input = new int[size * size];
            
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Color c = img.GetPixel(x, y);

                    if (c.ToArgb() == Color.Black.ToArgb()) input[x * size + y] = 1;
                    else input[x * size + y] = 0;
                }
            }

            return input;
        }

        //очистка экрана
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                picBoxG.Clear(Color.White);
                workG.Clear(Color.White);
            }
        }

        //сохранение
        private void button2_Click(object sender, EventArgs e)
        {
            net.SaveNeurons();
        }


        //загрузка
        private void button3_Click(object sender, EventArgs e)
        {
            net.LoadNeurons();
            listBox1neurons.Items.Clear();

            for (int i = 0; i < net.Count; i++)
            {
                listBox1neurons.Items.Add(net.GetNeuronName(i));
            }

        }

        //добавить нейрон
        private void button4add_Click(object sender, EventArgs e)
        {
            net.AddNeuron(textBox1value.Text);
            listBox1neurons.Items.Add(textBox1value.Text);
        }


        //обучение
        private void button4learn_Click(object sender, EventArgs e)
        {
            int pos = listBox1neurons.SelectedIndex;

            if (pos >= 0)
            {
                net.Learn(pos, ReadFromField());
                picBoxG.Clear(Color.White);
                workG.Clear(Color.White);
            }
        }
    }
}

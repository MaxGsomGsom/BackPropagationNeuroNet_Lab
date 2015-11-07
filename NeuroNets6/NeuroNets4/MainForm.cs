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
using System.IO;
using System.Globalization;

namespace NeuroNets6
{
    public partial class MainForm : Form
    {
        NeuroNet net; //сеть
        int size = 100; //размер квадрата
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

            picBoxG.DrawString("Result:", new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold), Brushes.Black, new PointF(10, 170));
            picBoxG.DrawString(result, new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold), Brushes.Red, new PointF(10, 200));
            //MessageBox.Show(result);
        }

        //рисование
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                picBoxG.FillEllipse(Brushes.Black, e.X - 7, e.Y - 7, 15, 15);
                picBoxG.DrawEllipse(Pens.Black, e.X - 7, e.Y - 7, 15, 15);

                workG.FillEllipse(Brushes.Black, e.X - 7 - 50, e.Y - 7 - 50, 15, 15);
                workG.DrawEllipse(Pens.Black, e.X - 7 - 50, e.Y - 7 - 50, 15, 15);
            }
        }


        //чтение массива из картинки
        private double[] ReadFromField()
        {
            double[] input = new double[size * size];
            
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
                picBoxG.DrawRectangle(Pens.Gray, 49, 49, 101, 101);
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

            //img.Save("test.bmp");
            //return;

            if (pos >= 0)
            {
                net.Learn(pos, ReadFromField());
                picBoxG.Clear(Color.White);
                workG.Clear(Color.White);
                picBoxG.DrawRectangle(Pens.Gray, 49, 49, 101, 101);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                Random r = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < 100; i++)
                {
                    int one = r.Next(50, 150);
                    int two = r.Next(50, 150);

                    picBoxG.FillRectangle(Brushes.Black, one, two, 2, 2);
                    workG.FillRectangle(Brushes.Black, one, two, 2, 2);

                    one = r.Next(50, 150);
                    two = r.Next(50, 150);

                    picBoxG.FillRectangle(Brushes.White, one, two, 2, 2);
                    workG.FillRectangle(Brushes.White, one, two, 2, 2);
                }
                return;

            }



            //if (e.KeyCode == Keys.Shift) return;
            string s = (new KeysConverter()).ConvertToString(null, new CultureInfo("ru"), e.KeyCode);
            //if (e.Shift == true) return;

            picBoxG.DrawString(s, new Font(FontFamily.GenericMonospace, 80, FontStyle.Bold), Brushes.Black, new PointF(40, 40));
            workG.DrawString(s, new Font(FontFamily.GenericMonospace, 80, FontStyle.Bold), Brushes.Black, new PointF(-10, -10));

        }



        private static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    Int32 avgR = 0, avgG = 0, avgB = 0;
                    Int32 blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (Int32 x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (Int32 y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (Int32 x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        for (Int32 y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return blurred;
        }

    }
}

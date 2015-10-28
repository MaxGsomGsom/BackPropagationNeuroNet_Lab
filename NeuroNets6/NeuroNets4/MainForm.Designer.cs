namespace NeuroNets6
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1recognize = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2save = new System.Windows.Forms.Button();
            this.button3load = new System.Windows.Forms.Button();
            this.listBox1neurons = new System.Windows.Forms.ListBox();
            this.button4learn = new System.Windows.Forms.Button();
            this.button4add = new System.Windows.Forms.Button();
            this.textBox1value = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1recognize
            // 
            this.button1recognize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button1recognize.Location = new System.Drawing.Point(32, 294);
            this.button1recognize.Margin = new System.Windows.Forms.Padding(4);
            this.button1recognize.Name = "button1recognize";
            this.button1recognize.Size = new System.Drawing.Size(157, 35);
            this.button1recognize.TabIndex = 25;
            this.button1recognize.Text = "Распознать";
            this.button1recognize.UseVisualStyleBackColor = true;
            this.button1recognize.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // button2save
            // 
            this.button2save.Location = new System.Drawing.Point(410, 61);
            this.button2save.Name = "button2save";
            this.button2save.Size = new System.Drawing.Size(144, 41);
            this.button2save.TabIndex = 27;
            this.button2save.Text = "Сохранить в файл";
            this.button2save.UseVisualStyleBackColor = true;
            this.button2save.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3load
            // 
            this.button3load.Location = new System.Drawing.Point(410, 12);
            this.button3load.Name = "button3load";
            this.button3load.Size = new System.Drawing.Size(144, 43);
            this.button3load.TabIndex = 28;
            this.button3load.Text = "Загрузить из файла";
            this.button3load.UseVisualStyleBackColor = true;
            this.button3load.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox1neurons
            // 
            this.listBox1neurons.FormattingEnabled = true;
            this.listBox1neurons.ItemHeight = 16;
            this.listBox1neurons.Location = new System.Drawing.Point(252, 127);
            this.listBox1neurons.Name = "listBox1neurons";
            this.listBox1neurons.Size = new System.Drawing.Size(302, 148);
            this.listBox1neurons.TabIndex = 29;
            // 
            // button4learn
            // 
            this.button4learn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button4learn.Location = new System.Drawing.Point(324, 294);
            this.button4learn.Name = "button4learn";
            this.button4learn.Size = new System.Drawing.Size(144, 35);
            this.button4learn.TabIndex = 30;
            this.button4learn.Text = "Обучить нейроны";
            this.button4learn.UseVisualStyleBackColor = true;
            this.button4learn.Click += new System.EventHandler(this.button4learn_Click);
            // 
            // button4add
            // 
            this.button4add.Location = new System.Drawing.Point(252, 61);
            this.button4add.Name = "button4add";
            this.button4add.Size = new System.Drawing.Size(122, 41);
            this.button4add.TabIndex = 31;
            this.button4add.Text = "Добавить класс";
            this.button4add.UseVisualStyleBackColor = true;
            this.button4add.Click += new System.EventHandler(this.button4add_Click);
            // 
            // textBox1value
            // 
            this.textBox1value.Location = new System.Drawing.Point(252, 31);
            this.textBox1value.Name = "textBox1value";
            this.textBox1value.Size = new System.Drawing.Size(122, 22);
            this.textBox1value.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 33;
            this.label1.Text = "Название класса:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-9, -25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 403);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 356);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1value);
            this.Controls.Add(this.button4add);
            this.Controls.Add(this.button4learn);
            this.Controls.Add(this.listBox1neurons);
            this.Controls.Add(this.button3load);
            this.Controls.Add(this.button2save);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1recognize);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NeuroNets6";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1recognize;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button2save;
        private System.Windows.Forms.Button button3load;
        private System.Windows.Forms.ListBox listBox1neurons;
        private System.Windows.Forms.Button button4learn;
        private System.Windows.Forms.Button button4add;
        private System.Windows.Forms.TextBox textBox1value;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}


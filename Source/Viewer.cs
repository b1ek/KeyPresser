using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeyPresser
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
            this.Show();
            Thread thr = new Thread(() => { updatePic(); });
            thr.Start();
        }

        private bool optimize = true;
        private void updatePic()
        {
            while (true)
            {
                if (optimize)
                    Thread.Sleep(150);
                Program.form1.initPresser();
                pictureBox1.Image = KeyPresser.getScreen();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //updatePic();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //updatePic();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileDialog fd = new SaveFileDialog();
            fd.InitialDirectory = Environment.CurrentDirectory;
            fd.Title = "Выберите куда сохранить файл";
            fd.Filter = "Изображение (*.png)| *.png";
            fd.ShowDialog();
            try {
                pictureBox1.Image.Save(fd.FileName);
            } catch (Exception) { /*ignored*/ }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            optimize = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox2.Checked;
        }
    }
}

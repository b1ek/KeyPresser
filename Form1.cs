using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyPresser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            x1label.Maximum = Screen.PrimaryScreen.Bounds.Width;
            y1label.Maximum = Screen.PrimaryScreen.Bounds.Height;
            x2label.Maximum = Screen.PrimaryScreen.Bounds.Width;
            y2label.Maximum = Screen.PrimaryScreen.Bounds.Height;

            x1label.Value = RegistryConfig.x1;
            y1label.Value = RegistryConfig.y1;
            x2label.Value = RegistryConfig.x2;
            y2label.Value = RegistryConfig.y2;

            KeyPresser.areaChanged += new EventHandler(performActions);
            this.FormClosed += (s, e) => { Environment.Exit(0); };
            setSmileGray(true);
        }
        Keys keyToPress = Keys.F2;

        private bool isCapture = false;
        private void start_capture()
        {
            isCapture = true;
            firstPos = true;
        }

        private bool firstPos = true;
        private bool settingKeyToPress = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData.HasFlag(Keys.Control) && keyData.HasFlag(Keys.H) && isCapture)
            {
                if (firstPos) // first pos
                {
                    x1label.Value = MousePosition.X;
                    y1label.Value = MousePosition.Y;
                    firstPos = false;
                } else
                {
                    x2label.Value = MousePosition.X;
                    y2label.Value = MousePosition.Y;
                    isCapture = false;
                    RegistryConfig.x1 = (int)x1label.Value;
                    RegistryConfig.y1 = (int)y1label.Value;
                    RegistryConfig.x2 = (int)x2label.Value;
                    RegistryConfig.y2 = (int)y2label.Value;
                }
            }
            if (settingKeyToPress)
            {
                keyToPress = keyData;
                settingKeyToPress = false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Поставьте мышку в 1 угол\nи нажмите CTRL+H.\nПоставьте во второй угол и нажмите опять.\nНе забудьте держать фокус на главном окне.\n\nВы можете закрыть это окно.");
            start_capture();
        }

        public void setSmileGray(bool status)
        {
            if (status)
            {
                Bitmap bitmap = (Bitmap) smile.Image;
                int wid = bitmap.Width;
                for (int x = 0; x < bitmap.Width; x++) {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        Color pix = bitmap.GetPixel(x, y);
                        int mid = (pix.R + pix.G + pix.B) / 3;
                        bitmap.SetPixel(x, y, Color.FromArgb(pix.A, mid, mid, mid));
                    }
                }
                smile.Image = bitmap;
                System.GC.Collect();
            } else
            {
                smile.Image = global::KeyPresser.Properties.Resources.utopia_smiley;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
            if (!checkBox1.Checked && !checkBox2.Checked)
            { button4.Enabled = false; }
            else button4.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = checkBox2.Checked;
            if (!checkBox1.Checked && !checkBox2.Checked)
            { button4.Enabled = false; }
            else button4.Enabled = true;
        }

        private string pathtofile = "";
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            DialogResult res = of.ShowDialog();
            pathtofile = of.FileName;
            MessageBox.Show($"Выбран файл: {pathtofile}");
            string[] spl = of.FileName.Split("\\");
            label6.Text = spl[spl.Length - 1];
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void smile_Click(object sender, EventArgs e)
        {
            setSmileGray(true);
        }

        public void initPresser()
        {
            KeyPresser.x1 = (int)x1label.Value;
            KeyPresser.y1 = (int)y1label.Value;
            KeyPresser.x2 = (int)x2label.Value;
            KeyPresser.y2 = (int)y2label.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            initPresser();
            KeyPresser.getScreen().Save("a.png");
            System.GC.Collect();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Viewer view = new Viewer();
            view.Show();
        }

        private bool isWorking = false;
        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked && pathtofile == "")
            {
                MessageBox.Show("Выберите файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!isWorking)
            {
                isWorking = true;
                initPresser();
                KeyPresser.run();
                button4.Text = "Стоп!";
                label7.Text = "Оно работает!";
                setSmileGray(false);
            } else
            {
                isWorking = false;
                KeyPresser.stop();
                button4.Text = "Запуск";
                label7.Text = "Оно не работает!";
                setSmileGray(true);
            }
        }

        private void performActions(object sender, EventArgs e)
        {
            if (checkBox1.Checked) {
                SendKeys.SendWait(textBox1.Text);
                
            }
            if (checkBox2.Checked)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd",
                $"/c start {pathtofile}".Replace("&", "^&"))
                { CreateNoWindow = true });
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd",
                "/c start https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=net-5.0#remarks".Replace("&", "^&"))
                { CreateNoWindow = true });
        }
    }
}

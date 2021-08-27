using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeyPresser
{
    class KeyPresser
    {
        public static int x1 = -1;
        public static int y1 = -1;
        public static int x2 = -1;
        public static int y2 = -1;
        public static Bitmap getScreen()
        {
            int width = x1 - x2;
            int height= y1 - y2;

            if (width < 0)
                width = width * -1;
            if (height < 0)
                height = height*-1;

            Bitmap bit = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(x1, y1, x2, y2);
            Graphics graph = Graphics.FromImage(bit);
            graph.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            graph = null;
            System.GC.Collect();
            return bit;
        }
        static string startHash = "";
        static bool running = false;
        static Thread thr;
        public static void run()
        {
            running = true;
            if (x1 < 0 | y1 < 0 | x2 < 0 | y2 < 0)
            {
                throw new Exception("Какой то идиот лез в программу.");
            }
            byte[] img = BitmapToBytes(getScreen());
            byte[] hash = Cryptography.sha1(img);
            img = null;
            startHash = BitConverter.ToString(hash).Replace("-", "").ToLower();
            //Clipboard.SetText(BitConverter.ToString(startHash));
            //MessageBox.Show(BitConverter.ToString(startHash));
            thr = new Thread(() => { iterate(); });
            thr.Start();
        }
        static void iterate()
        {
            while (true)
            {
                if (!running)
                    break;
                Thread.Sleep(150);
                string hash = Cryptography.ssha1(BitmapToBytes(getScreen())).Replace("-", "").ToLower();
                string hash1= startHash;
                if (hash.Equals(hash1) == false)
                {
                    OnAreaChanged(null, new EventArgs());
                }
                //MessageBox.Show(Encoding.UTF8.GetString(hash));
                startHash = hash;
                hash = null;
                hash1= null;
                System.GC.Collect();
            }
        }
        public static void stop()
        {
            running = false;
            try
            {
                thr.Abort();
            }
            catch (Exception) { /*ignored*/ }
        }
        public static byte[] ImageToBytes(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public static byte[] BitmapToBytes(Bitmap bit)
        {
            using (var stream = new MemoryStream())
            {
                bit.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public static event EventHandler areaChanged;
        private static void OnAreaChanged(object sender, EventArgs e) {
            areaChanged?.Invoke(sender, e);
        }

    }
}

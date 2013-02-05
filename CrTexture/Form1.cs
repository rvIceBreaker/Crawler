using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SdlDotNet;
using SdlDotNet.Graphics;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Threading;

namespace CrTexture
{
    public partial class Form1 : Form
    {
        Graphics g;

        Image image, original;

        Surface texture;

        double imgScale = 1.0;

        bool mAnimated, mUsesActivities, mImageLoaded;

        public Form1()
        {
            InitializeComponent();

            g = this.CreateGraphics();

            this.FormClosing += new FormClosingEventHandler(Form1_Close);

            Thread thread = new Thread(Form1_Paint);

            thread.Start();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint()
        {
            while (!this.IsDisposed)
            {
                if(pictureBox1.Image != image)
                    pictureBox1.Image = image;
            }
        }

        private void Form1_Close(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_Img.ShowDialog();
        }

        private void open_Img_FileOk(object sender, CancelEventArgs e)
        {
            image = Image.FromFile(open_Img.FileName);
            original = image;

            pictureBox1.Image = image;

            toolStripLabel1.Text = image.Width + ", " + image.Height;            
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_Tex.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_Tex.ShowDialog();
        }

        private void save_Tex_FileOk(object sender, CancelEventArgs e)
        {
            Stream stream = File.OpenWrite(save_Tex.FileName);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, texture);
            stream.Close();
        }

        private void open_Tex_OK(object sender, CancelEventArgs e)
        {
            Stream stream = File.Open(open_Tex.FileName, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            texture = (Surface)formatter.Deserialize(stream);

            stream.Close();

            image = (Image)texture.Bitmap;
            original = image;

            pictureBox1.Image = image;

            toolStripLabel1.Text = image.Width + ", " + image.Height;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void ImageContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            string text = textBox1.Text;

            text = text.Replace(' ', '_');
            text = text.Replace('-', '_');

            textBox2.Text = text;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void geometryTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_Img.ShowDialog();

            image = Bitmap.FromFile(open_Img.FileName);
            original = image;

            Properties.Settings.Default.mAnimated = false;
            Properties.Settings.Default.mUsesActivities = false;
            Properties.Settings.Default.mImageLoaded = true;
            Properties.Settings.Default.Save();
        }

        private void checkedListBox1_ItemCheck(object sender, EventArgs e)
        {
            System.Windows.Forms.ItemCheckEventArgs args = (System.Windows.Forms.ItemCheckEventArgs)e;

            switch (checkedListBox1.GetItemText(checkedListBox1.Items[args.Index]).ToLower())
            {
                case "flip x":
                    image = FlipImage(image, true, false);
                    break;

                case "flip y":
                    image = FlipImage(image, false, true);
                    break;
            }
        }

        public static Image FlipImage(Image image, bool flipHorizontally, bool flipVertically)
        {
            Bitmap flippedImage = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(flippedImage))
            {
                //Matrix transformation
                Matrix m = null;
                if (flipVertically && flipHorizontally)
                {
                    m = new Matrix(-1, 0, 0, -1, 0, 0);
                    m.Translate(flippedImage.Width, flippedImage.Height, MatrixOrder.Append);
                }
                else if (flipVertically)
                {
                    m = new Matrix(1, 0, 0, -1, 0, 0);
                    m.Translate(0, flippedImage.Height, MatrixOrder.Append);
                }
                else if (flipHorizontally)
                {
                    m = new Matrix(-1, 0, 0, 1, 0, 0);
                    m.Translate(flippedImage.Width, 0, MatrixOrder.Append);
                }

                //Draw
                g.Transform = m;
                g.DrawImage(image, 0, 0);

                //clean up
                m.Dispose();
            }

            return (Image)flippedImage;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap img, newImg;

        public Form1()
        {
            InitializeComponent();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            newImg = new Bitmap(img.Width, img.Height);
            
            for(int i = 0; i < img.Width; i++)
                for(int j = 0;  j < img.Height; j++)
                    newImg.SetPixel(i, j, img.GetPixel(i, j));
            pictureBox2.Image = newImg;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    int grey = (int)(pxl.R + pxl.G + pxl.B) / 3;
                    newImg.SetPixel(i, j, Color.FromArgb(grey, grey, grey));
                }
            pictureBox2.Image = newImg;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    newImg.SetPixel(i, j, Color.FromArgb(255 - pxl.R, 255 - pxl.G, 255 - pxl.B));
                }
            pictureBox2.Image = newImg;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    int grey = (int)(pxl.R + pxl.G + pxl.B) / 3;
                    newImg.SetPixel(i, j, Color.FromArgb(grey, grey, grey));
                }

            int[] histogramData = new int[256];

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = newImg.GetPixel(i, j);
                    histogramData[pxl.R]++;
                }

            Bitmap histoGraph = new Bitmap(256, 800);

            for (int i = 0; i < 256; i++)
                for (int j = 0; j < 800; j++)
                    histoGraph.SetPixel(i, j, Color.White);

            for (int i = 0; i < 256; i++)
                for (int j = 0; j < Math.Min(histogramData[i] / 5, 800); j++)
                    histoGraph.SetPixel(i, 799 - j, Color.Gray);

            pictureBox2.Image = histoGraph;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    int sepiaRed = (int)((0.393 * pxl.R) + (0.769 * pxl.G) + (0.189 * pxl.B));
                    sepiaRed = sepiaRed > 255 ? 255 : sepiaRed;
                    int sepiaGreen = (int)((0.349 * pxl.R) + (0.686 * pxl.G) + (0.168 * pxl.B));
                    sepiaGreen = sepiaGreen > 255 ? 255 : sepiaGreen;
                    int sepiaBlue = (int)((0.272 * pxl.R) + (0.534 * pxl.G) + (0.131 * pxl.B));
                    sepiaBlue = sepiaBlue > 255 ? 255 : sepiaBlue;

                    newImg.SetPixel(i, j, Color.FromArgb(sepiaRed, sepiaGreen, sepiaBlue));
                }
            }

            pictureBox2.Image = newImg;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
                saveFileDialog1.ShowDialog();
            else
                MessageBox.Show("There's nothing to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            newImg.Save(saveFileDialog1.FileName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            img = new Bitmap(openFileDialog1.FileName);

            pictureBox1.Image = img;
        }
    }
}

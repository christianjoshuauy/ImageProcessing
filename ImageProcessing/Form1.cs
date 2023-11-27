using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;
using static System.Net.Mime.MediaTypeNames;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap img, newImg, subtracted;
        Device selectedDevice;

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

        private void brightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            int val = trackBar1.Value;
            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    if(val >= 0)
                        newImg.SetPixel(i, j, Color.FromArgb(Math.Min(pxl.R + val, 255), Math.Min(pxl.G + val, 255), Math.Min(pxl.B + val, 255)));
                    else
                        newImg.SetPixel(i, j, Color.FromArgb(Math.Max(pxl.R + val, 0), Math.Max(pxl.G + val, 0), Math.Max(pxl.B + val, 0)));
                }
            pictureBox2.Image = newImg;
        }

        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            int val = trackBar1.Value;
            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    if (val >= 0)
                        newImg.SetPixel(i, j, Color.FromArgb(Math.Min(pxl.R + val, 255), Math.Min(pxl.G + val, 255), Math.Min(pxl.B + val, 255)));
                    else
                        newImg.SetPixel(i, j, Color.FromArgb(Math.Max(pxl.R + val, 0), Math.Max(pxl.G + val, 0), Math.Max(pxl.B + val, 0)));
                }
            pictureBox2.Image = newImg;
        }

        private void flipHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
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
                    newImg.SetPixel(img.Width - 1 - i, j, img.GetPixel(i, j));
                }
            pictureBox2.Image = newImg; 
        }

        private void flipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
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
                    newImg.SetPixel(i, img.Height - 1 - j, img.GetPixel(i, j));
                }
            pictureBox2.Image = newImg;
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            float angleRadians = (float)(30 * Math.PI / 180);
            int xCenter = (int)(img.Width / 2);
            int yCenter = (int)(img.Height / 2);
            int x0, y0, xs, ys;
            float cosA = (float)Math.Cos(angleRadians);
            float sinA = (float)Math.Sin(angleRadians);
            newImg = new Bitmap(img.Width, img.Height);

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    x0 = i - xCenter;
                    y0 = j - yCenter;
                    xs = (int)(x0 * cosA + y0 * sinA) + xCenter;
                    ys = (int)(-x0 * sinA + y0 * cosA) + yCenter;
                    xs = Math.Max(Math.Min(img.Width - 1, xs), 0);
                    ys = Math.Max(Math.Min(img.Height - 1, ys), 0);
                    newImg.SetPixel(i, j, img.GetPixel(xs, ys));
                }
            pictureBox2.Image = newImg;
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Please open an image to process.", "Error");
                return;
            }

            int targetWidth = 1920;
            int targetHeight = 1080;
            newImg = new Bitmap(targetWidth, targetHeight);

            for (int i = 0; i < targetWidth; i++)
                for (int j = 0; j < targetHeight; j++)
                {
                    newImg.SetPixel(i, j, img.GetPixel(i * img.Width / targetWidth, j * img.Height / targetHeight));
                }
            pictureBox2.Image = newImg;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subtracted = new Bitmap(img.Width, img.Height);
            Color myGreen = Color.FromArgb(0, 255, 0);
            int greyGreen = (myGreen.R + myGreen.G + myGreen.B) / 3;
            int threshold = 5;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pxl = img.GetPixel(i, j);
                    Color backPxl = newImg.GetPixel(i, j);
                    int grey = (pxl.R + pxl.G + pxl.B) / 3;
                    int subtractVal = Math.Abs(grey - greyGreen);
                    if (subtractVal < threshold)
                    {
                        subtracted.SetPixel(i, j, backPxl);
                    }
                    else
                    {
                        subtracted.SetPixel(i, j, pxl);
                    }
                }
            }
            pictureBox3.Image = subtracted;
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Device[] devices = DeviceManager.GetAllDevices();
            if (devices.Length > 0)
            {
                selectedDevice = DeviceManager.GetDevice(0);
                selectedDevice.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("No webcam device found.");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedDevice != null)
            {
                selectedDevice.Stop();
            }
            else
            {
                MessageBox.Show("No webcam devices found.");
            }
        }

        private void openBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            newImg = new Bitmap(openFileDialog2.FileName);

            pictureBox2.Image = newImg;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            img = new Bitmap(openFileDialog1.FileName);

            pictureBox1.Image = img;
        }
    }
}

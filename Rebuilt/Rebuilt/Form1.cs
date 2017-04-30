using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using System.Reflection;



namespace Rebuilt
{
    public partial class Form1 : Form
    {
        Bitmap originalImage;
        Color c, b;
        int i, j, x1, y1, extendedx, extendedy, xa, boundedw, boundedh, sizex, sizey, xmax, ymax;

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            var returnValue = openDialog.ShowDialog();
            if (returnValue == DialogResult.OK)
            {
                originalImage = new Bitmap(openDialog.FileName);
                pictureBox1.Image = originalImage;

                // pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
                Grayscale GrayImage = new Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap graycolor = GrayImage.Apply(originalImage);

                Threshold BinaryImageE = new Threshold();
                Bitmap binaryimage = BinaryImageE.Apply(graycolor);
                pictureBox2.Image = graycolor;

                Console.WriteLine(binaryimage.Width);
                c = binaryimage.GetPixel(1, 1);
                Console.WriteLine(c.R);
                for (i = 1; i < graycolor.Height; i++)
                {
                    for (j = 1; j < graycolor.Width; j++)
                    {
                        c = graycolor.GetPixel(j, i);
                      //  Console.WriteLine("mat[{0},{1}] = {2}", j, i, c.R);
                        if (c.R == 0 | c.G == 0 | c.B == 0)
                            {

                           extendedy = i;
                            //xa = j;
                             break;
                            }
                        


                    }
                }
               // Console.ReadKey();


                //Console.WriteLine(yB);
                //Console.WriteLine(xa);

                for (j = 1; j < graycolor.Width; j++)
                {
                    for (i = 1; i < graycolor.Height; i++)
                    {
                        c = graycolor.GetPixel(j, i);
                        if (c.R == 0 | c.G == 0 | c.B == 0)
                        {
                            extendedx = j;
                            break;
                        }


                    }
                }
               // Console.WriteLine(extendedx);

                for (i = graycolor.Height-1; i >=1; i--)
                {
                    for (j = graycolor.Width-1; j >=1; j--)
                    {
                        c = graycolor.GetPixel(j, i);
                        if (c.R == 0 | c.G == 0 | c.B == 0)
                        {
                            y1 = i;
                            break;
                        }


                    }
                }
               // Console.WriteLine(y);


                for (j = graycolor.Width-1; j >=1; j--)
                {
                    for (i = graycolor.Height-1; i >=1; i--)
                    {
                        c = graycolor.GetPixel(j, i);
                        if (c.R == 0 | c.G == 0 | c.B==0 )
                        {
                            x1 = j;
                            break;
                        }


                    }
                }
               // Console.WriteLine(x);
                sizex = extendedx - x1;
                sizey = extendedy - y1;

                //int[] pixelx = new int[sizey];
                //int[] pixelx1 = new int[sizey];
                //int prod1 = 0, prod2 = 0;
                //int[,] points = new int[30,30];


                //Bitmap boundedimage = (Bitmap)originalImage.Clone();
                Bitmap boundedimage1 = new Bitmap(sizex, sizey);
                Console.Write(sizex);
                //Console.Write(extendedx);
                Console.Write(sizey);
                //Console.Write(extendedy);
                for (i = x1,boundedw=0;i<extendedx;i++,boundedw++)
                {
                    for(j=y1,boundedh=0;j<extendedy;j++,boundedh++)
                    {
                        
                        c = graycolor.GetPixel(i, j);
                        boundedimage1.SetPixel(boundedw, boundedh, Color.FromArgb(c.R,c.G,c.B));
                        
                    }
                }
                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap boundedimage = filter.Apply(boundedimage1);
                Threshold th = new Threshold();
                th.ApplyInPlace(boundedimage);
                pictureBox2.Image = boundedimage;

                /*CannyEdgeDetector Edgeimage = new CannyEdgeDetector();
                Bitmap edge = Edgeimage.Apply(boundedimage);
                pictureBox2.Image = edge;*/


                //for(i=1;i<graycolor.Height;i++)
                //{
                //    Console.WriteLine(tempy[i]);
                //}


                /*---------------------SEGMENTATION--------------------------*/
                xmax = boundedimage.Width;
                ymax = boundedimage.Height;
                int[] xaxis = new int[xmax];
                int[] yaxis = new int[ymax];
                int[] tempx = new int[xmax];
                int[] tempy = new int[ymax];
                int y, x;
                int iy = 1;
                int ix = 1;
                yaxis[0] = 0;
                //for (y = 1; y < ymax; y++)
                //{
                //    for (x = 1; x < xmax; x++)
                //    {
                //        if (boundedimage.GetPixel(x, y).Name == "ff000000")
                //        {
                //            tempy[iy++] = y;
                //            break;
                //        }
                //    }
                //}
                //i = 1;
                //for (int p = 1; p < ymax - 1 ; p++)
                //{

                //    if ((tempy[p - 1] != tempy[p] - 1) || (tempy[p] + 1 != tempy[p + 1]))
                //    {
                //        yaxis[i++] = tempy[p] + 1;

                //    }
                //}

                yaxis[0] = 0;
                yaxis[1] = boundedimage.Height;
                


                for (x = 1; x < xmax; x++)
                {
                    for (y = 1; y < ymax; y++)
                    {
                        if (boundedimage.GetPixel(x, y).Name == "ff000000")
                        {
                            tempx[ix++] = x;
                            break;
                        }
                    }
                }
                i = 1;
                for (int p = 1; p < xmax; p++)
                {

                    if ((tempx[p - 1] != tempx[p] - 1) || (tempx[p] + 1 != tempx[p + 1]))
                    {
                        xaxis[i] = tempx[p] + 1;
                        i++;
                    }
                }

                Console.WriteLine("helllooooo");
                for (i = 0; i < ymax; i++)
                {
                    Console.WriteLine(yaxis[i] + " " + xaxis[i]);
                }
                //  Console.WriteLine(xaxis[2]+" "+ yaxis[2] + " " + (xaxis[3] - xaxis[2] )+ " " + (yaxis[3] - yaxis[2]));


                int k;
                Bitmap[] croppedImg = new Bitmap[30];
                Bitmap temp;
                Console.WriteLine("helllooooo-check");
                Console.WriteLine(xaxis.Length);

                for (i = 0, j = 1, k = 0; k < 2 ; k++, i = i + 2, j = j + 2)
                {
                   
                    Console.WriteLine(xaxis[i] + " " + yaxis[0] + " " + (xaxis[j] - xaxis[i]) + " " + (yaxis[1] - yaxis[0]));



                    Crop filter1 = new Crop(new Rectangle(xaxis[i], yaxis[0], (xaxis[j] - xaxis[i]), (yaxis[1] - yaxis[0])));
                    temp = filter1.Apply(boundedimage);
                    croppedImg[k] = temp;
                   
                   
                }
                pictureBox3.Image = croppedImg[0];

                pictureBox4.Image = croppedImg[1];
                pictureBox5.Image = croppedImg[2];
               // pictureBox6.Image = croppedImg[3];
                //pictureBox6.Image = croppedImg[4];

                for(i=0;i<2;i++)
                {
                    croppedImg[i].Save(@"I:\Works\Machine Learning - OCR\Samples\Sample" + i + ".jpg");
                }
//---------------------------ZONING-------------------------------------
                ResizeBilinear filter2 = new ResizeBilinear(60, 90);
                Bitmap resize = filter2.Apply(croppedImg[0]);
                pictureBox6.Image = resize;

                int count=0,m,n;
                int[,] zone = new int[60, 90];
                for (i=0;i<60;i=i+10)
                {
                    for(j=0;j<90;j=j+10)
                    {
                        
                        for(int a=0;a<10;a++)
                        {
                            for(int b=0;b<10;b++)
                            {
                                c = resize.GetPixel(i+a, j+b);
                                if (c.R == 0)
                                {
                                    count++;
                                }
                            }

                            

                        }
                        m = i / 10; n = j / 10;
                        zone[m, n] = count;
                        count = 0;
                       
                    }
                }
                for (i = 0; i < 6; i++)
                    for (j = 0; j < 9; j++)
                    {
                        Console.WriteLine("zone[{0},{1}] = {2}" ,i,j,zone[i,j]);
                    } 



            }
           




        }
        

    }
}

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
using System.IO;
using SVM;
using System.Runtime.InteropServices;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Speech.Synthesis;



namespace Rebuilt
{
    public partial class Form1 : Form
    {
        Bitmap originalImage;
        Color c, b;
        int i, j, x1, y1, extendedx, extendedy, xa, boundedw, boundedh, sizex, sizey, xmax, ymax;

        private void button5_Click(object sender, EventArgs e)
        {
             List<Bitmap> temp_bound = new List<Bitmap>();
             Bitmap boundedimage1 = new Bitmap("E://Works//Machine Learning - OCR//raw//graycolor.jpg");
             temp_bound.Add(boundedimage1);

             List<Bitmap> boundedimage = BoundImage(temp_bound);
             List<float[]> test_feature = Zone(boundedimage);
             write_testfile(test_feature);
            TestClassifier();
        }

        private void button4_Click(object sender, EventArgs e)
        {
             Bitmap testimage1 = new Bitmap("E://Works//Machine Learning - OCR//raw//testimage.png");
             List<Bitmap> testimage = new List<Bitmap>();
             testimage.Add(testimage1);
             List<Bitmap> testbound = BoundImage(testimage);
             List<float[]> testzone = Zone(testbound);
             List<int> labels = new List<int>();
             labels.Add('0');
             write_file(testzone, labels);
            

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        //    char[] charcters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        //    try
        //{   // Open the text file using a stream reader.
        //    using (StreamReader sr = new StreamReader("result"))
        //    {
	       // // Read the stream to a string, and write the string to the console.
        //        string line = sr.ReadToEnd();
        //        int x = 0;

        //        Int32.TryParse(line, out x);
        //        Console.WriteLine(line);
                
        //    }
        //}
        //catch (Exception w)
        //{
        //    Console.WriteLine("The file could not be read:");
        //    Console.WriteLine(w.Message);
        //}
        //    string c = Char.ConvertFromUtf32(65);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string line;
            List<string> text = new List<string>();
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;     // -10...10

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("result"))
                {
                    // Read the stream to a string, and write the string to the console.
                    char temp;
                    char[] characters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                    line = sr.ReadToEnd();
                    for (int c = 0; c < line.Length; c = c + 2)
                    {
                        char temp2 = line[c];
                        temp = characters[(line[c]) - 49];
                        text.Add(temp.ToString());
                    }
                    foreach (var ch in text)
                    {
                        Console.WriteLine(ch);
                        richTextBox1.AppendText(ch);
                        // Synchronous
                      //  synthesizer.Speak(ch);

                        // Asynchronous
                       synthesizer.SpeakAsync(ch);

                    }
                   
                }

            }
            catch (Exception w)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(w.Message);
            }
           

          

        }

       

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.Text = " ";
        }
        private FilterInfoCollection CaptureDevice; // list of webcam
        private VideoCaptureDevice FinalFrame;
        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);//constructor
            foreach (FilterInfo Device in CaptureDevice)
            {
                comboBox1.Items.Add(Device.Name);
            }

            comboBox1.SelectedIndex = 0; // default
            FinalFrame = new VideoCaptureDevice();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);// specified web cam and its filter moniker string
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);// click button event is fired, 
            FinalFrame.Start();
        }
        void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs) // must be void so that it can be accessed everywhere.
                                                                             // New Frame Event Args is an constructor of a class
        {
            pictureBox7.Image = (Bitmap)eventArgs.Frame.Clone();// clone the bitmap
        }
        private void From1_CLosing(object sender, EventArgs e)
        {
            if (FinalFrame.IsRunning == true) FinalFrame.Stop();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            if (pictureBox7.Image != null)
            {
                int count = 1;
                //Save First
                Bitmap varBmp = new Bitmap(pictureBox7.Image);
                Bitmap newBitmap = new Bitmap(varBmp);
                varBmp.Save(@"E:\Works\Machine Learning - OCR\capture.png");
                //varBmp.Save(@"E:\Works\Machine Learning - OCR\New folder\capture"+count+".png"); 
                //Now Dispose to free the memory
                 Bitmap new1 = new Bitmap("E:\\Works\\Machine Learning - OCR\\capture.png");
                //Bitmap new1 = new Bitmap("E:\\Works\\Machine Learning - OCR\\New folder\\capture" + count + ".png"); count++;
                pictureBox8.Image = new1;
                varBmp.Dispose();
               
                varBmp = null;
                
            }
            else
            { MessageBox.Show("null exception"); }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox8.Dispose();
            Bitmap new1 = new Bitmap("E:\\Works\\Machine Learning - OCR\\capture.png");
            new1.Dispose();
            new1 = null;


            FileStream file = new FileStream("E:\\Works\\Machine Learning - OCR\\capture.png", FileMode.Open);
            pictureBox8.Image = Image.FromStream(file);
            file.Close();

            File.Delete("E:\\Works\\Machine Learning - OCR\\capture.png");
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
                pictureBox1.Image = graycolor;
                graycolor.Save(@"E:\Works\Machine Learning - OCR\raw\graycolor.jpg");
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
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
                pictureBox1.Image = graycolor;
                //graycolor.Save(@"E:\Works\Machine Learning - OCR\raw\graycolor.jpg");

                List<Bitmap> temp_bound = new List<Bitmap>();
                //Bitmap boundedimage1 = new Bitmap("E://Works//Machine Learning - OCR//raw//graycolor.jpg");
                temp_bound.Add(graycolor);

                List<Bitmap> boundedimage = BoundImage(temp_bound);
                pictureBox2.Image = boundedimage[0];
                //boundedimage[0].Save(@"E:\Works\Machine Learning - OCR\raw\boundedimage.jpg");
                List<Bitmap> segments = Segment(boundedimage[0]);
                pictureBox2.Image = segments[0];
                List<float[]> test_feature = Zone(segments);
                write_testfile(test_feature);
                TestClassifier();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //Bitmap boundedimage1 = new Bitmap("E://Works//Machine Learning - OCR//raw//boundedimage.jpg");
            //List<Bitmap> segments = Segment(boundedimage1);
            //List<float[]> features = Zone(segments);
            //List<int> labels = new List<int>();
            List<int> labels;
            List<Bitmap> rawimages = ReadImage(@"E:\Works\Machine Learning - OCR\images\", 5, out labels);
            List<Bitmap> bound = BoundImage(rawimages);
            //List<int> labels = new List<int>();
            List<float[]> all_features = Zone(bound);
            //for(int a=0;a<3;a++)
            //{
            //    labels.Add(a + 1);
            //}
            

            write_file(all_features,labels);
            TrainClassifier();


        }

        public List<Bitmap> ReadImage(string FolderPath, int charCount, out List<int> labels)
        {
            char[] table = { 'a', 'b', 'c', 'd','e' };
            List<Bitmap> images = new List<Bitmap>();
            labels = new List<int>();

            for (int i = 0; i < charCount; ++i)
            {
                var finalFolderPath = FolderPath + table[i];
                string[] filepaths = Directory.GetFiles(finalFolderPath, "*.*");
               
                foreach (var path in filepaths)
                {
                    Bitmap image = new Bitmap(path);
                    images.Add(image);
                    labels.Add(i + 1);
                    
                }

               
            }

            
            return images;
            
        }


        public List<Bitmap> BoundImage(List<Bitmap> rawimages)
        {

            //Console.WriteLine(binaryimage.Width);
            //c = binaryimage.GetPixel(1, 1);
            //Console.WriteLine(c.R);
            List<Bitmap> BoundImages = new List<Bitmap>();
            foreach (var graycolor in rawimages)
            {


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

                for (i = graycolor.Height - 1; i >= 1; i--)
                {
                    for (j = graycolor.Width - 1; j >= 1; j--)
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


                for (j = graycolor.Width - 1; j >= 1; j--)
                {
                    for (i = graycolor.Height - 1; i >= 1; i--)
                    {
                        c = graycolor.GetPixel(j, i);
                        if (c.R == 0 | c.G == 0 | c.B == 0)
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
                for (i = x1, boundedw = 0; i < extendedx; i++, boundedw++)
                {
                    for (j = y1, boundedh = 0; j < extendedy; j++, boundedh++)
                    {

                        c = graycolor.GetPixel(i, j);
                        boundedimage1.SetPixel(boundedw, boundedh, Color.FromArgb(c.R, c.G, c.B));

                    }
                }
                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap boundedimage = filter.Apply(boundedimage1);
                Threshold th = new Threshold();
                th.ApplyInPlace(boundedimage);
                BoundImages.Add(boundedimage);

            }
            return BoundImages;
        }


        /*CannyEdgeDetector Edgeimage = new CannyEdgeDetector();
        Bitmap edge = Edgeimage.Apply(boundedimage);
        pictureBox2.Image = edge;*/


        //for(i=1;i<graycolor.Height;i++)
        //{
        //    Console.WriteLine(tempy[i]);
        //}

        public List<Bitmap> Segment(Bitmap boundedimage)
        {
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
            Console.WriteLine("helllooooo_check xaxis");
            Console.WriteLine(xaxis.Length);

            Console.WriteLine("helllooooo");
            for (i = 0; i < ymax; i++)
            {
                Console.WriteLine(yaxis[i] + " " + xaxis[i]);
            }
            //  Console.WriteLine(xaxis[2]+" "+ yaxis[2] + " " + (xaxis[3] - xaxis[2] )+ " " + (yaxis[3] - yaxis[2]));


            int k;
            List<Bitmap> croppedImg = new List<Bitmap>();
            Bitmap temp;
            Console.WriteLine("helllooooo-check");
            Console.WriteLine(xaxis.Length);

            for (i = 0, j = 1, k = 0; xaxis[j]<xaxis.Length+1; k++, i = i + 2, j = j + 2)
            {
                Console.WriteLine(xaxis[i] + " " + yaxis[0] + " " + (xaxis[j] - xaxis[i]) + " " + (yaxis[1] - yaxis[0]));
                try
                {


                    Crop filter1 = new Crop(new Rectangle(xaxis[i], yaxis[0], (xaxis[j] - xaxis[i]), (yaxis[1] - yaxis[0])));
                    temp = filter1.Apply(boundedimage);
                    croppedImg.Add(temp);
                    croppedImg[k].Save(@"E:\Works\Machine Learning - OCR\Samples\Sample" + k + ".jpg");
                }catch
                {
                    break;
                }
                }
            pictureBox3.Image = croppedImg[0];

            pictureBox4.Image = croppedImg[1];
            //pictureBox5.Image = croppedImg[2];
            // pictureBox6.Image = croppedImg[3];
            //pictureBox6.Image = croppedImg[4];

            
            return croppedImg;
        }

        public List<float[]> Zone(List<Bitmap> segments)
        {
            //---------------------------ZONING-------------------------------------
            int countz = 0, count = 0;
            List<float[]> features = new List<float[]>();
            ResizeBilinear filter2 = new ResizeBilinear(60, 90);
            foreach (var im in segments)
            {
                Bitmap resize = filter2.Apply(im);

                pictureBox6.Image = resize;
                List<float> imageFeatures = new List<float>();
                int total = 0;
                for (i = 0; i < 60; i = i + 10)
                {
                    for (j = 0; j < 90; j = j + 10)
                    {
                        count = 0;
                        for (int a = 0; a < 10; a++)
                        {
                            for (int b = 0; b < 10; b++)
                            {
                                c = resize.GetPixel(i + a, j + b);
                                if (c.R == 0)
                                {
                                    count++;
                                    total++;
                                }
                            }
                        }
                        imageFeatures.Add(count);

                    }
                }
                for(int i = 0; i < imageFeatures.Count; ++i)
                {
                    imageFeatures[i] /= total;
                }

                features.Add(imageFeatures.ToArray());
            }
            //for (int i = 0; i < 4; i++)
            //    for (int j = 0; j < 54; j++)
            //    {
            //        Console.WriteLine(features[i][j]);
            //    }
            return features;
            //return features;
           
        }

        private void write_file(List<float[]> features, List<int> labels)
        {
            FileStream fs = new FileStream("E:\\Works\\Machine Learning - OCR\\Rebuilt\\Rebuilt\\bin\\Debug\\train_samples", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            float sum = 0;
            
           for(int j = 0; j<features.Count; j++)
           {
                //int label = 1;
                
                sw.Write(labels[j] + " ");
                var feature = features[j];
                
                for (int i = 0; i < features[j].Length; ++i)
                {
                    sw.Write(i + 1 + ":" + feature[i] + " ");
                }
                sw.WriteLine();
                
           }
           
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        private void write_testfile(List<float[]> test_feature)
        {
            FileStream fs = new FileStream("E:\\Works\\Machine Learning - OCR\\Rebuilt\\Rebuilt\\bin\\Debug\\test_samples", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            float sum = 0;
            for(int j=0; j<test_feature.Count; j++)
            {
                sw.Write("0" + " ");
                var feat = test_feature[j];
               
                for (int i=0; i<test_feature[j].Length; i++)
                {
                    sw.Write(i + 1 + ":" + feat[i] + " ");
                }
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        Model model_svm;
        private void TrainClassifier()
        {
            Problem train = Problem.Read("train_samples");
            Parameter param = new Parameter();
            param.C = 32;
            param.Gamma = 8;
            model_svm = Training.Train(train, param);
        }

        private void TestClassifier()
        {
            Problem test = Problem.Read("test_samples");
            Prediction.Predict(test, "result", model_svm, false);
        }


    }
}




















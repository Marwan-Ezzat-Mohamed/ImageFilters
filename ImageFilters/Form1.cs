using System;
using System.Drawing;
using System.Windows.Forms;
using ZGraphTools;


namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;


        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PNG|*.png|All files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                AlphaTrim.Enabled = true;
                adaptive_Median.Enabled = true;
            }
        }
        private void AlphaTrim_Click(object sender, EventArgs e)
        {
            byte[,] array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);
            var watch = new System.Diagnostics.Stopwatch();

            double[] atfstime = new double[(int)numericUpDown1.Value / 2];
            double[] atfctime = new double[(int)numericUpDown1.Value / 2];

            double progValue = 0;
            progLable.Text = "Running Alpha-Trim Filter using counting sort...";

            for (int nOfIt = 3; nOfIt <= (int)numericUpDown1.Value; nOfIt += 2)
            {
                watch.Start();
                array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);


                //loop through each value in the array
                int windowSize = nOfIt;
                int[,] window = new int[windowSize, windowSize];
                int[] alphaWindow1d = new int[windowSize * windowSize];

                for (int i = 0; i < array2D.GetLength(0); i++)
                {
                    for (int j = 0; j < array2D.GetLength(1); j++)
                    {

                        alphaWindow1d = windowToLine(i, j, array2D, window, windowSize);
                        alphaWindow1d = sortingMethods.countingSort(alphaWindow1d);

                        int T = (int)numericUpDown2.Value;
                        double average = 0;
                        for (int ii = T; ii < alphaWindow1d.Length - T; ii++)
                        {
                            average += alphaWindow1d[ii];
                        }

                        average = average / ((windowSize * windowSize) - 2 * T);
                        array2D[i, j] = (byte)average;

                    }
                }
                watch.Stop();
                atfctime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;

                progValue = ((double)nOfIt / (double)numericUpDown1.Value) * 50;
                progressBar1.Value = (int)progValue;
                progressBar1.Refresh();
                //Form.ActiveForm.Update();

            }

            progValue = 0;
            progLable.Text = "Running Alpha-Trim Filter by selecting Kth element...";

            for (int nOfIt = 3; nOfIt <= (int)numericUpDown1.Value; nOfIt += 2)
            {

                watch.Start();
                array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);


                //loop through each value in the array
                int windowSize = nOfIt;
                int[,] window = new int[windowSize, windowSize];
                int[] alphaWindow1d = new int[windowSize * windowSize];
             
                for (int i = 0; i < array2D.GetLength(0); i++)
                {
                    for (int j = 0; j < array2D.GetLength(1); j++)
                    {

                        alphaWindow1d = windowToLine(i, j, array2D, window, windowSize);
                        //countingSort(alphaWindow1d);
                        int T = (int)numericUpDown2.Value;
                        double average = 0;
                        bool[] vis = new bool[windowSize * windowSize];
                        for (int ii = 0; ii < T; ii++)
                        {
                            int mn = (int)1e9, mx = (int)-1e9;
                            int mnIdx = -1, mxIdx = -1;
                            for (int jj = 0; jj < alphaWindow1d.Length; jj++)
                            {
                                if (!vis[jj] && alphaWindow1d[jj] > mx)
                                {
                                    mxIdx = jj;
                                    mx = alphaWindow1d[jj];
                                }
                                else if (!vis[jj] && alphaWindow1d[jj] < mn)
                                {
                                    mnIdx = jj;
                                    mn = alphaWindow1d[jj];
                                }
                            }
                            if(mnIdx!=-1)
                                vis[mnIdx] = true;
                            if (mxIdx != -1)
                                vis[mxIdx] = true;
                        }
                        for (int ii = 0; ii < alphaWindow1d.Length; ii++)
                        {
                            if (!vis[ii])
                            {
                                average += alphaWindow1d[ii];
                            }
                        }


                        average = average / ((windowSize * windowSize) - 2 * T);
                        array2D[i, j] = (byte)average;

                    }
                }


                watch.Stop();
                atfstime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;

                progValue = ((double)nOfIt / (double)numericUpDown1.Value) * 50 + 50;
                progressBar1.Value = (int)progValue;
                progressBar1.Refresh();
                //Form.ActiveForm.Update();

            }


           

            ImageOperations.DisplayImage(array2D, pictureBox2);
            double[] m = new double[(int)numericUpDown1.Value / 2];
            for (int h = 0; h < (int)numericUpDown1.Value / 2; h++)
                m[h] = h * 2 + 3;

            ZGraphForm adfgraph = new ZGraphForm("Alpha trim filter", "Window size", "Time(ms)");
            adfgraph.add_curve("Alpha-Trim curve by selecting Kth element", m, atfstime, Color.Red);
            adfgraph.add_curve("Alpha-Trim curve counting sort", m, atfctime, Color.Blue);
            adfgraph.Show();



        }
        private void adaptive_Median_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            byte[,] array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);

            double[] adfqtime = new double[(int)numericUpDown1.Value / 2]; //adaptive median filter quick sort time values
            double[] adfctime = new double[(int)numericUpDown1.Value / 2]; //adaptive median filter counting sort time values

            double progValue = 0;
            progLable.Text = "Running Adaptive Median Filter using quick sort...";
            ///Quick_Sort
            for (int nOfIt = 3; nOfIt <= (int)numericUpDown1.Value; nOfIt += 2)
            {
                array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);
                watch.Start();

                int windowSize = nOfIt;
                int[,] window = new int[windowSize, windowSize];
               
                for (int i = 0; i < array2D.GetLength(0); i++)
                {
                    for (int j = 0; j < array2D.GetLength(1); j++)
                    {
                        int[] window1d = windowToLine(i, j, array2D, window, windowSize);
                        window1d = sortingMethods.Quick_Sort(window1d, 0, window1d.Length - 1);
                        array2D[i, j] = (byte)adaptiveNewPixelValue(array2D[i, j], window1d, (int)numericUpDown1.Value, nOfIt);
                    }
                }
                watch.Stop();
                adfqtime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;

                progValue = ((double)nOfIt / (double)numericUpDown1.Value) * 50;
                progressBar1.Value = (int)progValue;
                progressBar1.Refresh();
                //Form.ActiveForm.Update();


            }

            progValue = 0;
            progLable.Text = "Running Adaptive Median Filter counting sort...";

            ///countingSort
            for (int nOfIt = 3; nOfIt <= (int)numericUpDown1.Value; nOfIt += 2)
            {
                array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);
                watch.Start();

                int windowSize = nOfIt;
                int[,] window = new int[windowSize, windowSize];

                for (int i = 0; i < array2D.GetLength(0); i++)
                {
                    for (int j = 0; j < array2D.GetLength(1); j++)
                    {

                        int[] window1d = windowToLine(i, j, array2D, window, windowSize);
                        window1d=sortingMethods.countingSort(window1d);
                        array2D[i, j] = (byte)adaptiveNewPixelValue(array2D[i, j], window1d, (int)numericUpDown1.Value, nOfIt);
                    }
                }
                watch.Stop();
                adfctime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;

                progValue = ((double)nOfIt / (double)numericUpDown1.Value) * 50 + 50;
                progressBar1.Value = (int)progValue;
                progressBar1.Refresh();
                //Form.ActiveForm.Update();
            }

            progLable.Text = "Done!";

            ImageOperations.DisplayImage(array2D, pictureBox2);

            double[] m = new double[(int)numericUpDown1.Value / 2];
            for (int h = 0; h < (int)numericUpDown1.Value / 2; h++)
                m[h] = h * 2 + 3;

            ZGraphForm adfgraph = new ZGraphForm("Adaptive median filter", "Window size", "Time(ms)");
            adfgraph.add_curve("Adaptive Median curve quick sort", m, adfqtime, Color.Red);
            adfgraph.add_curve("Adaptive Median curve counting sort", m, adfctime, Color.Blue);
            adfgraph.Show();


        }
        private int[] windowToLine(int i, int j, byte[,] array2D, int[,] window2D, int windowSize)
        {
            //start from the middle of the window and fill the window 
            for (int k = 0; k < window2D.GetLength(0); k++)
            {
                for (int l = 0; l < window2D.GetLength(1); l++)
                {
                    //check if the window value is not out of the array
                    if (i + k - 1 >= 0 && i + k - 1 < array2D.GetLength(0) && j + l - 1 >= 0 && j + l - 1 < array2D.GetLength(1))
                    {
                        //put the value in the window
                        window2D[k, l] = array2D[i + k - 1, j + l - 1];

                    }
                    else
                    {
                        window2D[k, l] = 0;
                    }
                }
            }

            int[] window1d = new int[windowSize * windowSize];
            int kk = 0;
            for (int k = 0; k < window2D.GetLength(0); k++)
            {
                for (int l = 0; l < window2D.GetLength(1); l++)
                {
                    window1d[kk] = window2D[k, l];
                    kk++;
                }
                //Console.WriteLine();
            }

            return window1d;


        }
        private double adaptiveNewPixelValue(int value, int[] window1d, int WS, int currentWs)
        {
            int index;
            if (window1d.Length % 2 == 0)
                index = window1d.Length / 2;
            else
                index = (window1d.Length + 1) / 2;

            int Zmax, Zmin, Zxy, Zmed = window1d[index], WH, WW;
            double NewPixelValue = 0.0;

            Zmax = window1d[window1d.Length - 1];
            Zmin = window1d[0];
            Zxy = value;
            int A1 = Zmed - Zmin;
            int A2 = Zmax - Zmed;
            if (A1 > 0 && A2 > 0)
            {
                int B1 = Zxy - Zmin;
                int B2 = Zmax - Zxy;
                if (B1 > 0 && B2 > 0)
                {
                    return Zxy;
                }
                else
                {
                    if (2 + currentWs <= WS)
                    {
                        return adaptiveNewPixelValue(value, window1d, WS, currentWs + 2);
                    }
                    else
                    {
                        return Zmed;
                    }
                }
            }
            else
            {
                return Zmed;
            }

        }
    }
}
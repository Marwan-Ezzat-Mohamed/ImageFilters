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
        int cnt = 0;
        //// counting sort-------------------
        static int[] countingSort(int[] Array)
        {
            int n = Array.Length;
            int max = 0;
            //find largest element in the Array
            for (int i = 0; i < n; i++)
            {
                if (max < Array[i])
                {
                    max = Array[i];
                }
            }

            //Create a freq array to store number of occurrences of 
            //each unique elements in the given array 
            int[] freq = new int[max + 1];
            for (int i = 0; i < max + 1; i++)
            {
                freq[i] = 0;
            }
            for (int i = 0; i < n; i++)
            {
                freq[Array[i]]++;
            }

            //sort the given array using freq array
            for (int i = 0, j = 0; i <= max; i++)
            {
                while (freq[i] > 0)
                {
                    Array[j] = i;
                    j++;
                    freq[i]--;
                }
            }

            return Array;
        }

        ////counting sort --------------
        ///
        ///quick sort----------------
        private static void Quick_Sort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                {
                    Quick_Sort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    Quick_Sort(arr, pivot + 1, right);
                }
            }

        }
        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[left];
            while (true)
            {

                while (arr[left] < pivot)
                {
                    left++;
                }

                while (arr[right] > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    int temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }



        private void btnOpen_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PNG|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                button1.Enabled = true;
                adaptive_Median.Enabled = true;

            }

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
                        cnt++;
                    }
                    else
                    {
                        window2D[k, l] = 0;
                    }
                }
            }


            //loop through the window and print the values
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

        private double adaptiveNewPixelValue(int i, int j, int[] window1d, byte[,] array2D, int windowSize)
        {
            int index;
            if (window1d.Length % 2 == 0)
                index = window1d.Length / 2;
            else
                index = (window1d.Length + 1) / 2;

            int Zmax, Zmin, Zxy, Zmed = window1d[index], WH, WW;
            double NewPixelValue = 0.0;

            Zmax = window1d[8];
            Zmin = window1d[0];
            Zxy = array2D[i, j];
            int A1 = Zmed - Zmin;
            int A2 = Zmax - Zmed;
            if (A1 > 0 && A2 > 0)
            {
                int B1 = Zxy - Zmin;
                int B2 = Zmax - Zxy;
                if (B1 > 0 && B2 > 0)
                    NewPixelValue = Zxy;
                else
                    NewPixelValue = Zmed;
            }
            else
            {
                WH = 2 + windowSize;
                WW = 2 + windowSize;
                if (WH <= windowSize && WW <= windowSize)
                {
                    if (A1 > 0 && A2 > 0)
                    {
                        int B1 = Zxy - Zmin;
                        int B2 = Zmax - Zxy;
                        if (B1 > 0 && B2 > 0)
                            NewPixelValue = Zxy;
                        else
                            NewPixelValue = Zmed;

                    }
                }
                else
                {
                    NewPixelValue = Zmed;
                }

            }
            return NewPixelValue;
        }

        private void minaaa()
        {



            ImageConverter converter = new ImageConverter();

            var watch = new System.Diagnostics.Stopwatch();
            byte[,] array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);

            double[] atfstime = new double[(int)numericUpDown1.Value / 2];
            double[] atfctime = new double[(int)numericUpDown1.Value / 2];



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
                        cnt = 0;
                        alphaWindow1d = windowToLine(i, j, array2D, window, windowSize);
                        countingSort(alphaWindow1d);
                        int T = (int)numericUpDown2.Value;
                        double average = 0;
                        for (int ii = T; ii < alphaWindow1d.Length - T; ii++)
                        {
                            average += alphaWindow1d[ii];
                        }

                        average = average / cnt;
                        array2D[(i + nOfIt) / 2, (j + nOfIt) / 2] = (byte)average;

                    }
                }
                watch.Stop();
                atfctime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;
            }
            for (int nOfIt = 3; nOfIt <= (int)numericUpDown1.Value; nOfIt += 2)
            {
                watch.Start();
                array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);


                //loop through each value in the array
                int windowSize = nOfIt;
                int[,] window = new int[windowSize, windowSize];
                int[] alphaWindow1d = new int[windowSize * windowSize];
                bool [] vis = new bool[windowSize*windowSize];

                for (int i = 0; i < array2D.GetLength(0); i++)
                {
                    for (int j = 0; j < array2D.GetLength(1); j++)
                    {
                        cnt = 0;
                        alphaWindow1d = windowToLine(i, j, array2D, window, windowSize);
                        //countingSort(alphaWindow1d);
                        int T = (int)numericUpDown2.Value;
                        double average = 0;
                        for (int ii = 0; ii < T; ii++)
                        {
                            int mn = (int)1e9, mx = (int)-1e9;
                            int mnIdx = -1, mxIdx = -1;
                            for(int jj=0; jj<alphaWindow1d.Length; jj++)
                            {
                                if(!vis[jj] && alphaWindow1d[jj] > mx)
                                {
                                    mxIdx = jj; 
                                    mx = alphaWindow1d[jj];
                                }
                                if (!vis[jj] && alphaWindow1d[jj] < mn)
                                {
                                    mnIdx = jj;
                                    mn = alphaWindow1d[jj];
                                }
                            }
                            vis[mxIdx] = true;
                            vis[mnIdx] = true;
                        }
                        for(int ii = 0; ii < alphaWindow1d.Length; ii++)
                        {
                            if (!vis[ii])
                            {
                                average += alphaWindow1d[ii];
                            }
                        }

                        average = average / cnt;
                        array2D[(i + nOfIt) / 2, (j + nOfIt) / 2] = (byte)average;

                    }
                }
                watch.Stop();
                atfstime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;
            }
            ImageOperations.DisplayImage(array2D, pictureBox2);
            double[] m = new double[(int)numericUpDown1.Value / 2];
            for (int h = 0; h < (int)numericUpDown1.Value / 2; h++)
                m[h] = h * 2 + 3;

            ZGraphForm adfgraph = new ZGraphForm("adaptive median filter", "window size", "time(ms)");
            adfgraph.add_curve("alpha trim curve by selection", atfstime, m, Color.Red);
            adfgraph.add_curve("alpha trim curve counting sort", atfctime, m, Color.Blue);
            adfgraph.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            minaaa();
        }

        private void btnZGraph_Click(object sender, EventArgs e)
        {
            // Make up some data points from the N, N log(N) functions
            int N = 40;
            double[] x_values = new double[N];
            double[] y_values_N = new double[N];
            double[] y_values_NLogN = new double[N];

            for (int i = 0; i < N; i++)
            {
                x_values[i] = i;
                y_values_N[i] = i;
                y_values_NLogN[i] = i * Math.Log(i);
            }

            //Create a graph and add two curves to it
            /* ZGraphForm ZGF = new ZGraphForm("Sample Graph", "N", "f(N)");
            ZGF.add_curve("f(N) = N", x_values, y_values_N,Color.Red);
            ZGF.add_curve("f(N) = N Log(N)", x_values, y_values_NLogN, Color.Blue);
            ZGF.Show();*/
        }

        ///quick sort----------------

        private void adaptive_Median_Click(object sender, EventArgs e)
        {


            ImageConverter converter = new ImageConverter();


            var watch = new System.Diagnostics.Stopwatch();
            byte[,] array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);


            double[] adfqtime = new double[(int)numericUpDown1.Value / 2];
            double[] adfctime = new double[(int)numericUpDown1.Value / 2];
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

                        Quick_Sort(window1d, 0, window1d.Length - 1);
                        //countingSort(window1d);


                        array2D[i, j] = (byte)adaptiveNewPixelValue(i, j, window1d, array2D, windowSize);
                    }
                }
                watch.Stop();
                adfqtime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;

            }

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

                        //Quick_Sort(window1d, 0, window1d.Length - 1);
                        countingSort(window1d);


                        array2D[i, j] = (byte)adaptiveNewPixelValue(i, j, window1d, array2D, windowSize);
                    }
                }
                watch.Stop();
                adfctime[nOfIt / 2 - 1] = watch.ElapsedMilliseconds;

            }

            ImageOperations.DisplayImage(array2D, pictureBox2);



            double[] m = new double[(int)numericUpDown1.Value / 2];
            for (int h = 0; h < (int)numericUpDown1.Value / 2; h++)
                m[h] = h * 2 + 3;

            ZGraphForm adfgraph = new ZGraphForm("adaptive median filter", "window size", "time(ms)");
            adfgraph.add_curve("adaptive median curve quick sort", adfqtime, m, Color.Red);
            adfgraph.add_curve("adaptive median curve counting sort", adfctime, m, Color.Blue);
            adfgraph.Show();


        }

        private void pictureBox1_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            button1.Enabled = true;
            adaptive_Median.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }

        private void minaaa()
        {

            ImageConverter converter = new ImageConverter();


            byte[,] array2D = ImageOperations.ImageTo2DByteArray((Bitmap)pictureBox1.Image);

            // ImageOperations.OpenImage("C:\\Users\\minab\\Desktop\\8.png");

            /* for (int i = 0; i < buffer.GetLength(0); i++)
             {
                 for (int j = 0; j < buffer.GetLength(1); j++)
                 {
                     Console.Write(buffer[i, j] + " ");
                 }
                 Console.WriteLine();
             }*/


            int windowHight = 5, windowWidth = 5;

            /*int[,] array2D = new int[,] {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 },
            { 13, 14, 15, 16 } };*/

            //Console.WriteLine(array2D.GetLength(0)) =4;
            //Console.WriteLine(array2D.GetLength(1)) =4;
            int[,] window = new int[windowHight, windowWidth];
            //loop through each value in the array
            for (int i = 0; i < array2D.GetLength(0); i++)
            {
                for (int j = 0; j < array2D.GetLength(1); j++)
                {
                    //start from the middle of the window and fill the window 
                    for (int k = 0; k < window.GetLength(0); k++)
                    {
                        for (int l = 0; l < window.GetLength(1); l++)
                        {
                            //check if the window value is not out of the array
                            if (i + k - 1 >= 0 && i + k - 1 < array2D.GetLength(0) && j + l - 1 >= 0 && j + l - 1 < array2D.GetLength(1))
                            {
                                //put the value in the window
                                window[k, l] = array2D[i + k - 1, j + l - 1];
                            }
                            else
                            {
                                window[k, l] = 0;
                            }
                        }
                    }

                    //loop through the window and print the values
                    int[] window1d = new int[windowHight * windowWidth];
                    int kk = 0;
                    for (int k = 0; k < window.GetLength(0); k++)
                    {
                        for (int l = 0; l < window.GetLength(1); l++)
                        {
                            window1d[kk] = window[k, l];
                            kk++;
                        }
                        //Console.WriteLine();
                    }

                    ////counting sort henaaaaaaaaaaaaaaaaaaaaaaaa
                    Array.Sort(window1d);


                    int T = 5;

                    for (int k = 0; k < window1d.Length; k++)
                    {
                        //remove k smallest elemnts
                        for (int l = 0; l < T; l++)
                        {
                            window1d[l] = 0;
                        }
                        //remove k largest elemnts
                        for (int l = window1d.Length - 1; l >= window1d.Length - T; l--)
                        {
                            window1d[l] = 0;
                        }

                    }

                    int average = 0;
                    for (int k = 0; k < window1d.Length; k++)
                    {
                        average += window1d[k];
                    }


                    average = average / (window1d.Length - (T * 2));
                    array2D[i, j] = (byte)average;



                    //loop through the window and print the values
                    /* for (int k = 0; k < window.GetLength(0); k++)
                     {
                         for (int l = 0; l < window.GetLength(1); l++)
                         {
                             Console.Write(window[k, l] + " ");
                         }
                         Console.WriteLine();
                     }
                     /*Console.WriteLine("{0}\n", average);
                     Console.WriteLine("---------------------------------\n");*/

                }
                    Console.WriteLine("---------***************--\n");

            }
            Console.WriteLine("---------------------------------\n");
            ImageOperations.DisplayImage(array2D, pictureBox2);

            //Console.ReadLine();


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

        private void button1_Click(object sender, EventArgs e)
        {
            minaaa();
            
        }
    }
}
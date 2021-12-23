using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImageFilters
{

   

    static class Program
    {

        
        static int[] countingsort(int[] Array)
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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
            // byte[,] array2D = ImageOperations.OpenImage("C:\\Users\\minab\\Desktop\\5.png");

            ///* for (int i = 0; i < buffer.GetLength(0); i++)
            // {
            //     for (int j = 0; j < buffer.GetLength(1); j++)
            //     {
            //         Console.Write(buffer[i, j] + " ");
            //     }
            //     Console.WriteLine();
            // }*/

            
            //int windowHight=5, windowWidth=5;

            ///*int[,] array2D = new int[,] {
            //{ 1, 2, 3, 4 },
            //{ 5, 6, 7, 8 },
            //{ 9, 10, 11, 12 },
            //{ 13, 14, 15, 16 } };*/

            ////Console.WriteLine(array2D.GetLength(0)) =4;
            ////Console.WriteLine(array2D.GetLength(1)) =4;
            //int[,] window = new int[windowHight, windowWidth];
            ////loop through each value in the array
            //for (int i = 0; i < array2D.GetLength(0); i++)
            //{
            //    for (int j = 0; j < array2D.GetLength(1); j++)
            //    {
            //        //start from the middle of the window and fill the window 
            //        for (int k = 0; k < window.GetLength(0); k++)
            //        {
            //            for (int l = 0; l < window.GetLength(1); l++)
            //            {
            //                //check if the window value is not out of the array
            //                if (i + k - 1 >= 0 && i + k - 1 < array2D.GetLength(0) && j + l - 1 >= 0 && j + l - 1 < array2D.GetLength(1))
            //                {
            //                    //put the value in the window
            //                    window[k, l] = array2D[i + k - 1, j + l - 1];
            //                }
            //                else
            //                {
            //                    window[k, l] = 0;
            //                }
            //            }
            //        }

            //        //loop through the window and print the values
            //        int[] window1d = new int[windowHight*windowWidth];
            //        int kk = 0;
            //        for (int k = 0; k < window.GetLength(0); k++)
            //        {
            //            for (int l = 0; l < window.GetLength(1); l++)
            //            {
            //                window1d[kk] = window[k, l];
            //                kk++;
            //            }
            //            Console.WriteLine();
            //        }

            //        Array.Sort(window1d);
            //        int T = 5;
                   
            //        for (int k = 0; k < window1d.Length; k++)
            //        {
            //            //remove k smallest elemnts
            //            for (int l = 0; l < T; l++)
            //            {
            //                window1d[l] = 0;
            //            }
            //            //remove k largest elemnts
            //            for (int l = window1d.Length - 1; l >= window1d.Length - T; l--)
            //            {
            //                window1d[l] = 0;
            //            }

            //        }

            //        int average = 0;
            //        for (int k = 0; k < window1d.Length; k++)
            //        {
            //            average += window1d[k];
            //        }


            //        average = average / (window1d.Length - (T * 2));





            //        //loop through the window and print the values
            //        for (int k = 0; k < window.GetLength(0); k++)
            //        {
            //            for (int l = 0; l < window.GetLength(1); l++)
            //            {
            //                Console.Write(window[k, l] + " ");
            //            }
            //            Console.WriteLine();
            //        }
            //        Console.WriteLine("{0}\n", average);
            //        Console.WriteLine("---------------------------------\n");

            //    }
            //}

            //ImageOperations.DisplayImage(array2D, form);

            //Console.ReadLine();


            


        }
    }
}
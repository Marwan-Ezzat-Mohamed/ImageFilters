using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    class sortingMethods
    {

        //// counting sort-------------------
        ///  
        public static int[] countingSort(int[] Array)
        {
            int max = 255;
            int min = 0;
            int range = max - min + 1;
            int[] count = new int[range];
            int[] output = new int[Array.Length];
            for (int i = 0; i < Array.Length; i++)
            {
                count[Array[i] - min]++;
            }
            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }
            for (int i = Array.Length - 1; i >= 0; i--)
            {
                output[count[Array[i] - min] - 1] = Array[i];
                count[Array[i] - min]--;
            }
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = output[i];
            }
            return Array;
        }


        ///quick sort---------------
        public static int Partition(int[] arr, int left, int right)
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
        ///quick sort----------------
        //
        public static int[] Quick_Sort(int[] arr, int left, int right)
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
            return arr;

        }


    }
}

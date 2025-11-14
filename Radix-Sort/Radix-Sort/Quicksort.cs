namespace Radix_Sort;

public class Quicksort
{
    // QuickSort using Hoare partition scheme
    public static void QuickSort(int[] array, int left, int right)
    {
        if (array == null || array.Length < 2) return;
        if (left >= right) return;

        int p = Partition(array, left, right); // returns j such that [left..j] <= pivot <= [j+1..right]
        QuickSort(array, left, p);
        QuickSort(array, p + 1, right);
    }

// Hoare partition (stable bounds contract for QuickSort above)
    private static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[(left + right) / 2];
        int i = left - 1;
        int j = right + 1;

        while (true)
        {
            do { i++; } while (arr[i] < pivot);
            do { j--; } while (arr[j] > pivot);

            if (i >= j) return j;

            // swap
            int tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }
}
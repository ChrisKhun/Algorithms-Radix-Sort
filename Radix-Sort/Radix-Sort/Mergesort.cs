namespace Radix_Sort;

public class Mergesort
{
    // -----------------------------
// Merge Sort (top-down)
// -----------------------------
    public static void MergeSort(int[] arr, int left, int right)
    {
        if (left >= right)
            return;

        int mid = (left + right) / 2;

        MergeSort(arr, left, mid);
        MergeSort(arr, mid + 1, right);

        Merge(arr, left, mid, right);
    }

// -----------------------------
// Merge step
// -----------------------------
    private static void Merge(int[] arr, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;

        // create temp arrays
        int[] L = new int[n1];
        int[] R = new int[n2];

        // copy data into temp arrays
        for (int i = 0; i < n1; i++)
            L[i] = arr[left + i];

        for (int j = 0; j < n2; j++)
            R[j] = arr[mid + 1 + j];

        int p = 0; // L pointer
        int q = 0; // R pointer
        int k = left; // merged array pointer

        // merge back into arr
        while (p < n1 && q < n2)
        {
            if (L[p] <= R[q])
            {
                arr[k++] = L[p++];
            }
            else
            {
                arr[k++] = R[q++];
            }
        }

        // copy any leftovers
        while (p < n1)
            arr[k++] = L[p++];

        while (q < n2)
            arr[k++] = R[q++];
    }
}
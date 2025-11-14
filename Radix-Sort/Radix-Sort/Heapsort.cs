namespace Radix_Sort;

public class Heapsort
{
    // HeapSort implementation (in-place)
    public static void HeapSort(int[] array)
    {
        if (array == null || array.Length <= 1)
            return;

        int n = array.Length;

        // Build max heap
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(array, n, i);
        }

        // One by one extract elements from heap
        for (int i = n - 1; i > 0; i--)
        {
            // Move current root to end
            int temp = array[0];
            array[0] = array[i];
            array[i] = temp;

            // Call max heapify on the reduced heap
            Heapify(array, i, 0);
        }
    }

    private static void Heapify(int[] array, int heapSize, int rootIndex)
    {
        int largest = rootIndex;
        int left = 2 * rootIndex + 1;
        int right = 2 * rootIndex + 2;

        // If left child is larger than root
        if (left < heapSize && array[left] > array[largest])
        {
            largest = left;
        }

        // If right child is larger than largest so far
        if (right < heapSize && array[right] > array[largest])
        {
            largest = right;
        }

        // If largest is not root
        if (largest != rootIndex)
        {
            int swap = array[rootIndex];
            array[rootIndex] = array[largest];
            array[largest] = swap;

            // Recursively heapify the affected sub-tree
            Heapify(array, heapSize, largest);
        }
    }
}
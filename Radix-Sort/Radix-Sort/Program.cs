using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    // Loads a CSV into a flat 1-D array (int[])
    public static int[] LoadCSV(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return Array.Empty<int>();
        }

        var all = new List<int>();
        foreach (var raw in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;

            var fields = raw.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var f in fields)
            {
                if (int.TryParse(f.Trim(), out int num))
                    all.Add(num);
            }
        }

        Console.WriteLine($"{Path.GetFileName(filePath)} has been loaded successfully.");
        return all.ToArray();
    }

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

    // Placeholders now take 1-D arrays
    public static void MergeSort(int[] array, int left, int right)
    {
        // TODO
    }

    public static void HeapSort(int[] array)
    {
        // TODO
    }

    static void Main(string[] args)
    {
        // Base project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        // New Datasets folder path
        string datasetDir = Path.Combine(projectDir, "Datasets");

        // -------------------------
        // Short CSV paths
        // -------------------------
        string randomPath      = Path.Combine(datasetDir, "[short]random_data.csv");
        string nearlyPath      = Path.Combine(datasetDir, "[short]nearly_sorted.csv");
        string reversePath     = Path.Combine(datasetDir, "[short]reverse_sorted.csv");
        string duplicatePath   = Path.Combine(datasetDir, "[short]duplicate_data.csv");

        // -------------------------
        // Long CSV paths
        // -------------------------
        string longRandomPath    = Path.Combine(datasetDir, "[long]random_data.csv");
        string longNearlyPath    = Path.Combine(datasetDir, "[long]nearly_sorted.csv");
        string longReversePath   = Path.Combine(datasetDir, "[long]reverse_sorted.csv");
        string longDuplicatePath = Path.Combine(datasetDir, "[long]duplicate_data.csv");

        // -------------------------
        // Load short datasets
        // -------------------------
        int[] randomData     = LoadCSV(randomPath);
        int[] nearlySorted   = LoadCSV(nearlyPath);
        int[] reverseSorted  = LoadCSV(reversePath);
        int[] duplicateData  = LoadCSV(duplicatePath);

        // -------------------------
        // Load long datasets
        // -------------------------
        int[] longRandomData    = LoadCSV(longRandomPath);
        int[] longNearlySorted  = LoadCSV(longNearlyPath);
        int[] longReverseSorted = LoadCSV(longReversePath);
        int[] longDuplicateData = LoadCSV(longDuplicatePath);

        Console.WriteLine("\nAll CSV load attempts completed.");

        // -------------------------
        // Helper for QuickSort timing
        // -------------------------
        void TimeQuickSort(int[] data, string label)
        {
            if (data == null || data.Length <= 1)
            {
                Console.WriteLine($"{label}: not enough data (length={data.Length}).");
                return;
            }

            var clone = (int[])data.Clone();

            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(clone, 0, clone.Length - 1);
            sw.Stop();

            Console.WriteLine($"{label} sorted successfully.");
            Console.WriteLine($"  Time: {sw.Elapsed.TotalMilliseconds:F3} ms");
        }

        // -------------------------
        // Sort each LONG dataset
        // -------------------------
        Console.WriteLine("\n=== QuickSort on LONG datasets ===");
        TimeQuickSort(longRandomData,    "[long]random_data");
        TimeQuickSort(longNearlySorted,  "[long]nearly_sorted");
        TimeQuickSort(longReverseSorted, "[long]reverse_sorted");
        TimeQuickSort(longDuplicateData, "[long]duplicate_data");

        // -------------------------
        // If you need to regenerate datasets:
        // -------------------------
        //CreateData.CreateDuplicatedCSV(Path.Combine(datasetDir, "[long]duplicate_data.csv"), 1_000_000);
        //CreateData.CreateNearlySortedCSV(Path.Combine(datasetDir, "[long]nearly_sorted.csv"), 1_000_000);
        //CreateData.CreateRandomCSV(Path.Combine(datasetDir, "[long]random_data.csv"), 1_000_000);
        //CreateData.CreateReverseSortedCSV(Path.Combine(datasetDir, "[long]reverse_sorted.csv"), 1_000_000);
    }
}
 
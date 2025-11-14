using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Radix_Sort;

class Program
{
    // Loads a CSV into a flat array
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

    // =========================
    // timing & memory display
    // =========================
    public static void TimeSort(
        string algoName,
        string datasetLabel,
        int[] data,
        Action<int[]> sortAlgorithm)
    {
        if (data == null || data.Length <= 1)
        {
            Console.WriteLine($"{algoName} on {datasetLabel}: not enough data (length={data?.Length ?? 0}).");
            return;
        }

        // Work on a copy so original stays unchanged
        var clone = (int[])data.Clone();

        // Clean up memory BEFORE we measure, so runs are more comparable
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Measure managed memory before
        long beforeBytes = GC.GetTotalMemory(true);

        // Time the sort
        Stopwatch sw = Stopwatch.StartNew();
        sortAlgorithm(clone);
        sw.Stop();

        // Measure managed memory after
        long afterBytes = GC.GetTotalMemory(false);
        long deltaBytes = afterBytes - beforeBytes;

        double timeMs = sw.Elapsed.TotalMilliseconds;
        double deltaMB = deltaBytes / (1024.0 * 1024.0);

        Console.WriteLine($"{algoName} on {datasetLabel} sorted successfully.");
        Console.WriteLine($"  Time:   {timeMs:F3} ms");
        Console.WriteLine($"  Memory: {deltaMB:F6} MB ({deltaBytes} bytes)");
    }


    // =========================
    // Algorithm wrappers
    // =========================
    public static void QuickSortWrapper(int[] data)
    {
        Quicksort.QuickSort(data, 0, data.Length - 1);
    }

    public static void MergeSortWrapper(int[] data)
    {
        Mergesort.MergeSort(data, 0, data.Length - 1);
    }

    public static void HeapSortWrapper(int[] data)
    {
        Heapsort.HeapSort(data);
    }

    public static void RadixSortWrapper(int[] data)
    {
        Radix_Sort.RadixSort.Sort(data);
    }

    // =========================
    // Dataset regeneration helpers
    // =========================
    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Console.WriteLine($"Deleted existing file: {path}");
        }
    }

    private static void RegenerateDatasets(
        string datasetDir,
        int shortSize = 100_000,
        int longSize = 1_000_000)
    {
        // Short CSV paths
        string randomPath      = Path.Combine(datasetDir, "[short]random_data.csv");
        string nearlyPath      = Path.Combine(datasetDir, "[short]nearly_sorted.csv");
        string reversePath     = Path.Combine(datasetDir, "[short]reverse_sorted.csv");
        string duplicatePath   = Path.Combine(datasetDir, "[short]duplicate_data.csv");

        // Long CSV paths
        string longRandomPath    = Path.Combine(datasetDir, "[long]random_data.csv");
        string longNearlyPath    = Path.Combine(datasetDir, "[long]nearly_sorted.csv");
        string longReversePath   = Path.Combine(datasetDir, "[long]reverse_sorted.csv");
        string longDuplicatePath = Path.Combine(datasetDir, "[long]duplicate_data.csv");

        // Ensure folder exists
        Directory.CreateDirectory(datasetDir);

        // Delete old files
        DeleteIfExists(randomPath);
        DeleteIfExists(nearlyPath);
        DeleteIfExists(reversePath);
        DeleteIfExists(duplicatePath);
        DeleteIfExists(longRandomPath);
        DeleteIfExists(longNearlyPath);
        DeleteIfExists(longReversePath);
        DeleteIfExists(longDuplicatePath);

        Console.WriteLine("\nRecreating SHORT datasets...");
        CreateData.CreateRandomCSV(randomPath, shortSize);
        CreateData.CreateNearlySortedCSV(nearlyPath, shortSize);
        CreateData.CreateReverseSortedCSV(reversePath, shortSize);
        CreateData.CreateDuplicatedCSV(duplicatePath, shortSize);

        Console.WriteLine("\nRecreating LONG datasets...");
        CreateData.CreateRandomCSV(longRandomPath, longSize);
        CreateData.CreateNearlySortedCSV(longNearlyPath, longSize);
        CreateData.CreateReverseSortedCSV(longReversePath, longSize);
        CreateData.CreateDuplicatedCSV(longDuplicatePath, longSize);

        Console.WriteLine("\nDataset regeneration complete.\n");
    }

    static void Main(string[] args)
    {
        // Base project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        // Datasets folder path
        string datasetDir = Path.Combine(projectDir, "Datasets");

        // Ask if user wants to regenerate datasets
        Console.Write("Regenerate CSV datasets? (y/n): ");
        var key = Console.ReadKey();
        Console.WriteLine();

        if (char.ToLowerInvariant(key.KeyChar) == 'y')
        {
            RegenerateDatasets(datasetDir);
        }

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

        // ====================================================
        // RadixSort on LONG datasets
        // ====================================================
        Console.WriteLine("\n=== RadixSort on LONG datasets ===");
        TimeSort("RadixSort", "[long]random_data",    longRandomData,    RadixSortWrapper);
        TimeSort("RadixSort", "[long]nearly_sorted",  longNearlySorted,  RadixSortWrapper);
        TimeSort("RadixSort", "[long]reverse_sorted", longReverseSorted, RadixSortWrapper);
        TimeSort("RadixSort", "[long]duplicate_data", longDuplicateData, RadixSortWrapper);

        // ====================================================
        // QuickSort on LONG datasets
        // ====================================================
        Console.WriteLine("\n=== QuickSort on LONG datasets ===");
        TimeSort("QuickSort", "[long]random_data",    longRandomData,    QuickSortWrapper);
        TimeSort("QuickSort", "[long]nearly_sorted",  longNearlySorted,  QuickSortWrapper);
        TimeSort("QuickSort", "[long]reverse_sorted", longReverseSorted, QuickSortWrapper);
        TimeSort("QuickSort", "[long]duplicate_data", longDuplicateData, QuickSortWrapper);

        // ====================================================
        // MergeSort on LONG datasets
        // ====================================================
        Console.WriteLine("\n=== MergeSort on LONG datasets ===");
        TimeSort("MergeSort", "[long]random_data",    longRandomData,    MergeSortWrapper);
        TimeSort("MergeSort", "[long]nearly_sorted",  longNearlySorted,  MergeSortWrapper);
        TimeSort("MergeSort", "[long]reverse_sorted", longReverseSorted, MergeSortWrapper);
        TimeSort("MergeSort", "[long]duplicate_data", longDuplicateData, MergeSortWrapper);

        // ====================================================
        // HeapSort on LONG datasets
        // ====================================================
        Console.WriteLine("\n=== HeapSort on LONG datasets ===");
        TimeSort("HeapSort", "[long]random_data",    longRandomData,    HeapSortWrapper);
        TimeSort("HeapSort", "[long]nearly_sorted",  longNearlySorted,  HeapSortWrapper);
        TimeSort("HeapSort", "[long]reverse_sorted", longReverseSorted, HeapSortWrapper);
        TimeSort("HeapSort", "[long]duplicate_data", longDuplicateData, HeapSortWrapper);
    }
}

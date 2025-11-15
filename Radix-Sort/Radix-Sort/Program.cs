using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Radix_Sort;

class Program
{
    private static string _resultsCsvPath;

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

    // Timing + CSV logging
    public static void TimeSort(string algoName, string datasetLabel, int[] data, Action<int[]> sortAlgorithm)
    {
        if (data == null || data.Length <= 1)
        {
            Console.WriteLine($"{algoName} on {datasetLabel}: not enough data.");
            return;
        }

        var clone = (int[])data.Clone();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long beforeBytes = GC.GetTotalMemory(true);

        Stopwatch sw = Stopwatch.StartNew();
        sortAlgorithm(clone);
        sw.Stop();

        long afterBytes = GC.GetTotalMemory(false);
        long deltaBytes = afterBytes - beforeBytes;

        double timeMs = sw.Elapsed.TotalMilliseconds;
        double memMB = deltaBytes / (1024.0 * 1024.0);

        Console.WriteLine($"{algoName} on {datasetLabel} sorted successfully.");
        Console.WriteLine($"  Time:   {timeMs:F3} ms");
        Console.WriteLine($"  Memory: {memMB:F6} MB ({deltaBytes} bytes)");

        AppendToCsv(algoName, datasetLabel, data.Length, timeMs, memMB, deltaBytes);
    }

    private static void AppendToCsv(string algo, string dataset, int size, double timeMs, double memMB, long memBytes)
    {
        bool exists = File.Exists(_resultsCsvPath);

        using (var w = new StreamWriter(_resultsCsvPath, append: true))
        {
            if (!exists)
                w.WriteLine("Timestamp,Algorithm,Dataset,DataSize,TimeMs,MemoryMB,MemoryBytes");

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            w.WriteLine(string.Format(
                CultureInfo.InvariantCulture,
                "{0},{1},{2},{3},{4:F6},{5:F6},{6}",
                timestamp, algo, dataset, size, timeMs, memMB, memBytes
            ));
        }
    }

    // Wrappers
    public static void QuickSortWrapper(int[] data) => Quicksort.QuickSort(data, 0, data.Length - 1);
    public static void MergeSortWrapper(int[] data) => Mergesort.MergeSort(data, 0, data.Length - 1);
    public static void HeapSortWrapper(int[] data) => Heapsort.HeapSort(data);
    public static void RadixSortWrapper(int[] data) => Radix_Sort.RadixSort.Sort(data);

    // Dataset regeneration
    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Console.WriteLine($"Deleted existing file: {path}");
        }
    }

    private static void RegenerateDatasets(string datasetDir, int shortSize = 100_000, int longSize = 1_000_000)
    {
        // SHORT
        string randomPath      = Path.Combine(datasetDir, "[short]random_data.csv");
        string nearlyPath      = Path.Combine(datasetDir, "[short]nearly_sorted.csv");
        string reversePath     = Path.Combine(datasetDir, "[short]reverse_sorted.csv");
        string duplicatePath   = Path.Combine(datasetDir, "[short]duplicate_data.csv");

        // LONG
        string longRandomPath    = Path.Combine(datasetDir, "[long]random_data.csv");
        string longNearlyPath    = Path.Combine(datasetDir, "[long]nearly_sorted.csv");
        string longReversePath   = Path.Combine(datasetDir, "[long]reverse_sorted.csv");
        string longDuplicatePath = Path.Combine(datasetDir, "[long]duplicate_data.csv");

        Directory.CreateDirectory(datasetDir);

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
        // Project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        // CSV saved next to .cs files
        _resultsCsvPath = Path.Combine(projectDir, "sort_results.csv");

        // Dataset directory
        string datasetDir = Path.Combine(projectDir, "Datasets");

        // Ask how many times to run
        Console.WriteLine("How many times do you want to run the algorithms?");
        Console.WriteLine("");
        string? runsInput = Console.ReadLine();
        int runs;
        if (!int.TryParse(runsInput, out runs) || runs < 1)
        {
            runs = 1;
            Console.WriteLine("Invalid input. Defaulting to 1 run.");
        }

        // Ask if we regenerate before each run
        Console.WriteLine("Regenerate CSV datasets before each run? (y/n)");
        string? regenInput = Console.ReadLine()?.Trim().ToLower();

        bool regenerateEachRun;
        if (regenInput == "y")
            regenerateEachRun = true;
        else if (regenInput == "n")
            regenerateEachRun = false;
        else
        {
            regenerateEachRun = false;
            Console.WriteLine("Invalid input. Defaulting to 'no' (reuse datasets).");
        }


        // ORIGINAL PATH NAMES
        string randomPath      = Path.Combine(datasetDir, "[short]random_data.csv");
        string nearlyPath      = Path.Combine(datasetDir, "[short]nearly_sorted.csv");
        string reversePath     = Path.Combine(datasetDir, "[short]reverse_sorted.csv");
        string duplicatePath   = Path.Combine(datasetDir, "[short]duplicate_data.csv");

        string longRandomPath    = Path.Combine(datasetDir, "[long]random_data.csv");
        string longNearlyPath    = Path.Combine(datasetDir, "[long]nearly_sorted.csv");
        string longReversePath   = Path.Combine(datasetDir, "[long]reverse_sorted.csv");
        string longDuplicatePath = Path.Combine(datasetDir, "[long]duplicate_data.csv");

        // Datasets we’ll reuse if not regenerating each time
        int[] randomData        = Array.Empty<int>();
        int[] nearlySorted      = Array.Empty<int>();
        int[] reverseSorted     = Array.Empty<int>();
        int[] duplicateData     = Array.Empty<int>();
        int[] longRandomData    = Array.Empty<int>();
        int[] longNearlySorted  = Array.Empty<int>();
        int[] longReverseSorted = Array.Empty<int>();
        int[] longDuplicateData = Array.Empty<int>();

        if (!regenerateEachRun)
        {
            // Just use whatever CSVs already exist, load once
            randomData        = LoadCSV(randomPath);
            nearlySorted      = LoadCSV(nearlyPath);
            reverseSorted     = LoadCSV(reversePath);
            duplicateData     = LoadCSV(duplicatePath);
            longRandomData    = LoadCSV(longRandomPath);
            longNearlySorted  = LoadCSV(longNearlyPath);
            longReverseSorted = LoadCSV(longReversePath);
            longDuplicateData = LoadCSV(longDuplicatePath);

            Console.WriteLine("\nAll CSV load attempts completed (no regeneration per run).");
        }

        for (int run = 1; run <= runs; run++)
        {
            Console.WriteLine($"\n================ RUN {run} of {runs} ================");

            if (regenerateEachRun)
            {
                // Regenerate fresh data each loop
                RegenerateDatasets(datasetDir);

                randomData        = LoadCSV(randomPath);
                nearlySorted      = LoadCSV(nearlyPath);
                reverseSorted     = LoadCSV(reversePath);
                duplicateData     = LoadCSV(duplicatePath);
                longRandomData    = LoadCSV(longRandomPath);
                longNearlySorted  = LoadCSV(longNearlyPath);
                longReverseSorted = LoadCSV(longReversePath);
                longDuplicateData = LoadCSV(longDuplicatePath);

                Console.WriteLine("\nAll CSV load attempts completed (after regeneration).");
            }

            Console.WriteLine($"Results will be logged to: {_resultsCsvPath}");

            // === RadixSort on LONG datasets ===
            Console.WriteLine("\n=== RadixSort on LONG datasets ===");
            TimeSort("RadixSort", "[long]random_data",    longRandomData,    RadixSortWrapper);
            TimeSort("RadixSort", "[long]nearly_sorted",  longNearlySorted,  RadixSortWrapper);
            TimeSort("RadixSort", "[long]reverse_sorted", longReverseSorted, RadixSortWrapper);
            TimeSort("RadixSort", "[long]duplicate_data", longDuplicateData, RadixSortWrapper);

            // === QuickSort on LONG datasets ===
            Console.WriteLine("\n=== QuickSort on LONG datasets ===");
            TimeSort("QuickSort", "[long]random_data",    longRandomData,    QuickSortWrapper);
            TimeSort("QuickSort", "[long]nearly_sorted",  longNearlySorted,  QuickSortWrapper);
            TimeSort("QuickSort", "[long]reverse_sorted", longReverseSorted, QuickSortWrapper);
            TimeSort("QuickSort", "[long]duplicate_data", longDuplicateData, QuickSortWrapper);

            // === MergeSort on LONG datasets ===
            Console.WriteLine("\n=== MergeSort on LONG datasets ===");
            TimeSort("MergeSort", "[long]random_data",    longRandomData,    MergeSortWrapper);
            TimeSort("MergeSort", "[long]nearly_sorted",  longNearlySorted,  MergeSortWrapper);
            TimeSort("MergeSort", "[long]reverse_sorted", longReverseSorted, MergeSortWrapper);
            TimeSort("MergeSort", "[long]duplicate_data", longDuplicateData, MergeSortWrapper);

            // === HeapSort on LONG datasets ===
            Console.WriteLine("\n=== HeapSort on LONG datasets ===");
            TimeSort("HeapSort", "[long]random_data",    longRandomData,    HeapSortWrapper);
            TimeSort("HeapSort", "[long]nearly_sorted",  longNearlySorted,  HeapSortWrapper);
            TimeSort("HeapSort", "[long]reverse_sorted", longReverseSorted, HeapSortWrapper);
            TimeSort("HeapSort", "[long]duplicate_data", longDuplicateData, HeapSortWrapper);
        }
    }
}

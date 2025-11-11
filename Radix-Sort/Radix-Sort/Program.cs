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
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        string randomPath    = Path.Combine(projectDir, "random_data.csv");
        string nearlyPath    = Path.Combine(projectDir, "nearly_sorted.csv");
        string reversePath   = Path.Combine(projectDir, "reverse_sorted.csv");
        string duplicatePath = Path.Combine(projectDir, "duplicate_data.csv");

        int[] randomData      = LoadCSV(randomPath);
        int[] nearlySorted    = LoadCSV(nearlyPath);
        int[] reverseSorted   = LoadCSV(reversePath);
        int[] duplicateData   = LoadCSV(duplicatePath);

        Console.WriteLine("\nAll CSVs have been processed successfully.");

        if (randomData.Length > 1)
        {
            var clone = (int[])randomData.Clone();
    
            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(clone, 0, clone.Length - 1);
            sw.Stop();
    
            Console.WriteLine("\nRandom data sorted successfully.");
            Console.WriteLine($"Elapsed time: {sw.Elapsed.TotalMilliseconds:F3} ms");
        }
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        
        
    }
}

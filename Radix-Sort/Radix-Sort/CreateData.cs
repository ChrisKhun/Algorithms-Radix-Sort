using System;
using System.IO;
using System.Collections.Generic;

public static class CreateData
{
    // ---------------------------------------------------------
    // RANDOM DATASET: n unique numbers, fully shuffled
    // ---------------------------------------------------------
    public static void CreateRandomCSV(string filePath, int n = 1_000_000)
    {
        // Create sorted array 
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = i + 1;

        // Fully shuffle 
        Random rand = new Random();
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        // Ensure directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        // Save to CSV in chunks
        using (var writer = new StreamWriter(filePath))
        {
            const int chunkSize = 10_000;
            for (int i = 0; i < n; i += chunkSize)
            {
                int end = Math.Min(i + chunkSize, n);
                string line = string.Join(",", numbers[i..end]);
                writer.WriteLine(line);
            }
        }

        Console.WriteLine($"Created FULLY RANDOM CSV '{filePath}' with {n:N0} values.");
    }

    // ---------------------------------------------------------
    // NEARLY SORTED DATASET: shuffle every 40 elements
    // ---------------------------------------------------------
    public static void CreateNearlySortedCSV(string filePath, int n = 1_000_000)
    {
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = i + 1;

        const int blockSize = 40;
        Random rand = new Random();

        for (int start = 0; start < n; start += blockSize)
        {
            int end = Math.Min(start + blockSize, n);

            // Fisher-Yates inside block
            for (int i = end - 1; i > start; i--)
            {
                int j = rand.Next(start, i + 1);
                (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
            }
        }

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        using (var writer = new StreamWriter(filePath))
        {
            const int chunkSize = 10_000;
            for (int i = 0; i < n; i += chunkSize)
            {
                int end = Math.Min(i + chunkSize, n);
                string line = string.Join(",", numbers[i..end]);
                writer.WriteLine(line);
            }
        }

        Console.WriteLine($"Created NEARLY SORTED CSV '{filePath}' using 40-element block shuffling.");
    }

    // ---------------------------------------------------------
    // REVERSE SORTED DATASET: n down to 1
    // ---------------------------------------------------------
    public static void CreateReverseSortedCSV(string filePath, int n = 1_000_000)
    {
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = n - i;

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        using (var writer = new StreamWriter(filePath))
        {
            const int chunkSize = 10_000;
            for (int i = 0; i < n; i += chunkSize)
            {
                int end = Math.Min(i + chunkSize, n);
                string line = string.Join(",", numbers[i..end]);
                writer.WriteLine(line);
            }
        }

        Console.WriteLine($"Created REVERSE SORTED CSV '{filePath}' with {n:N0} values (n → 1).");
    }

    // ---------------------------------------------------------
    // DUPLICATED DATASET: 1–7 repeats per value, then shuffled
    // ---------------------------------------------------------
    public static void CreateDuplicatedCSV(string filePath, int n = 1_000_000)
    {
        var values = new List<int>(n);
        Random rand = new Random();
        int currentValue = 1;

        while (values.Count < n)
        {
            int repeats = rand.Next(1, 8); // 1 to 7

            for (int i = 0; i < repeats && values.Count < n; i++)
                values.Add(currentValue);

            currentValue++;
        }

        int[] numbers = values.ToArray();

        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        using (var writer = new StreamWriter(filePath))
        {
            const int chunkSize = 10_000;
            for (int i = 0; i < numbers.Length; i += chunkSize)
            {
                int end = Math.Min(i + chunkSize, numbers.Length);
                string line = string.Join(",", numbers[i..end]);
                writer.WriteLine(line);
            }
        }

        Console.WriteLine($"Created DUPLICATED CSV '{filePath}' with {numbers.Length:N0} elements (1–7 repeats per value).");
    }
}

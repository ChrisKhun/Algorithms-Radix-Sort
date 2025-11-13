using System;
using System.IO;

public static class CreateData
{
    // ---------------------------------------------------------
    // RANDOM DATASET: 1 million unique numbers, fully shuffled
    // ---------------------------------------------------------
    public static void CreateRandomCSV(string fileName, int n = 1_000_000)
    {
        // Determine project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        string filePath = Path.Combine(projectDir, fileName);

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

        Console.WriteLine($"Created FULLY RANDOM CSV '{fileName}' with {n:N0} values.");
        Console.WriteLine(filePath);
    }


    // ---------------------------------------------------------
    // NEARLY SORTED DATASET: shuffle every 40 elements
    // ---------------------------------------------------------
    public static void CreateNearlySortedCSV(string fileName, int n = 1_000_000)
    {
        // Determine project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        string filePath = Path.Combine(projectDir, fileName);

        // Create sorted array 1..n
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = i + 1;

        // Shuffle inside each block of 40 elements
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

        Console.WriteLine($"Created NEARLY SORTED CSV '{fileName}' using 40-element block shuffling.");
        Console.WriteLine(filePath);
    }
    
    // ---------------------------------------------------------
// REVERSE SORTED DATASET: n down to 1
// ---------------------------------------------------------
    public static void CreateReverseSortedCSV(string fileName, int n = 1_000_000)
    {
        // Determine project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        string filePath = Path.Combine(projectDir, fileName);

        // Create reverse sorted array
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = n - i;

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

        Console.WriteLine($"Created REVERSE SORTED CSV '{fileName}' with {n:N0} values (n → 1).");
        Console.WriteLine(filePath);
    }
    
    // ---------------------------------------------------------
// DUPLICATED DATASET:
// Values start at 1, each value appears 1–7 times (random),
// until we reach n elements. Then the whole thing is shuffled.
// Not every number is repeated.
// ---------------------------------------------------------
    public static void CreateDuplicatedCSV(string fileName, int n = 1_000_000)
    {
        // Determine project directory
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        string filePath = Path.Combine(projectDir, fileName);

        // Build list with duplicates: each number repeats 1–7 times
        var values = new List<int>(n);
        Random rand = new Random();
        int currentValue = 1;

        while (values.Count < n)
        {
            int repeats = rand.Next(1, 8); // 1 to 7 (upper bound exclusive)

            for (int i = 0; i < repeats && values.Count < n; i++)
            {
                values.Add(currentValue);
            }

            currentValue++;
        }

        // Convert to array and fully shuffle
        int[] numbers = values.ToArray();
        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        // Save to CSV in chunks
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

        Console.WriteLine($"Created DUPLICATED CSV '{fileName}' with {numbers.Length:N0} elements (1–7 repeats per value).");
        Console.WriteLine(filePath);
    }
}

using System;
using System.IO;

public static class CreateData
{
    public static void CreateCSV(string fileName, int n = 1_000_000)
    {
        // figure out where the project is running from
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        string filePath = Path.Combine(projectDir, fileName);

        // make 1..n
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
            numbers[i] = i + 1;

        // shuffle (Fisher–Yates)
        Random rand = new Random();
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        // save to CSV
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

        Console.WriteLine($"Created {fileName} with {n:N0} unique numbers at:");
        Console.WriteLine(filePath);
    }
}
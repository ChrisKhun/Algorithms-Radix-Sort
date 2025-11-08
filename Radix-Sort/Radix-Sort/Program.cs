using System;
using System.IO;

class Program
{
    public static void loadCSV(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            Console.WriteLine($"Looked in: {filePath}");
            return;
        }

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var fields = line.Split(',');
            foreach (var field in fields)
                Console.Write($"{field.PadLeft(5)}");

            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";

        // Build full path to the CSV file in main directory
        string filePath = Path.Combine(projectDir, "reverse_sorted.csv");

        Console.WriteLine($"Trying to load: {filePath}");
        loadCSV(filePath);
    }
}
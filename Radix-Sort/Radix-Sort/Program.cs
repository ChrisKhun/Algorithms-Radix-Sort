using System;
using System.IO;
using System.Collections.Generic;

// QuickSort algorithm adapted from W3Resource
// Source: https://www.w3resource.com/csharp-exercises/searching-and-sorting-algorithm/searching-and-sorting-algorithm-exercise-9.php
// 


class Program
{
    // Loads a CSV and returns the data as a List<List<int>>
    public static List<List<int>> LoadCSV(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return new List<List<int>>();
        }
        var lines = File.ReadAllLines(filePath);
        var data = new List<List<int>>();
        
        // test to print all csv data
        // Console.WriteLine($"\nLoaded: {Path.GetFileName(filePath)}");
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            var fields = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var row = new List<int>();

            foreach (var field in fields)
            {
                if (int.TryParse(field.Trim(), out int num))
                {
                    row.Add(num);
                    // Console.Write($"{num.ToString().PadLeft(5)}");
                }
            }
            data.Add(row);
            // Console.WriteLine();
        }

        Console.WriteLine($"{Path.GetFileName(filePath)} has been loaded successfully.");
        return data;
    }

    public static void QuickSort(List<List<int>> list, int left, int right)
    {
        
    }

    static void Main(string[] args)
    {
        string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?.Parent?.FullName ?? "";
        
        // set file path
        string randomPath = Path.Combine(projectDir, "random_data.csv");
        string nearlyPath = Path.Combine(projectDir, "nearly_sorted.csv");
        string reversePath = Path.Combine(projectDir, "reverse_sorted.csv");
        string duplicatePath = Path.Combine(projectDir, "duplicate_data.csv");

        // load each file into var
        var randomData = LoadCSV(randomPath);
        var nearlySortedData = LoadCSV(nearlyPath);
        var reverseSortedData = LoadCSV(reversePath);
        var duplicateData = LoadCSV(duplicatePath);
        Console.WriteLine("\nAll CSVs have been processed successfully.");
        
    }
}

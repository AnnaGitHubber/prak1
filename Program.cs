using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] sizes = { 10, 100, 1000, 10000 };
        int[][] arrays = sizes.Select(size => GenerateRandomArray(size)).ToArray();

        // Creating additional variations of the largest array
        int[] largeArray = GenerateRandomArray(10000);
        int[] almostSortedArray = MakeAlmostSorted(largeArray);
        int[] reversedArray = largeArray.OrderByDescending(x => x).ToArray();

        Console.WriteLine("Unsorted arrays:");
        foreach (var arr in arrays) PrintArray(arr);

        // Measuring sorting performance
        TestSortingAlgorithms(arrays);

        // Testing search performance before and after sorting
        TestSearchAlgorithms(arrays);

        // Finding unique elements in A or B
        FindUniqueElements(arrays[0], arrays[1]);
    }

    static int[] GenerateRandomArray(int size)
    {
        Random rand = new Random();
        return Enumerable.Range(0, size).Select(_ => rand.Next(10000)).ToArray();
    }

    static int[] MakeAlmostSorted(int[] array)
    {
        int[] arr = (int[])array.Clone();
        Array.Sort(arr);
        int swapCount = arr.Length / 10;
        Random rand = new Random();
        for (int i = 0; i < swapCount; i++)
        {
            int idx1 = rand.Next(arr.Length);
            int idx2 = rand.Next(arr.Length);
            (arr[idx1], arr[idx2]) = (arr[idx2], arr[idx1]);
        }
        return arr;
    }

    static void PrintArray(int[] array)
    {
        Console.WriteLine(string.Join(", ", array.Take(10)) + (array.Length > 10 ? "..." : ""));
    }

    static void TestSortingAlgorithms(int[][] arrays)
    {
        Console.WriteLine("Sorting performance:");
        foreach (var arr in arrays)
        {
            int[] copy = (int[])arr.Clone();
            Stopwatch stopwatch = Stopwatch.StartNew();
            SelectionSort(copy);
            stopwatch.Stop();
            Console.WriteLine($"Array size {arr.Length}: Selection Sort {stopwatch.ElapsedMilliseconds} ms");

            copy = (int[])arr.Clone();
            stopwatch.Restart();
            QuickSort(copy, 0, copy.Length - 1);
            stopwatch.Stop();
            Console.WriteLine($"Array size {arr.Length}: Quick Sort {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    static void SelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }
            (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
        }
    }

    static void QuickSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);
            QuickSort(arr, left, pivot - 1);
            QuickSort(arr, pivot + 1, right);
        }
    }

    static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        (arr[i + 1], arr[right]) = (arr[right], arr[i + 1]);
        return i + 1;
    }

    static void TestSearchAlgorithms(int[][] arrays)
    {
        Random rand = new Random();
        Console.WriteLine("Search performance:");
        foreach (var arr in arrays)
        {
            int key = arr[rand.Next(arr.Length)];
            Stopwatch stopwatch = Stopwatch.StartNew();
            int index = LinearSearch(arr, key);
            stopwatch.Stop();
            Console.WriteLine($"Array size {arr.Length}, Key {key}: Linear Search found at {index} in {stopwatch.ElapsedTicks} ticks");

            Array.Sort(arr);
            stopwatch.Restart();
            index = InterpolationSearch(arr, key);
            stopwatch.Stop();
            Console.WriteLine($"Array size {arr.Length}, Key {key}: Interpolation Search found at {index} in {stopwatch.ElapsedTicks} ticks");
        }
    }

    static int LinearSearch(int[] arr, int key)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == key) return i;
        }
        return -1;
    }

    static int InterpolationSearch(int[] arr, int key)
    {
        int low = 0, high = arr.Length - 1;
        while (low <= high && key >= arr[low] && key <= arr[high])
        {
            if (low == high)
            {
                if (arr[low] == key) return low;
                return -1;
            }
            int pos = low + (((key - arr[low]) * (high - low)) / (arr[high] - arr[low]));
            if (arr[pos] == key) return pos;
            if (arr[pos] < key) low = pos + 1;
            else high = pos - 1;
        }
        return -1;
    }

    static void FindUniqueElements(int[] A, int[] B)
    {
        var uniqueA = A.Except(B);
        var uniqueB = B.Except(A);
        Console.WriteLine("Unique elements in A:");
        Console.WriteLine(string.Join(", ", uniqueA));
        Console.WriteLine("Unique elements in B:");
        Console.WriteLine(string.Join(", ", uniqueB));
    }
}

using System;
using System.IO;

namespace AtomSizeCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "";
            while (!File.Exists(sourcePath))
            {
                Console.WriteLine("Enter source path:");
                sourcePath = Console.ReadLine();
            }

            Console.WriteLine("Enter result path:");
            string resultPath = Console.ReadLine();

            Console.WriteLine("Enter 4 byte signature (e.g. mdat):");
            string signature = Console.ReadLine();

            AtomSizeCounter.Count(sourcePath, resultPath, signature);
            Console.WriteLine($"Completed. Results written to {resultPath}");
            Console.ReadKey();
        }
    }
}

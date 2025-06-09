using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GenealogicalAlgorithmist.Genes; // Import the genes
using GenealogicalAlgorithmist.Interfaces; // Import the interface

namespace GenealogicalAlgorithmist
{
    /// <summary>
    /// Main program entry point.
    /// </summary>
    public class Program
    {
        private static readonly Random GeneRandom = new Random();

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Genealogical Algorithmist";

            PrintIntro();

            // --- Configuration ---
            const int populationSize = 100;
            const int maxGenerations = 50;
            const double targetFitness = 1.0;

            // --- Define Progenitors ---
            var progenitors = new List<AlgorithmChromosome>
            {
                new AlgorithmChromosome(new List<ILogicGene> { new SwapIfGreaterGene() }),
                new AlgorithmChromosome(new List<ILogicGene> { new SwapIfLessGene() }),
                new AlgorithmChromosome(new List<ILogicGene> { new UnconditionalSwapGene() }),
            };

            // --- Initialize the Engine ---
            var engine = new EvolutionEngine(populationSize, CalculateSortingFitness, GetRandomSortingGene, progenitors);

            // --- Evolution Loop ---
            while (engine.Generation < maxGenerations && (engine.FittestAlgorithm == null || engine.FittestAlgorithm.Fitness < targetFitness))
            {
                engine.Evolve();
                PrintGenerationStatus(engine);
                Thread.Sleep(100);
            }

            PrintFinalResults(engine);
        }

        public static double CalculateSortingFitness(int[] data)
        {
            int correctPairs = 0;
            int totalPairs = 0;
            for (int i = 0; i < data.Length - 1; i++)
            {
                for (int j = i + 1; j < data.Length; j++)
                {
                    if (data[i] <= data[j])
                    {
                        correctPairs++;
                    }
                    totalPairs++;
                }
            }
            return totalPairs == 0 ? 1.0 : (double)correctPairs / totalPairs;
        }

        public static ILogicGene GetRandomSortingGene()
        {
            int r = GeneRandom.Next(3);
            switch (r)
            {
                case 0: return new SwapIfGreaterGene();
                case 1: return new SwapIfLessGene();
                case 2: return new UnconditionalSwapGene();
                default: throw new InvalidOperationException("Invalid gene type.");
            }
        }

        #region Console UI

        private static void PrintIntro()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ██████╗ ███████╗███╗   ██╗███████╗ █████╗ ██╗      ██████╗  ██████╗ ██╗  ██╗
    ██╔══██╗██╔════╝████╗  ██║██╔════╝██╔══██╗██║     ██╔═══██╗██╔════╝ ██║  ██║
    ██████╔╝█████╗  ██╔██╗ ██║█████╗  ███████║██║     ██║   ██║██║  ███╗███████║
    ██╔═══╝ ██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██║██║     ██║   ██║██║   ██║██╔══██║
    ██║     ███████╗██║ ╚████║███████╗██║  ██║███████╗╚██████╔╝╚██████╔╝██║  ██║
    ╚═╝     ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
            A L G O R I T H M I S T
");
            Console.ResetColor();
            Console.WriteLine("\nInitializing evolution to create a sorting algorithm from scratch...");
            Console.WriteLine("The system will combine, mutate, and evolve logical 'genes' over generations.\n");
        }

        private static void PrintGenerationStatus(EvolutionEngine engine)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Generation: {engine.Generation.ToString().PadLeft(3)} | ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Best Fitness: {engine.FittestAlgorithm.Fitness:F4} | ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Fittest Algorithm Genes: [ ");
            var geneDescriptions = engine.FittestAlgorithm.Genes.Select(g => g.GetType().Name);
            Console.Write(string.Join(", ", geneDescriptions));
            Console.WriteLine(" ]");
            Console.ResetColor();
        }

        private static void PrintFinalResults(EvolutionEngine engine)
        {
            Console.WriteLine("\n----------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("EVOLUTION COMPLETE.");
            Console.ResetColor();

            if (engine.FittestAlgorithm.Fitness >= 0.99)
            {
                Console.WriteLine($"Success! A highly effective algorithm was evolved in {engine.Generation} generations.");
            }
            else
            {
                Console.WriteLine($"Evolution finished after {engine.Generation} generations, but the result may be suboptimal.");
            }

            Console.WriteLine("\n--- Final Evolved Algorithm ---");
            Console.WriteLine($"Fitness Score: {engine.FittestAlgorithm.Fitness:F4}");
            Console.WriteLine("Logical Gene Sequence:");
            int step = 1;
            foreach (var gene in engine.FittestAlgorithm.Genes)
            {
                Console.WriteLine($"  {step++}. {gene.Description}");
            }

            Console.WriteLine("\n--- Testing Evolved Algorithm ---");
            int[] testArray = EvolutionEngine.CreateUnsortedArray(15);
            Console.WriteLine($"Unsorted Data: [ {string.Join(", ", testArray)} ]");

            engine.FittestAlgorithm.Run(testArray);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sorted Data:   [ {string.Join(", ", testArray)} ]");
            Console.ResetColor();
            Console.WriteLine("\n----------------------------------------------------");
        }
        #endregion
    }
}
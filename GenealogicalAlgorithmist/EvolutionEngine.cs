using System;
using System.Collections.Generic;
using System.Linq;
using GenealogicalAlgorithmist.Interfaces;

namespace GenealogicalAlgorithmist
{
    /// <summary>
    /// The engine that manages the population, breeding, mutation, and evolution.
    /// </summary>
    public class EvolutionEngine
    {
        private readonly List<AlgorithmChromosome> _population;
        private readonly Func<int[], double> _fitnessFunction;
        private readonly Func<ILogicGene> _getRandomGene;
        private readonly Random _random = new Random();

        public int Generation { get; private set; }
        public AlgorithmChromosome FittestAlgorithm { get; private set; }

        public EvolutionEngine(
            int populationSize,
            Func<int[], double> fitnessFunction,
            Func<ILogicGene> getRandomGene,
            List<AlgorithmChromosome> progenitorAlgorithms)
        {
            _population = new List<AlgorithmChromosome>();
            _fitnessFunction = fitnessFunction;
            _getRandomGene = getRandomGene;
            _population.AddRange(progenitorAlgorithms);

            // Fill the rest of the population with random algorithms
            while (_population.Count < populationSize)
            {
                var randomChromosome = new AlgorithmChromosome();
                int geneCount = _random.Next(1, 6); // Random initial length
                for (int i = 0; i < geneCount; i++)
                {
                    randomChromosome.Genes.Add(_getRandomGene());
                }
                _population.Add(randomChromosome);
            }
        }

        /// <summary>
        /// Runs one complete cycle of evolution: evaluation, selection, and reproduction.
        /// </summary>
        public void Evolve()
        {
            // 1. Evaluate Fitness of each algorithm in the population
            EvaluatePopulation();

            // 2. Selection: Create a new generation by breeding the fittest
            List<AlgorithmChromosome> newPopulation = new List<AlgorithmChromosome>();

            // Elitism: Carry over the top N% of the population to the next generation
            int eliteCount = (int)(_population.Count * 0.1); // Keep top 10%
            newPopulation.AddRange(_population.OrderByDescending(c => c.Fitness).Take(eliteCount));

            // Breeding
            while (newPopulation.Count < _population.Count)
            {
                var parent1 = SelectParent();
                var parent2 = SelectParent();

                var child = Crossover(parent1, parent2);
                Mutate(child);
                newPopulation.Add(child);
            }

            _population.Clear();
            _population.AddRange(newPopulation);

            Generation++;
            FittestAlgorithm = _population.OrderByDescending(c => c.Fitness).First();
        }

        private void EvaluatePopulation()
        {
            // Use a parallel loop for faster fitness evaluation on multi-core systems
            foreach (var chromosome in _population)
            {
                int[] testData = CreateUnsortedArray(20);
                chromosome.Run(testData);
                chromosome.Fitness = _fitnessFunction(testData);
            }
        }

        private AlgorithmChromosome SelectParent()
        {
            int tournamentSize = 5;
            AlgorithmChromosome best = null;
            for (int i = 0; i < tournamentSize; i++)
            {
                var contestant = _population[_random.Next(_population.Count)];
                if (best == null || contestant.Fitness > best.Fitness)
                {
                    best = contestant;
                }
            }
            return best;
        }

        private AlgorithmChromosome Crossover(AlgorithmChromosome parent1, AlgorithmChromosome parent2)
        {
            var child = new AlgorithmChromosome();
            int crossoverPoint = (parent1.Genes.Count > 0 && parent2.Genes.Count > 0) ? _random.Next(Math.Min(parent1.Genes.Count, parent2.Genes.Count)) : 0;

            child.Genes.AddRange(parent1.Genes.Take(crossoverPoint));
            child.Genes.AddRange(parent2.Genes.Skip(crossoverPoint));

            if (child.Genes.Count == 0)
            {
                child.Genes.Add(_getRandomGene());
            }

            return child;
        }

        private void Mutate(AlgorithmChromosome chromosome)
        {
            double mutationRate = 0.05;
            for (int i = 0; i < chromosome.Genes.Count; i++)
            {
                if (_random.NextDouble() < mutationRate)
                {
                    int mutationType = _random.Next(3);
                    switch (mutationType)
                    {
                        case 0: // Replace a gene
                            chromosome.Genes[i] = _getRandomGene();
                            break;
                        case 1: // Add a new gene
                            chromosome.Genes.Insert(_random.Next(chromosome.Genes.Count + 1), _getRandomGene());
                            break;
                        case 2: // Remove a gene
                            if (chromosome.Genes.Count > 1)
                            {
                                chromosome.Genes.RemoveAt(i);
                            }
                            break;
                    }
                }
            }
        }

        public static int[] CreateUnsortedArray(int size)
        {
            Random rand = new Random();
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rand.Next(100);
            }
            return arr;
        }
    }
}
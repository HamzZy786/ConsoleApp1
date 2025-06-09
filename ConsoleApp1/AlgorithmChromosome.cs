using System.Collections.Generic;
using GenealogicalAlgorithmist.Interfaces;

namespace GenealogicalAlgorithmist
{
    /// <summary>
    /// Represents a complete algorithm, composed of a sequence of ILogicGenes.
    /// This is the equivalent of a "chromosome".
    /// </summary>
    public class AlgorithmChromosome
    {
        public List<ILogicGene> Genes { get; }
        public double Fitness { get; set; }

        public AlgorithmChromosome()
        {
            Genes = new List<ILogicGene>();
            Fitness = 0.0;
        }

        public AlgorithmChromosome(IEnumerable<ILogicGene> genes)
        {
            Genes = new List<ILogicGene>(genes);
            Fitness = 0.0;
        }

        /// <summary>
        /// Executes the entire algorithm on a dataset.
        /// </summary>
        public void Run(int[] data)
        {
            if (Genes.Count == 0) return;

            for (int i = 0; i < data.Length - 1; i++)
            {
                for (int j = i + 1; j < data.Length; j++)
                {
                    // Each gene gets a chance to act within the loops.
                    foreach (var gene in Genes)
                    {
                        gene.Execute(data, i, j);
                    }
                }
            }
        }
    }
}
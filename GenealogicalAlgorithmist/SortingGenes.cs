using GenealogicalAlgorithmist.Interfaces;

namespace GenealogicalAlgorithmist.Genes
{
    /// <summary>
    /// A gene that swaps two elements if the first is greater than the second.
    /// This is the core logic of Bubble Sort.
    /// </summary>
    public class SwapIfGreaterGene : ILogicGene
    {
        public string Description => "IF data[i] > data[j] THEN SWAP(i, j)";
        public void Execute(int[] data, int i, int j)
        {
            if (data[i] > data[j])
            {
                (data[i], data[j]) = (data[j], data[i]);
            }
        }
    }

    /// <summary>
    /// A gene that swaps two elements if the first is less than the second.
    /// This is the inverse of what we want, an "anti-pattern" gene.
    /// </summary>
    public class SwapIfLessGene : ILogicGene
    {
        public string Description => "IF data[i] < data[j] THEN SWAP(i, j)";
        public void Execute(int[] data, int i, int j)
        {
            if (data[i] < data[j])
            {
                (data[i], data[j]) = (data[j], data[i]);
            }
        }
    }

    /// <summary>
    /// A gene that always swaps two elements, regardless of their values.
    /// This is a disruptive, generally unhelpful gene.
    /// </summary>
    public class UnconditionalSwapGene : ILogicGene
    {
        public string Description => "SWAP(i, j)";
        public void Execute(int[] data, int i, int j)
        {
            (data[i], data[j]) = (data[j], data[i]);
        }
    }
}
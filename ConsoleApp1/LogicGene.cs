namespace GenealogicalAlgorithmist.Interfaces
{
    /// <summary>
    /// Represents a single logical step in an algorithm, like "Swap elements at i and j".
    /// This is the equivalent of a "gene".
    /// </summary>
    public interface ILogicGene
    {
        /// <summary>
        /// A human-readable description of the gene's action.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Executes the gene's logic on a given dataset.
        /// </summary>
        /// <param name="data">The array being manipulated.</param>
        /// <param name="i">The primary index, often from an outer loop.</param>
        /// <param name="j">The secondary index, often from an inner loop.</param>
        void Execute(int[] data, int i, int j);
    }
}

namespace HA
{
    /// <summary>
    /// Represents a quest objective with initialization, progress tracking,
    /// completion status, and progress description retrieval.
    /// </summary>
    public interface IQuestObjective
    {
        /// <summary>
        /// Indicates whether the objective's completion condition has been met.
        /// </summary>
        public bool IsCompleted { get; }

        /// <summary>
        /// Initializes the objective based on the provided quest information.
        /// </summary>
        /// <param name="questInfo">Quest data containing objective requirements.</param>
        public void Initialize(QuestInfoSO questInfo);

        /// <summary>
        /// Updates the objective's progress using the given identifier.
        /// </summary>
        /// <param name="id">Identifier related to the progress update (e.g., enemy ID, item ID).</param>
        public void UpdateProgress(string id);

        /// <summary>
        /// Returns a human-readable description of the current progress.
        /// </summary>
        public string GetProgressDescription();
    }
}

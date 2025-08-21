namespace HA
{
    /// <summary>
    /// Holds the next spawn point ID across scene transitions.
    /// Use this to carry target placement information between scenes.
    /// </summary>
    public static class WarpContext
    {
        /// <summary>
        /// The spawn point ID to use after the next scene load.
        /// </summary>
        public static string NextSpawnPointId = null;

        /// <summary>
        /// Sets the next spawn point ID to be consumed after scene load.
        /// </summary>
        /// <param name="spawnPointId">Target spawn point identifier.</param>
        public static void Set(string spawnPointId)
        {
            NextSpawnPointId = spawnPointId;
        }


        /// <summary>
        /// Clears the stored spawn point ID once it has been consumed.
        /// </summary>
        public static void Clear()
        {
            NextSpawnPointId = null;
        }
    }
}

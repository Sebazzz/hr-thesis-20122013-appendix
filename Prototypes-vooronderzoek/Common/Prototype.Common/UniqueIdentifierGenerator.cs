namespace Prototype.Common {
    using System.Threading;

    /// <summary>
    ///   Generates a unique identifier for the specified entity
    /// </summary>
    /// <typeparam name="TEntity"> </typeparam>
    public static class UniqueIdentifierGenerator<TEntity>
            where TEntity : class {
        private static long CurrentUniqueId;

        public static long GetNewId() {
            return Interlocked.Increment(ref CurrentUniqueId);
        }
    }
}
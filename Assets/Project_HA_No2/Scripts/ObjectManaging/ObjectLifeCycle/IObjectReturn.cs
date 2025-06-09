using UnityEngine;
using System;

namespace HA
{
    /// <summary>
    /// Defines a contract for returning objects back to their source (e.g., pool).
    /// Helps maintain object lifecycle and memory efficiency.
    /// </summary>
    public interface IObjectReturn
    {
        /// <summary>
        /// Event triggered when an object return is requested.
        /// Can be used for cleanup notifications or pooling hooks.
        /// </summary>
        event Action OnReturnRequested;

        /// <summary>
        /// Returns the specified object to its original source using the given key.
        /// </summary>
        /// <param name="key">Unique key used to identify the object.</param>
        /// <param name="component">Component instance to return.</param>
        void Return(string key, Component component);
    }
}

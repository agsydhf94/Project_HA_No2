using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Represents a modular death effect that can be played on a target GameObject.
    /// Implement this interface to define visual or audio effects triggered on character death.
    /// </summary>
    public interface IDeathEffect
    {
        /// <summary>
        /// Plays the death effect asynchronously on the given target.
        /// </summary>
        /// <param name="target">The GameObject on which to play the effect.</param>
        /// <returns>A UniTask representing the asynchronous operation.</returns>
        UniTask PlayAsync(GameObject target);
    }
}

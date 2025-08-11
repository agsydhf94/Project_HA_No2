using System;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// ViewModel for presenting a single quest objective to the UI layer.
    /// Wraps an <see cref="IQuestObjective"/> and exposes lightweight,
    /// UI-friendly properties (e.g., progress text, completion state),
    /// along with a change notification event.
    /// </summary>
    public class QuestObjectiveViewModel : MonoBehaviour
    {
        /// <summary>
        /// Fired when the bound objective changes or when an explicit <see cref="Notify"/> is called.
        /// UI can subscribe to refresh its presentation.
        /// </summary>
        public event Action OnChanged;

        private IQuestObjective questObjective;

        /// <summary>
        /// Binds an <see cref="IQuestObjective"/> to this ViewModel and immediately notifies listeners.
        /// </summary>
        /// <param name="objective">Objective instance to present. If null, a warning is logged and no binding occurs.</param>
        public void Initialize(IQuestObjective objective)
        {
            questObjective = objective;
            Notify();
        }


        /// <summary>
        /// Triggers <see cref="OnChanged"/> for subscribers (e.g., to force a UI refresh).
        /// </summary>
        public void Notify()
        {
            OnChanged?.Invoke();
        }

        /// <summary>
        /// A short, user-facing textual description of the current progress
        /// (e.g., "2 / 5" or "Defeat 3 more enemies").
        /// </summary>
        public string ProgressText => questObjective.GetProgressDescription();

        /// <summary>
        /// Indicates whether the objective's completion condition has been met.
        /// </summary>
        public bool IsCompleted => questObjective.IsCompleted;
    }
}

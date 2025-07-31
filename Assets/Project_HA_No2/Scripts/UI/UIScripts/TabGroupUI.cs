using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages a group of TabButtonUI instances, handling their selection state.
    /// </summary>
    public class TabGroupUI : MonoBehaviour
    {
        [SerializeField] private List<TabButtonUI> tabButtons;      // All tab buttons managed by this group


        /// <summary>
        /// Registers this group as the manager for each tab button in the list.
        /// </summary>
        private void Awake()
        {
            // Register this group to all tabs
            foreach (var tab in tabButtons)
            {
                tab.SetTabGroup(this);
            }
        }


        /// <summary>
        /// Marks the given tab as selected and unselects all others.
        /// </summary>
        /// <param name="selectedTab">The tab button to select.</param>
        public void SelectTab(TabButtonUI selectedTab)
        {
            foreach (var tab in tabButtons)
            {
                tab.SetSelected(tab == selectedTab);
            }
        }


        /// <summary>
        /// Triggers lazy initialization on all tab buttons in this group.
        /// Useful when enabling all tabs at runtime.
        /// </summary>
        public void InitializeAllTabs()
        {
            foreach (var tab in tabButtons)
            {
                tab.LazyInitialize(); // 지연 초기화 트리거
            }
        }

    }
}

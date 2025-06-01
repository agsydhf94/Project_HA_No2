
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemPopupContainerUI : MonoBehaviour
    {
        public ItemPopupUI popupUIPrefab;
        [SerializeField] private int maxPopups;
        [SerializeField] private float popupDuration;
        [SerializeField] private float verticalSpacing;

        private readonly Queue<ItemPopupUI> activePopups = new();

        public void ShowItemPopup(ItemDataSO itemDataSO)
        {
            ItemPopupUI popup = Instantiate(popupUIPrefab, transform);
            popup.Initialize(itemDataSO, popupDuration, OnPopupComplete);

            activePopups.Enqueue(popup); // 맨 위에 추가
            UpdatePopupPositions();

            // 초과되면 가장 오래된 팝업 제거
            if (activePopups.Count > maxPopups)
            {
                ItemPopupUI oldest = activePopups.Dequeue();
                Destroy(oldest.gameObject);
                UpdatePopupPositions();
            }
        }

        private void UpdatePopupPositions()
        {
            int index = 0;
            foreach (var popup in activePopups)
            {
                float yOffset = -index * verticalSpacing;
                popup.SetVerticalOffset(yOffset);
                index++;
            }
        }

        private void OnPopupComplete(ItemPopupUI popup)
        {
            Queue<ItemPopupUI> newQueue = new();
            while (activePopups.Count > 0)
            {
                var current = activePopups.Dequeue();
                if (current != popup)
                    newQueue.Enqueue(current);
                else
                    Destroy(current.gameObject);
            }

            while (newQueue.Count > 0)
                activePopups.Enqueue(newQueue.Dequeue());

            UpdatePopupPositions();
        }
    }
}

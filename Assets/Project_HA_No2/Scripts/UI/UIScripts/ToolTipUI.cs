using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HA
{
    public class ToolTipUI : MonoBehaviour
    {
        [SerializeField] private float xLimit = 960f;
        [SerializeField] private float yLimit = 540f;

        [SerializeField] private float xOffset = 150f;
        [SerializeField] private float yOffset = 150f;

        public virtual void AdjustPosition()
        {
            Vector2 mousePosition = Input.mousePosition;

            float newXOffset = 0f;
            float newYOffset = 0f;

            if (mousePosition.x > xLimit)
                newXOffset = -xOffset;
            else
                newXOffset = xOffset;

            if (mousePosition.y > yLimit)
                newYOffset = -yOffset;
            else
                newYOffset = yOffset;

            transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
        }

        public void AdjustFontSize(TMP_Text _text)
        {
            if(_text.text.Length > 12)
            {
                _text.fontSize *= 0.7f;
            }
        }
    }
}

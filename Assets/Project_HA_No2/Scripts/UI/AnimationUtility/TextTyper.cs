using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Types out text character by character with a delay between each character.
    /// Also provides a fade-out effect by hiding characters in reverse order.
    /// </summary>
    public class TextTyper : MonoBehaviour
    {
        public TMP_Text textComponent;
        public float charDelay = 0.05f;

        public async UniTask TypeText(string text, CancellationToken ct = default)
        {
            textComponent.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                textComponent.text += text[i];
                await UniTask.Delay(TimeSpan.FromSeconds(charDelay), cancellationToken: ct);
            }
        }

        public async UniTask FadeOut(CancellationToken ct = default)
        {
            int length = textComponent.text.Length;
            for (int i = length; i >= 0; i--)
            {
                textComponent.maxVisibleCharacters = i;
                await UniTask.Delay(TimeSpan.FromSeconds(0.025f), cancellationToken: ct);
            }
            textComponent.text = "";
        }
    }
}

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

        /// <summary>
        /// Event fired every time a character is typed.
        /// Parameters: (characterIndex, typedCharacter)
        /// Use this to trigger per-character effects like glow, sound, etc.
        /// </summary>
        public event Action<int, char> OnCharacterTyped;


        /// <summary>
        /// Types out the given text one character at a time with delay between each.
        /// Fires OnCharacterTyped event after each character is added.
        /// </summary>
        public async UniTask TypeText(string text, CancellationToken ct = default)
        {
            textComponent.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                textComponent.text += text[i];

                // Notify external listeners that a new character has been typed
                OnCharacterTyped?.Invoke(i, text[i]);
                
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

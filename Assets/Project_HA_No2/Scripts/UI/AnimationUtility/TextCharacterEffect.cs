using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Spawns a glow background effect behind each character typed by the TextTyper.
    /// Subscribes to the OnCharacterTyped event and places a pooled UI effect behind the character's position.
    /// The effect is returned to the pool after a specified duration.
    /// </summary>
    [RequireComponent(typeof(TextTyper))]
    public class TextCharacterEffect : MonoBehaviour
    {
        /// <summary>
        /// The TMP_Text component that displays the typed text.
        /// </summary>
        public TMP_Text textComponent;

        /// <summary>
        /// The parent transform under which the glow effect objects will be spawned.
        /// Typically a UI canvas container.
        /// </summary>
        public Transform effectParent;

        /// <summary>
        /// Duration in seconds that each background effect remains active before being returned to the pool.
        /// </summary>
        public float lifetime;

        private TextTyper typer;

        private ObjectManager objectManager;

        /// <summary>
        /// Subscribes to the typing event and caches ObjectManager.
        /// </summary>
        private void Awake()
        {
            typer = GetComponent<TextTyper>();
            typer.OnCharacterTyped += OnCharacterTyped;
            objectManager = ObjectManager.Instance;
        }


        /// <summary>
        /// Cleans up the event subscription to avoid memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            typer.OnCharacterTyped -= OnCharacterTyped;
        }


        /// <summary>
        /// Triggered when a character is typed.
        /// Spawns the background effect behind the corresponding character.
        /// </summary>
        private void OnCharacterTyped(int index, char c)
        {
            SpawnEffectForCharacter(index).Forget();
        }


        /// <summary>
        /// Computes the world position of the typed character and spawns a background effect there.
        /// Uses ObjectManager for pooling and returns the object after the lifetime expires.
        /// </summary>
        private async UniTask SpawnEffectForCharacter(int index)
        {
            textComponent.ForceMeshUpdate();
            var info = textComponent.textInfo;
            if (index >= info.characterCount || !info.characterInfo[index].isVisible)
                return;

            var charInfo = info.characterInfo[index];
            Vector3 worldPos = textComponent.transform.TransformPoint((charInfo.bottomLeft + charInfo.topRight) * 0.5f);

            Component effectUIComponent = objectManager.Spawn("TextBackgroundEffectUI", Vector3.zero, Quaternion.identity, transform, ObjectSourceType.UIPooling);
            GameObject effectUIGameObject = effectUIComponent.gameObject;

            effectUIGameObject.transform.position = worldPos;

            // 크기 및 정렬 조정 (필요시)
            effectUIGameObject.transform.localScale = Vector3.one * 1.2f;

            await UniTask.Delay((int)(lifetime * 1000));
            objectManager.Return("TextBackgroundEffectUI", effectUIComponent);
        }
    }
}

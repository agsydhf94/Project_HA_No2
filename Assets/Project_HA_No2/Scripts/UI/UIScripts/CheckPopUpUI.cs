using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages a self-contained UI pop-up sequence that includes:
    /// - Progress bar fills
    /// - Typing text display
    /// - Image blinking
    /// - Fade-out animation
    /// Used for brief notifications or alerts with animated feedback.
    /// </summary>
    public class CheckPopUpUI : MonoBehaviour
    {
        public static CheckPopUpUI Instance { get; private set; }

        [Header("Modules")]
        [SerializeField] private TextTyper typer;
        [SerializeField] private ImageBlinker blinker;
        [SerializeField] private ImageFiller upperBarFiller;
        [SerializeField] private ImageFiller backgroundFiller;

        [Header("Default")]
        [TextArea] public string defaultMessage;
        public Color defaultBlinkColor = Color.white;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ShowPopup(string message, Color color)
        {
            RunSequence(message, color, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask RunSequence(string message, Color blinkColor, CancellationToken ct)
        {
            await upperBarFiller.FillTo(1f, ct);
            await backgroundFiller.FillTo(1f, ct);

            await typer.TypeText(message, ct);

            blinker.targetImage.color = blinkColor;
            await blinker.BlinkForSeconds(3f, ct);

            await typer.FadeOut(ct);

            await backgroundFiller.FillTo(0f, ct);
            await upperBarFiller.FillTo(0f, ct);
        }
    }
}

